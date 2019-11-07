using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Public;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using OdinSdk.BaseLib.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.OKEx.Public
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
                    base.publicClient = new OKExClient("public");

                return base.publicClient;
            }
        }

        private OKExClient okexapiClient
        {
            get
            {
                return publicClient as OKExClient;
            }
        }

        private OKExClient __webapi_client = null;

        private OKExClient webapiClient
        {
            get
            {
                if (__webapi_client == null)
                    __webapi_client = new OKExClient("web");

                return __webapi_client;
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

            webapiClient.ExchangeInfo.ApiCallWait(TradeType.Public);
            {
                var _params = webapiClient.MergeParamsAndArgs(args);

                var _json_value = await webapiClient.CallApiGet1Async("/spot/markets/products", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = webapiClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _markets = okexapiClient.DeserializeObject<OMarkets>(_json_value.Content);
                    foreach (var _m in _markets.result)
                    {
                        _m.active = _m.online != 0;
                        if (_m.active == false)
                            continue;

                        _m.baseId = _m.symbol.Split('_')[0];
                        _m.quoteId = _m.symbol.Split('_')[1];

                        _m.baseName = okexapiClient.ExchangeInfo.GetCommonCurrencyName(_m.baseId);
                        _m.quoteName = okexapiClient.ExchangeInfo.GetCommonCurrencyName(_m.quoteId);

                        _m.marketId = _m.baseName + "/" + _m.quoteName;

                        _m.precision = new MarketPrecision
                        {
                            quantity = _m.maxSizeDigit,
                            price = _m.maxPriceDigit,
                            amount = _m.maxPriceDigit
                        };

                        _m.lot = (decimal)Math.Pow(10.0, -_m.precision.quantity);

                        _m.type = "spot";
                        _m.spot = true;
                        _m.future = false;

                        _m.makerFee = 0.15m / 100;
                        _m.takerFee = 0.20m / 100;

                        _m.limits = new MarketLimits
                        {
                            quantity = new MarketMinMax
                            {
                                min = _m.minTradeSize,
                                max = decimal.MaxValue
                            },
                            price = new MarketMinMax
                            {
                                min = (decimal)Math.Pow(10, -_m.precision.price),
                                max = decimal.MaxValue
                            },
                            amount = new MarketMinMax
                            {
                                min = _m.minTradeSize * (decimal)Math.Pow(10, -_m.precision.price),
                                max = decimal.MaxValue
                            }
                        };

                        _result.result.Add(_m.marketId, _m);

                        if (okexapiClient.ExchangeInfo.Options.futures.ContainsKey(_m.baseName) == true)
                        {
                            if (okexapiClient.ExchangeInfo.Options.futures[_m.baseName] == false)
                                continue;

                            foreach (var _f in okexapiClient.ExchangeInfo.Options.fiats)
                            {
                                var _market = _m.CreateCopy();

                                _market.marketId = _m.baseName + "/" + _f;
                                if (_result.result.ContainsKey(_market.marketId) == true)
                                    continue;

                                _market.symbol = _m.baseId + "_" + _f.ToLower();

                                _market.quoteName = _f;
                                _market.quoteId = _f.ToLower();

                                _market.type = "future";
                                _market.spot = false;
                                _market.future = true;

                                _market.makerFee = 0.03m / 100;
                                _market.takerFee = 0.05m / 100;

                                _result.result.Add(_market.marketId, _market);
                            }
                        }
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
        /// <param name="args">Add additional attributes for each exchange: contract_type(this_week, next_week, month, quarter)</param>
        /// <returns></returns>
        public override async ValueTask<Ticker> FetchTicker(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new Ticker(base_name, quote_name);

            var _market = await this.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                okexapiClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);

                    okexapiClient.MergeParamsAndArgs(_params, args);
                }

                var _end_point = okexapiClient.CheckFuturesUrl(_market.result, "/ticker.do", "/future_ticker.do", _params);

                var _json_value = await okexapiClient.CallApiGet1Async(_end_point.endPoint, _end_point.args);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = okexapiClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = okexapiClient.DeserializeObject<OTicker>(_json_value.Content);
                    {
                        var _ticker = _json_data.result;

                        _ticker.symbol = _market.result.symbol;
                        _ticker.timestamp = _json_data.timestamp * 1000;

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
                okexapiClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = publicClient.MergeParamsAndArgs(args);

                var _json_value = await okexapiClient.CallApiGet1Async("/tickers.do", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = okexapiClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = okexapiClient.DeserializeObject<OTickers>(_json_value.Content);
                    {
                        foreach (var _ticker in _json_data.result)
                        {
                            _ticker.timestamp = _json_data.timestamp * 1000;
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
        /// <param name="args">Add additional attributes for each exchange: contract_type(this_week, next_week, month, quarter)</param>
        /// <returns></returns>
        public override async ValueTask<OrderBooks> FetchOrderBooks(string base_name, string quote_name, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new OrderBooks(base_name, quote_name);

            var _market = await this.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                okexapiClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("size", $"{limits}");

                    okexapiClient.MergeParamsAndArgs(_params, args);
                }

                var _end_point = okexapiClient.CheckFuturesUrl(_market.result, "/depth.do", "/future_depth.do", _params);

                var _json_value = await okexapiClient.CallApiGet1Async(_end_point.endPoint, _end_point.args);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = okexapiClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orderbook = okexapiClient.DeserializeObject<OOrderBook>(_json_value.Content);
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
        /// Fetch array of symbol name and OHLCVs data
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limits">maximum number of items (optional): default 20</param>
        /// <param name="args">Add additional attributes for each exchange: contract_type(this_week, next_week, month, quarter)</param>
        /// <returns></returns>
        public override async ValueTask<OHLCVs> FetchOHLCVs(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new OHLCVs(base_name, quote_name);

            var _market = await this.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                okexapiClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _timeframe = okexapiClient.ExchangeInfo.GetTimeframe(timeframe);
                var _timestamp = okexapiClient.ExchangeInfo.GetTimestamp(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("type", _timeframe);
                    _params.Add("size", limits);

                    if (since > 0)
                        _params.Add("since", since);

                    okexapiClient.MergeParamsAndArgs(_params, args);
                }

                var _end_point = okexapiClient.CheckFuturesUrl(_market.result, "/kline.do", "/future_kline.do", _params);

                var _json_value = await okexapiClient.CallApiGet1Async(_end_point.endPoint, _end_point.args);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = okexapiClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = okexapiClient.DeserializeObject<List<JArray>>(_json_value.Content);

                    _result.result.AddRange(
                         _json_data
                             .Select(x => new OHLCVItem
                             {
                                 timestamp = x[0].Value<long>(),
                                 openPrice = x[1].Value<decimal>(),
                                 highPrice = x[2].Value<decimal>(),
                                 lowPrice = x[3].Value<decimal>(),
                                 closePrice = x[4].Value<decimal>(),
                                 volume = x[5].Value<decimal>()
                             })
                             .Where(o => o.timestamp >= since)
                             .OrderByDescending(o => o.timestamp)
                             .Take(limits)
                         );
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
        /// <param name="args">Add additional attributes for each exchange: contract_type(this_week, next_week, month, quarter)</param>
        /// <returns></returns>
        public override async ValueTask<CompleteOrders> FetchCompleteOrders(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new CompleteOrders(base_name, quote_name);

            var _market = await this.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                okexapiClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _timeframe = okexapiClient.ExchangeInfo.GetTimeframe(timeframe);
                var _timestamp = okexapiClient.ExchangeInfo.GetTimestamp(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);

                    okexapiClient.MergeParamsAndArgs(_params, args);
                }

                var _end_point = okexapiClient.CheckFuturesUrl(_market.result, "/trades.do", "/future_trades.do", _params);

                var _json_value = await okexapiClient.CallApiGet1Async(_end_point.endPoint, _end_point.args);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = okexapiClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = okexapiClient.DeserializeObject<List<OCompleteOrderItem>>(_json_value.Content);
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