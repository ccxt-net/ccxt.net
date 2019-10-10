using CCXT.NET.Coin;
using CCXT.NET.Coin.Trade;
using CCXT.NET.Coin.Types;
using CCXT.NET.Extension;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCXT.NET.Bitstamp.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class TradeApi : CCXT.NET.Coin.Trade.TradeApi, ITradeApi
    {
        private readonly string __connect_key;
        private readonly string __secret_key;
        private readonly string __user_name;
        private readonly string __user_passsword;

        /// <summary>
        ///
        /// </summary>
        public TradeApi(string connect_key, string secret_key, string user_name, string user_password)
        {
            __connect_key = connect_key;
            __secret_key = secret_key;
            __user_name = user_name;
            __user_passsword = user_password;
        }

        /// <summary>
        ///
        /// </summary>
        public override XApiClient tradeClient
        {
            get
            {
                if (base.tradeClient == null)
                    base.tradeClient = new BitstampClient("trade", __connect_key, __secret_key, __user_name, __user_passsword);

                return base.tradeClient;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override CCXT.NET.Coin.Public.PublicApi publicApi
        {
            get
            {
                if (base.publicApi == null)
                    base.publicApi = new CCXT.NET.Bitstamp.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Check an order's status.
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

                var _params = tradeClient.MergeParamsAndArgs(
                    new Dictionary<string, object>
                    {
                        { "id", order_id }
                    },
                    args
                );

                var _json_value = await tradeClient.CallApiPost1Async("/order_status/", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<BMyOrder>(_json_value.Content);
                    {
                        _result.marketId = _market.result.marketId;

                        var _base_id = await publicApi.LoadCurrencyId(base_name);
                        var _quote_id = await publicApi.LoadCurrencyId(quote_name);

                        var _order = (BMyOrderItem)_json_data.result;

                        _order.symbol = _market.result.symbol;
                        _order.orderStatus = _json_data.orderStatus;
                        _order.orderId = order_id;

                        foreach (var _o in _json_data.transactions)
                        {
                            var _t = tradeClient.DeserializeObject<BMyOrderItem>(_o.ToString());

                            _t.quantity = _o[_base_id.result].Value<decimal>();
                            _t.amount = _o[_quote_id.result].Value<decimal>();

                            _order.quantity += _t.quantity;
                            _order.amount += _t.amount;
                            _order.fee += _t.fee;

                            _order.remaining += _t.quantity;

                            if (_json_data.orderStatus == OrderStatus.Closed)
                            {
                                _order.filled += _t.quantity;
                                _order.cost += _t.amount;
                                _order.remaining -= _t.quantity;
                            }

                            _order.timestamp = _t.timestamp;
                            _order.type = _t.type;

                            _order.count++;
                        }

                        if (_order.quantity != 0)
                            _order.price = _order.amount / _order.quantity;

                        _result.result = _json_data.result;
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
        public override async Task<MyOrders> FetchOpenOrders(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new MyOrders(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = tradeClient.MergeParamsAndArgs(args);

                var _json_value = await tradeClient.CallApiPost1Async($"/v2/open_orders/{_market.result.symbol}/", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orders = tradeClient.DeserializeObject<List<BOpenOrderItem>>(_json_value.Content);
                    {
                        foreach (var _o in _orders)
                        {
                            _o.symbol = _market.result.symbol;
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
        /// Get all open orders on a symbol. Careful when accessing this with no symbol.
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

                var _params = tradeClient.MergeParamsAndArgs(args);

                var _json_value = await tradeClient.CallApiPost1Async("/v2/open_orders/all/", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orders = tradeClient.DeserializeObject<List<BOpenOrderItem>>(_json_value.Content);
                    {
                        foreach (var _o in _orders)
                        {
                            var _market = _markets.GetMarketByMarketId(_o.marketId);

                            _o.symbol = _market.symbol;

                            _o.orderStatus = OrderStatus.Open;
                            _o.makerType = MakerType.Maker;
                            _o.orderType = OrderType.Limit;

                            _o.amount = _o.price * _o.quantity;
                            _o.remaining = _o.quantity;

                            _o.count++;

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
        /// Get trades for a specific account and symbol.
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

                //var _timeframe = tradeClient.ExchangeInfo.GetTimeframe(timeframe);
                //var _timestamp = tradeClient.ExchangeInfo.GetTimestamp(timeframe);

                var _params = tradeClient.MergeParamsAndArgs(
                    new Dictionary<string, object>
                    {
                        { "offset", 0 },
                        { "limit", limits },
                        { "sort", "desc" }
                    },
                    args
                );

                var _json_value = await tradeClient.CallApiPost1Async($"/v2/user_transactions/{_market.result.symbol}/", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<JArray>(_json_value.Content);
                    foreach (var _j in _json_data)
                    {
                        var _t = tradeClient.DeserializeObject<BMyTradeItem>(_j.ToString());
                        if (_t.type != 2)
                            continue;

                        var _base_id = _market.result.baseId;
                        var _quote_id = _market.result.quoteId;

                        _t.symbol = _market.result.symbol;

                        _t.price = _j[$"{_base_id}_{_quote_id}"].Value<decimal>();
                        _t.quantity = _j[_base_id].Value<decimal>();
                        _t.amount = _j[_quote_id].Value<decimal>();

                        _t.sideType = _t.amount < 0 ? SideType.Ask : SideType.Bid;

                        _result.result.Add(_t);
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
        /// Send in a new order.
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

                var _params = tradeClient.MergeParamsAndArgs(
                    new Dictionary<string, object>
                    {
                        { "amount", quantity },
                        { "price", price }
                    },
                    args
                );

                var _json_value = await tradeClient.CallApiPost1Async($"/v2/{_buy_sell}/{_market.result.symbol}/", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<BPlaceOrderItem>(_json_value.Content);
                    {
                        _json_data.symbol = _market.result.symbol;

                        _json_data.orderType = OrderType.Limit;
                        _json_data.orderStatus = OrderStatus.Open;

                        _json_data.amount = (_json_data.quantity * _json_data.price).Normalize();
                        _json_data.remaining = _json_data.quantity;

                        _json_data.count++;

                        _result.result = _json_data;
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
        /// Send in a new order.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="price">price of coin</param>
        /// <param name="sideType">type of buy(bid) or sell(ask)</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<MyOrder> CreateMarketOrder(string base_name, string quote_name, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _buy_sell = sideType == SideType.Bid ? "buy" : "sell";

                var _params = tradeClient.MergeParamsAndArgs(
                    new Dictionary<string, object>
                    {
                        { "amount", quantity },
                    },
                    args
                );

                var _json_value = await tradeClient.CallApiPost1Async($"/v2/{_buy_sell}/market/{_market.result.symbol}/", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<BPlaceOrderItem>(_json_value.Content);
                    {
                        _json_data.symbol = _market.result.symbol;

                        _json_data.orderType = OrderType.Market;
                        _json_data.orderStatus = OrderStatus.Open;

                        _json_data.amount = (_json_data.quantity * _json_data.price).Normalize();
                        _json_data.remaining = _json_data.quantity;

                        _json_data.count++;

                        _result.result = _json_data;
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
        /// Cancel an active order.
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

                var _params = tradeClient.MergeParamsAndArgs(
                    new Dictionary<string, object>
                    {
                        { "id", order_id }
                    },
                    args
                );

                var _json_value = await tradeClient.CallApiPost1Async($"/v2/cancel_order/", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<BPlaceOrderItem>(_json_value.Content);
                    {
                        _json_data.orderStatus = OrderStatus.Canceled;
                        _json_data.amount = (_json_data.quantity * _json_data.price).Normalize();

                        _result.result = _json_data;
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
        /// Cancel all active orders at once.
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<MyOrders> CancelAllOrders(Dictionary<string, object> args = null)
        {
            var _result = new MyOrders();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = tradeClient.MergeParamsAndArgs(args);

                var _json_value = await tradeClient.CallApiPost1Async($"/cancel_all_orders/", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = _json_value.Content;
                    {
                        if (_json_data != "true")
                            _result.SetFailure();
                        else
                            _result.SetSuccess("success");
                    }

                    _json_result.SetResult(_result);
                }

                _result.SetResult(_json_result);
            }
            else
            {
                _result.SetResult(_markets);
            }

            return _result;
        }
    }
}