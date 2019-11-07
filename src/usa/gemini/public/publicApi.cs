using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Public;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Gemini.Public
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
                    base.publicClient = new GeminiClient("public");

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
                var _params = publicClient.MergeParamsAndArgs(args);

                var _json_value = await publicClient.CallApiGet1Async("/v1/symbols", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _symbols = publicClient.DeserializeObject<JArray>(_json_value.Content);
                    foreach (var _market in _symbols)
                    {
                        var _symbol = _market.ToString();

                        var _base_id = _symbol.Substring(0, 3);
                        var _quote_id = _symbol.Substring(3);

                        var _base_name = publicClient.ExchangeInfo.GetCommonCurrencyName(_base_id);
                        var _quote_name = publicClient.ExchangeInfo.GetCommonCurrencyName(_quote_id);

                        var _market_id = _base_name + "/" + _quote_name;

                        var _precision = new MarketPrecision
                        {
                            quantity = 8,
                            price = 8,
                            amount = 8
                        };

                        var _lot = (decimal)(-1.0 * Math.Log10(_precision.quantity));
                        var _active = true;

                        var _limits = new MarketLimits
                        {
                            quantity = new MarketMinMax
                            {
                                min = (decimal)Math.Pow(10, -_precision.quantity),
                                max = decimal.MaxValue
                            },
                            price = new MarketMinMax
                            {
                                min = (decimal)Math.Pow(10, -_precision.price),
                                max = decimal.MaxValue
                            },
                            amount = new MarketMinMax
                            {
                                min = _lot,
                                max = decimal.MaxValue
                            }
                        };

                        var _entry = new MarketItem
                        {
                            marketId = _market_id,

                            symbol = _symbol,
                            baseId = _base_id,
                            quoteId = _quote_id,
                            baseName = _base_name,
                            quoteName = _quote_name,

                            lot = _lot,
                            active = _active,

                            precision = _precision,
                            limits = _limits
                        };

                        _result.result.Add(_entry.marketId, _entry);
                    }
                }

                _result.SetResult(_json_result);
            }

            return _result;
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

                var _params = publicClient.MergeParamsAndArgs(args);

                var _json_value = await publicClient.CallApiGet1Async($"/v1/pubticker/{_market.result.symbol}", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _ticker = publicClient.DeserializeObject<GTickerItem>(_json_value.Content);
                    {
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

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("limit_asks", limits);
                    _params.Add("limit_bids", limits);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async($"/v1/book/{_market.result.symbol}", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orderbook = publicClient.DeserializeObject<GOrderBook>(_json_value.Content);
                    {
                        _result.result.asks = _orderbook.asks.OrderBy(o => o.price).Take(limits).ToList();
                        _result.result.bids = _orderbook.bids.OrderByDescending(o => o.price).Take(limits).ToList();

                        _result.result.symbol = _market.result.symbol;
                        _result.result.timestamp = CUnixTime.NowMilli;
                        _result.result.nonce = CUnixTime.Now;
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
        /// Fetch array of recent trades data
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limits">maximum number of items (optional): default 20</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<CompleteOrders> FetchCompleteOrders(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new CompleteOrders(base_name, quote_name);

            var _market = await this.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _timeframe = publicClient.ExchangeInfo.GetTimeframe(timeframe);
                var _timestamp = publicClient.ExchangeInfo.GetTimestamp(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    if (since > 0)
                    {
                        since /= 1000;

                        var _till_time = CUnixTime.Now;
                        var _from_time = _till_time - (7 * 24 * 60 * 60 - 60 * 10);
                        if (_from_time < since)
                            _from_time = since;

                        _params.Add("since", _from_time);       // Optional. Only return trades after this timestamp. See Data Types: Timestamps for more information. If not present, will show the most recent trades. For backwards compatibility, you may also use the alias 'since'.
                    }

                    _params.Add("limit_trades", limits);        // Optional. The maximum number of trades to return. The default is 50.

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async($"/v1/trades/{_market.result.symbol}", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<List<GCompleteOrderItem>>(_json_value.Content);
                    {
                        var _orders = _json_data
                                            .Where(t => t.timestamp >= since)
                                            .OrderByDescending(t => t.timestamp)
                                            .Take(limits);

                        foreach (var _o in _orders)
                        {
                            _o.orderType = OrderType.Limit;
                            _o.fillType = FillType.Fill;

                            _o.amount = _o.quantity * _o.price;
                            _result.result.Add(_o);
                        }
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