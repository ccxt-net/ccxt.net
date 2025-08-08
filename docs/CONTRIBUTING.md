# Contributing To The CCXT.NET Library

Thank you for your interest in contributing to CCXT.NET! This document provides guidelines and instructions for contributing to the project.

## Table of Contents
- [Code of Conduct](#code-of-conduct)
- [How To Submit An Issue](#how-to-submit-an-issue)
- [How To Contribute Code](#how-to-contribute-code)
- [Development Setup](#development-setup)
- [Repository Structure](#repository-structure)
- [Coding Standards](#coding-standards)
- [Testing Guidelines](#testing-guidelines)
- [Pull Request Process](#pull-request-process)
- [Adding New Exchanges](#adding-new-exchanges)
- [Documentation](#documentation)
- [Community](#community)

## Code of Conduct

We are committed to providing a welcoming and inspiring community for all. Please read and follow our Code of Conduct:

- Be respectful and inclusive
- Welcome newcomers and help them get started
- Focus on what is best for the community
- Show empathy towards other community members

## How To Submit An Issue

Before submitting an issue, please:

1. **Search existing issues** to avoid duplicates
2. **Use issue templates** when available
3. **Provide detailed information**:
   - CCXT.NET version
   - .NET version
   - Operating system
   - Exchange name (if applicable)
   - Complete error messages
   - Code sample that reproduces the issue
   - Expected vs actual behavior

### Issue Categories

- **Bug Report**: Something isn't working as expected
- **Feature Request**: Suggest new functionality
- **Exchange Request**: Request support for a new exchange
- **Documentation**: Improvements to documentation
- **Question**: General questions (consider Stack Overflow first)

## How To Contribute Code

### Before You Start

1. **Fork the repository** to your GitHub account
2. **Create a feature branch** from `master`
3. **Check existing PRs** to avoid duplicate work
4. **Discuss major changes** by opening an issue first

### Development Setup

#### Prerequisites

- **.NET SDK 8.0 or 9.0**: Download from [dotnet.microsoft.com](https://dotnet.microsoft.com/download)
- **Visual Studio 2022** or **VS Code** with C# extension
- **Git**: For version control
- **PowerShell**: For running scripts (Windows) or bash (Linux/Mac)

#### Initial Setup

```bash
# Clone your fork
git clone https://github.com/YOUR_USERNAME/ccxt.net.git
cd ccxt.net

# Add upstream remote
git remote add upstream https://github.com/ccxt-net/ccxt.net.git

# Install dependencies and build
dotnet restore
dotnet build

# Run tests
dotnet test
```

## Repository Structure

```shell
ccxt.net/
├── src/                          # Main library source code
│   ├── exchanges/                # Exchange implementations (120 total)
│   │   ├── {country_code}/       # 2-letter ISO country code folders
│   │   │   └── {exchange}/       # Individual exchange folder
│   │   │       ├── {exchange}.cs # Main exchange client class
│   │   │       ├── private/      # Private API implementations
│   │   │       │   ├── privateApi.cs
│   │   │       │   ├── address.cs
│   │   │       │   ├── balance.cs
│   │   │       │   ├── transfer.cs
│   │   │       │   └── wallet.cs
│   │   │       ├── public/       # Public API implementations
│   │   │       │   ├── publicApi.cs
│   │   │       │   ├── completeOrder.cs
│   │   │       │   ├── market.cs
│   │   │       │   ├── ohlcv.cs
│   │   │       │   ├── orderBook.cs
│   │   │       │   └── ticker.cs
│   │   │       └── trade/        # Trading API implementations
│   │   │           ├── tradeApi.cs
│   │   │           ├── order.cs
│   │   │           ├── place.cs
│   │   │           ├── position.cs
│   │   │           └── trade.cs
│   ├── shared/                   # Shared components
│   │   ├── standard/             # Standard interfaces and base classes
│   │   │   ├── XApiClient.cs    # Base API client
│   │   │   ├── IXApiClient.cs   # Client interface
│   │   │   └── errorCode.cs     # Error code definitions
│   │   └── types/                # Type definitions
│   └── ccxt.net.csproj          # Main project file
├── tests/                        # Test projects
│   ├── ccxt.tests.csproj        # Test project file
│   ├── private/                  # Private API tests
│   ├── public/                   # Public API tests
│   └── trade/                    # Trading API tests
├── samples/                      # Sample applications
│   └── ccxt.samples.csproj      # Sample project file
├── docs/                         # Documentation
│   ├── CHANGELOG.md              # Version history
│   ├── CONTRIBUTING.md          # This file
│   ├── PUBLISH.md               # Publishing guide
│   └── SECURITY.md              # Security policy
├── scripts/                      # Build and deployment scripts
└── README.md                     # Main documentation
```

## Coding Standards

### C# Style Guidelines

We follow the [.NET coding conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions):

```csharp
// Use PascalCase for class names and methods
public class ExchangeClient
{
    // Use camelCase for local variables and parameters
    public async Task<ApiResult<Ticker>> GetTickerAsync(string symbol)
    {
        // Use meaningful variable names
        var tickerRequest = new TickerRequest(symbol);
        
        // Always use braces, even for single-line statements
        if (tickerRequest.IsValid)
        {
            return await SendRequestAsync(tickerRequest);
        }
        
        // Use explicit types for clarity
        ApiResult<Ticker> errorResult = new ApiResult<Ticker>();
        return errorResult;
    }
    
    // Private fields start with underscore
    private readonly HttpClient _httpClient;
    
    // Properties use PascalCase
    public string ExchangeName { get; set; }
}
```

### Best Practices

1. **Async/Await**: Use async methods for all I/O operations
2. **Null Safety**: Use nullable reference types and null checks
3. **Exception Handling**: Catch specific exceptions, log errors appropriately
4. **Resource Management**: Use `using` statements for disposable resources
5. **SOLID Principles**: Follow Single Responsibility, Open/Closed, etc.
6. **DRY**: Don't repeat yourself - extract common code
7. **Comments**: Write XML documentation for public APIs

### Naming Conventions

- **Namespaces**: `CCXT.NET.{ExchangeName}`
- **Classes**: `{Exchange}Client`, `{Exchange}PublicApi`
- **Interfaces**: `I{Name}` (e.g., `IXApiClient`)
- **Async Methods**: `{Method}Async` (e.g., `GetTickerAsync`)
- **Private Fields**: `_{fieldName}` (e.g., `_httpClient`)

## Testing Guidelines

### Writing Tests

```csharp
[Fact]
public async Task GetTicker_WithValidSymbol_ReturnsTickerData()
{
    // Arrange
    var client = new BinanceClient("public");
    var symbol = "BTC/USDT";
    
    // Act
    var result = await client.GetTickerAsync(symbol);
    
    // Assert
    Assert.True(result.Success);
    Assert.NotNull(result.Data);
    Assert.Equal(symbol, result.Data.Symbol);
}
```

### Test Categories

- **Unit Tests**: Test individual methods in isolation
- **Integration Tests**: Test API endpoints with real exchanges
- **Performance Tests**: Measure response times and throughput

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test category
dotnet test --filter "Category=Public"

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Pull Request Process

### Before Submitting

1. **Update your branch** with the latest upstream changes:
   ```bash
   git fetch upstream
   git rebase upstream/master
   ```

2. **Run all tests** and ensure they pass:
   ```bash
   dotnet test
   ```

3. **Check code style** and fix any issues:
   ```bash
   dotnet format
   ```

4. **Update documentation** if you've made API changes

### PR Guidelines

1. **Title**: Use clear, descriptive titles
   - ✅ "Add support for Binance futures trading"
   - ❌ "Fixed stuff"

2. **Description**: Include:
   - What changes were made
   - Why they were necessary
   - How to test the changes
   - Related issues (use "Fixes #123")

3. **Commits**: 
   - Use meaningful commit messages
   - Squash minor commits
   - Follow conventional commits format:
     ```
     feat: add Binance futures support
     fix: correct rate limiting for Kraken
     docs: update API documentation
     test: add tests for new endpoints
     ```

4. **Size**: Keep PRs focused and reasonably sized
   - Separate large changes into multiple PRs
   - One feature/fix per PR

### Review Process

1. **Automated Checks**: CI/CD runs tests and code analysis
2. **Code Review**: Maintainers review code quality and design
3. **Testing**: Additional manual testing may be required
4. **Feedback**: Address reviewer comments promptly
5. **Merge**: Once approved, maintainers will merge your PR

## Adding New Exchanges

### Step 1: Create Exchange Structure

```bash
# Create directory structure
mkdir -p src/exchanges/{country_code}/{exchange_name}
mkdir -p src/exchanges/{country_code}/{exchange_name}/private
mkdir -p src/exchanges/{country_code}/{exchange_name}/public
mkdir -p src/exchanges/{country_code}/{exchange_name}/trade
```

### Step 2: Implement Main Client Class

Create `src/exchanges/{country_code}/{exchange_name}/{exchange_name}.cs`:

```csharp
using CCXT.NET.Shared.Coin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCXT.NET.{ExchangeName}
{
    public sealed class {ExchangeName}Client : XApiClient, IXApiClient
    {
        public override string DealerName { get; set; } = "{ExchangeName}";
        
        public {ExchangeName}Client(string division)
            : base(division)
        {
        }
        
        public {ExchangeName}Client(string division, string connect_key, string secret_key)
            : base(division, connect_key, secret_key, authentication: true)
        {
        }
        
        public override ExchangeInfo ExchangeInfo
        {
            get
            {
                // Implementation details
            }
        }
    }
}
```

### Step 3: Implement API Endpoints

Follow the existing pattern for public, private, and trade APIs.

### Step 4: Add Tests

Create corresponding test files in the `tests/` directory.

### Step 5: Update Documentation

Add the exchange to README.md and update the supported exchanges list.

## Documentation

### Code Documentation

Use XML documentation comments for all public APIs:

```csharp
/// <summary>
/// Retrieves the current ticker information for a trading pair
/// </summary>
/// <param name="symbol">Trading pair symbol (e.g., "BTC/USDT")</param>
/// <returns>Ticker information including price, volume, and 24h changes</returns>
/// <exception cref="ArgumentNullException">Thrown when symbol is null</exception>
public async Task<ApiResult<Ticker>> GetTickerAsync(string symbol)
{
    // Implementation
}
```

### README Updates

When adding features or exchanges, update:
- Supported exchanges table
- Feature list
- Usage examples
- API documentation links

### Changelog

Document all changes in `docs/CHANGELOG.md`:
- New features
- Bug fixes
- Breaking changes
- Deprecations

## Community

### Getting Help

- **GitHub Issues**: For bugs and feature requests
- **Email**: help@odinsoft.co.kr
- **Stack Overflow**: Tag questions with `ccxt.net`

### Contributing Beyond Code

- **Documentation**: Improve guides and API docs
- **Examples**: Share usage examples and tutorials
- **Testing**: Report bugs and test pre-releases
- **Translation**: Help translate documentation
- **Support**: Answer questions from other users

## Financial Contributions

If you find CCXT.NET useful, consider supporting the project:

### Cryptocurrency Donations
- **Bitcoin**: `15DAoUfaCanpBpTs7VQBK8dRmbQqEnF9WG`
- **Ethereum**: `0x556E7EdbcCd669a42f00c1Df53D550C00814B0e3`

### Sponsorship

Contact us at help@odinsoft.co.kr for sponsorship opportunities.

## Recognition

### Contributors

All contributors are recognized in our README and release notes. Regular contributors may be invited to join the core team.

### Hall of Fame

Outstanding contributions are featured in our Hall of Fame section.

## License

By contributing to CCXT.NET, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing to CCXT.NET! Your efforts help make cryptocurrency trading accessible to .NET developers worldwide.

**Happy Coding!** 🚀