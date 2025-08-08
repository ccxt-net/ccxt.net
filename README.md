# CCXT.NET - CryptoCurrency eXchange Trading Library for .NET

[![Build status](https://ci.appveyor.com/api/projects/status/dnp9i3t6sexv9tpa?svg=true)](https://ci.appveyor.com/project/ccxt-net/ccxt-net)
[![NuGet Downloads](https://img.shields.io/nuget/dt/ccxt.net.svg)](https://www.nuget.org/packages/CCXT.NET)
[![License](https://img.shields.io/github/license/ccxt-net/ccxt.net.svg)](https://github.com/ccxt-net/ccxt.net/blob/master/LICENSE.txt)

This project is an extension of [ccxt](https://github.com/ccxt/ccxt), a popular cryptocurrency trading library. CCXT.NET provides a unified way to programmatically interact with cryptocurrency exchanges using .NET/C#.

## Features

- 🌍 **Support for 120 exchanges** across 22 countries
- 🔄 **Unified API** across all exchanges
- 📊 **Public market data** (tickers, order books, trades, OHLCV)
- 💰 **Private account management** (balances, orders, trades)
- 🔐 **Secure authentication** handling for each exchange
- ⚡ **Async/await** support for all operations
- 🎯 **Strong typing** with full IntelliSense support
- 📦 **NuGet package** for easy installation

## Supported Cryptocurrency Exchange Markets

The ccxt.net library currently supports **120 cryptocurrency exchange markets** across **22 countries**, organized by ISO 2-letter country codes.

### 🟢 Fully Implemented Exchanges (26)

These exchanges have complete API implementations with full testing:

| Exchange | Country | Type | Documentation |
|----------|---------|------|---------------|
| ANXPro | JP | Spot | [API](https://anxv2.docs.apiary.io) |
| Binance | HK | Spot | [API](https://github.com/binance-exchange/binance-official-api-docs/blob/master/rest-api.md) |
| Bitfinex | GB | Spot | [API](https://bitfinex.readme.io/v1/docs) |
| bitFlyer | JP | Spot | [API](https://lightning.bitflyer.com/docs?lang=en) |
| Bitforex | CN | Spot | [API](https://github.com/bitforexapi/API_Docs/wiki) |
| Bithumb | KR | Spot | [API](https://apidocs.bithumb.com) |
| BitMEX | SC | Futures | [API](https://www.bitmex.com/app/apiOverview) |
| Bitstamp | GB | Spot | [API](https://www.bitstamp.net/api) |
| Bittrex | US | Spot | [API](https://bittrex.github.io/api/) |
| CEX.IO | GB | Spot | [API](https://cex.io/cex-api) |
| Coincheck | JP | Spot | [API](https://coincheck.com/documents/exchange/api) |
| CoinOne | KR | Spot | [API](https://doc.coinone.co.kr) |
| Gate.io | CN | Spot | [API](https://gate.io/api2) |
| GDAX | US | Spot | [API](https://docs.gdax.com) |
| Gemini | US | Spot | [API](https://docs.gemini.com/rest-api) |
| Gopax | KR | Spot | [API](https://www.gopax.co.kr/API) |
| Huobi Pro | CN | Spot | [API](https://github.com/huobiapi/API_Docs/wiki/REST_api_reference) |
| itBit | US | Spot | [API](https://api.itbit.com/docs) |
| Korbit | KR | Spot | [API](https://apidocs.korbit.co.kr/) |
| Kraken | US | Spot | [API](https://www.kraken.com/en-us/help/api) |
| OKCoin KOR | KR | Spot | [API](https://www.okcoinkr.com/api) |
| OKEX | CN | Spot/Futures | [API](https://github.com/okcoin-okex/API-docs-OKEx.com) |
| Poloniex | US | Spot | [API](https://docs.poloniex.com) |
| QUOINEX | JP | Spot | [API](https://developers.quoine.com) |
| Upbit | KR | Spot | [API](https://docs.upbit.com/docs/) |
| ZB | CN | Spot | [API](https://www.zb.com/i/developer) |

### 🟡 Basic Implementation (94)

These exchanges have basic structure implemented and are ready for API integration:

#### Priority 1 - Major Exchanges (To be implemented first)
- **Binance Derivatives**: binancecoinm, binanceusdm (US)
- **Bybit** (CN) - Major derivatives exchange
- **OKX** (CN) - Major global exchange
- **KuCoin** (CN) - Popular altcoin exchange
- **Coinbase** (US) - Leading US exchange
- **Deribit** (AE) - Options/Futures specialist

#### Priority 2 - Regional Leaders
- **Bitso** (MX) - Latin America leader
- **Mercado** (BR) - Brazilian market leader
- **Indodax** (ID) - Indonesian leader
- **Bitbns** (IN) - Indian market
- **Luno** (GB) - UK/Africa focused

#### Priority 3 - Other Exchanges
<details>
<summary>Click to see full list (83 exchanges)</summary>

**Asia-Pacific:**
- CN: bigone, bingx, bitget, coinex, digifinex, gate, hashkey, hitbtc, htx, kucoinfutures, lbank, mexc, woo, woofipro, xt
- JP: bitbank, btcbox, zaif, bittrade
- KR: probit
- SG: bitrue, coinsph, delta, derive, ellipx, hibachi, hyperliquid, independentreserve
- AU: btcmarkets, coinspot
- ID: tokocrypto

**Americas:**
- US: alpaca, apex, ascendex, binanceus, coinbaseadvanced, coinbaseexchange, coinbaseinternational, crypto, cryptocom, krakenfutures, okcoin, okxus, paradex, phemex, vertex
- CA: ndax, timex
- BR: foxbit, novadax

**Europe & Others:**
- EU: bit2c, bitopro, bitvavo, btcalpha, btcturk, coinmate, exmo, onetrading, paymium, wavesexchange, whitebit, yobit, zonda
- GB: bitteam, blockchaincom, coinmetro
- EE: latoken
- LT: cryptomus
- MT: bequant
- KY: bitmart, blofin
- BS: fmfwio

**Global:**
- coincatch, defx, hollaex, myokx, oceanex, oxfun, p2b, tradeogre

</details>

### Implementation Status Summary
- ✅ **Fully Implemented**: 26 exchanges (22%)
- 🚧 **Basic Structure**: 94 exchanges (78%)
- 📊 **Total**: 120 exchanges

The library is updated frequently with new crypto markets, altcoin exchanges, bug fixes, and API endpoints. 
If you need support for a specific exchange that's not currently included, please open an issue on GitHub or contact us via email.

The library is under [MIT license](https://github.com/ccxt-net/ccxt.net/blob/master/LICENSE.txt), that means it's absolutely free for any developer to build commercial and opensource software on top of it, but use it at your own risk with no warranties, as is.


## Installation

### NuGet Package

```bash
# Package Manager Console
Install-Package CCXT.NET -Version 1.5.2

# .NET CLI
dotnet add package CCXT.NET --version 1.5.2

# PackageReference
<PackageReference Include="CCXT.NET" Version="1.5.2" />
```

### Clone Repository

```bash
git clone https://github.com/ccxt-net/ccxt.net.git
cd ccxt.net
dotnet build
```

## Quick Start

```csharp
using CCXT.NET.Binance;

// Create client for public API
var publicClient = new BinanceClient("public");

// Get ticker
var ticker = await publicClient.FetchTickerAsync("BTC/USDT");
Console.WriteLine($"BTC/USDT Price: {ticker.result.close}");

// Create client for private API (requires API keys)
var privateClient = new BinanceClient("private", "YOUR_API_KEY", "YOUR_SECRET_KEY");

// Get account balance
var balance = await privateClient.FetchBalanceAsync();
```

## Documentation

- 📚 [Wiki](https://github.com/ccxt-net/ccxt.net/wiki) - Detailed documentation
- 📝 [API Reference](https://github.com/ccxt-net/ccxt.net/wiki/API-Reference) - Complete API documentation
- 🚀 [Examples](https://github.com/ccxt-net/ccxt.net/tree/master/samples) - Sample code

## Releases

- 📦 [NuGet Package](https://www.nuget.org/packages/CCXT.NET)
- 🏷️ [GitHub Releases](https://github.com/ccxt-net/ccxt.net/releases)
- 📋 [Changelog](https://github.com/ccxt-net/ccxt.net/blob/master/docs/CHANGELOG.md)

## Contributing

We welcome contributions! Please read our [Contributing Guidelines](https://github.com/ccxt-net/ccxt.net/blob/master/docs/CONTRIBUTING.md) for details on:
- How to submit issues
- How to contribute code
- Coding standards
- Testing requirements


## Support

### Need Help?
- 📖 Check the [Wiki](https://github.com/ccxt-net/ccxt.net/wiki)
- 🐛 Report issues on [GitHub](https://github.com/ccxt-net/ccxt.net/issues)
- 📧 Contact us at help@odinsoft.co.kr
- 🌐 Visit [ODINSOFT](https://www.odinsoft.co.kr)

### Support the Project
If you find CCXT.NET useful, consider supporting development:

**Cryptocurrency Donations:**
- **Bitcoin**: `15DAoUfaCanpBpTs7VQBK8dRmbQqEnF9WG`
- **Ethereum**: `0x556E7EdbcCd669a42f00c1Df53D550C00814B0e3`

## 👥 Team

### **Core Development Team**
- **SEONGAHN** - Lead Developer & Project Architect ([lisa@odinsoft.co.kr](mailto:lisa@odinsoft.co.kr))
- **YUJIN** - Senior Developer & Exchange Integration Specialist ([yoojin@odinsoft.co.kr](mailto:yoojin@odinsoft.co.kr))
- **SEJIN** - Software Developer & API Implementation ([saejin@odinsoft.co.kr](mailto:saejin@odinsoft.co.kr))

---

**Built with ❤️ by the ODINSOFT Team**
