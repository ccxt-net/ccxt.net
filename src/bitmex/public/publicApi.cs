using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Public;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using OdinSdk.BaseLib.Converter;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.BitMEX.Public
{
    /// <summary>
    /// exchange's public API implement class
    /// </summary>
    public class PublicApi : OdinSdk.BaseLib.Coin.Public.PublicApi, IPublicApi
    {
        /// <summary>
        /// 
        /// </summary>
        public PublicApi(bool is_live = true)
        {
            IsLive = is_live;
        }

        private bool IsLive
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override XApiClient publicClient
        {
            get
            {
                if (base.publicClient == null)
                {
                    var _division = (IsLive == false ? "test." : "") + "public";
                    base.publicClient = new BitmexClient(_division);
                }

                return base.publicClient;
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

                var _json_value = await publicClient.CallApiGet1Async("/api/v1/instrument/active", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _markets = publicClient.DeserializeObject<List<BMarketItem>>(_json_value.Content);
                    foreach (var _m in _markets)
                    {
                        _m.active = _m.state != "Unlisted";
                        if (_m.active == false)
                            continue;

                        var _base_id = _m.underlying;
                        var _quote_id = _m.quoteCurrency;

                        var _base_name = publicClient.ExchangeInfo.GetCommonCurrencyName(_base_id);
                        var _quote_name = publicClient.ExchangeInfo.GetCommonCurrencyName(_quote_id);

                        var _market_id = _base_name + "/" + _quote_name;

                        var _base_quote = _base_id + _quote_id;
                        if (_m.symbol == _base_quote)
                        {
                            _m.swap = true;
                            _m.type = "swap";
                        }
                        else
                        {
                            var _symbols = _m.symbol.Split('_');
                            if (_symbols.Length > 1)
                                _market_id = _symbols[0] + "/" + _symbols[1];
                            else
                                _market_id = _m.symbol.Substring(0, 3) + "/" + _m.symbol.Substring(3);

                            if (_m.symbol.IndexOf("B_") >= 0)
                            {
                                _m.prediction = true;
                                _m.type = "prediction";
                            }
                            else
                            {
                                _m.future = true;
                                _m.type = "future";
                            }
                        }

                        _m.marketId = _market_id;

                        _m.baseId = (_base_name != "BTC") ? _base_id : _m.settlCurrency;
                        _m.quoteId = (_quote_name != "BTC") ? _quote_id : _m.settlCurrency;
                        _m.baseName = _base_name;
                        _m.quoteName = _quote_name;

                        _m.lot = _m.lotSize;

                        _m.precision = new MarketPrecision()
                        {
                            quantity = Numerical.PrecisionFromString(Numerical.TruncateToString(_m.lotSize, 16)),
                            price = Numerical.PrecisionFromString(Numerical.TruncateToString(_m.tickSize, 16)),
                            amount = Numerical.PrecisionFromString(Numerical.TruncateToString(_m.tickSize, 16))
                        };

                        var _lot_size = _m.lotSize;
                        var _max_order_qty = _m.maxOrderQty;
                        var _tick_size = _m.tickSize;
                        var _max_price = _m.maxPrice;

                        _m.limits = new MarketLimits
                        {
                            quantity = new MarketMinMax
                            {
                                min = _lot_size,
                                max = _max_order_qty
                            },
                            price = new MarketMinMax
                            {
                                min = _tick_size,
                                max = _max_price
                            },
                            amount = new MarketMinMax
                            {
                                min = _lot_size * _tick_size,
                                max = _max_order_qty * _max_price
                            }
                        };

                        if (_m.initMargin != 0)
                            _m.maxLeverage = (int)(1 / _m.initMargin);

                        _result.result.Add(_m.marketId, _m);
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

                var _timevalue = (args != null && args.ContainsKey("timeframe") == true) ? args["timeframe"].ToString() : "1d";
                var _timeframe = publicClient.ExchangeInfo.GetTimeframe(_timevalue);
                var _timestamp = publicClient.ExchangeInfo.GetTimestamp(_timevalue);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("binSize", _timeframe);     // Time interval to bucket by. Available options: [1m,5m,1h,1d].
                    _params.Add("count", 1);
                    _params.Add("partial", false);          // If true, will send in-progress (incomplete) bins for the current time period.
                    _params.Add("reverse", true);           // If true, will sort results newest first.

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

                var _json_result = publicClient.GetResponseMessage();

                var _json_trade = await publicClient.CallApiGet1Async("/api/v1/trade/bucketed", _params);

                var _trade_result = publicClient.GetResponseMessage(_json_trade.Response);
                if (_trade_result.success == true)
                {
#if DEBUG
                    _result.rawJson = _json_trade.Content;
#endif
                    var _trade = (publicClient.DeserializeObject<List<BTickerItem>>(_json_trade.Content))[0];

                    var _json_quote = await publicClient.CallApiGet1Async("/api/v1/quote/bucketed", _params);

                    var _quote_result = publicClient.GetResponseMessage(_json_quote.Response);
                    if (_quote_result.success == true)
                    {
#if DEBUG
                        _result.rawJson += _json_quote.Content;
#endif
                        var _quote = (publicClient.DeserializeObject<List<BTickerItem>>(_json_quote.Content))[0];
                        {
                            _trade.bidPrice = _quote.bidPrice;
                            _trade.bidQuantity = _quote.bidQuantity;
                            _trade.askPrice = _quote.askPrice;
                            _trade.askQuantity = _quote.askQuantity;
                        }
                    }

                    _trade.symbol = _market.result.symbol;

                    _result.result = _trade;
                }
                else
                {
                    _json_result.SetResult(_trade_result);
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
        public override async Task<OrderBooks> FetchOrderBooks(string base_name, string quote_name, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new OrderBooks(base_name, quote_name);

            var _market = await this.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("depth", limits);

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

                var _json_value = await publicClient.CallApiGet1Async("/api/v1/orderBook/L2", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orderbooks = publicClient.DeserializeObject<List<BOrderBookItem>>(_json_value.Content);
                    {
                        var _asks = new List<IOrderBookItem>();
                        var _bids = new List<IOrderBookItem>();

                        foreach (var _o in _orderbooks)
                        {
                            _o.amount = _o.quantity * _o.price;
                            _o.count = 1;

                            if (_o.side.ToLower() == "sell")
                                _asks.Add(_o);
                            else
                                _bids.Add(_o);
                        }

                        _result.result.asks = _asks.OrderBy(o => o.price).Take(limits).ToList();
                        _result.result.bids = _bids.OrderByDescending(o => o.price).Take(limits).ToList();

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
        public override async Task<OHLCVs> FetchOHLCVs(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new OHLCVs(base_name, quote_name);

            var _market = await this.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _timeframe = publicClient.ExchangeInfo.GetTimeframe(timeframe);
                var _timestamp = publicClient.ExchangeInfo.GetTimestamp(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    var _limit = limits <= 1 ? 1
                               : limits <= 500 ? limits
                               : 500;

                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("binSize", _timeframe);
                    _params.Add("count", _limit);
                    _params.Add("partial", false);
                    _params.Add("reverse", true);

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

                var _json_value = await publicClient.CallApiGet1Async("/api/v1/trade/bucketed", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<List<BTickerItem>>(_json_value.Content);

                    _result.result.AddRange(
                         _json_data
                             .Select(x => new OHLCVItem
                             {
                                 timestamp = x.timestamp,
                                 openPrice = x.openPrice,
                                 highPrice = x.highPrice,
                                 lowPrice = x.lowPrice,
                                 closePrice = x.closePrice,
                                 volume = x.baseVolume
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
                               : limits <= 500 ? limits
                               : 500;

                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("count", limits);
                    _params.Add("reverse", true);

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

                var _json_value = await publicClient.CallApiGet1Async("/api/v1/trade", _params);
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