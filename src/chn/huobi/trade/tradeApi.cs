using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using OdinSdk.BaseLib.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Huobi.Trade
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
                    base.tradeClient = new HuobiClient("trade", __connect_key, __secret_key);

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
                    base.publicApi = new CCXT.NET.Huobi.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Get the status of an order. Is it active? Was it cancelled? To what extent has it been executed? etc.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="order_id">Order number registered for sale or purchase</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<MyOrder> FetchMyOrder(string base_name, string quote_name, string order_id = "", Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = tradeClient.MergeParamsAndArgs(args);

                var _json_value = await tradeClient.CallApiGet1Async($"/v1/order/orders/{order_id}", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orderItem = tradeClient.DeserializeObject<HMyOrder>(_json_value.Content);
                    {
                        _orderItem.result.amount = _orderItem.result.price * _orderItem.result.quantity;
                        _result.result = _orderItem.result;
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
        /// View your latest inactive orders.
        /// Limited to last 3 days and 1 request per minute.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limits">maximum number of items (optional): default 20</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<MyOrders> FetchMyOrders(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new MyOrders(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _timeframe = tradeClient.ExchangeInfo.GetTimeframe(timeframe);
                var _timestamp = tradeClient.ExchangeInfo.GetTimestamp(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("states", "pre-submitted,submitted,partial-filled,partial-canceled,filled,canceled");

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiGet1Async("/v1/order/orders", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<HMyOrders>(_json_value.Content);
                    {
                        var _orders = _json_data.result
                                            .Where(o => o.symbol == _market.result.symbol && o.timestamp >= since)
                                            .OrderByDescending(o => o.timestamp)
                                            .Take(limits);

                        foreach (var _o in _orders)
                        {
                            _o.amount = _o.price * _o.quantity;
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
        /// Get all orders that you currently have opened. A specific market can be requested.
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
                if (args.ContainsKey("account-id") && args["account-id"].ToString() != "")
                {
                    tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                    var _params = new Dictionary<string, object>();
                    {
                        _params.Add("account-id", args["account-id"].ToString());
                        _params.Add("symbol", _market.result.symbol);

                        tradeClient.MergeParamsAndArgs(_params, args);
                    }

                    var _json_value = await tradeClient.CallApiGet1Async("/v1/order/openOrders", _params);
#if RAWJSON
                    _result.rawJson = _json_value.Content;
#endif
                    var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _orders = tradeClient.DeserializeObject<HMyOrders>(_json_value.Content);
                        {
                            foreach (var _order in _orders.result)
                            {
                                _order.orderStatus = OrderStatus.Open;
                                //_order.filled = _order.quantity - _order.remaining;
                                _order.amount = _order.price * _order.quantity;

                                _result.result.Add(_order);
                            }
                        }
                    }

                    _result.SetResult(_json_result);
                }
                else
                {
                    _result.SetFailure("required args[account-id]");
                }
            }
            else
            {
                _result.SetResult(_market);
            }

            return _result;
        }

        /// <summary>
        /// View your past trades.
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
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _timeframe = tradeClient.ExchangeInfo.GetTimeframe(timeframe);
                var _timestamp = tradeClient.ExchangeInfo.GetTimestamp(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("states", "filled");

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiGet1Async("/v1/order/orders", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<HMyTrades>(_json_value.Content);
                    {
                        var _trades = _json_data.result
                                            .Where(t => t.timestamp >= since)
                                            .OrderByDescending(t => t.timestamp)
                                            .Take(limits);

                        foreach (var _t in _trades)
                        {
                            _t.amount = _t.price * _t.quantity;
                            _t.tradeId = _t.timestamp.ToString(); // tradeId 제공 안함
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

        /// <summary>
        /// Submit a new Order
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
                if (args.ContainsKey("account-id") && args["account-id"].ToString() != "")
                {
                    tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                    var _buy_sell = sideType == SideType.Bid ? "buy" : "sell";

                    var _params = new Dictionary<string, object>();
                    {
                        _params.Add("account-id", args["account-id"].ToString());
                        _params.Add("amount", quantity.ToString());
                        _params.Add("price", price.ToString());
                        //_params.Add("source", ); // 'api' for spot trade and 'margin-api' for margin trade
                        _params.Add("symbol", _market.result.symbol);
                        _params.Add("type", _buy_sell + "-limit");

                        tradeClient.MergeParamsAndArgs(_params, args);
                    }

                    var _json_value = await tradeClient.CallApiPost1Async($"/v1/order/orders/place", _params);
#if RAWJSON
                    _result.rawJson = _json_value.Content;
#endif
                    var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _order = tradeClient.DeserializeObject<HPlaceOrder>(_json_value.Content);
                        {
                            _order.result.orderType = OrderType.Limit;
                            _order.result.price = price;
                            _order.result.quantity = quantity;
                            _order.result.sideType = sideType;
                            _order.result.symbol = _market.result.symbol;
                            _order.result.timestamp = CUnixTime.NowMilli;
                            _order.result.amount = (_order.result.quantity * _order.result.price).Normalize();
                            _order.result.fee = _order.result.amount * tradeClient.ExchangeInfo.Fees.trading.maker;

                            _result.result = _order.result;
                        }
                    }

                    _result.SetResult(_json_result);
                }
                else
                {
                    _result.SetFailure("required args[account-id]");
                }
            }
            else
            {
                _result.SetResult(_market);
            }

            return _result;
        }

        /// <summary>
        /// Submit a new Order
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
                if (args.ContainsKey("account-id") && args["account-id"].ToString() != "")
                {
                    tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                    var _buy_sell = sideType == SideType.Bid ? "buy" : "sell";

                    var _params = new Dictionary<string, object>();
                    {
                        _params.Add("account-id", args["account-id"].ToString());
                        _params.Add("amount", quantity.ToString());
                        //_params.Add("source", ); // 'api' for spot trade and 'margin-api' for margin trade
                        _params.Add("symbol", _market.result.symbol);
                        _params.Add("type", _buy_sell + "-market");

                        tradeClient.MergeParamsAndArgs(_params, args);
                    }

                    var _json_value = await tradeClient.CallApiPost1Async($"/v1/order/orders/place", _params);
#if RAWJSON
                    _result.rawJson = _json_value.Content;
#endif
                    var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _order = tradeClient.DeserializeObject<HPlaceOrder>(_json_value.Content);
                        {
                            _order.result.orderType = OrderType.Market;
                            //_order.result.price = price;
                            _order.result.quantity = quantity;
                            _order.result.sideType = sideType;
                            _order.result.symbol = _market.result.symbol;
                            _order.result.timestamp = CUnixTime.NowMilli;
                            //_order.result.amount = (_order.result.quantity * _order.result.price).Normalize();
                            _order.result.fee = _order.result.amount * tradeClient.ExchangeInfo.Fees.trading.maker;

                            _result.result = _order.result;
                        }
                    }

                    _result.SetResult(_json_result);
                }
                else
                {
                    _result.SetFailure("required args[account-id]");
                }
            }
            else
            {
                _result.SetResult(_market);
            }

            return _result;
        }

        /// <summary>
        /// Cancel an order.
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
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = tradeClient.MergeParamsAndArgs(args);

                var _json_value = await tradeClient.CallApiPost1Async($"/v1/order/orders/{order_id}/submitcancel", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<HPlaceOrder>(_json_value.Content);
                    {
                        var _order = new HPlaceOrderItem
                        {
                            symbol = _market.result.symbol,
                            orderId = _json_data.result.orderId,
                            orderType = OrderType.Limit,
                            orderStatus = OrderStatus.Canceled,
                            sideType = sideType,
                            quantity = quantity,
                            price = price,
                            amount = quantity * price,
                            //fee = quantity * price * publicApi.ExchangeInfo.Fees.trading.maker,
                            timestamp = CUnixTime.NowMilli
                        };

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
        /// Cancel Open Orders
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="order_ids"></param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<MyOrders> CancelOrders(string base_name, string quote_name, string[] order_ids, Dictionary<string, object> args = null)
        {
            var _result = new MyOrders(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                if (order_ids.Length > 0)
                {
                    tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                    var _params = new Dictionary<string, object>();
                    {
                        _params.Add("order_ids", order_ids);

                        tradeClient.MergeParamsAndArgs(_params, args);
                    }

                    var _json_value = await tradeClient.CallApiPost1Async("/v1/order/orders/batchcancel", _params);
#if RAWJSON
                    _result.rawJson = _json_value.Content;
#endif
                    var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _json_data = tradeClient.DeserializeObject<HCancelOrders>(_json_value.Content);
                        {
                            if (_json_data.success == true)
                            {
                                foreach (var _cancelOrder in _json_data.result)
                                {
                                    _result.result.Add(_cancelOrder);
                                }
                            }
                            else
                                _result.SetFailure();
                        }

                        //_json_result.SetResult(_result);
                    }

                    _result.SetResult(_json_result);
                }
                else
                {
                    if (args.ContainsKey("account-id") && args["account-id"].ToString() != "")
                    {
                        tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                        var _params = new Dictionary<string, object>();
                        {
                            _params.Add("symbol", _market.result.symbol);
                            _params.Add("account-id", args["account-id"].ToString());

                            tradeClient.MergeParamsAndArgs(_params, args);
                        }

                        var _json_value = await tradeClient.CallApiPost1Async("/v1/order/orders/batchCancelOpenOrders", _params);
#if RAWJSON
                        _result.rawJson = _json_value.Content;
#endif
                        var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                        if (_json_result.success == true)
                        {
                            var _json_data = tradeClient.DeserializeObject<HCancelAllOrders>(_json_value.Content);
                            {
                                if (_json_data.success == true)
                                {
                                    _result.result.Add(_json_data.result);
                                }
                                else
                                    _result.SetFailure();
                            }

                            //_json_result.SetResult(_result);
                        }

                        _result.SetResult(_json_result);
                    }
                    else
                    {
                        _result.SetFailure("주문ID로 취소할 경우 string[] order_ids에 넣어주시고 Symbol로 취소할 경우 args[account-id]가 필요합니다.");
                    }
                }
            }
            else
            {
                _result.SetResult(_market);
            }

            return _result;
        }

        /// <summary>
        /// Cancel all active orders at once.
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<MyOrders> CancelAllOrders(Dictionary<string, object> args = null)
        {
            var _result = new MyOrders();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                if (args.ContainsKey("account-id") && args["account-id"].ToString() != "")
                {
                    tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                    var _params = tradeClient.MergeParamsAndArgs(args);

                    var _json_value = await tradeClient.CallApiPost1Async($"/v1/order/orders/batchCancelOpenOrders", _params);
#if RAWJSON
                    _result.rawJson = _json_value.Content;
#endif
                    var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _json_data = tradeClient.DeserializeObject<HCancelAllOrders>(_json_value.Content);
                        {
                            if (_json_data.success == true)
                            {
                                _result.result.Add(_json_data.result);
                            }
                            else
                                _result.SetFailure();
                        }
                    }

                    _result.SetResult(_json_result);
                }
                else
                {
                    _result.SetFailure("required args[account-id]");
                }
            }
            else
            {
                _result.SetResult(_markets);
            }

            return _result;
        }
    }
}