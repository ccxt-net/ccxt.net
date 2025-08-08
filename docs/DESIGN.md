# CCXT.NET Exchange Implementation Standard

## Overview
This document defines the standard interface and implementation requirements that all 120 exchanges in CCXT.NET must follow. This ensures consistency, maintainability, and ease of use across all supported exchanges.

## Core Requirements

### 1. Namespace and Class Structure
```csharp
namespace CCXT.NET.{ExchangeName}
{
    public sealed class {ExchangeName}Client : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        public override string DealerName { get; set; } = "{ExchangeName}";
        
        // Constructor for public API (no authentication)
        public {ExchangeName}Client(string division) : base(division) { }
        
        // Constructor for private API (with authentication)
        public {ExchangeName}Client(string division, string connect_key, string secret_key)
            : base(division, connect_key, secret_key, authentication: true) { }
    }
}
```

### 2. Folder Structure
```
exchanges/{country_code}/{exchange_name}/
├── {exchange_name}.cs          # Main client class
├── public/                      # Public API implementations
│   ├── publicApi.cs            # Base public API class
│   ├── ticker.cs               # Price ticker endpoints
│   ├── orderBook.cs            # Order book endpoints
│   ├── completeOrder.cs        # Trade history endpoints
│   ├── market.cs               # Market information endpoints
│   └── ohlcv.cs               # Candlestick data endpoints
├── private/                     # Private API implementations
│   ├── privateApi.cs           # Base private API class
│   ├── balance.cs              # Balance endpoints
│   ├── address.cs              # Deposit address endpoints
│   ├── transfer.cs             # Transfer/withdrawal endpoints
│   └── wallet.cs               # Wallet management endpoints
└── trade/                       # Trading API implementations
    ├── tradeApi.cs             # Base trade API class
    ├── order.cs                # Order management endpoints
    ├── place.cs                # Order placement endpoints
    ├── trade.cs                # Trade history endpoints
    └── position.cs             # Position management (futures)
```

## Standard API Functions

### Public API (No Authentication Required)

#### Market Data Functions
| Function | Description | Required | Return Type |
|----------|-------------|----------|-------------|
| `FetchMarketsAsync()` | Get all available trading pairs | ✅ Yes | `Markets` |
| `FetchTickerAsync(base, quote)` | Get current price for a pair | ✅ Yes | `Ticker` |
| `FetchTickersAsync()` | Get prices for all pairs | ✅ Yes | `Tickers` |
| `FetchOrderBooksAsync(base, quote, limits)` | Get order book depth | ✅ Yes | `OrderBooks` |
| `FetchOHLCVsAsync(base, quote, timeframe, since, limits)` | Get candlestick data | ✅ Yes | `OHLCVs` |
| `FetchCompleteOrdersAsync(base, quote, timeframe, since, limits)` | Get recent trades | ✅ Yes | `CompleteOrders` |

### Private API (Authentication Required)

#### Account Management Functions
| Function | Description | Required | Return Type |
|----------|-------------|----------|-------------|
| `FetchBalanceAsync(base, quote)` | Get specific balance | ✅ Yes | `Balance` |
| `FetchBalancesAsync()` | Get all balances | ✅ Yes | `Balances` |
| `FetchAddressAsync(currency)` | Get deposit address | ✅ Yes | `Address` |
| `FetchAddressesAsync()` | Get all deposit addresses | ✅ Yes | `Addresses` |
| `CreateAddressAsync(currency)` | Create new deposit address | ⚠️ Optional | `Address` |

#### Transfer Functions
| Function | Description | Required | Return Type |
|----------|-------------|----------|-------------|
| `CoinWithdrawAsync(currency, address, tag, quantity)` | Withdraw cryptocurrency | ✅ Yes | `Transfer` |
| `CancelCoinWithdrawAsync(currency, transferId)` | Cancel withdrawal | ⚠️ Optional | `Transfer` |
| `FetchTransfersAsync(currency, timeframe, since, limits)` | Get transfer history | ✅ Yes | `Transfers` |
| `FetchAllTransfersAsync(timeframe, since, limits)` | Get all transfers | ✅ Yes | `Transfers` |

### Trade API (Authentication Required)

#### Order Management Functions
| Function | Description | Required | Return Type |
|----------|-------------|----------|-------------|
| `CreateLimitOrderAsync(base, quote, quantity, price, side)` | Create limit order | ✅ Yes | `MyOrder` |
| `CreateMarketOrderAsync(base, quote, quantity, price, side)` | Create market order | ✅ Yes | `MyOrder` |
| `CancelOrderAsync(base, quote, orderId, quantity, price, side)` | Cancel order | ✅ Yes | `MyOrder` |
| `CancelOrdersAsync(base, quote, orderIds)` | Cancel multiple orders | ⚠️ Optional | `MyOrders` |
| `CancelAllOrdersAsync()` | Cancel all orders | ⚠️ Optional | `MyOrders` |

#### Order Query Functions
| Function | Description | Required | Return Type |
|----------|-------------|----------|-------------|
| `FetchMyOrderAsync(base, quote, orderId)` | Get specific order | ✅ Yes | `MyOrder` |
| `FetchMyOrdersAsync(base, quote, timeframe, since, limits)` | Get order history | ✅ Yes | `MyOrders` |
| `FetchOpenOrdersAsync(base, quote)` | Get open orders | ✅ Yes | `MyOrders` |
| `FetchAllOpenOrdersAsync()` | Get all open orders | ✅ Yes | `MyOrders` |
| `FetchMyTradesAsync(base, quote, timeframe, since, limits)` | Get trade history | ✅ Yes | `MyTrades` |

### Futures/Derivatives (Optional)
| Function | Description | Required | Return Type |
|----------|-------------|----------|-------------|
| `FetchOpenPositionsAsync(base, quote)` | Get open positions | ⚠️ Optional | `MyPositions` |
| `FetchAllOpenPositionsAsync()` | Get all positions | ⚠️ Optional | `MyPositions` |

## Implementation Guidelines

### 1. Return Type Pattern
All API methods must return `ValueTask<ApiResult<T>>` where:
- `ApiResult<T>` wraps the actual result with success/error information
- `T` is the specific return type (e.g., `Ticker`, `Balance`, `MyOrder`)

### 2. Parameter Conventions
```csharp
// Standard parameter names
string base_name      // Base currency (e.g., "BTC")
string quote_name     // Quote currency (e.g., "USDT")
string currency_name  // Single currency (e.g., "ETH")
string timeframe      // Time period (e.g., "1h", "1d")
long since           // Unix timestamp in milliseconds
int limits           // Number of results to return
SideType sideType    // Buy or Sell enum
Dictionary<string, object> args  // Exchange-specific parameters
```

### 3. Error Handling
```csharp
public async ValueTask<Ticker> FetchTickerAsync(string base_name, string quote_name)
{
    var _result = new Ticker(base_name, quote_name);
    
    try
    {
        // Load market information
        var _market = await LoadMarketAsync($"{base_name}/{quote_name}");
        if (!_market.success)
            return _result.SetResult(_market);
        
        // Make API call
        var _response = await CallApiAsync("/ticker", parameters);
        
        // Parse and return result
        return ParseTicker(_response);
    }
    catch (Exception ex)
    {
        return _result.SetError(ex.Message);
    }
}
```

### 4. Authentication
```csharp
public class ExchangePublicApi : XApiClient
{
    // Public API - no authentication needed
    public ExchangePublicApi(string connect_key = "", string secret_key = "")
        : base("exchange", connect_key, secret_key, authentication: false)
    {
    }
}

public class ExchangePrivateApi : XApiClient
{
    // Private API - authentication required
    public ExchangePrivateApi(string connect_key, string secret_key)
        : base("exchange", connect_key, secret_key, authentication: true)
    {
    }
}
```

### 5. Market Loading Pattern
```csharp
// All functions should load market data first
var _market = await LoadMarketAsync($"{base_name}/{quote_name}");
if (!_market.success)
    return _result.SetResult(_market);

// Convert to exchange-specific IDs
var _base_id = await LoadCurrencyIdAsync(base_name);
var _quote_id = await LoadCurrencyIdAsync(quote_name);
```

### 6. Rate Limiting
```csharp
// Respect exchange rate limits
await ApiCallWait(TradeType.Public);  // For public API
await ApiCallWait(TradeType.Private); // For private API
await ApiCallWait(TradeType.Trade);   // For trade API
```

## Testing Requirements

### 1. Public API Tests
- ✅ Test all market data functions with valid pairs
- ✅ Test error handling with invalid pairs
- ✅ Verify data format and field presence
- ✅ Check rate limiting compliance

### 2. Private API Tests (when API keys available)
- ✅ Test balance fetching
- ✅ Test address generation/fetching
- ✅ Test transfer history retrieval
- ✅ Verify authentication errors with invalid keys

### 3. Trade API Tests (testnet preferred)
- ✅ Test order creation (limit and market)
- ✅ Test order cancellation
- ✅ Test order status queries
- ✅ Test trade history retrieval

## Compliance Checklist

Before marking an exchange as "fully implemented", ensure:

- [ ] All required public API functions implemented
- [ ] All required private API functions implemented
- [ ] All required trade API functions implemented
- [ ] Proper error handling in all functions
- [ ] Rate limiting implemented
- [ ] Authentication working correctly
- [ ] Market loading pattern followed
- [ ] Return types match standard
- [ ] Parameter naming conventions followed
- [ ] Basic tests passing
- [ ] Documentation updated

## Exchange-Specific Extensions

Exchanges may implement additional functions beyond the standard, but must:
1. Implement all required standard functions first
2. Document exchange-specific functions clearly
3. Use the `args` parameter for optional features
4. Maintain backward compatibility

## Version Compatibility

- **Minimum .NET Version**: .NET Standard 2.1
- **Supported Frameworks**: .NET 8.0, .NET 9.0
- **Required Dependencies**:
  - Newtonsoft.Json 13.0.3+
  - RestSharp 112.1.0+
  - System.IdentityModel.Tokens.Jwt 8.13.0+

## Support

For questions about implementing the standard:
- Email: help@odinsoft.co.kr
- GitHub Issues: https://github.com/ccxt-net/ccxt.net/issues
- Documentation: https://github.com/ccxt-net/ccxt.net/wiki

---

**Last Updated**: January 2025  
**Standard Version**: 1.0.0