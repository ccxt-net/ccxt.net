using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Poloniex.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public class TradeApi : OdinSdk.BaseLib.Coin.Trade.TradeApi, ITradeApi
    {
        private readonly string __connect_key;
        private readonly string __secret_key;

        /// <summary>
        /// 
        /// </summary>
        public TradeApi(string connect_key, string secret_key)
        {
            __connect_key = connect_key;
            __secret_key = secret_key;
        }

        /// <summary>
        /// 
        /// </summary>
        public override XApiClient tradeClient
        {
            get
            {
                if (base.tradeClient == null)
                    base.tradeClient = new PoloniexClient("trade", __connect_key, __secret_key);

                return base.tradeClient;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override OdinSdk.BaseLib.Coin.Public.PublicApi publicApi
        {
            get
            {
                if (base.publicApi == null)
                    base.publicApi = new CCXT.NET.Poloniex.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Returns your open orders for a given market.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<MyOrders> FetchOpenOrders(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new MyOrders(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("command", "returnOpenOrders");
                    _params.Add("currencyPair", _market.result.symbol);

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

                var _json_value = await tradeClient.CallApiPost1Async("", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orders = tradeClient.DeserializeObject<List<PMyOrderItem>>(_json_value.Content);
                    {
                        foreach (var _o in _orders)
                        {
                            _o.orderStatus = OrderStatus.Open;
                            _o.orderType = OrderType.Limit;
                            _o.symbol = _market.result.symbol;

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

        /// <summary>
        /// Returns your open orders for all markets.
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<MyOrders> FetchAllOpenOrders(Dictionary<string, object> args = null)
        {
            var _result = new MyOrders();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("command", "returnOpenOrders");
                    _params.Add("currencyPair", "all");

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

                var _json_value = await tradeClient.CallApiPost1Async("", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orders = tradeClient.DeserializeObject<Dictionary<string, List<PMyOrderItem>>>(_json_value.Content);
                    {
                        foreach (var _o in _orders)
                        {
                            var _market = _markets.GetMarketBySymbol(_o.Key);
                            if (_market == null)
                                continue;

                            foreach (var _order in _o.Value)
                            {
                                _order.symbol = _market.symbol;

                                _order.orderStatus = OrderStatus.Open;
                                _order.orderType = OrderType.Limit;

                                _result.result.Add(_order);
                            }
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
        /// Returns your trade history for a given market.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limits">maximum number of items (optional): default 20</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<MyTrades> FetchMyTrades(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new MyTrades(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _timeframe = tradeClient.ExchangeInfo.GetTimeframe(timeframe);
                var _timestamp = tradeClient.ExchangeInfo.GetTimestamp(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("command", "returnTradeHistory");
                    _params.Add("currencyPair", _market.result.symbol);
                    _params.Add("start", since / 1000);
                    _params.Add("end", CUnixTime.Now);
                    _params.Add("limit", limits);

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

                var _json_value = await tradeClient.CallApiPost1Async("", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _trades = tradeClient.DeserializeObject<Dictionary<string, List<PMyTradeItem>>>(_json_value.Content);
                    {
                        foreach (var _t in _trades)
                        {
                            foreach (var _trade in _t.Value)
                            {
                                _trade.amount = _trade.quantity * _trade.price;
                                _result.result.Add(_trade);
                            }
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
        /// Places a limit order in a given market.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="price">fiat rate of coin</param>
        /// <param name="sideType">type of buy(bid) or sell(ask)</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<MyOrder> CreateLimitOrder(string base_name, string quote_name, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("command", sideType == SideType.Bid ? "buy" : "sell");
                    _params.Add("currencyPair", _market.result.symbol);
                    _params.Add("rate", price.ToStringNormalized());
                    _params.Add("amount", quantity.ToStringNormalized());

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

                var _json_value = await tradeClient.CallApiPost1Async("", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<PPlaceOrders>(_json_value.Content);
                    {
                        var _order = new PPlaceOrderItem
                        {
                            orderId = _json_data.orderId,
                            sideType = sideType,
                            orderType = OrderType.Limit,
                            orderStatus = OrderStatus.Open,
                            timestamp = _json_data.resultingTrades.Max(t => t.timestamp),

                            price = price,
                            quantity = quantity,
                            amount = quantity * price,

                            filled = 0m,
                            cost = 0m
                        };

                        foreach (var _trade in _json_data.resultingTrades)
                        {
                            _order.filled += _trade.quantity;
                            _order.cost += _trade.price * _trade.quantity;
                        }

                        _result.result = _order;
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
        /// Cancels an order you have placed in a given market.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="order_id">Order number registered for sale or purchase</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="price">fiat rate of coin</param>
        /// <param name="sideType">type of buy(bid) or sell(ask)</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<MyOrder> CancelOrder(string base_name, string quote_name, string order_id, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("command", "cancelOrder");
                    _params.Add("currencyPair", _market.result.symbol);
                    _params.Add("orderNumber", order_id);

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

                var _json_value = await tradeClient.CallApiPost1Async("", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<JToken>(_json_value.Content);
                    {
                        var _order = new MyOrderItem
                        {
                            orderId = order_id,
                            symbol = _market.result.symbol,

                            timestamp = CUnixTime.NowMilli,
                            orderStatus = OrderStatus.Canceled,
                            sideType = sideType,

                            quantity = quantity,
                            price = price,
                            amount = quantity * price
                        };

                        _result.result = _order;

                        var _status = _json_data["success"].Value<int>() == 1;
                        if (_status == false)
                            _json_result.SetFailure();
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