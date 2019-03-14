using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Bitforex.Trade
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
                    base.tradeClient = new BitforexClient("trade", __connect_key, __secret_key);

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
                    base.publicApi = new CCXT.NET.Bitforex.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Used to retrieve a single order by uuid.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="order_id">Order number registered for sale or purchase</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<MyOrder> FetchMyOrder(string base_name, string quote_name, string order_id, Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("orderId", order_id);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async("/api/v1/trade/orderInfo", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _order = tradeClient.DeserializeObject<BMyOrder>(_json_value.Content);
                    {
                        var _o = _order.result;
                        _o.amount = _o.quantity * _o.price;
                        _o.orderType = OrderType.Limit;

                        _result.result = _o;
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
        public override async Task<MyOrders> FetchOpenOrders(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new MyOrders(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("state", 0); // orderState: 0 uncomplete, 1 completed

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async("/api/v1/trade/orderInfos", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orders = tradeClient.DeserializeObject<BMyOrders>(_json_value.Content);
                    {
                        foreach (var _order in _orders.result)
                        {
                            _order.orderStatus = OrderStatus.Open;
                            _order.amount = _order.price * _order.quantity;

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

        /// <summary>
        /// 
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
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("state", 1);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async("/api/v1/trade/orderInfos", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<BMyTrades>(_json_value.Content);
                    {
                        var _trades = _json_data.result
                                                .Where(t => t.timestamp >= since)
                                                .OrderByDescending(t => t.timestamp)
                                                .Take(limits);

                        foreach (var _t in _trades)
                        {
                            _t.amount = _t.quantity * _t.price;
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
        /// Used to place a limit order in a specific market.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="price">price of coin</param>
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

                var _buy_sell = sideType == SideType.Bid ? "buy" : "sell";

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("price", price);
                    _params.Add("amount", quantity);
                    _params.Add("tradeType", sideType == SideType.Bid ? 1 : 2);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async($"/api/v1/trade/placeOrder", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _order = tradeClient.DeserializeObject<BPlaceOrder>(_json_value.Content);
                    {
                        var _orderItem = _order.result;
                        {
                            _orderItem.symbol = _market.result.symbol;
                            _orderItem.orderType = OrderType.Limit;
                            _orderItem.orderStatus = OrderStatus.Open;
                            _orderItem.sideType = sideType;
                            _orderItem.quantity = quantity;
                            _orderItem.price = price;
                            _orderItem.amount = quantity * price;
                            //_orderItem.fee = quantity * price * publicApi.ExchangeInfo.Fees.trading.maker;
                            _orderItem.timestamp = CUnixTime.NowMilli;
                        };

                        _result.result = _orderItem;
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
        /// Used to cancel a buy or sell order.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="order_id">Order number registered for sale or purchase</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="price">price of coin</param>
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
                    _params.Add("uuid", order_id);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async($"/api/v1/trade/cancelOrder", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<BCancelOrder>(_json_value.Content);
                    if (_json_data.success == true && _json_data.data == true)
                    {
                        var _order = new BPlaceOrderItem
                        {
                            orderId = order_id,
                            symbol = _market.result.symbol,
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
        /// Cancel Open Orders
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="order_ids"></param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<MyOrders> CancelOrders(string base_name, string quote_name, string[] order_ids, Dictionary<string, object> args = null)
        {
            var _result = new MyOrders(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = new Dictionary<string, object>();
                {
                    if (order_ids.Length > 0)
                    {
                        var _orderIds = "";
                        foreach (var order_id in order_ids)
                        {
                            if (_orderIds == "")
                                _orderIds = order_id;
                            else
                                _orderIds = _orderIds + "," + order_id;
                        }
                        _params.Add("orderIds", _orderIds);
                    }
                    
                    _params.Add("symbol", _market.result.symbol);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _url = "";
                if (order_ids.Length > 0)
                    _url = "/api/v1/trade/cancelMultiOrder";
                else
                    _url = "/api/v1/trade/cancelAllOrder";

                var _json_value = await tradeClient.CallApiPost1Async(_url, _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    if (order_ids.Length > 0)
                    {
                        var _json_data = tradeClient.DeserializeObject<BCancelOrders>(_json_value.Content);
                        {
                            if (_json_data.success == true)
                            {
                                foreach (var _j in _json_data.result)
                                    _result.result.Add(_j);
                                _result.SetSuccess();
                            }
                            else
                                _result.SetFailure();
                        }
                    }
                    else
                    {
                        var _json_data = tradeClient.DeserializeObject<BCancelOrder>(_json_value.Content);
                        {
                            if (_json_data.success == true && _json_data.data == true)
                                _result.SetSuccess();
                            else
                                _result.SetFailure();
                        }
                    }

                    _json_result.SetResult(_result);
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