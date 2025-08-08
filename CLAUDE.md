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
dotnet test tests/ccxt.test/ccxt.test.csproj

# Run specific test by name
dotnet test --filter "FullyQualifiedName~TestName"

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"
```

### Sample Applications
```bash
# Run sample applications
dotnet run --project samples/binance/binance.csproj
dotnet run --project samples/bitmex/bitmex.csproj
dotnet run --project samples/kraken/kraken.csproj
dotnet run --project samples/bittrex/bittrex.csproj
```

## Architecture Overview

### Project Structure
- **src/**: Main library code targeting .NET Standard 2.1, .NET 8.0, and .NET 9.0
- **tests/**: Test projects using xUnit framework
- **samples/**: Example implementations for various exchanges
- **docs/**: Documentation files (CHANGELOG, CONTRIBUTING, SECURITY)

### Exchange Implementation Pattern
Each exchange follows a consistent structure organized by country/region:
- **exchanges/{region}/{exchange}/**: Base exchange implementation
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

### Supported Exchanges by Region
- **CHN (China)**: Bitforex, Gate.io, Huobi, OKEX, ZB
- **JPN (Japan)**: ANXPro, bitFlyer, Coincheck, Quoinex
- **KOR (Korea)**: Bithumb, CoinOne, Gopax, Korbit, OKCoin KR, Upbit
- **HK (Hong Kong)**: Binance
- **GBR (UK)**: Bitfinex, Bitstamp, CEX.IO
- **USA**: Bittrex, GDAX, Gemini, itBit, Kraken, Poloniex
- **SEY (Seychelles)**: BitMEX

### Error Handling
- Standardized error codes in `src/shared/standard/errorCode.cs`
- Exchange-specific error mapping in ErrorMessages dictionary
- Consistent ApiResult<T> pattern for all API responses

## Important Notes

- When modifying exchange implementations, maintain consistency with the established pattern
- All public API methods should return ApiResult<T> wrapped responses
- Private API methods require proper authentication setup
- Exchange rate limits should be respected (implemented per exchange)
- When adding new exchanges, follow the existing directory structure: exchanges/{region}/{exchange}/
- Test coverage should include both public and private API endpoints when API keys are available