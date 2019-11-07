using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Public;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Bitforex.Public
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
                    base.publicClient = new BitforexClient("public");

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

                var _json_value = await publicClient.CallApiGet1Async("/api/v1/market/symbols", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_markets = publicClient.DeserializeObject<BMarkets>(_json_value.Content);

                    foreach (var _market in _json_markets.result.ToDictionary(m => m.symbol, m => m))
                    {
                        var _m = _market.Value as BMarketItem;

                        var _split_symbol = _m.symbol.Split('-');
                        if (_split_symbol.Length != 3)
                            continue;

                        _m.baseId = _split_symbol[2];
                        _m.quoteId = _split_symbol[1];

                        _m.baseName = publicClient.ExchangeInfo.GetCommonCurrencyName(_m.baseId);
                        _m.quoteName = publicClient.ExchangeInfo.GetCommonCurrencyName(_m.quoteId);
                        _m.marketId = _m.baseName + "/" + _m.quoteName;

                        _m.lot = (decimal)Math.Pow(10.0, -_m.precision.quantity);
                        _m.active = true;

                        _m.limits.quantity.max = decimal.MaxValue;
                        _m.limits.price.min = (decimal)Math.Pow(10.0, -_m.precision.price);
                        _m.limits.price.max = decimal.MaxValue;

                        _m.limits.amount = new MarketMinMax
                        {
                            min = _m.limits.quantity.min * _m.limits.price.min,
                            max = decimal.MaxValue
                        };

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
        public override async ValueTask<Ticker> FetchTicker(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new Ticker(base_name, quote_name);

            var _market = await this.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("/api/v1/market/ticker", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _ticker = publicClient.DeserializeObject<BTicker>(_json_value.Content);
                    {
                        _ticker.result.symbol = _market.result.symbol;

                        _result.result = _ticker.result;
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
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("size", limits); // type required buy, sell or both to identify the type of orderbook to return

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("/api/v1/market/depth", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orderbooks = publicClient.DeserializeObject<BOrderBooks>(_json_value.Content);
                    if (_orderbooks.success == true)
                    {
                        var _ob_asks = _orderbooks.result.asks.OrderBy(o => o.price).Take(limits).ToList();
                        var _ob_bids = _orderbooks.result.bids.OrderByDescending(o => o.price).Take(limits).ToList();

                        var _orderbook = new OrderBook
                        {
                            asks = new List<OrderBookItem>(),
                            bids = new List<OrderBookItem>()
                        };

                        foreach (var _ask in _ob_asks)
                        {
                            _ask.amount = _ask.price * _ask.quantity;
                            _ask.count = 1;

                            _orderbook.asks.Add(_ask);
                        }

                        foreach (var _bid in _ob_bids)
                        {
                            _bid.amount = _bid.price * _bid.quantity;
                            _bid.count = 1;

                            _orderbook.bids.Add(_bid);
                        }

                        _result.result = _orderbook;

                        _result.result.symbol = _market.result.symbol;
                        _result.result.timestamp = CUnixTime.NowMilli;
                        _result.result.nonce = CUnixTime.Now;
                    }
                    else
                    {
                        _json_result.SetFailure(_orderbooks.message);
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

                var _timeframe = publicClient.ExchangeInfo.GetTimeframe(timeframe);
                var _timestamp = publicClient.ExchangeInfo.GetTimestamp(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("ktype", _timeframe);
                    _params.Add("size", limits);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("/api/v1/market/kline", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _ohlcvs = publicClient.DeserializeObject<BOHLCVs>(_json_value.Content);
                    if (_ohlcvs.success == true)
                    {
                        _result.result.AddRange(
                                    _ohlcvs.result
                                        .Where(o => o.timestamp >= since)
                                        .OrderByDescending(o => o.timestamp)
                                        .Take(limits)
                                );
                    }
                    else
                    {
                        _json_result.SetFailure(_ohlcvs.message);
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
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("size", limits);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("/api/v1/market/trades", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<BCompleteOrders>(_json_value.Content);
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