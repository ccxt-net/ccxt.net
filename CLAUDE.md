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
dotnet test tests/ccxt.tests.csproj

# Run specific test by name
dotnet test --filter "FullyQualifiedName~TestName"

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"
```

### Sample Applications
```bash
# Run sample application
dotnet run --project samples/ccxt.samples.csproj

# Run specific exchange samples (if individual samples exist)
dotnet run --project samples/Program.cs
```

## Architecture Overview

### Project Structure
- **src/**: Main library code targeting .NET Standard 2.1, .NET 8.0, and .NET 9.0
- **tests/**: Test projects using xUnit framework
- **samples/**: Example implementations for various exchanges
- **docs/**: Documentation files (CHANGELOG, CONTRIBUTING, SECURITY)

### Exchange Implementation Pattern
Each exchange follows a consistent structure organized by 2-letter ISO country codes (ISO 3166-1 alpha-2):
- **exchanges/{country_code}/{exchange}/**: Base exchange implementation
  - **{exchange}.cs**: Main exchange client class inheriting from XApiClient
  - **public/**: Public API endpoints (no authentication required)
    - `publicApi.cs`: Base public API implementation
    - `ticker.cs`: Price ticker information
    - `orderBook.cs`: Order book depth data
    - `completeOrder.cs`: Completed orders/trades history
    - `market.cs`: Market information and trading pairs
    - `ohlcv.cs`: OHLCV (candlestick) data
  - **private/**: Private API endpoints (authentication required)
    - `privateApi.cs`: Base private API implementation
    - `balance.cs`: Account balance information
    - `address.cs`: Deposit addresses
    - `transfer.cs`: Deposit/withdrawal history
    - `wallet.cs`: Wallet management
  - **trade/**: Trading operations (authentication required)
    - `tradeApi.cs`: Base trading API implementation
    - `order.cs`: Order management
    - `place.cs`: Place new orders
    - `trade.cs`: Trade history and execution
    - `position.cs`: Position management (for futures/derivatives)

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

#### Implementation Status
- **✅ Fully Implemented**: 26 exchanges (22%) - Complete API implementation with testing
- **🚧 Basic Structure**: 94 exchanges (78%) - Standard folder structure ready for API integration

#### Exchanges by Country Code (ISO 3166-1 alpha-2)
- **AE (UAE)**: deribit
- **AU (Australia)**: btcmarkets, coinspot  
- **BR (Brazil)**: foxbit, mercado, novadax
- **BS (Bahamas)**: fmfwio
- **CA (Canada)**: ndax, timex
- **CN (China)**: Bitforex✅, Gate.io✅, Huobi✅, OKEX✅, ZB✅, bigone, bingx, bitget, bybit, coinex, digifinex, gate, hashkey, hitbtc, htx, kucoin, kucoinfutures, lbank, mexc, okx, woo, woofipro, xt
- **EE (Estonia)**: latoken
- **EU (Europe)**: bit2c, bitopro, bitvavo, btcalpha, btcturk, coinmate, exmo, onetrading, paymium, wavesexchange, whitebit, yobit, zonda
- **GB (UK)**: Bitfinex✅, Bitstamp✅, CEX.IO✅, bitteam, blockchaincom, coinmetro, luno
- **GLOBAL**: coincatch, defx, hollaex, myokx, oceanex, oxfun, p2b, tradeogre
- **ID (Indonesia)**: indodax, tokocrypto
- **IN (India)**: bitbns, modetrade
- **JP (Japan)**: ANXPro✅, bitFlyer✅, Coincheck✅, Quoinex✅, bitbank, btcbox, zaif, bittrade
- **KR (Korea)**: Bithumb✅, CoinOne✅, Gopax✅, Korbit✅, OKCoin KR✅, Upbit✅, probit
- **KY (Cayman Islands)**: bitmart, blofin
- **LT (Lithuania)**: cryptomus
- **MT (Malta)**: bequant
- **MX (Mexico)**: bitso
- **SC (Seychelles)**: BitMEX✅
- **SG (Singapore)**: bitrue, coinsph, delta, derive, ellipx, hibachi, hyperliquid, independentreserve
- **HK (Hong Kong)**: Binance✅
- **US (United States)**: Bittrex✅, GDAX✅, Gemini✅, itBit✅, Kraken✅, Poloniex✅, alpaca, apex, ascendex, binancecoinm, binanceus, binanceusdm, coinbaseadvanced, coinbaseexchange, coinbaseinternational, coinbase, crypto, cryptocom, krakenfutures, okcoin, okxus, paradex, phemex, vertex

*(✅ = Fully implemented with complete API support)*

### Error Handling
- Standardized error codes in `src/shared/standard/errorCode.cs`
- Exchange-specific error mapping in ErrorMessages dictionary
- Consistent ApiResult<T> pattern for all API responses

## Exchange Implementation Guidelines

### Standard Exchange Class Structure
```csharp
namespace CCXT.NET.{ExchangeName}
{
    public sealed class {ExchangeName}Client : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        public override string DealerName { get; set; } = "{ExchangeName}";
        
        public {ExchangeName}Client(string division) : base(division) { }
        
        public {ExchangeName}Client(string division, string connect_key, string secret_key)
            : base(division, connect_key, secret_key, authentication: true) { }
    }
}
```

### Important Implementation Notes

- **Pattern Consistency**: All exchanges must follow the established folder and class structure
- **API Response Pattern**: All public API methods should return `ApiResult<T>` wrapped responses
- **Authentication**: Private API methods require proper authentication setup via constructor
- **Rate Limiting**: Exchange rate limits must be respected (implemented per exchange)
- **Directory Structure**: Always use 2-letter ISO country codes: `exchanges/{country_code}/{exchange}/`
- **Testing**: Include both public and private API endpoint tests when API keys are available
- **Error Handling**: Use standardized error codes and map exchange-specific errors appropriately
- **Async Operations**: All API calls should be async with proper cancellation token support

## Development Workflow

1. **Before Making Changes**: Read relevant documentation in `docs/` folder
2. **During Development**: Follow the established patterns and coding standards
3. **After Implementation**: Update documentation to reflect changes
4. **Testing**: Run tests to ensure no breaking changes
5. **Documentation**: Keep CHANGELOG.md updated with all changes

## Quick Reference

### Currently Implemented Exchanges (26)
ANXPro, Binance, Bitfinex, bitFlyer, Bitforex, Bithumb, BitMEX, Bitstamp, Bittrex, CEX.IO, Coincheck, CoinOne, Gate.io, GDAX, Gemini, Gopax, Huobi Pro, itBit, Korbit, Kraken, OKCoin KR, OKEX, Poloniex, QUOINEX, Upbit, ZB

### Pending Implementation (94)
All have basic structure in place, ready for API integration. Priority should be given to:
1. Major derivatives exchanges (Bybit, Deribit, KuCoin Futures)
2. High-volume spot exchanges (OKX, KuCoin, Coinbase)
3. Regional leaders (Bitso, Mercado, Indodax, Bitbns)