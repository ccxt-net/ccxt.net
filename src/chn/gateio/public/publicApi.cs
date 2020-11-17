using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Coin.Public;
using CCXT.NET.Shared.Coin.Types;
using CCXT.NET.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.GateIO.Public
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
                    base.publicClient = new GateIOClient("public");

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

                var _json_value = await publicClient.CallApiGet1Async("/1/marketinfo", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _markets = publicClient.DeserializeObject<GMarkets>(_json_value.Content);
                    foreach (var _m in _markets.result)
                    {
                        var _market = _m.First().Value;
                        _market.symbol = _m.First().Key;

                        _market.baseId = _market.symbol.Split('_')[0];
                        _market.quoteId = _market.symbol.Split('_')[1];

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

                        //_market.takerFee = 0.05m / 100;
                        //_market.makerFee = 0.05m / 100;

                        _market.limits = new MarketLimits
                        {
                            quantity = new MarketMinMax
                            {
                                min = (decimal)Math.Pow(10, -(double)_market.precision.quantity),
                                max = decimal.MaxValue
                            },
                            price = new MarketMinMax
                            {
                                min = (decimal)Math.Pow(10, -(double)_market.precision.price),
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
        public override async ValueTask<Ticker> FetchTickerAsync(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new Ticker(base_name, quote_name);

            var _market = await this.LoadMarketAsync(_result.marketId);
            if (_market.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = publicClient.MergeParamsAndArgs(args);

                var _json_value = await publicClient.CallApiGet1Async($"/1/ticker/{_market.result.symbol}", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _ticker = publicClient.DeserializeObject<GTickerItem>(_json_value.Content);
                    {
                        if (_ticker != null)
                        {
                            _ticker.symbol = _market.result.symbol;
                            // openPrice 제공안함
                            _ticker.average = (_ticker.lastPrice + _ticker.openPrice) / 2;
                            _ticker.timestamp = CUnixTime.NowMilli;
                            _ticker.vwap = _ticker.quoteVolume / _ticker.baseVolume;

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
        public override async ValueTask<Tickers> FetchTickersAsync(Dictionary<string, object> args = null)
        {
            var _result = new Tickers();

            var _markets = await this.LoadMarketsAsync();
            if (_markets.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = publicClient.MergeParamsAndArgs(args);

                var _json_value = await publicClient.CallApiGet1Async("/1/tickers", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _tickers = publicClient.DeserializeObject<Dictionary<string, GTickerItem>>(_json_value.Content);
                    {
                        foreach (var _t in _tickers)
                        {
                            var _ticker = _t.Value;

                            _ticker.symbol = _t.Key;
                            // openPrice 제공안함
                            _ticker.average = (_ticker.lastPrice + _ticker.openPrice) / 2;
                            _ticker.timestamp = CUnixTime.NowMilli;
                            if (_ticker.baseVolume != 0)
                                _ticker.vwap = _ticker.quoteVolume / _ticker.baseVolume;

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

                var _params = publicClient.MergeParamsAndArgs(args);

                var _json_value = await publicClient.CallApiGet1Async($"/1/orderBook/{_market.result.symbol}", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orderbooks = publicClient.DeserializeObject<GOrderBook>(_json_value.Content);
                    {
                        if (_orderbooks.asks.Count > 0 || _orderbooks.bids.Count > 0)
                        {
                            _orderbooks.symbol = _market.result.symbol;

                            _orderbooks.asks = _orderbooks.asks.OrderBy(o => o.price).Take(limits).ToList();
                            _orderbooks.bids = _orderbooks.bids.OrderByDescending(o => o.price).Take(limits).ToList();

                            _orderbooks.timestamp = CUnixTime.NowMilli;
                            _orderbooks.nonce = _orderbooks.timestamp / 1000;

                            _result.result = _orderbooks;
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
                    _params.Add("market", _market.result.symbol);
                    _params.Add("count", limits);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _url = "";
                if (limits <= 80)
                    _url = "/1/trade";
                else
                    _url = "/1/tradeHistory";

                var _json_value = await publicClient.CallApiGet1Async($"{_url}/{_market.result.symbol}", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<GCompleteOrders>(_json_value.Content);
                    {
                        var _orders = _json_data.result
                                                .Where(t => t.timestamp >= since)
                                                .OrderByDescending(t => t.timestamp)
                                                .Take(limits);

                        foreach (var _o in _orders)
                        {
                            _o.fillType = FillType.Fill;
                            //_o.orderType = OrderType.Limit;

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