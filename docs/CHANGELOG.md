# Сhangelog

## Version 1.5.2 2025-08-08

### Major Exchange Reorganization
- **Country Code Standardization**: Migrated from 3-letter to 2-letter ISO codes
  - `chn` → `cn` (China)
  - `gbr` → `gb` (United Kingdom)  
  - `jpn` → `jp` (Japan)
  - `kor` → `kr` (Korea)
  - `sey` → `sc` (Seychelles)
  - `usa` → `us` (United States)

- **New Exchange Implementations**: Added 94 new exchanges across 22 countries
  - Total supported exchanges increased from 26 to 120
  - All exchanges follow standardized folder structure:
    ```
    {country_code}/{exchange_name}/
    ├── {exchange_name}.cs (main client class)
    ├── private/ (authentication required endpoints)
    ├── public/  (public API endpoints)
    └── trade/   (trading operations)
    ```

- **New Exchanges by Region**:
  - **AE (UAE)**: deribit
  - **AU (Australia)**: btcmarkets, coinspot
  - **BR (Brazil)**: foxbit, mercado, novadax
  - **BS (Bahamas)**: fmfwio
  - **CA (Canada)**: ndax, timex
  - **CN (China)**: bigone, bingx, bitget, bybit, coinex, digifinex, gate, hashkey, hitbtc, htx, kucoin, kucoinfutures, lbank, mexc, okx, woo, woofipro, xt
  - **EE (Estonia)**: latoken
  - **EU (Europe)**: bit2c, bitopro, bitvavo, btcalpha, btcturk, coinmate, exmo, onetrading, paymium, wavesexchange, whitebit, yobit, zonda
  - **GB (UK)**: bitteam, blockchaincom, coinmetro, luno
  - **GLOBAL**: coincatch, defx, hollaex, myokx, oceanex, oxfun, p2b, tradeogre
  - **ID (Indonesia)**: indodax, tokocrypto
  - **IN (India)**: bitbns, modetrade
  - **JP (Japan)**: bitbank, btcbox, zaif, bittrade
  - **KR (Korea)**: probit
  - **KY (Cayman Islands)**: bitmart, blofin
  - **LT (Lithuania)**: cryptomus
  - **MT (Malta)**: bequant
  - **MX (Mexico)**: bitso
  - **SG (Singapore)**: bitrue, coinsph, delta, derive, ellipx, hibachi, hyperliquid, independentreserve
  - **US (United States)**: alpaca, apex, ascendex, binancecoinm, binanceus, binanceusdm, binance, coinbaseadvanced, coinbaseexchange, coinbaseinternational, coinbase, crypto, cryptocom, krakenfutures, okcoin, okxus, paradex, phemex, vertex

### Technical Improvements
- Fixed namespace conflicts and build errors
- Updated to support .NET 8.0 and .NET 9.0
- Standardized exchange implementation patterns
- All exchanges inherit from `CCXT.NET.Shared.Coin.XApiClient`
- Implement `IXApiClient` interface with proper authentication flow

### Namespace Convention Standardization
- **Namespace Pattern**: All exchanges now use `CCXT.NET.{ExchangeName}` without country codes
- **Directory Structure**: Maintains `src/exchanges/{country_code}/{exchange_name}/` for organization
- **Important Changes**:
  - Removed country codes from all namespaces (e.g., `CCXT.NET.US.Binance` → `CCXT.NET.Binance`)
  - Fixed 94 exchange files to follow the new convention
  - Ensures no naming conflicts between exchanges
  - Simplifies exchange references in code
  
- **Special Naming Cases**:
  - All caps: `OKX`, `OKEX`, `GDAX`, `CEX`
  - Mixed case: `BTCMarkets`, `BinanceUS`, `BinanceCOINM`, `BinanceUSDM`
  - Special: `CryptoCom`, `BlockchainCom`

- **Benefits**:
  - No naming conflicts regardless of exchange location
  - Easier to reference exchanges without knowing their country
  - Aligns with how developers think about exchanges (by name, not location)
  - Prevents issues when exchanges operate in multiple countries

## Version 1.5.1 2024-09-19

	- Bump RestSharp from 108.0.3 to 112.0.0
	- The substring array trading error on Gemini
	- Bump System.IdentityModel.Tokens.Jwt from 6.24.0 to 6.34.0
	
## Version 1.4.13 2022-05-25

	- RestSharp update and upgrade to .net core 6.

## Version 1.3.8 2020-11-17

	- zech Culture Decimal Parse Error Fix update #16

## Version 1.3.4 2019-06-26

	- add exchanges gopax, okcoinkr
	- rename namespace OdinSdk to CCXT.NET

## Version 1.3.3 2019-03-28

	- Merge with OdinSdk.BaseLib
	- I will change the namespace OdinSdk to CCXT.NET later.

## Version 1.3.2 2019-03-18

	- New Exchanges
		. gateio,huobipro,okex,zb,bitfinex,bitstamp,cex,anxpro,binance
		. bitflyer,coincheck,quoinex,upbit,bittrex,gdax,gemini,itbit,kraken