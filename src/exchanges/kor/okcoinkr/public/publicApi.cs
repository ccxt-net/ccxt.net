using Newtonsoft.Json.Linq;
using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Coin.Public;
using CCXT.NET.Shared.Coin.Types;
using CCXT.NET.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.OkCoinKr.Public
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
                    base.publicClient = new OkCoinKrClient("public");

                return base.publicClient;
            }
        }

        private OkCoinKrClient __webpub_client = null;

        /// <summary>
        ///
        /// </summary>
        public OkCoinKrClient webpubClient
        {
            get
            {
                if (__webpub_client == null)
                    __webpub_client = new OkCoinKrClient("web");

                return __webpub_client;
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

                var _json_value = await webpubClient.CallApiGet1Async("/spot/markets/products", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _markets = publicClient.DeserializeObject<OMarkets>(_json_value.Content);
                    foreach (var _m in _markets.result)
                    {
                        var _symbol = _m.symbol;

                        _m.baseId = _symbol.Split('_')[0];
                        _m.quoteId = _symbol.Split('_')[1];

                        _m.baseName = publicClient.ExchangeInfo.GetCommonCurrencyName(_m.baseId);
                        _m.quoteName = publicClient.ExchangeInfo.GetCommonCurrencyName(_m.quoteId);

                        _m.marketId = _m.baseName + "/" + _m.quoteName;

                        _m.precision = new MarketPrecision
                        {
                            quantity = _m.maxSizeDigit,
                            price = _m.maxPriceDigit,
                            amount = _m.maxPriceDigit
                        };

                        _m.lot = (decimal)Math.Pow(10.0, -(double)_m.precision.quantity);
                        _m.active = _m.online != 0;

                        _m.takerFee = 0.05m / 100;
                        _m.makerFee = 0.05m / 100;

                        _m.type = "spot";
                        _m.spot = true;
                        _m.future = false;

                        _m.limits = new MarketLimits
                        {
                            quantity = new MarketMinMax
                            {
                                min = _m.minTradeSize,
                                max = decimal.MaxValue
                            },
                            price = new MarketMinMax
                            {
                                min = (decimal)Math.Pow(10.0, -(double)_m.precision.price),
                                max = decimal.MaxValue
                            },
                            amount = new MarketMinMax
                            {
                                min = _m.minTradeSize * (decimal)Math.Pow(10.0, -(double)_m.precision.price),
                                max = decimal.MaxValue
                            }
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

                var _json_value = await publicClient.CallApiGet1Async("/v1/ticker.do", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<OTicker>(_json_value.Content);
                    {
                        var _ticker = _json_data.result;

                        _ticker.symbol = _market.result.symbol;
                        _ticker.timestamp = _json_data.timestamp;

                        _ticker.askQuantity = _ticker.closeVolume;
                        _ticker.bidQuantity = _ticker.closeVolume;

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
            if (_market.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("size", limits);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("/v1/depth.do", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orderbook = publicClient.DeserializeObject<OOrderBook>(_json_value.Content);
                    {
                        _orderbook.asks = _orderbook.asks.OrderBy(o => o.price).Take(limits).ToList();
                        _orderbook.bids = _orderbook.bids.OrderByDescending(o => o.price).Take(limits).ToList();

                        _orderbook.symbol = _market.result.symbol;
                        _orderbook.timestamp = CUnixTime.NowMilli;
                        _orderbook.nonce = _orderbook.timestamp / 1000;

                        _result.result = _orderbook;
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

                var _timestamp = publicClient.ExchangeInfo.GetTimestamp(timeframe);
                var _timeframe = publicClient.ExchangeInfo.GetTimeframe(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("type", _timeframe);
                    _params.Add("size", limits);
                    if (since > 0)
                        _params.Add("since", since);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("/v1/kline.do", _params);
#if DEBUG
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
                        _params.Add("since", since);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async($"/v1/trades.do", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<List<OCompleteOrderItem>>(_json_value.Content);
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