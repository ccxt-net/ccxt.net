using CCXT.NET.Coin;
using CCXT.NET.Coin.Public;
using CCXT.NET.Coin.Types;
using CCXT.NET.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.CoinCheck.Public
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
                    base.publicClient = new CoinCheckClient("public");

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
                var _markets = new List<MarketItem>()
                {
                    new MarketItem() { symbol = "btc_jpy", baseId = "btc", quoteId = "jpy" }, // the only real pair
                    //new MarketItem() { symbol= "eth_jpy", baseId= "eth", quoteId= "jpy" },
                    //new MarketItem() { symbol= "etc_jpy", baseId= "etc", quoteId= "jpy" },
                    //new MarketItem() { symbol= "dao_jpy", baseId= "dao", quoteId= "jpy" },
                    //new MarketItem() { symbol= "lsk_jpy", baseId= "lsk", quoteId= "jpy" },
                    //new MarketItem() { symbol= "fct_jpy", baseId= "fct", quoteId= "jpy" },
                    //new MarketItem() { symbol= "xmr_jpy", baseId= "xmr", quoteId= "jpy" },
                    //new MarketItem() { symbol= "rep_jpy", baseId= "rep", quoteId= "jpy" },
                    //new MarketItem() { symbol= "xrp_jpy", baseId= "xrp", quoteId= "jpy" },
                    //new MarketItem() { symbol= "zec_jpy", baseId= "zec", quoteId= "jpy" },
                    //new MarketItem() { symbol= "xem_jpy", baseId= "xem", quoteId= "jpy" },
                    //new MarketItem() { symbol= "ltc_jpy", baseId= "ltc", quoteId= "jpy" },
                    //new MarketItem() { symbol= "dash_jpy", baseId= "dash", quoteId= "jpy" },
                    //new MarketItem() { symbol= "eth_btc", baseId= "eth", quoteId= "btc" },
                    //new MarketItem() { symbol= "etc_btc", baseId= "etc", quoteId= "btc" },
                    //new MarketItem() { symbol= "lsk_btc", baseId= "lsk", quoteId= "btc" },
                    //new MarketItem() { symbol= "fct_btc", baseId= "fct", quoteId= "btc" },
                    //new MarketItem() { symbol= "xmr_btc", baseId= "xmr", quoteId= "btc" },
                    //new MarketItem() { symbol= "rep_btc", baseId= "rep", quoteId= "btc" },
                    //new MarketItem() { symbol= "xrp_btc", baseId= "xrp", quoteId= "btc" },
                    //new MarketItem() { symbol= "zec_btc", baseId= "zec", quoteId= "btc" },
                    //new MarketItem() { symbol= "xem_btc", baseId= "xem", quoteId= "btc" },
                    //new MarketItem() { symbol= "ltc_btc", baseId= "ltc", quoteId= "btc" },
                    //new MarketItem() { symbol= "dash_btc", baseId= "dash", quoteId= "btc" },
                };

                _markets.ForEach((m) =>
                {
                    var _base_name = publicClient.ExchangeInfo.GetCommonCurrencyName(m.baseId);
                    var _quote_name = publicClient.ExchangeInfo.GetCommonCurrencyName(m.quoteId);

                    m.baseName = _base_name;
                    m.quoteName = _quote_name;

                    m.marketId = _base_name + "/" + _quote_name;
                    m.active = true;
                });

                _result.result = _markets.ToDictionary(m => m.marketId, m => m as IMarketItem);

                var _json_result = publicClient.GetResponseMessage();
                {
                    _result.SetResult(_json_result);
                }
            }

            return await Task.FromResult(_result);
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

                var _params = publicClient.MergeParamsAndArgs(args);

                var _json_value = await publicClient.CallApiGet1Async("/api/ticker", _params);
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
        /// Fetch pending or registered order details
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="limit">maximum number of items (optional): default 20</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<OrderBooks> FetchOrderBooks(string base_name, string quote_name, int limit = 20, Dictionary<string, object> args = null)
        {
            var _result = new OrderBooks(base_name, quote_name);

            var _market = await this.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                publicClient.ExchangeInfo.ApiCallWait(TradeType.Public);

                var _params = publicClient.MergeParamsAndArgs(args);

                var _json_value = await publicClient.CallApiGet1Async("/api/order_books", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<COrderBook>(_json_value.Content);
                    {
                        var _orderbook = new OrderBook { asks = new List<IOrderBookItem>(), bids = new List<IOrderBookItem>() };

                        foreach (var _ask in _json_data.asks.OrderBy(o => o.price).Take(limit).ToList())
                        {
                            _ask.amount = _ask.price * _ask.quantity;
                            _ask.count = 1;

                            _orderbook.asks.Add(_ask);
                        }

                        foreach (var _bid in _json_data.bids.OrderByDescending(o => o.price).Take(limit).ToList())
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
        /// <param name="limit">maximum number of items (optional): default 20, max 100</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<CompleteOrders> FetchCompleteOrders(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limit = 20, Dictionary<string, object> args = null)
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
                    _params.Add("limit", limit);

                    publicClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await publicClient.CallApiGet1Async("/api/trades", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = publicClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = publicClient.DeserializeObject<CCompleteOrders>(_json_value.Content);
                    {
                        var _orders = _json_data.result
                                                .Where(t => t.timestamp >= since)
                                                .OrderByDescending(t => t.timestamp)
                                                .Take(limit);

                        foreach (var _o in _orders)
                        {
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