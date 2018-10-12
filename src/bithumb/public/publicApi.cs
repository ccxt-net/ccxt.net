using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Public;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Bithumb.Public
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
                    base.publicClient = new BithumbClient("public");

                return base.publicClient;
            }
        }

        private BithumbClient __public_web = null;

        /// <summary>
        /// 
        /// </summary>
        public BithumbClient publicWeb
        {
            get
            {
                if (__public_web == null)
                    __public_web = new BithumbClient("web");

                return __public_web;
            }
        }

        /// <summary>
        /// Fetch symbols, market ids and exchanger's information
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<Markets> FetchMarkets(Dictionary<string, object> args = null)
        {
            var _result = new Markets();

            publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);
            {
                var _params = new Dictionary<string, object>();
                {
                    if (args != null)
                    {
                        foreach (var _a in args)
                        {
                            if (_params.ContainsKey(_a.Key) == true)
                                _params.Remove(_a.Key);

                            _params.Add(_a.Key, _a.Value);
                        }
                    }
                }

                var _json_value = await publicClient.CallApiGet1Async("/ticker/ALL", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<JObject>(_json_value.Content);

                    var _tickers = _json_data["data"].ToObject<JObject>();
                    foreach (var _market in _tickers)
                    {
                        var _symbol = _market.Key;
                        if (_symbol == "date")
                            continue;

                        var _base_id = _symbol.ToLower();
                        var _quote_id = "krw";
                        var _base_name = publicClient.ExchangeInfo.GetCommonCurrencyName(_base_id);
                        var _quote_name = publicClient.ExchangeInfo.GetCommonCurrencyName(_quote_id);
                        var _market_id = _base_name + "/" + _quote_name;

                        var _precision = new MarketPrecision
                        {
                            quantity = 8,
                            price = 0,
                            amount = 0
                        };

                        var _lot = (decimal)Math.Pow(10.0, -_precision.price);

                        var _limits = new MarketLimits
                        {
                            quantity = new MarketMinMax
                            {
                                min = 0,
                                max = decimal.MaxValue
                            },
                            price = new MarketMinMax
                            {
                                min = 0,
                                max = decimal.MaxValue
                            },
                            amount = new MarketMinMax
                            {
                                min = 0,
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
                            active = true,

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
        public override async Task<Ticker> FetchTicker(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new Ticker(base_name, quote_name);

            var _market = await this.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = new Dictionary<string, object>();
                {
                    if (args != null)
                    {
                        foreach (var _a in args)
                        {
                            if (_params.ContainsKey(_a.Key) == true)
                                _params.Remove(_a.Key);

                            _params.Add(_a.Key, _a.Value);
                        }
                    }
                }

                var _json_value = await publicClient.CallApiGet1Async($"/ticker/{_market.result.symbol}", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<BTicker>(_json_value.Content);
                    if (_json_data.success == true)
                    {
                        _result.SetResult(_json_data);
                        {
                            _json_data.result.symbol = _market.result.symbol;
                            _result.marketId = _market.result.marketId;
                            _result.result = _json_data.result;
                        }
                    }
                    else
                    {
                        var _message = publicClient.GetErrorMessage(_json_data.statusCode);
                        _json_result.SetFailure(
                                _message,
                                ErrorCode.ResponseDataError
                            );
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
        public override async Task<Tickers> FetchTickers(Dictionary<string, object> args = null)
        {
            var _result = new Tickers();

            var _markets = await this.LoadMarkets();
            if (_markets.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = new Dictionary<string, object>();
                {
                    if (args != null)
                    {
                        foreach (var _a in args)
                        {
                            if (_params.ContainsKey(_a.Key) == true)
                                _params.Remove(_a.Key);

                            _params.Add(_a.Key, _a.Value);
                        }
                    }
                }

                var _json_value = await publicClient.CallApiGet1Async("/ticker/ALL", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<BTickers>(_json_value.Content);
                    if (_json_data.statusCode == 0)
                    {
                        var _timestamp = _json_data.result["date"].Value<long>();

                        foreach (var _t in _json_data.result)
                        {
                            if (_t.Value.GetType() == typeof(JValue))
                                continue;

                            var _ticker = publicClient.DeserializeObject<BTickerItem>(_t.Value.ToString());
                            {
                                _ticker.symbol = _t.Key;
                                _ticker.timestamp = _timestamp;

                                _result.result.Add(_ticker);
                            }
                        }
                    }
                    else
                    {
                        var _message = publicClient.GetErrorMessage(_json_data.statusCode);
                        _json_result.SetFailure(
                                     _message,
                                     ErrorCode.ResponseDataError
                                 );
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
        public override async Task<OrderBooks> FetchOrderBooks(string base_name, string quote_name, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new OrderBooks(base_name, quote_name);

            var _market = await this.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = new Dictionary<string, object>();
                {
                    var _limit = limits <= 1 ? 1
                               : limits <= 50 ? limits
                               : 50;

                    _params.Add("count", _limit);
                    _params.Add("group_orders", 1); // group_orders Int Value : 0 또는 1 

                    if (args != null)
                    {
                        foreach (var _a in args)
                        {
                            if (_params.ContainsKey(_a.Key) == true)
                                _params.Remove(_a.Key);

                            _params.Add(_a.Key, _a.Value);
                        }
                    }
                }

                var _json_value = await publicClient.CallApiGet1Async($"/orderbook/{_market.result.symbol}", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<BOrderBooks>(_json_value.Content);
                    if (_json_data.success == true)
                    {
                        _result.result.asks = _json_data.result.asks.OrderBy(o => o.price).Take(limits).ToList();
                        _result.result.bids = _json_data.result.bids.OrderByDescending(o => o.price).Take(limits).ToList();

                        _result.result.symbol = _market.result.symbol;
                        _result.result.timestamp = _json_data.result.timestamp;
                        _result.result.nonce = _json_data.result.timestamp / 1000;
                    }
                    else
                    {
                        var _message = publicClient.GetErrorMessage(_json_data.statusCode);
                        _json_result.SetFailure(
                                     _message,
                                     ErrorCode.ResponseDataError
                                 );
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
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<OHLCVs> FetchOHLCVs(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new OHLCVs(base_name, quote_name);

            var _market = await this.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _timestamp = publicClient.ExchangeInfo.GetTimestamp(timeframe);
                var _timeframe = publicClient.ExchangeInfo.GetTimeframe(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("resolution", 0.5);

                    if (since > 0 && limits > 0)
                    {
                        _params.Add("from", since);
                        _params.Add("to", since + limits * _timestamp);     // 가져올 갯수 만큼 timeframe * limits 간격으로 데이터 양 계산
                    }

                    _params.Add("strTime", CUnixTime.NowMilli);

                    if (args != null)
                    {
                        foreach (var _a in args)
                        {
                            if (_params.ContainsKey(_a.Key) == true)
                                _params.Remove(_a.Key);

                            _params.Add(_a.Key, _a.Value);
                        }
                    }
                }

                var _json_value = await publicWeb.CallApiGet1Async($"/resources/chart/{_market.result.symbol}_xcoinTrade_{_timeframe}.json", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<JArray>(_json_value.Content);
                    {
                        _result.result.AddRange(
                            _json_data
                                .Select(x => new BOHLCVItem
                                {
                                    timestamp = x[0].Value<long>(),
                                    openPrice = x[1].Value<decimal>(),
                                    highPrice = x[3].Value<decimal>(),
                                    lowPrice = x[4].Value<decimal>(),
                                    closePrice = x[2].Value<decimal>(),
                                    volume = x[5].Value<decimal>()
                                })
                                .Where(o => o.timestamp >= since)
                                .OrderByDescending(o => o.timestamp)
                                .Take(limits)
                            );
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
        public override async Task<CompleteOrders> FetchCompleteOrders(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
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
                    var _limit = limits <= 1 ? 1
                               : limits <= 100 ? limits
                               : 100;

                    //_params.Add("cont_no", 0);    // 체결 번호
                    _params.Add("count", _limit);

                    if (args != null)
                    {
                        foreach (var _a in args)
                        {
                            if (_params.ContainsKey(_a.Key) == true)
                                _params.Remove(_a.Key);

                            _params.Add(_a.Key, _a.Value);
                        }
                    }
                }

                var _json_value = await publicClient.CallApiGet1Async($"/transaction_history/{_market.result.symbol}", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<BCompleteOrders>(_json_value.Content);
                    if (_json_data.success == true)
                    {
                        var _orders = _json_data.result
                                                .Where(t => t.timestamp >= since)
                                                .OrderByDescending(t => t.timestamp)
                                                .Take(limits);

                        foreach (var _o in _orders)
                        {
                            _o.fillType = FillType.Fill;
                            _o.orderType = OrderType.Limit;

                            _o.amount = _o.quantity * _o.price;
                            _result.result.Add(_o);
                        }
                    }
                    else
                    {
                        var _message = publicClient.GetErrorMessage(_json_data.statusCode);
                        _json_result.SetFailure(
                                     _message,
                                     ErrorCode.ResponseDataError
                                 );
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