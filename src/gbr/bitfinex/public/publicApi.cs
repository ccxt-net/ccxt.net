using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Public;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Bitfinex.Public
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
                    base.publicClient = new BitfinexClient("public");

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

                var _json_value = await publicClient.CallApiGet1Async("/v1/symbols_details", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _symbols = publicClient.DeserializeObject<JArray>(_json_value.Content);

                    foreach (var _market in _symbols)
                    {
                        var _symbol = _market["pair"].ToString();

                        var _base_id = _symbol.Substring(0, 3);
                        var _quote_id = _symbol.Substring(3);

                        var _base_name = publicClient.ExchangeInfo.GetCommonCurrencyName(_base_id);
                        var _quote_name = publicClient.ExchangeInfo.GetCommonCurrencyName(_quote_id);

                        var _market_id = _base_name + "/" + _quote_name;

                        var _precision = new MarketPrecision
                        {
                            quantity = _market["price_precision"].Value<int>(),
                            price = _market["price_precision"].Value<int>(),
                            amount = _market["price_precision"].Value<int>()
                        };

                        var _lot = (decimal)Math.Pow(10.0, -_precision.price);

                        var _limits = new MarketLimits
                        {
                            quantity = new MarketMinMax
                            {
                                min = _market["minimum_order_size"].Value<decimal>(),
                                max = _market["maximum_order_size"].Value<decimal>()
                            },
                            price = new MarketMinMax
                            {
                                min = (decimal)Math.Pow(10.0, -_precision.price),
                                max = (decimal)Math.Pow(10.0, _precision.price)
                            }
                        };

                        _limits.amount = new MarketMinMax
                        {
                            min = _limits.quantity.min * _limits.price.min,
                            max = decimal.MaxValue
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
        public override async ValueTask<Ticker> FetchTicker(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new Ticker(base_name, quote_name);

            var _market = await this.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = publicClient.MergeParamsAndArgs(args);

                var _json_value = await publicClient.CallApiGet1Async($"/v1/pubticker/{_market.result.symbol}", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _ticker = publicClient.DeserializeObject<BTickerItem>(_json_value.Content);
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

                var _params = publicClient.MergeParamsAndArgs(args);

                var _json_value = await publicClient.CallApiGet1Async("/v1/tickers", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _tickers = publicClient.DeserializeObject<List<BTickerItem>>(_json_value.Content);
                    {
                        foreach (var _ticker in _tickers)
                        {
                            _ticker.symbol = _ticker.symbol.ToLower();         // bitfinex markets의 symbol은 lowercase 입니다.

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

                var _params = new Dictionary<string, object>();
                {
                    if (limits > 0)
                    {
                        _params.Add("limit_bids", limits);
                        _params.Add("limit_asks", limits);
                    }
                    _params.Add("group", 1); // group false [0/1] 1 If 1, orders are grouped by price in the orderbook. If 0, orders are not grouped and sorted individually

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async($"/v1/book/{_market.result.symbol}", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orderbook = publicClient.DeserializeObject<BOrderBook>(_json_value.Content);
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
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<OHLCVs> FetchOHLCVs(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
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
                    var _limits = limits <= 1 ? 1
                               : limits <= 1000 ? limits
                               : 1000;

                    if (since == 0)
                        since = CUnixTime.NowMilli - (_timestamp * 1000) * _limits;  // 가져올 갯수 만큼 timeframe * limits 간격으로 데이터 양 계산

                    _params.Add("sort", "1");               // if = 1 it sorts results returned with old > new
                    _params.Add("limit", _limits);
                    _params.Add("start", since);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _section = "hist"; // Section(string) REQUIRED Available values: "last", "hist"

                var _json_value = await publicClient.CallApiGet1Async($"/v2/candles/trade:{_timeframe}:t{_market.result.symbol.ToUpper()}/{_section}", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<List<JArray>>(_json_value.Content);

                    _result.result.AddRange(
                         _json_data
                             .Select(x => new OHLCVItem
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
                    var _limits = limits <= 1 ? 1
                               : limits <= 1000 ? limits
                               : 1000;

                    _params.Add("sort", "-1");          // if = -1 it sorts results returned with old < new
                    _params.Add("limit", _limits);
                    if (since > 0)
                        _params.Add("start", since);    // timestamp false [time]  Only show trades at or after this timestamp

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _section = "hist"; // Section(string) REQUIRED Available values: "last", "hist"

                var _json_value = await publicClient.CallApiGet1Async($"/v2/trades/t{_market.result.symbol.ToUpper()}/{_section}", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<List<JArray>>(_json_value.Content);
                    {
                        var _orders = new List<BCompleteOrderItem>();

                        foreach (var _o in _json_data)
                        {
                            var _transactionId = _o[0].Value<string>();
                            var _timevalue = _o[1].Value<long>();
                            var _quantity = _o[2].Value<decimal>();
                            var _price = _o[3].Value<decimal>();

                            var _sideValue = (_quantity < 0) ? "sell" : "buy";
                            if (_quantity < 0)
                                _quantity = -_quantity;

                            _orders.Add(new BCompleteOrderItem()
                            {
                                transactionId = _transactionId,
                                timestamp = _timevalue,

                                fillType = FillType.Fill,
                                sideType = SideTypeConverter.FromString(_sideValue),
                                orderType = OrderType.Limit,

                                quantity = _quantity,
                                price = _price,
                                amount = _quantity * _price
                            });
                        }

                        _result.result.AddRange(
                                    _orders
                                        .Where(t => t.timestamp >= since)
                                        .OrderByDescending(t => t.timestamp)
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
    }
}