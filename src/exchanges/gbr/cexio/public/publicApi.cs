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

namespace CCXT.NET.CEXIO.Public
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
                    base.publicClient = new CexioClient("public");

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

                var _json_value = await publicClient.CallApiGet1Async("/currency_limits", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<JObject>(_json_value.Content);

                    var _pairs = _json_data["data"]["pairs"];
                    foreach (var _market in _pairs)
                    {
                        var _base_id = _market["symbol1"].ToString();
                        var _quote_id = _market["symbol2"].ToString();
                        var _symbol = _base_id + "/" + _quote_id;
                        var _base_name = publicClient.ExchangeInfo.GetCommonCurrencyName(_base_id);
                        var _quote_name = publicClient.ExchangeInfo.GetCommonCurrencyName(_quote_id);
                        var _market_id = _base_name + "/" + _quote_name;

                        var _lot = _market["minLotSize"].Value<decimal>();
                        var _maxLot = (_market["maxLotSize"].HasValues) ? _market["maxLotSize"].Value<decimal>() : decimal.MaxValue;

                        var _precision = new MarketPrecision
                        {
                            quantity = (int)(-1 * Math.Log10((double)_lot)),
                            price = Numerical.PrecisionFromString(_market["minPrice"].Value<string>()),
                            amount = Numerical.PrecisionFromString(_market["minPrice"].Value<string>())
                        };

                        var _limits = new MarketLimits
                        {
                            quantity = new MarketMinMax
                            {
                                min = _market["minLotSize"].Value<decimal>(),
                                max = _maxLot
                            },
                            price = new MarketMinMax
                            {
                                min = _market["minPrice"].Value<decimal>(),
                                max = _market["maxPrice"].Value<decimal>()
                            },
                            amount = new MarketMinMax
                            {
                                min = _market["minLotSizeS2"].Value<decimal>(),
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
        public override async ValueTask<Ticker> FetchTickerAsync(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new Ticker(base_name, quote_name);

            var _market = await this.LoadMarketAsync(_result.marketId);
            if (_market.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = publicClient.MergeParamsAndArgs(args);

                var _json_value = await publicClient.CallApiGet1Async($"/ticker/{_market.result.symbol}", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _ticker = publicClient.DeserializeObject<CTickerItem>(_json_value.Content);
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

                var _currency_pairs = "";
                foreach (var _market in _markets.result.GroupBy(m => m.Value.quoteName))
                {
                    if (String.IsNullOrEmpty(_currency_pairs) == false)
                        _currency_pairs += "/";
                    _currency_pairs += _market.Key;
                }

                var _json_value = await publicClient.CallApiGet1Async($"/tickers/{_currency_pairs}", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<CTickers>(_json_value.Content);
                    if (_json_data.success == true)
                    {
                        foreach (var _ticker in _json_data.result)
                        {
                            _ticker.symbol = _ticker.symbol.Replace(':', '/');

                            _result.result.Add(_ticker);
                        }
                    }
                    else
                    {
                        _json_result.SetFailure(_json_data.message);
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
                    if (limits > 0)
                        _params.Add("depth", limits);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async($"/order_book/{_market.result.symbol}/", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orderbook = publicClient.DeserializeObject<COrderBook>(_json_value.Content);
                    {
                        _result.result.asks = _orderbook.asks.OrderBy(o => o.price).Take(limits).ToList();
                        _result.result.bids = _orderbook.bids.OrderByDescending(o => o.price).Take(limits).ToList();

                        _result.result.symbol = _market.result.symbol;
                        _result.result.timestamp = _orderbook.timestamp;
                        _result.result.nonce = _orderbook.id;
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

                var _params = publicClient.MergeParamsAndArgs(args);

                var _yesterday = CUnixTime.UtcNow.AddDays(-1).ToString("yyyyMMdd");
                var _dataframe = $"data{_timeframe}";

                var _json_value = await publicClient.CallApiGet1Async($"/ohlcv/hd/{_yesterday}/{_market.result.symbol}", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<Dictionary<string, JToken>>(_json_value.Content);
                    if (_json_data.ContainsKey(_dataframe) == true)
                    {
                        var _candles = publicClient.DeserializeObject<JArray>(_json_data[_dataframe].ToString());

                        _result.result.AddRange(
                             _candles
                                 .Select(x => new OHLCVItem
                                 {
                                     timestamp = x[0].Value<long>() * 1000,
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
                    else
                    {
                        _json_result.SetFailure($"not exist '{_timeframe}' timeframe", ErrorCode.NotFoundData);
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

                var _params = publicClient.MergeParamsAndArgs(args);

                var _json_value = await publicClient.CallApiGet1Async($"/trade_history/{_market.result.symbol}/", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<List<CCompleteOrderItem>>(_json_value.Content);
                    {
                        var _orders = _json_data
                                            .Where(t => t.timestamp >= since)
                                            .OrderByDescending(t => t.timestamp)
                                            .Take(limits);

                        foreach (var _t in _orders)
                        {
                            _t.orderType = OrderType.Limit;
                            _t.fillType = FillType.Fill;

                            _t.amount = _t.quantity * _t.price;
                            _result.result.Add(_t);
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