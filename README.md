# CCXT.NET - CryptoCurrency eXchange Trading Library for .NET

[![Build status](https://ci.appveyor.com/api/projects/status/dnp9i3t6sexv9tpa?svg=true)](https://ci.appveyor.com/project/lisa3907/ccxt.net)

This project is an extension of [ccxt](https://github.com/ccxt/ccxt).

If you are a .Net C# programmer, ccxt.net might be useful. We created ccxt.net using open-source [ccxt](https://github.com/ccxt/ccxt).

This project started in 2018/04. I plan to update it continuously.


## Supported Cryptocurrency Exchange Markets

The ccxt.net library currently supports the following 14 cryptocurrency exchange markets and trading APIs:

|                                                                                                                           | id                 | name                                                      | ver | doc                                                                                          | countries                               |
|---------------------------------------------------------------------------------------------------------------------------|--------------------|-----------------------------------------------------------|:---:|:--------------------------------------------------------------------------------------------:|-----------------------------------------|
|![bithumb](https://user-images.githubusercontent.com/1294454/30597177-ea800172-9d5e-11e7-804c-b9d4fa9b56b0.jpg)            | bithumb            | [Bithumb](https://www.bithumb.com)                        | *   | [API](https://www.bithumb.com/u1/US127)                                                      | South Korea                             |
|![coinone](https://user-images.githubusercontent.com/1294454/38003300-adc12fba-323f-11e8-8525-725f53c4a659.jpg)            | coinone            | [CoinOne](https://coinone.co.kr)                          | 2   | [API](https://doc.coinone.co.kr)                                                             | South Korea                             |
|![korbit](https://github.com/lisa3907/ccxt.net/blob/master/logo-files/favicon-korbit-16x16.png?raw=true) korbit           | korbit            | [Korbit](https://www.korbit.co.kr/)                          | 1   | [API](https://apidocs.korbit.co.kr/)                                                             | South Korea                             |
|![poloniex](https://user-images.githubusercontent.com/1294454/27766817-e9456312-5ee6-11e7-9b3c-b628ca5626a5.jpg)           | poloniex           | [Poloniex](https://poloniex.com)                          | *   | [API](https://poloniex.com/support/api/)                                                     | US                                      |

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

### Intro

The ccxt.net library consists of a public part and a private part. Anyone can use the public part out-of-the-box immediately after installation. Public APIs open access to public information from all exchange markets without registering user accounts and without having API keys.

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


Read the [Manual](https://github.com/lisa3907/ccxt.net/wiki) for more details.


## Contributing

Please read the [CONTRIBUTING](https://github.com/lisa3907/ccxt.net/blob/master/CONTRIBUTING.md) document before making changes that you would like adopted in the code. Also, read the [Manual](https://github.com/lisa3907/ccxt.net/wiki) for more details.

## Support Developer Team

We are investing a significant amount of time into the development of this library. If CCXT.NET made your life easier and you like it and want to help us improve it further or if you want to speed up new features and exchanges, please, support us with a tip. We appreciate all contributions!

### Nuget

 > Install-Package CCXT.NET -Version 1.0.0

### Crypto Donation

```
ETH 0x556E7EdbcCd669a42f00c1Df53D550C00814B0e3
BTC 15DAoUfaCanpBpTs7VQBK8dRmbQqEnF9WG
```
### Contact

home-page: http://www.odinsoftware.co.kr

e-mail: help@odinsoftware.co.kr