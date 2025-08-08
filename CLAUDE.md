# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

**Important**: 
1. Always read and understand the documentation files in the `docs/` folder to maintain comprehensive knowledge of the project. These documents contain critical information about the project's architecture, implementation details, and version history.
2. After completing each task or implementation, immediately update the relevant documentation to reflect the changes. Keep all documentation synchronized with the current state of the codebase.

## About CCXT.NET

CCXT.NET is a .NET implementation of the CCXT (CryptoCurrency eXchange Trading) library, providing a unified API for interacting with various cryptocurrency exchanges.

## Build and Development Commands

### Build
```bash
# Build the entire solution
dotnet build

# Build specific project
dotnet build src/ccxt.net.csproj

# Build in Release mode
dotnet build -c Release

# Create NuGet package
dotnet pack src/ccxt.net.csproj -c Release
```

### Test
```bash
# Run all tests
dotnet test

# Run tests for specific project
dotnet test tests/ccxt.tests/ccxt.tests.csproj

# Run specific test by name
dotnet test --filter "FullyQualifiedName~TestName"

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"
```

### Sample Applications
```bash
# Run sample application
dotnet run --project samples/ccxt.samples.csproj
```

## Architecture Overview

### Project Structure
- **src/**: Main library code targeting .NET Standard 2.1, .NET 8.0, and .NET 9.0
- **tests/**: Test projects using xUnit framework
- **samples/**: Example implementations for various exchanges
- **docs/**: Documentation files (CHANGELOG, CONTRIBUTING, SECURITY)

### Exchange Implementation Pattern
Each exchange follows a consistent structure organized by 2-letter ISO country codes:
- **exchanges/{country_code}/{exchange}/**: Base exchange implementation
  - **{exchange}.cs**: Main exchange client class
  - **public/**: Public API endpoints (ticker, orderBook, completeOrder, market, ohlcv)
  - **private/**: Private API endpoints requiring authentication (balance, address, transfer, wallet)
  - **trade/**: Trading operations (order, place, trade, position)

### Core Components
- **XApiClient**: Base API client handling HTTP communication, authentication, and error handling
- **Standard Interfaces**: Unified interfaces in `src/shared/standard/` providing consistent API across all exchanges
- **Type System**: Strong typing for all exchange operations in `src/shared/types/`
- **Exchange-specific Extensions**: Each exchange extends the base implementation with custom features

### Key Dependencies
- **Newtonsoft.Json**: JSON serialization/deserialization
- **RestSharp**: HTTP client for API calls
- **System.IdentityModel.Tokens.Jwt**: JWT token handling for authentication
- **CellWars.Threading.AsyncLock**: Thread-safe async operations

### Authentication Flow
1. Exchange implementations inherit from XApiClient
2. API keys are configured via ConnectKey and SecretKey properties
3. Each exchange implements its specific authentication mechanism (HMAC, JWT, etc.)
4. Private endpoints automatically include authentication headers

### Supported Exchanges by Region (120 Total)
- **AE (UAE)**: deribit
- **AU (Australia)**: btcmarkets, coinspot
- **BR (Brazil)**: foxbit, mercado, novadax
- **BS (Bahamas)**: fmfwio
- **CA (Canada)**: ndax, timex
- **CN (China)**: Bitforex, Gate.io, Huobi, OKEX, ZB, bigone, bingx, bitget, bybit, coinex, digifinex, gate, hashkey, hitbtc, htx, kucoin, kucoinfutures, lbank, mexc, okx, woo, woofipro, xt
- **EE (Estonia)**: latoken
- **EU (Europe)**: bit2c, bitopro, bitvavo, btcalpha, btcturk, coinmate, exmo, onetrading, paymium, wavesexchange, whitebit, yobit, zonda
- **GB (UK)**: Bitfinex, Bitstamp, CEX.IO, bitteam, blockchaincom, coinmetro, luno
- **GLOBAL**: coincatch, defx, hollaex, myokx, oceanex, oxfun, p2b, tradeogre
- **ID (Indonesia)**: indodax, tokocrypto
- **IN (India)**: bitbns, modetrade
- **JP (Japan)**: ANXPro, bitFlyer, Coincheck, Quoinex, bitbank, btcbox, zaif, bittrade
- **KR (Korea)**: Bithumb, CoinOne, Gopax, Korbit, OKCoin KR, Upbit, probit
- **KY (Cayman Islands)**: bitmart, blofin
- **LT (Lithuania)**: cryptomus
- **MT (Malta)**: bequant
- **MX (Mexico)**: bitso
- **SC (Seychelles)**: BitMEX
- **SG (Singapore)**: bitrue, coinsph, delta, derive, ellipx, hibachi, hyperliquid, independentreserve
- **US (United States)**: Bittrex, GDAX, Gemini, itBit, Kraken, Poloniex, alpaca, apex, ascendex, binancecoinm, binanceus, binanceusdm, binance, coinbaseadvanced, coinbaseexchange, coinbaseinternational, coinbase, crypto, cryptocom, krakenfutures, okcoin, okxus, paradex, phemex, vertex

### Error Handling
- Standardized error codes in `src/shared/standard/errorCode.cs`
- Exchange-specific error mapping in ErrorMessages dictionary
- Consistent ApiResult<T> pattern for all API responses

## Important Notes

- When modifying exchange implementations, maintain consistency with the established pattern
- All public API methods should return ApiResult<T> wrapped responses
- Private API methods require proper authentication setup
- Exchange rate limits should be respected (implemented per exchange)
- When adding new exchanges, follow the existing directory structure: exchanges/{country_code}/{exchange}/
- Test coverage should include both public and private API endpoints when API keys are available