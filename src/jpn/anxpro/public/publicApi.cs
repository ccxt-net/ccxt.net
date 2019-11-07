using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Public;
using OdinSdk.BaseLib.Coin.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Anxpro.Public
{
    /// <summary>
    /// exchange's public API implement class
    /// </summary>
    public class PublicApi : OdinSdk.BaseLib.Coin.Public.PublicApi, IPublicApi
    {
        /// <summary>
        ///
        /// </summary>
        public PublicApi()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public override XApiClient publicClient
        {
            get
            {
                if (base.publicClient == null)
                    base.publicClient = new AnxproClient("public");

                return base.publicClient;
            }
        }

        /// <summary>
        /// Fetch symbols, market ids and exchanger's information
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Markets> FetchMarkets(Dictionary<string, object> args = null)
        {
            var _result = new Markets();

            publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);
            {
                var _markets = new List<MarketItem>()
                {
                    new MarketItem() { symbol = "BTCUSD", baseId = "BTC", quoteId = "USD", lot = 100000 },
                    new MarketItem() { symbol = "BTCHKD", baseId = "BTC", quoteId = "HKD", lot = 100000 },
                    new MarketItem() { symbol = "BTCEUR", baseId = "BTC", quoteId = "EUR", lot = 100000 },
                    new MarketItem() { symbol = "BTCCAD", baseId = "BTC", quoteId = "CAD", lot = 100000 },
                    new MarketItem() { symbol = "BTCAUD", baseId = "BTC", quoteId = "AUD", lot = 100000 },
                    new MarketItem() { symbol = "BTCSGD", baseId = "BTC", quoteId = "SGD", lot = 100000 },
                    new MarketItem() { symbol = "BTCJPY", baseId = "BTC", quoteId = "JPY", lot = 100000 },
                    new MarketItem() { symbol = "BTCGBP", baseId = "BTC", quoteId = "GBP", lot = 100000 },
                    new MarketItem() { symbol = "BTCNZD", baseId = "BTC", quoteId = "NZD", lot = 100000 },
                    new MarketItem() { symbol = "LTCBTC", baseId = "LTC", quoteId = "BTC", lot = 100000 },
                    new MarketItem() { symbol = "STRBTC", baseId = "STR", quoteId = "BTC", lot = 100000000 },
                    new MarketItem() { symbol = "XRPBTC", baseId = "XRP", quoteId = "BTC", lot = 100000000 },
                    new MarketItem() { symbol = "DOGEBTC", baseId = "DOGE", quoteId = "BTC", lot = 100000000 }
                };

                _markets.ForEach((m) =>
                {
                    var _base_name = publicClient.ExchangeInfo.GetCommonCurrencyName(m.baseId);
                    var _quote_name = publicClient.ExchangeInfo.GetCommonCurrencyName(m.quoteId);

                    m.baseName = _base_name;
                    m.quoteName = _quote_name;

                    m.marketId = _base_name + "/" + _quote_name;
                    m.active = true;
                });

                _result.result = _markets.ToDictionary(m => m.marketId, m => m as IMarketItem);

                var _json_result = publicClient.GetResponseMessage();
                {
                    _result.SetResult(_json_result);
                }
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        /// Fetch current best bid and ask, as well as the last trade price.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Ticker> FetchTicker(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new Ticker(base_name, quote_name);

            var _market = await this.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("extraCcyPairs", _market.result.symbol);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("money/ticker", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<ATickers>(_json_value.Content);
                    {
                        var _ticker = _json_data.result[_market.result.symbol];
                        _ticker.symbol = _market.result.symbol;

                        _result.result = _ticker;
                    }
                }

                _result.SetResult(_json_result);
            }
            else
            {
                _result.SetResult(_market);
            }

            return _result;
        }

        /// <summary>
        /// Fetch price change statistics
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Tickers> FetchTickers(Dictionary<string, object> args = null)
        {
            var _result = new Tickers();

            var _markets = await this.LoadMarkets();
            if (_markets.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                //var _asset_pairs = new StringBuilder();
                //{
                //    foreach (var _market in _markets.result)
                //        _asset_pairs.Append($"{_market.Value.symbol},");
                //    _asset_pairs.Length--;
                //}

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("extraCcyPairs", String.Join(",", _markets.result.Select(m => m.Value.symbol)));

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("money/ticker", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<ATickers>(_json_value.Content);
                    {
                        foreach (var _t in _json_data.result)
                        {
                            var _ticker = _t.Value;
                            _ticker.symbol = _t.Key;

                            _result.result.Add(_ticker);
                        }
                    }
                }

                _result.SetResult(_json_result);
            }
            else
            {
                _result.SetResult(_markets);
            }

            return _result;
        }

        /// <summary>
        /// Fetch pending or registered order details
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="limits">maximum number of items (optional): default 20</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<OrderBooks> FetchOrderBooks(string base_name, string quote_name, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new OrderBooks(base_name, quote_name);

            var _market = await this.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = publicClient.MergeParamsAndArgs(args);

                var _json_value = await publicClient.CallApiGet1Async($"{_market.result.symbol}/money/depth/full", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orderbook = publicClient.DeserializeObject<AOrderBooks>(_json_value.Content);
                    {
                        _result.result.asks = _orderbook.result.asks.OrderBy(o => o.price).Take(limits).ToList();
                        _result.result.bids = _orderbook.result.bids.OrderByDescending(o => o.price).Take(limits).ToList();

                        _result.result.symbol = _market.result.symbol;
                        _result.result.timestamp = _orderbook.result.dataUpdateTime / 1000;
                        _result.result.nonce = _orderbook.result.now / 1000000;
                    }
                }

                _result.SetResult(_json_result);
            }
            else
            {
                _result.SetResult(_market);
            }

            return _result;
        }
    }
}