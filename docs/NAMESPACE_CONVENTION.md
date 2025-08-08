# Namespace Convention

## Overview
All exchange implementations in CCXT.NET follow a consistent namespace pattern that does NOT include country codes.

## Namespace Pattern
```csharp
namespace CCXT.NET.{ExchangeName}
```

## Important Notes
- **NO Country Codes**: Namespaces should NOT include country codes (e.g., NOT `CCXT.NET.US.Binance`)
- **Exchange Name Only**: Use only the exchange name in the namespace (e.g., `CCXT.NET.Binance`)
- **Consistent Casing**: Use proper PascalCase for exchange names

## Examples

### Correct ✅
```csharp
namespace CCXT.NET.Binance
namespace CCXT.NET.Bithumb
namespace CCXT.NET.Kraken
namespace CCXT.NET.OKX
namespace CCXT.NET.Bybit
```

### Incorrect ❌
```csharp
namespace CCXT.NET.US.binance  // Don't include country code
namespace CCXT.NET.KR.Bithumb  // Don't include country code
namespace CCXT.NET.CN.okx      // Don't include country code
```

## Directory Structure vs Namespace
While the directory structure includes country codes for organization:
```
src/exchanges/{country_code}/{exchange_name}/
```

The namespace does NOT include the country code:
```csharp
// File: src/exchanges/kr/bithumb/bithumb.cs
namespace CCXT.NET.Bithumb  // No 'KR' in namespace
```

## Special Naming Cases
Some exchanges require special casing:
- `OKX` (all caps)
- `OKEX` (all caps)
- `GDAX` (all caps)
- `CEX` (all caps)
- `BTCMarkets` (mixed case)
- `BinanceUS` (for Binance US variant)
- `BinanceCOINM` (for Binance COIN-M futures)
- `BinanceUSDM` (for Binance USD-M futures)
- `CryptoCom` (for Crypto.com)
- `BlockchainCom` (for Blockchain.com)

## Rationale
This convention ensures:
1. **No Naming Conflicts**: Each exchange has a unique namespace regardless of location
2. **Simplicity**: Easier to reference exchanges without knowing their country
3. **Consistency**: Aligns with how users think about exchanges (by name, not location)
4. **Compatibility**: Prevents issues when exchanges operate in multiple countries