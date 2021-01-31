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

namespace CCXT.NET.Binance.Public
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
                    base.publicClient = new BinanceClient("public");

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

                var _json_value = await publicClient.CallApiGet1Async("/exchangeInfo", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _exchange_info = publicClient.DeserializeObject<JObject>(_json_value.Content);

                    var _symbols = _exchange_info["symbols"].ToObject<JArray>();
                    foreach (var _market in _symbols)
                    {
                        var _symbol = _market["symbol"].ToString();
                        if (_symbol == "123456")     // "123456" is a "test symbol/market"
                            continue;

                        var _base_id = _market["baseAsset"].ToString();
                        var _quote_id = _market["quoteAsset"].ToString();
                        var _base_name = publicClient.ExchangeInfo.GetCommonCurrencyName(_base_id);
                        var _quote_name = publicClient.ExchangeInfo.GetCommonCurrencyName(_quote_id);
                        var _market_id = _base_name + "/" + _quote_name;

                        var _precision = new MarketPrecision
                        {
                            quantity = _market["baseAssetPrecision"].Value<int>(),
                            price = _market["quotePrecision"].Value<int>(),
                            amount = _market["quotePrecision"].Value<int>()
                        };

                        var _lot = (decimal)(-1.0 * Math.Log10((double)_precision.quantity));
                        var _active = _market["status"].ToString().ToUpper() == "TRADING";

                        var _limits = new MarketLimits
                        {
                            quantity = new MarketMinMax
                            {
                                min = (decimal)Math.Pow(10.0, -(double)_precision.quantity),
                                max = decimal.MaxValue
                            },
                            price = new MarketMinMax
                            {
                                min = (decimal)Math.Pow(10.0, -(double)_precision.price),
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

                        var _filters = _market["filters"];
                        {
                            var _price_filter = _filters.SingleOrDefault(f => f["filterType"].ToString() == "PRICE_FILTER");
                            if (_price_filter != null)
                            {
                                _entry.precision.price = Numerical.PrecisionFromString(_price_filter["tickSize"].ToString());
                                _entry.limits.price.min = _price_filter["minPrice"].Value<decimal>();
                                _entry.limits.price.max = _price_filter["maxPrice"].Value<decimal>();
                            }

                            var _lot_size = _filters.SingleOrDefault(f => f["filterType"].ToString() == "LOT_SIZE");
                            if (_lot_size != null)
                            {
                                _entry.precision.quantity = Numerical.PrecisionFromString(_lot_size["stepSize"].ToString());
                                _entry.limits.quantity.min = _lot_size["minQty"].Value<decimal>();
                                _entry.limits.quantity.max = _lot_size["maxQty"].Value<decimal>();
                            }

                            var _min_notional = _filters.SingleOrDefault(f => f["filterType"].ToString() == "MIN_NOTIONAL");
                            if (_min_notional != null)
                            {
                                _entry.limits.amount.min = _min_notional["minNotional"].Value<decimal>();
                            }
                        }

                        if (_result.result.ContainsKey(_entry.marketId) == false)
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
            if (_market.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("/ticker/24hr", _params);
#if DEBUG
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
        public override async ValueTask<Tickers> FetchTickersAsync(Dictionary<string, object> args = null)
        {
            var _result = new Tickers();

            var _markets = await this.LoadMarketsAsync();
            if (_markets.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = publicClient.MergeParamsAndArgs(args);

                var _json_value = await publicClient.CallApiGet1Async("/ticker/24hr", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _tickers = publicClient.DeserializeObject<List<BTickerItem>>(_json_value.Content);
                    {
                        foreach (var _ticker in _tickers)
                        {
                            if (_ticker.symbol == "123456")     // "123456" is a "test symbol/market"
                                continue;

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
        public override async ValueTask<OrderBooks> FetchOrderBooksAsync(string base_name, string quote_name, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new OrderBooks(base_name, quote_name);

            var _market = await this.LoadMarketAsync(_result.marketId);
            if (_market.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = new Dictionary<string, object>();
                {
                    var _limits = limits <= 5 ? 5
                                : limits <= 10 ? 10
                                : limits <= 20 ? 20
                                : limits <= 50 ? 50
                                : limits <= 100 ? 100
                                : limits <= 500 ? 500
                                : 1000;

                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("limit", _limits);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("/depth", _params);
#if DEBUG
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
                        _result.result.nonce = _orderbook.lastUpdateId;
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
            if (_market.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _timeframe = publicClient.ExchangeInfo.GetTimeframe(timeframe);
                var _timestamp = publicClient.ExchangeInfo.GetTimestamp(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("interval", _timeframe);

                    if (since > 0)
                        _params.Add("startTime", since);

                    _params.Add("limit", limits);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("/klines", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<JArray>(_json_value.Content);

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
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<CompleteOrders> FetchCompleteOrdersAsync(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new CompleteOrders(base_name, quote_name);

            var _market = await this.LoadMarketAsync(_result.marketId);
            if (_market.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _timeframe = publicClient.ExchangeInfo.GetTimeframe(timeframe);
                var _timestamp = publicClient.ExchangeInfo.GetTimestamp(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    if (since > 0)
                    {
                        _params.Add("startTime", since);
                        _params.Add("endTime", since + 3600 * 1000);        // More than 1 hours between startTime and endTime
                    }
                    if (limits > 0)
                        _params.Add("limit", limits);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("/aggTrades", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<List<BCompleteOrderItem>>(_json_value.Content);
                    {
                        var _orders = _json_data
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