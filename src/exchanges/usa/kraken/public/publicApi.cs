using CCXT.NET.Coin;
using CCXT.NET.Coin.Public;
using CCXT.NET.Coin.Types;
using CCXT.NET.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Kraken.Public
{
    /// <summary>
    /// exchange's public API implement class
    /// </summary>
    public class PublicApi : CCXT.NET.Coin.Public.PublicApi, IPublicApi
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
                    base.publicClient = new KrakenClient("public");

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
                var _params = publicClient.MergeParamsAndArgs(args);

                var _json_value = await publicClient.CallApiGet1Async("/0/public/AssetPairs", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _markets = publicClient.DeserializeObject<KResponse<Dictionary<string, KMarketItem>>>(_json_value.Content);

                    foreach (var _m in _markets.result)
                    {
                        var _market = _m.Value;
                        _market.symbol = _m.Key;

                        _market.baseId = _market.baseLongName;
                        if ((_market.baseId.Substring(0, 1) == "X" || _market.baseId.Substring(0, 1) == "Z") && _market.baseId.Length > 3)
                            _market.baseId = _market.baseId.Substring(1, 3);

                        _market.quoteId = _market.quoteLongName;
                        if (_market.quoteId.Substring(0, 1) == "X" || _market.quoteId.Substring(0, 1) == "Z")
                            _market.quoteId = _market.quoteId.Substring(1, 3);

                        _market.baseName = publicClient.ExchangeInfo.GetCommonCurrencyName(_market.baseId);
                        _market.quoteName = publicClient.ExchangeInfo.GetCommonCurrencyName(_market.quoteId);
                        var _market_id = _market.baseName + "/" + _market.quoteName;

                        _market.dark_pool = _m.Key.Contains(".d");
                        _market.marketId = _market.dark_pool ? _market.altname : _market_id;

                        if (_market.fees_maker != null)
                            _market.makerFee = _market.fees_maker[0][1].Value<decimal>() / 100;
                        if (_market.fees != null)
                            _market.takerFee = _market.fees[0][1].Value<decimal>() / 100;

                        _market.precision = new MarketPrecision
                        {
                            quantity = _market.lot_decimals,
                            price = _market.pair_decimals,
                            amount = _market.pair_decimals
                        };

                        _market.lot = (decimal)(-1.0 * Math.Log10(_market.precision.quantity));
                        _market.active = true;

                        _market.limits = new MarketLimits
                        {
                            quantity = new MarketMinMax
                            {
                                min = (decimal)Math.Pow(10, -_market.precision.quantity), // minAmount
                                max = (decimal)Math.Pow(10, _market.precision.quantity)
                            },
                            price = new MarketMinMax
                            {
                                min = (decimal)Math.Pow(10, -_market.precision.price),
                                max = decimal.MaxValue
                            },
                            amount = new MarketMinMax
                            {
                                min = 0,
                                max = decimal.MaxValue
                            }
                        };

                        _result.result.Add(_market.marketId, _market);
                    }
                }

                _result.SetResult(_json_result);
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

                //var _asset_pairs = new StringBuilder();
                //{
                //    foreach (var _market in _markets.result)
                //        _asset_pairs.Append($"{_market.Value.symbol},");
                //    _asset_pairs.Length--;
                //}

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("pair", String.Join(",", _markets.result.Select(m => m.Value.symbol)));

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("/0/public/Ticker", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<JObject>(_json_value.Content);
                    {
                        var _tickers = publicClient.DeserializeObject<Dictionary<string, JToken>>(_json_data["result"].ToString());

                        foreach (var _t in _tickers)
                        {
                            var _ticker = publicClient.DeserializeObject<KTickerItem>(_t.Value.ToString());

                            _ticker.symbol = _t.Key;
                            _ticker.timestamp = CUnixTime.NowMilli;

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
        public override async Task<OrderBooks> FetchOrderBooks(string base_name, string quote_name, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new OrderBooks(base_name, quote_name);

            var _market = await this.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("pair", _market.result.symbol);
                    _params.Add("count", limits);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("/0/public/Depth", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<JObject>(_json_value.Content);
                    {
                        var _orderbook = publicClient.DeserializeObject<KOrderBook>(_json_data["result"][_market.result.symbol].ToString());
                        {
                            _result.result.asks = _orderbook.asks.OrderBy(o => o.price).Take(limits).ToList();
                            _result.result.bids = _orderbook.bids.OrderByDescending(o => o.price).Take(limits).ToList();

                            _result.result.symbol = _market.result.symbol;
                            _result.result.timestamp = CUnixTime.NowMilli;
                            _result.result.nonce = CUnixTime.Now;
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
                    _params.Add("pair", _market.result.symbol);
                    _params.Add("interval", _timeframe);

                    if (since > 0)
                        _params.Add("since", since / 1000);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("/0/public/OHLC", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<JObject>(_json_value.Content);
                    var _json_ohlcvs = JArray.FromObject((_json_data["result"] as JObject)[_market.result.symbol]);

                    _result.result.AddRange(
                         _json_ohlcvs
                             .Select(x => new OHLCVItem
                             {
                                 timestamp = x[0].Value<long>() * 1000,
                                 openPrice = x[1].Value<decimal>(),
                                 highPrice = x[2].Value<decimal>(),
                                 lowPrice = x[3].Value<decimal>(),
                                 closePrice = x[4].Value<decimal>(),
                                 volume = x[6].Value<decimal>()
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
                    _params.Add("pair", _market.result.symbol);

                    if (since > 0)
                        _params.Add("since", since * 1000000);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("/0/public/Trades", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<JObject>(_json_value.Content);
                    {
                        var _json_orders = JArray.FromObject((_json_data["result"] as JObject)[_market.result.symbol]);

                        var _orders = new List<KCompleteOrderItem>();
                        foreach (var _o in _json_orders)
                        {
                            var _sideValue = _o[3].Value<string>();
                            var _orderValue = _o[4].Value<string>();
                            var _timeValue = (long)(_o[2].Value<decimal>() * 1000m);
                            var _price = _o[0].Value<decimal>();
                            var _quantity = _o[1].Value<decimal>();

                            _orders.Add(new KCompleteOrderItem
                            {
                                transactionId = (_timeValue * 1000).ToString(),
                                timestamp = _timeValue,

                                sideType = SideTypeConverter.FromString(_sideValue),
                                orderType = OrderTypeConverter.FromString(_orderValue),

                                price = _price,
                                quantity = _quantity,
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