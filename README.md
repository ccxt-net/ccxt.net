# CCXT.NET - CryptoCurrency eXchange Trading Library for .NET

[![Build status](https://ci.appveyor.com/api/projects/status/dnp9i3t6sexv9tpa?svg=true)](https://ci.appveyor.com/project/lisa3907/ccxt-net)

This project is an extension of [ccxt](https://github.com/ccxt/ccxt).

If you are a .Net C# programmer, ccxt.net might be useful. We created ccxt.net using open-source [ccxt](https://github.com/ccxt/ccxt).

This project started in 2018/04. I plan to update it continuously.


## Supported Cryptocurrency Exchange Markets

The ccxt.net library currently supports the following 14 cryptocurrency exchange markets and trading APIs:

|&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;logo&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                                                                                                     | id                 | name                                                                                 | ver   | doc                                                                                              | certified                                                                                                                  |
|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------|--------------------------------------------------------------------------------------|:-----:|:------------------------------------------------------------------------------------------------:|----------------------------------------------------------------------------------------------------------------------------|
|[![binance](https://user-images.githubusercontent.com/1294454/29604020-d5483cdc-87ee-11e7-94c7-d1a8d9169293.jpg)](https://www.binance.com/?ref=10205187)									  | binance			   | [Binance](https://www.binance.com/?ref=10205187) | *   | [API](https://github.com/binance-exchange/binance-official-api-docs/blob/master/rest-api.md) | [![CCXT Certified](https://img.shields.io/badge/CCXT-certified-green.svg)](https://github.com/ccxt/ccxt/wiki/Certification) | Japan                 |
|[![bitfinex](https://user-images.githubusercontent.com/1294454/27766244-e328a50c-5ed2-11e7-947b-041416579bb3.jpg)](https://www.bitfinex.com)												  | bitfinex		   | [Bitfinex](https://www.bitfinex.com)             | 1   | [API](https://bitfinex.readme.io/v1/docs)                                                    | [![CCXT Certified](https://img.shields.io/badge/CCXT-certified-green.svg)](https://github.com/ccxt/ccxt/wiki/Certification) | British Virgin Islands|
|[![bitflyer](https://user-images.githubusercontent.com/1294454/28051642-56154182-660e-11e7-9b0d-6042d1e6edd8.jpg)](https://bitflyer.jp)													  | bitflyer           | [bitFlyer](https://bitflyer.jp)                                                      | 1     | [API](https://lightning.bitflyer.com/docs?lang=en)                                               |                                                                                                                             | Japan                                   |
|[![bithumb](https://user-images.githubusercontent.com/1294454/30597177-ea800172-9d5e-11e7-804c-b9d4fa9b56b0.jpg)](https://www.bithumb.com)                                                   | bithumb            | [Bithumb](https://www.bithumb.com)                                                   | *     | [API](https://apidocs.bithumb.com)                                                               |                                                                                                                             | South Korea                             |
|[![bitmex](https://user-images.githubusercontent.com/1294454/27766319-f653c6e6-5ed4-11e7-933d-f0bc3699ae8f.jpg)](https://www.bitmex.com/register/rm3C16)                                     | bitmex             | [BitMEX](https://www.bitmex.com/register/rm3C16)                                     | 1     | [API](https://www.bitmex.com/app/apiOverview)                                                    |                                                                                                                             | Seychelles                              |
|[![bitstamp](https://user-images.githubusercontent.com/1294454/27786377-8c8ab57e-5fe9-11e7-8ea4-2b05b6bcceec.jpg)](https://www.bitstamp.net)                                                 | bitstamp           | [Bitstamp](https://www.bitstamp.net)                                                 | 2     | [API](https://www.bitstamp.net/api)                                                              |                                                                                                                             | UK                                      |
|[![bittrex](https://user-images.githubusercontent.com/1294454/27766352-cf0b3c26-5ed5-11e7-82b7-f3826b7a97d8.jpg)](https://bittrex.com)														  | bittrex			   | [Bittrex](https://bittrex.com)                   | 1.1 | [API](https://bittrex.github.io/api/)                                                        | [![CCXT Certified](https://img.shields.io/badge/CCXT-certified-green.svg)](https://github.com/ccxt/ccxt/wiki/Certification) | US                    |
|[![cex](https://user-images.githubusercontent.com/1294454/27766442-8ddc33b0-5ed8-11e7-8b98-f786aef0f3c9.jpg)](https://cex.io/r/0/up105393824/0/)                                             | cex                | [CEX.IO](https://cex.io/r/0/up105393824/0/)                                          | *     | [API](https://cex.io/cex-api)                                                                    |                                                                                                                             | UK, EU, Cyprus, Russia                  |
|[![coinone](https://user-images.githubusercontent.com/1294454/38003300-adc12fba-323f-11e8-8525-725f53c4a659.jpg)](https://coinone.co.kr)                                                     | coinone            | [CoinOne](https://coinone.co.kr)                                                     | 2     | [API](https://doc.coinone.co.kr)                                                                 |                                                                                                                             | South Korea                             |
|[![gdax](https://user-images.githubusercontent.com/1294454/27766527-b1be41c6-5edb-11e7-95f6-5b496c469e2c.jpg)](https://www.gdax.com)                                                         | gdax               | [GDAX](https://www.gdax.com)                                                         | *     | [API](https://docs.gdax.com)                                                                     |                                                                                                                             | US                                      |
|[![gemini](https://user-images.githubusercontent.com/1294454/27816857-ce7be644-6096-11e7-82d6-3c257263229c.jpg)](https://gemini.com)                                                         | gemini             | [Gemini](https://gemini.com)                                                         | 1     | [API](https://docs.gemini.com/rest-api)                                                          |                                                                                                                             | US                                      |
|[![itbit](https://user-images.githubusercontent.com/1294454/27822159-66153620-60ad-11e7-89e7-005f6d7f3de0.jpg)](https://www.itbit.com)                                                       | itbit              | [itBit](https://www.itbit.com)                                                       | 1     | [API](https://api.itbit.com/docs)                                                                |                                                                                                                             | US                                      |
|[![korbit](https://github.com/lisa3907/ccxt.net/blob/master/logo-files/favicon-korbit-16x16.png?raw=true)](https://www.korbit.com)															  | korbit             | [Korbit](https://www.korbit.co.kr/)												  | 1     | [API](https://apidocs.korbit.co.kr/)															 |																															   | South Korea                             |
|[![kraken](https://user-images.githubusercontent.com/1294454/27766599-22709304-5ede-11e7-9de1-9f33732e1509.jpg)](https://www.kraken.com)                                                     | kraken             | [Kraken](https://www.kraken.com)                                                     | 0     | [API](https://www.kraken.com/en-us/help/api)                                                     | [![CCXT Certified](https://img.shields.io/badge/CCXT-certified-green.svg)](https://github.com/ccxt/ccxt/wiki/Certification) | US                                      |
|[![okex](https://user-images.githubusercontent.com/1294454/32552768-0d6dd3c6-c4a6-11e7-90f8-c043b64756a7.jpg)](https://www.okex.com)                                                         | okex               | [OKEX](https://www.okex.com)                                                         | 1     | [API](https://github.com/okcoin-okex/API-docs-OKEx.com)                                          |                                                                                                                             | China, US                               |
|[![poloniex](https://user-images.githubusercontent.com/1294454/27766817-e9456312-5ee6-11e7-9b3c-b628ca5626a5.jpg)](https://poloniex.com)                                                     | poloniex           | [Poloniex](https://poloniex.com)                                                     | *     | [API](https://docs.poloniex.com)                                                                 |                                                                                                                             | US                                      |
|[![quoinex](https://user-images.githubusercontent.com/1294454/45798859-1a872600-bcb4-11e8-8746-69291ce87b04.jpg)](https://www.liquid.com?affiliate=SbzC62lt30976)                            | quoinex            | [QUOINEX](https://www.liquid.com?affiliate=SbzC62lt30976)                            | 2     | [API](https://developers.quoine.com)                                                             |                                                                                                                             | Japan, China, Taiwan                    |


The list above is updated frequently, new crypto markets, altcoin exchanges, bug fixes, API endpoints are introduced and added on a regular basis. 
If you don't find a cryptocurrency exchange market in the list above and/or want another exchange to be added, post or send us a link to it by opening an issue here on GitHub or via email.

The library is under [MIT license](https://github.com/ccxt.net/blob/master/LICENSE.txt), that means it's absolutely free for any developer to build commercial and opensource software on top of it, but use it at your own risk with no warranties, as is.

## Install

You can also clone it into your project directory from [ccxt.net GitHub repository](https://github.com/lisa3907/ccxt.net):

```shell
git clone https://github.com/lisa3907/ccxt.net.git
```


## Documentation

Read the [Manual](https://github.com/lisa3907/ccxt.net/wiki) for more details.

## Usage

## Contributing

Please read the [CONTRIBUTING](https://github.com/lisa3907/ccxt.net/blob/master/CONTRIBUTING.md) document before making changes that you would like adopted in the code. Also, read the [Manual](https://github.com/lisa3907/ccxt.net/wiki) for more details.

## Support Developer Team

We are investing a significant amount of time into the development of this library. If CCXT.NET made your life easier and you like it and want to help us improve it further or if you want to speed up new features and exchanges, please, support us with a tip. We appreciate all contributions!

### Nuget

 > Install-Package CCXT.NET -Version 1.2.0

### Crypto Donation

```
ETH 0x556E7EdbcCd669a42f00c1Df53D550C00814B0e3
BTC 15DAoUfaCanpBpTs7VQBK8dRmbQqEnF9WG
```

### Contact

home-page: http://www.odinsoftware.co.kr

e-mail: help@odinsoftware.co.kr