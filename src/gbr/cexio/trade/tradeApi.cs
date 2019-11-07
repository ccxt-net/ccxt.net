using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using OdinSdk.BaseLib.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.CEXIO.Trade
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
                    base.tradeClient = new CexioClient("trade", __connect_key, __secret_key, __user_name, __user_password);

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
                    base.publicApi = new CCXT.NET.CEXIO.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Get order details
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
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("id", order_id);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async("/get_order/", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _order = tradeClient.DeserializeObject<CMyOrderItem>(_json_value.Content);
                    if (_order != null)
                    {
                        _order.amount = _order.price * _order.quantity;
                        _result.result = _order;
                    }
                    else
                    {
                        _json_result.SetFailure("Order not found", ErrorCode.OrderNotFound);
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
        /// Archived orders
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

                var _params = tradeClient.MergeParamsAndArgs(args);

                var _json_value = await tradeClient.CallApiPost1Async($"/archived_orders/{_market.result.symbol}", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<List<CMyOrderItems>>(_json_value.Content);
                    {
                        var _orders = _json_data
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
        /// Open orders on a symbol.
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
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = tradeClient.MergeParamsAndArgs(args);

                var _json_value = await tradeClient.CallApiPost1Async($"/open_orders/{_market.result.symbol}", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orders = tradeClient.DeserializeObject<List<COpenOrderItem>>(_json_value.Content);
                    {
                        foreach (var _o in _orders)
                        {
                            _o.orderStatus = OrderStatus.Open;
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
        /// Open orders
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<MyOrders> FetchAllOpenOrders(Dictionary<string, object> args = null)
        {
            var _result = new MyOrders();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = tradeClient.MergeParamsAndArgs(args);

                var _json_value = await tradeClient.CallApiPost1Async("/open_orders/", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orders = tradeClient.DeserializeObject<List<COpenOrderItem>>(_json_value.Content);
                    {
                        foreach (var _o in _orders)
                        {
                            _o.orderStatus = OrderStatus.Open;
                            _o.amount = _o.price * _o.quantity;
                            _result.result.Add(_o);
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
        /// Get position
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<MyPositions> FetchAllOpenPositions(Dictionary<string, object> args)
        {
            var _result = new MyPositions();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = tradeClient.MergeParamsAndArgs(args);

                var _json_value = await tradeClient.CallApiPost1Async($"/get_position", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<CMyPositions>(_json_value.Content);
                    {
                        var _positions = _json_data.result
                                                .OrderByDescending(o => o.timestamp);

                        foreach (var _p in _positions)
                        {
                            _p.orderType = OrderType.Position;
                            _p.amount = Math.Abs(_p.quantity) * _p.price;

                            _result.result.Add(_p);
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
        /// Place limit order
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
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _buy_sell = sideType == SideType.Bid ? "buy" : "sell";

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("type", _buy_sell);
                    _params.Add("amount", quantity);
                    _params.Add("price", price);
                    // limit, market별 request param 설명이 없음,  response 다름(https://cex.io/rest-api) -> Place order

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async($"/place_order/{_market.result.symbol}", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _order = tradeClient.DeserializeObject<CPlaceOrderItem>(_json_value.Content);
                    {
                        _order.orderType = OrderType.Limit;
                        _order.amount = (_order.quantity * _order.price).Normalize();
                        _order.fee = _order.amount * tradeClient.ExchangeInfo.Fees.trading.maker;

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
        /// Place market order
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
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _buy_sell = sideType == SideType.Bid ? "BUY" : "SELL";

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("type", _buy_sell);
                    _params.Add("amount", quantity);
                    _params.Add("price", price);
                    // limit, market별 request param 설명이 없음,  response 다름(https://cex.io/rest-api) -> Place order

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async($"/place_order/{_market.result.symbol}", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _order = tradeClient.DeserializeObject<CPlaceOrderItem>(_json_value.Content); // limit order랑 넘어오는 데이터가 다름
                    {
                        _order.orderType = OrderType.Market;
                        _order.amount = (_order.quantity * _order.price).Normalize();
                        _order.fee = _order.amount * tradeClient.ExchangeInfo.Fees.trading.maker;

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
        /// Cancel order
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

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("id", Convert.ToInt64(order_id));

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async($"/cancel_order/", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<bool>(_json_value.Content);
                    {
                        if (_json_data == true)
                        {
                            var _order = new CMyOrderItem
                            {
                                symbol = _market.result.symbol,
                                orderId = order_id,
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
        /// Cancel all orders for given pair
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
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = tradeClient.MergeParamsAndArgs(args);

                var _json_value = await tradeClient.CallApiPost1Async($"/cancel_orders/{_market.result.symbol}", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<CCancelOrders>(_json_value.Content);
                    {
                        foreach (var _o in _json_data.result)
                        {
                            var _order = new CMyOrderItem
                            {
                                orderId = _o.orderId,
                                symbol = _market.result.symbol,
                                timestamp = CUnixTime.NowMilli,

                                orderStatus = _json_data.message == "ok" ? OrderStatus.Canceled : OrderStatus.Unknown,
                                orderType = OrderType.Limit
                            };

                            _result.result.Add(_order);
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