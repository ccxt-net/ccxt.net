using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.OKEx.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class TradeApi : OdinSdk.BaseLib.Coin.Trade.TradeApi, ITradeApi
    {
        private readonly string __connect_key;
        private readonly string __secret_key;
        private readonly string __user_name;
        private readonly string __user_password;

        /// <summary>
        ///
        /// </summary>
        public TradeApi(string connect_key, string secret_key, string user_name, string user_password)
        {
            __connect_key = connect_key;
            __secret_key = secret_key;
            __user_name = user_name;
            __user_password = user_password;
        }

        /// <summary>
        ///
        /// </summary>
        public override XApiClient tradeClient
        {
            get
            {
                if (base.tradeClient == null)
                    base.tradeClient = new OKExClient("trade", __connect_key, __secret_key, __user_name, __user_password);

                return base.tradeClient;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public OKExClient okexClient
        {
            get
            {
                return tradeClient as OKExClient;
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
                    base.publicApi = new CCXT.NET.OKEx.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Get Order Information in Batch
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="order_id">Order number registered for sale or purchase</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<MyOrder> FetchMyOrder(string base_name, string quote_name, string order_id, Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                okexClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("order_id", order_id);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _end_point = okexClient.CheckFuturesUrl(_market.result, "/order_info.do", "/future_order_info.do", _params);

                var _json_value = await okexClient.CallApiPost1Async(_end_point.endPoint, _end_point.args);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = okexClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = okexClient.DeserializeObject<OMyOrders>(_json_value.Content);
                    if (_json_data.success == true)
                    {
                        var _orders = _json_data.result.Where(o => o.orderId == order_id);
                        if (_orders.Count() > 0)
                        {
                            foreach (var _o in _orders)
                            {
                                _o.amount = _o.price * _o.quantity;
                                _result.result = _o;
                            }
                        }
                        else
                        {
                            _json_result.SetFailure("Order not found", ErrorCode.OrderNotFound);
                        }
                    }
                    else
                    {
                        _json_result.SetResult(_json_data);
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
        /// Get all open orders on a symbol. Careful when accessing this with no symbol.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<MyOrders> FetchOpenOrders(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new MyOrders(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                okexClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("order_id", -1); // if order_id is -1, then return all unfilled orders, otherwise return the order specified

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _end_point = okexClient.CheckFuturesUrl(_market.result, "/order_info.do", "/future_order_info.do", _params);

                var _json_value = await okexClient.CallApiPost1Async(_end_point.endPoint, _end_point.args);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = okexClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = okexClient.DeserializeObject<OMyOrders>(_json_value.Content);
                    if (_json_data.success == true)
                    {
                        var _orders = _json_data.result
                                            .Where(o => o.symbol == _market.result.symbol)
                                            .OrderByDescending(o => o.timestamp);

                        foreach (var _o in _orders)
                        {
                            _o.amount = _o.price * _o.quantity;
                            _result.result.Add(_o);
                        }
                    }
                    else
                    {
                        _json_result.SetResult(_json_data);
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
        /// Get trades for a specific account and symbol.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limits">maximum number of items (optional): default 20</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<MyTrades> FetchMyTrades(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new MyTrades(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                okexClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _timeframe = okexClient.ExchangeInfo.GetTimeframe(timeframe);
                var _timestamp = okexClient.ExchangeInfo.GetTimestamp(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("status", 1); // query type: 0 for unfilled (open) orders, 1 for filled orders
                    _params.Add("current_page", 1);
                    _params.Add("page_length", 200);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _end_point = okexClient.CheckFuturesUrl(_market.result, "/order_history.do", "/future_order_history.do", _params);

                var _json_value = await okexClient.CallApiPost1Async(_end_point.endPoint, _end_point.args);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = okexClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = okexClient.DeserializeObject<OMyTrades>(_json_value.Content);
                    if (_json_data.success == true)
                    {
                        var _trades = _json_data.result
                                            .Where(t => t.timestamp >= since)
                                            .OrderByDescending(t => t.timestamp)
                                            .Take(limits);

                        foreach (var _t in _trades)
                        {
                            _t.tradeId = _t.timestamp.ToString(); // tradeId 제공 안함
                            _t.amount = _t.price * _t.quantity;

                            _result.result.Add(_t);
                        }
                    }
                    else
                    {
                        _json_result.SetResult(_json_data);
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
        /// Place Limit Order.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="price">price of coin</param>
        /// <param name="sideType">type of buy(bid) or sell(ask)</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<MyOrder> CreateLimitOrder(string base_name, string quote_name, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                okexClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _buy_sell = sideType == SideType.Bid ? "buy" : "sell";

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("type", _buy_sell);
                    _params.Add("amount", quantity);
                    _params.Add("price", price);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _end_point = okexClient.CheckFuturesUrl(_market.result, "/trade.do", "/future_trade.do", _params);

                var _json_value = await okexClient.CallApiPost1Async(_end_point.endPoint, _end_point.args);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = okexClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = okexClient.DeserializeObject<OPlaceOrderItem>(_json_value.Content);

                    if (_json_data.result == true)
                    {
                        var _order = new OPlaceOrderItem
                        {
                            orderId = _json_data.orderId.ToUpper(),
                            timestamp = CUnixTime.NowMilli,

                            orderType = OrderType.Limit,
                            orderStatus = OrderStatus.Open,
                            sideType = sideType,

                            price = price,
                            quantity = quantity,
                            amount = quantity * price,
                            //fee = quantity * price * publicApi.ExchangeInfo.Fees.trading.maker
                        };

                        _result.result = _order;
                    }
                    else
                        _result.SetFailure();
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
        /// Place Market Order.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="price">price of coin</param>
        /// <param name="sideType">type of buy(bid) or sell(ask)</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<MyOrder> CreateMarketOrder(string base_name, string quote_name, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                okexClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _buy_sell = sideType == SideType.Bid ? "buy_market" : "sell_market";

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("type", _buy_sell);
                    _params.Add("amount", quantity);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _end_point = okexClient.CheckFuturesUrl(_market.result, "/trade.do", "/future_trade.do", _params);

                var _json_value = await okexClient.CallApiPost1Async(_end_point.endPoint, _end_point.args);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = okexClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = okexClient.DeserializeObject<OPlaceOrderItem>(_json_value.Content);

                    if (_json_data.result == true)
                    {
                        var _order = new OPlaceOrderItem
                        {
                            orderId = _json_data.orderId.ToUpper(),
                            timestamp = CUnixTime.NowMilli,

                            orderType = OrderType.Market,
                            orderStatus = OrderStatus.Open,
                            sideType = sideType,

                            price = price,
                            quantity = quantity,
                            amount = quantity * price,
                            //fee = quantity * price * publicApi.ExchangeInfo.Fees.trading.maker
                        };

                        _result.result = _order;
                    }
                    else
                        _result.SetFailure();
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
        /// Cancel Order
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="order_id">Order number registered for sale or purchase</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="price">price of coin</param>
        /// <param name="sideType">type of buy(bid) or sell(ask)</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<MyOrder> CancelOrder(string base_name, string quote_name, string order_id, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                okexClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("order_id", Convert.ToInt64(order_id));

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _end_point = okexClient.CheckFuturesUrl(_market.result, "/cancel_order.do", "/future_cancel.do", _params);

                var _json_value = await okexClient.CallApiPost1Async(_end_point.endPoint, _end_point.args);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = okexClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = okexClient.DeserializeObject<OCancelOrderItem>(_json_value.Content);
                    {
                        var _order_id = "";

                        var _order_status = OrderStatus.Unknown;
                        {
                            if (String.IsNullOrEmpty(_json_data.success) == false)
                            {
                                _order_id = _json_data.success;
                                _order_status = OrderStatus.Canceled;
                            }
                            else if (String.IsNullOrEmpty(_json_data.failure) == false)
                            {
                                _order_id = _json_data.failure;
                                _order_status = OrderStatus.Unknown;
                            }
                        }

                        if (String.IsNullOrEmpty(_order_id) == false)
                        {
                            var _order = new OMyOrderItem
                            {
                                symbol = _market.result.symbol,
                                orderId = _order_id,
                                orderType = OrderType.Limit,
                                orderStatus = _order_status,
                                sideType = sideType,
                                quantity = quantity,
                                price = price,
                                amount = quantity * price,
                                //fee = quantity * price * publicApi.ExchangeInfo.Fees.trading.maker,
                                timestamp = CUnixTime.NowMilli
                            };

                            _result.result = _order;
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
        /// Cancel orders. Send multiple order IDs to cancel in bulk.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="order_ids">order ID (multiple orders are separated by a comma ',', Max of 3 orders are allowed per request)</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<MyOrders> CancelOrders(string base_name, string quote_name, string[] order_ids, Dictionary<string, object> args = null)
        {
            var _result = new MyOrders(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                okexClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("order_id", String.Join(",", order_ids));

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _end_point = okexClient.CheckFuturesUrl(_market.result, "/cancel_order.do", "/future_cancel.do", _params);

                var _json_value = await okexClient.CallApiPost1Async(_end_point.endPoint, _end_point.args);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = okexClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = okexClient.DeserializeObject<OCancelOrderItem>(_json_value.Content);
                    {
                        var _order_success = _json_data.success.Split(',');
                        foreach (var _o in _order_success)
                        {
                            _result.result.Add(new OMyOrderItem
                            {
                                orderId = _o,
                                symbol = _market.result.symbol,
                                timestamp = CUnixTime.NowMilli,

                                orderStatus = OrderStatus.Canceled,
                                orderType = OrderType.Limit
                            });
                        }

                        var _order_failure = _json_data.failure.Split(',');
                        foreach (var _o in _order_success)
                        {
                            _result.result.Add(new OMyOrderItem
                            {
                                orderId = _o,
                                symbol = _market.result.symbol,
                                timestamp = CUnixTime.NowMilli,

                                orderStatus = OrderStatus.Unknown,
                                orderType = OrderType.Limit
                            });
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