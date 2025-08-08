using Newtonsoft.Json.Linq;
using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Coin.Public;
using CCXT.NET.Shared.Coin.Types;
using CCXT.NET.Shared.Configuration;
using CCXT.NET.Shared.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.GDAX.Public
{
    /// <summary>
    /// exchange's public API implement class
    /// </summary>
    public class PublicApi : CCXT.NET.Shared.Coin.Public.PublicApi, IPublicApi
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
                    base.publicClient = new GdaxClient("public");

                return base.publicClient;
            }
        }

        /// <summary>
        /// Fetch symbols, market ids and exchanger's information
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Markets> FetchMarketsAsync(Dictionary<string, object> args = null)
        {
            var _result = new Markets();

            publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);
            {
                var _params = publicClient.MergeParamsAndArgs(args);

                var _json_value = await publicClient.CallApiGet1Async("/products", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _products = publicClient.DeserializeObject<List<JObject>>(_json_value.Content);
                    foreach (var _market in _products)
                    {
                        var _symbol = _market["id"].ToString();

                        var _base_id = _market["base_currency"].ToString();
                        var _quote_id = _market["quote_currency"].ToString();

                        var _base_name = publicClient.ExchangeInfo.GetCommonCurrencyName(_base_id);
                        var _quote_name = publicClient.ExchangeInfo.GetCommonCurrencyName(_quote_id);

                        var _market_id = _base_name + "/" + _quote_name;
                        if (_result.result.ContainsKey(_market_id))
                            continue;

                        var _precision = new MarketPrecision
                        {
                            quantity = 8,
                            price = Numerical.PrecisionFromString(_market["quote_increment"].Value<string>()),
                            amount = 0
                        };

                        var _lot = (decimal)Math.Pow(10, -(double)_precision.quantity);
                        var _active = _market["status"].ToString() == "online";

                        var _limits = new MarketLimits
                        {
                            quantity = new MarketMinMax
                            {
                                min = _market["base_min_size"].Value<decimal>(),
                                max = _market["base_max_size"].Value<decimal>()
                            },
                            price = new MarketMinMax
                            {
                                min = _market["quote_increment"].Value<decimal>(),
                                max = decimal.MaxValue
                            },
                            amount = new MarketMinMax
                            {
                                min = _market["min_market_funds"].Value<decimal>(),
                                max = _market["max_market_funds"].Value<decimal>()
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
        public override async ValueTask<Ticker> FetchTickerAsync(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new Ticker(base_name, quote_name);

            var _market = await this.LoadMarketAsync(_result.marketId);
            if (_market.success)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = publicClient.MergeParamsAndArgs(args);

                var _json_value = await publicClient.CallApiGet1Async($"/products/{_market.result.symbol}/ticker", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
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
        public override async ValueTask<OrderBooks> FetchOrderBooksAsync(string base_name, string quote_name, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new OrderBooks(base_name, quote_name);

            var _market = await this.LoadMarketAsync(_result.marketId);
            if (_market.success)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = new Dictionary<string, object>();
                {
                    if (limits == 1)
                        _params.Add("level", 1);        // Only the best bid and ask
                    else
                        _params.Add("level", 2);        // Top 50 bids and asks (aggregated)

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async($"/products/{_market.result.symbol}/book", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _orderbook = publicClient.DeserializeObject<GOrderBook>(_json_value.Content);
                    {
                        _result.result.asks = _orderbook.asks.OrderBy(o => o.price).Take(limits).ToList();
                        _result.result.bids = _orderbook.bids.OrderByDescending(o => o.price).Take(limits).ToList();

                        _result.result.symbol = _market.result.symbol;
                        _result.result.timestamp = CUnixTime.NowMilli;
                        _result.result.nonce = _orderbook.sequence;
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
        public override async ValueTask<OHLCVs> FetchOHLCVsAsync(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new OHLCVs(base_name, quote_name);

            var _market = await this.LoadMarketAsync(_result.marketId);
            if (_market.success)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _timeframe = publicClient.ExchangeInfo.GetTimeframe(timeframe);
                var _timestamp = publicClient.ExchangeInfo.GetTimestamp(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("granularity", _timestamp);

                    var _till_time = CUnixTime.Now;
                    var _from_time = (since > 0) ? since / 1000 : _till_time - _timestamp * limits;     // 가져올 갯수 만큼 timeframe * limits 간격으로 데이터 양 계산

                    _params.Add("start", CUnixTime.ConvertToUtcTime(_from_time).ToString("o"));         // ISO 8601 datetime string with seconds
                    _params.Add("end", CUnixTime.ConvertToUtcTime(_till_time).ToString("o"));

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async($"/products/{_market.result.symbol}/candles", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _json_data = publicClient.DeserializeObject<JArray>(_json_value.Content);

                    _result.result.AddRange(
                         _json_data
                             .Select(x => new OHLCVItem
                             {
                                 timestamp = x[0].Value<long>() * 1000,
                                 openPrice = x[3].Value<decimal>(),
                                 highPrice = x[2].Value<decimal>(),
                                 lowPrice = x[1].Value<decimal>(),
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
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<CompleteOrders> FetchCompleteOrdersAsync(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new CompleteOrders(base_name, quote_name);

            var _market = await this.LoadMarketAsync(_result.marketId);
            if (_market.success)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _timeframe = publicClient.ExchangeInfo.GetTimeframe(timeframe);
                var _timestamp = publicClient.ExchangeInfo.GetTimestamp(timeframe);

                var _params = publicClient.MergeParamsAndArgs(args);

                var _json_value = await publicClient.CallApiGet1Async($"/products/{_market.result.symbol}/trades", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
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