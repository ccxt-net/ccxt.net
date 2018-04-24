# CCXT.NET ? CryptoCurrency eXchange Trading Library For .NET

[![Build status](https://ci.appveyor.com/api/projects/status/dnp9i3t6sexv9tpa?svg=true)](https://ci.appveyor.com/project/lisa3907/ccxt.net)

## Supported Cryptocurrency Exchange Markets

The ccxt library currently supports the following 14 cryptocurrency exchange markets and trading APIs:

|                                                                                                                           | id                 | name                                                      | ver | doc                                                                                          | countries                               |
|---------------------------------------------------------------------------------------------------------------------------|--------------------|-----------------------------------------------------------|:---:|:--------------------------------------------------------------------------------------------:|-----------------------------------------|
|![binance](https://user-images.githubusercontent.com/1294454/29604020-d5483cdc-87ee-11e7-94c7-d1a8d9169293.jpg)            | binance            | [Binance](https://www.binance.com)                        | *   | [API](https://github.com/binance-exchange/binance-official-api-docs/blob/master/rest-api.md) | Japan                                   |
|![bitfinex](https://user-images.githubusercontent.com/1294454/27766244-e328a50c-5ed2-11e7-947b-041416579bb3.jpg)           | bitfinex           | [Bitfinex](https://www.bitfinex.com)                      | 1   | [API](https://bitfinex.readme.io/v1/docs)                                                    | British Virgin Islands                  |
|![bitflyer](https://user-images.githubusercontent.com/1294454/28051642-56154182-660e-11e7-9b0d-6042d1e6edd8.jpg)           | bitflyer           | [bitFlyer](https://bitflyer.jp)                           | 1   | [API](https://bitflyer.jp/API)                                                               | Japan                                   |
|![bithumb](https://user-images.githubusercontent.com/1294454/30597177-ea800172-9d5e-11e7-804c-b9d4fa9b56b0.jpg)            | bithumb            | [Bithumb](https://www.bithumb.com)                        | *   | [API](https://www.bithumb.com/u1/US127)                                                      | South Korea                             |
|![bitmex](https://user-images.githubusercontent.com/1294454/27766319-f653c6e6-5ed4-11e7-933d-f0bc3699ae8f.jpg)             | bitmex             | [BitMEX](https://www.bitmex.com)                          | 1   | [API](https://www.bitmex.com/app/apiOverview)                                                | Seychelles                              |
|![bitstamp](https://user-images.githubusercontent.com/1294454/27786377-8c8ab57e-5fe9-11e7-8ea4-2b05b6bcceec.jpg)           | bitstamp           | [Bitstamp](https://www.bitstamp.net)                      | 2   | [API](https://www.bitstamp.net/api)                                                          | UK                                      |
|![bittrex](https://user-images.githubusercontent.com/1294454/27766352-cf0b3c26-5ed5-11e7-82b7-f3826b7a97d8.jpg)            | bittrex            | [Bittrex](https://bittrex.com)                            | 1.1 | [API](https://bittrex.com/Home/Api)                                                          | US                                      |
|![coinone](https://user-images.githubusercontent.com/1294454/38003300-adc12fba-323f-11e8-8525-725f53c4a659.jpg)            | coinone            | [CoinOne](https://coinone.co.kr)                          | 2   | [API](https://doc.coinone.co.kr)                                                             | South Korea                             |
|![gdax](https://user-images.githubusercontent.com/1294454/27766527-b1be41c6-5edb-11e7-95f6-5b496c469e2c.jpg)               | gdax               | [GDAX](https://www.gdax.com)                              | *   | [API](https://docs.gdax.com)                                                                 | US                                      |
|![gemini](https://user-images.githubusercontent.com/1294454/27816857-ce7be644-6096-11e7-82d6-3c257263229c.jpg)             | gemini             | [Gemini](https://gemini.com)                              | 1   | [API](https://docs.gemini.com/rest-api)                                                      | US                                      |
|![itbit](https://user-images.githubusercontent.com/1294454/27822159-66153620-60ad-11e7-89e7-005f6d7f3de0.jpg)              | itbit              | [itBit](https://www.itbit.com)                            | 1   | [API](https://api.itbit.com/docs)                                                            | US                                      |
|![kraken](https://user-images.githubusercontent.com/1294454/27766599-22709304-5ede-11e7-9de1-9f33732e1509.jpg)             | kraken             | [Kraken](https://www.kraken.com)                          | 0   | [API](https://www.kraken.com/en-us/help/api)                                                 | US                                      |
|![poloniex](https://user-images.githubusercontent.com/1294454/27766817-e9456312-5ee6-11e7-9b3c-b628ca5626a5.jpg)           | poloniex           | [Poloniex](https://poloniex.com)                          | *   | [API](https://poloniex.com/support/api/)                                                     | US                                      |
|![quoinex](https://user-images.githubusercontent.com/1294454/35047114-0e24ad4a-fbaa-11e7-96a9-69c1a756083b.jpg)            | quoinex            | [QUOINEX](https://quoinex.com/)                           | 2   | [API](https://developers.quoine.com)                                                         | Japan, Singapore, Vietnam               |

The list above is updated frequently, new crypto markets, altcoin exchanges, bug fixes, API endpoints are introduced and added on a regular basis. 
If you don't find a cryptocurrency exchange market in the list above and/or want another exchange to be added, post or send us a link to it by opening an issue here on GitHub or via email.

The library is under [MIT license](https://github.com/ccxt.net/blob/master/LICENSE.txt), that means it's absolutely free for any developer to build commercial and opensource software on top of it, but use it at your own risk with no warranties, as is.

## Install

You can also clone it into your project directory from [ccxt GitHub repository](https://github.com/lisa3907/ccxt.net):

```shell
git clone https://github.com/lisa3907/ccxt.net.git
```


## Documentation

Read the [Manual](https://github.com/lisa3907/ccxt.net/wiki) for more details.

## Usage

### Intro

The ccxt library consists of a public part and a private part. Anyone can use the public part out-of-the-box immediately after installation. Public APIs open access to public information from all exchange markets without registering user accounts and without having API keys.

Public APIs include the following:

- market data
- instruments/trading pairs
- price feeds (exchange rates)
- order books
- trade history
- tickers
- OHLC(V) for charting
- other public endpoints

For trading with private APIs you need to obtain API keys from/to exchange markets. It often means registering with exchanges and creating API keys with your account. Most exchanges require personal info or identification. Some kind of verification may be necessary as well. If you want to trade you need to register yourself, this library will not create accounts or API keys for you. Some exchange APIs expose interface methods for registering an account from within the code itself, but most of exchanges don't. You have to sign up and create API keys with their websites.

Private APIs allow the following:

- manage personal account info
- query account balances
- trade by making market and limit orders
- deposit and withdraw fiat and crypto funds
- query personal orders
- get ledger history
- transfer funds between accounts
- use merchant services

This library implements full public and private REST APIs for all exchanges. WebSocket and FIX implementations in JavaScript, PHP, Python and other languages coming soon.

The ccxt library supports both camelcase notation (preferred in JavaScript) and underscore notation (preferred in Python and PHP), therefore all methods can be called in either notation or coding style in any language.

```
// both of these notations work in JavaScript/Python/PHP
exchange.methodName ()  // camelcase pseudocode
exchange.method_name () // underscore pseudocode
```

Read the [Manual](https://github.com/lisa3907/ccxt.net/wiki) for more details.


## Contributing

Please read the [CONTRIBUTING](https://github.com/lisa3907/ccxt.net/blob/master/CONTRIBUTING.md) document before making changes that you would like adopted in the code. Also, read the [Manual](https://github.com/lisa3907/ccxt.net/wiki) for more details.

## Support Developer Team

We are investing a significant amount of time into the development of this library. If CCXT made your life easier and you like it and want to help us improve it further or if you want to speed up new features and exchanges, please, support us with a tip. We appreciate all contributions!

### Sponsors

Support this project by becoming a sponsor. Your logo will show up here with a link to your website.

[[Become a sponsor](https://opencollective.com/ccxt.net#sponsor)]

<a href="https://opencollective.com/ccxt.net/sponsor/0/website" target="_blank"><img src="https://opencollective.com/ccxt.net/sponsor/0/avatar.svg"></a>
<a href="https://opencollective.com/ccxt.net/sponsor/1/website" target="_blank"><img src="https://opencollective.com/ccxt.net/sponsor/1/avatar.svg"></a>
<a href="https://opencollective.com/ccxt.net/sponsor/2/website" target="_blank"><img src="https://opencollective.com/ccxt.net/sponsor/2/avatar.svg"></a>
<a href="https://opencollective.com/ccxt.net/sponsor/3/website" target="_blank"><img src="https://opencollective.com/ccxt.net/sponsor/3/avatar.svg"></a>
<a href="https://opencollective.com/ccxt.net/sponsor/4/website" target="_blank"><img src="https://opencollective.com/ccxt.net/sponsor/4/avatar.svg"></a>
<a href="https://opencollective.com/ccxt.net/sponsor/5/website" target="_blank"><img src="https://opencollective.com/ccxt.net/sponsor/5/avatar.svg"></a>
<a href="https://opencollective.com/ccxt.net/sponsor/6/website" target="_blank"><img src="https://opencollective.com/ccxt.net/sponsor/6/avatar.svg"></a>
<a href="https://opencollective.com/ccxt.net/sponsor/7/website" target="_blank"><img src="https://opencollective.com/ccxt.net/sponsor/7/avatar.svg"></a>
<a href="https://opencollective.com/ccxt.net/sponsor/8/website" target="_blank"><img src="https://opencollective.com/ccxt.net/sponsor/8/avatar.svg"></a>
<a href="https://opencollective.com/ccxt.net/sponsor/9/website" target="_blank"><img src="https://opencollective.com/ccxt.net/sponsor/9/avatar.svg"></a>

### Backers

Thank you to all our backers! [[Become a backer](https://opencollective.com/ccxt.net#backer)]

<a href="https://opencollective.com/ccxt.net#backers" target="_blank"><img src="https://opencollective.com/ccxt.net/backers.svg?width=890"></a>

### Crypto

```
ETH 0x556E7EdbcCd669a42f00c1Df53D550C00814B0e3
BTC 15DAoUfaCanpBpTs7VQBK8dRmbQqEnF9WG
```
