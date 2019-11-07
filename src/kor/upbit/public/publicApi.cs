using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Public;
using OdinSdk.BaseLib.Coin.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Upbit.Public
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
                    base.publicClient = new UpbitClient("public");

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

                var _json_value = await publicClient.CallApiGet1Async("/market/all", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _markets = publicClient.DeserializeObject<List<UMarketItem>>(_json_value.Content);
                    foreach (var _market in _markets)
                    {
                        var _symbol = _market.symbol;

                        _market.baseId = _symbol.Split('-')[1];
                        _market.quoteId = _symbol.Split('-')[0];

                        _market.baseName = publicClient.ExchangeInfo.GetCommonCurrencyName(_market.baseId);
                        _market.quoteName = publicClient.ExchangeInfo.GetCommonCurrencyName(_market.quoteId);

                        _market.marketId = _market.baseName + "/" + _market.quoteName;

                        _market.precision = new MarketPrecision
                        {
                            quantity = 8,
                            price = 8,
                            amount = 8
                        };

                        _market.lot = 1.0m;
                        _market.active = true;

                        _market.takerFee = 0.05m / 100;
                        _market.makerFee = 0.05m / 100;

                        _market.limits = new MarketLimits
                        {
                            quantity = new MarketMinMax
                            {
                                min = (decimal)Math.Pow(10, -_market.precision.quantity),
                                max = decimal.MaxValue
                            },
                            price = new MarketMinMax
                            {
                                min = (decimal)Math.Pow(10, -_market.precision.price),
                                max = decimal.MaxValue
                            },
                            amount = new MarketMinMax
                            {
                                min = _market.lot,
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
                    _params.Add("markets", _market.result.symbol);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("/ticker", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _tickers = publicClient.DeserializeObject<List<UTickerItem>>(_json_value.Content);
                    {
                        var _ticker = _tickers.FirstOrDefault();
                        if (_ticker != null)
                        {
                            _ticker.askPrice = _ticker.closePrice;
                            _ticker.askQuantity = _ticker.closeVolume;
                            _ticker.bidPrice = _ticker.closePrice;
                            _ticker.bidQuantity = _ticker.closeVolume;

                            _ticker.baseVolume = _ticker.totalQuantity24h;
                            _ticker.average = (_ticker.lastPrice + _ticker.openPrice) / 2;
                            _ticker.percentage = (_ticker.changePrice / _ticker.openPrice) * 100;

                            _result.result = _ticker;
                        }
                        else
                        {
                            _json_result.SetFailure();
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

                //var _symbols = new StringBuilder();
                //{
                //    foreach (var _market in _markets.result)
                //        _symbols.Append(_market.Value.symbol + ",");

                //    if (_symbols.Length > 0)
                //        _symbols.Length--;
                //}

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("markets", String.Join(",", _markets.result.Select(m => m.Value.symbol)));

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("/ticker", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _tickers = publicClient.DeserializeObject<List<UTickerItem>>(_json_value.Content);
                    {
                        foreach (var _ticker in _tickers)
                        {
                            _ticker.askPrice = _ticker.closePrice;
                            _ticker.askQuantity = _ticker.closeVolume;
                            _ticker.bidPrice = _ticker.closePrice;
                            _ticker.bidQuantity = _ticker.closeVolume;

                            _ticker.baseVolume = _ticker.totalQuantity24h;
                            _ticker.average = (_ticker.lastPrice + _ticker.openPrice) / 2;
                            _ticker.percentage = (_ticker.changePrice / _ticker.openPrice) * 100;

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
                    _params.Add("markets", _market.result.symbol);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("/orderbook", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orderbooks = publicClient.DeserializeObject<List<UOrderBook>>(_json_value.Content);
                    {
                        var _orderbook = _orderbooks.FirstOrDefault();
                        if (_orderbook != null)
                        {
                            _orderbook.asks = _orderbook.asks.OrderBy(o => o.price).Take(limits).ToList();
                            _orderbook.bids = _orderbook.bids.OrderByDescending(o => o.price).Take(limits).ToList();

                            _orderbook.nonce = _orderbook.timestamp / 1000;

                            _result.result = _orderbook;
                        }
                        else
                        {
                            _json_result.SetFailure();
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
        /// <param name="timeframe">time frame interval (optional): default "1d"
        /// <list type="timeframe">
        /// <item><description>Minute(1m, 3m, 5m, 10m, 15m, 30m, 60m, 240m)</description></item>
        /// <item><description>Day(1d)</description></item>
        /// <item><description>Week(1w)</description></item>
        /// <item><description>Month(1M)</description></item>
        /// </list>
        /// </param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limits">maximum number of items (optional): default 20</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns>OHLCVs</returns>
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
                    _params.Add("market", _market.result.symbol);
                    _params.Add("count", limits);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _end_point = _timeframe == "minutes"
                               ? $"/candles/{_timeframe}/{Convert.ToInt32(timeframe.Substring(0, timeframe.Length - 1))}"
                               : $"/candles/{_timeframe}";

                var _json_value = await publicClient.CallApiGet1Async(_end_point, _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<List<UOHLCVItem>>(_json_value.Content);
                    {
                        _result.result.AddRange(
                                    _json_data
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
                    _params.Add("market", _market.result.symbol);
                    _params.Add("count", limits);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async($"/trades/ticks", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<List<UCompleteOrderItem>>(_json_value.Content);
                    {
                        var _orders = _json_data
                                                .Where(t => t.timestamp >= since)
                                                .OrderByDescending(t => t.timestamp)
                                                .Take(limits);

                        foreach (var _o in _orders)
                        {
                            _o.transactionId = (_o.timestamp * 1000).ToString();

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