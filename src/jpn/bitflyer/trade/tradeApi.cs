using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using OdinSdk.BaseLib.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Bitflyer.Trade
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
                    base.tradeClient = new BitflyerClient("trade", __connect_key, __secret_key);

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
                    base.publicApi = new CCXT.NET.Bitflyer.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Get Order
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
                    _params.Add("product_code", _market.result.symbol);
                    _params.Add("child_order_id", order_id);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                // 실제 넘어오는 데이터 확인해야함. sample에는 list로 넘어옴.
                var _json_value = await tradeClient.CallApiGet1Async("/v1/me/getchildorders", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orders = tradeClient.DeserializeObject<List<BMyOrderItem>>(_json_value.Content);
                    var _order = _orders.Where(o => o.orderId == order_id).SingleOrDefault();

                    _order.amount = _order.price * _order.quantity;

                    _result.result = _order;
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
        /// List Orders
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
                    _params.Add("product_code", _market.result.symbol);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiGet1Async("/v1/me/getchildorders", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<List<BMyOrderItem>>(_json_value.Content);
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
        /// 미 체결 주문내역
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

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("product_code", _market.result.symbol);
                    _params.Add("child_order_state", "ACTIVE");

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiGet1Async("/v1/me/getchildorders", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orders = tradeClient.DeserializeObject<List<BMyOrderItem>>(_json_value.Content);
                    {
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
        /// 미 체결 주문내역(ALL)
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

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("child_order_state", "ACTIVE");

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiGet1Async("/v1/me/getchildorders", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orders = tradeClient.DeserializeObject<List<BMyOrderItem>>(_json_value.Content);
                    {
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
                _result.SetResult(_markets);
            }

            return _result;
        }

        /// <summary>
        /// Get Open Interest Summary
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<MyPositions> FetchAllOpenPositions(Dictionary<string, object> args = null)
        {
            var _result = new MyPositions();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = tradeClient.MergeParamsAndArgs(args);

                var _json_value = await tradeClient.CallApiGet1Async("/v1/me/getpositions", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<List<BMyPositionItem>>(_json_value.Content);
                    {
                        var _positions = _json_data
                                                .OrderByDescending(o => o.timestamp);

                        foreach (var _p in _positions)
                        {
                            var _market = publicApi.publicClient.ExchangeInfo.Markets.GetMarketBySymbol(_p.symbol);
                            if (_market == null)
                                continue;

                            _p.positionId = _p.timestamp.ToString(); // positionId 제공 안함
                            _p.orderStatus = OrderStatus.Open;
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
        /// List Executions
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
                    _params.Add("product_code", _market.result.symbol);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiGet1Async("/v1/me/getexecutions", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<List<BMyTradeItem>>(_json_value.Content);
                    {
                        var _trades = _json_data
                                            .Where(t => t.timestamp >= since)
                                            .OrderByDescending(t => t.timestamp)
                                            .Take(limits);

                        foreach (var _t in _trades)
                        {
                            _t.amount = _t.price * _t.quantity;
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
        /// Send a New Limit Order
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

                var _buy_sell = sideType == SideType.Bid ? "BUY" : "SELL";

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("side", _buy_sell);
                    _params.Add("type", "LIMIT");
                    _params.Add("timeInForce", "GTC");  // 'GTC' = Good To Cancel (default), 'IOC' = Immediate Or Cancel
                    _params.Add("quantity", quantity);
                    _params.Add("price", price);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async($"/v1/me/sendchildorder", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _order = tradeClient.DeserializeObject<BPlaceOrderItem>(_json_value.Content);
                    {
                        _order.orderType = OrderType.Limit;
                        _order.price = price;
                        _order.quantity = quantity;
                        _order.amount = (_order.quantity * _order.price).Normalize();
                        _order.fee = _order.amount * tradeClient.ExchangeInfo.Fees.trading.maker;
                        _order.sideType = sideType;
                        _order.timestamp = CUnixTime.NowMilli;
                        _order.symbol = _market.result.symbol;

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
        /// Send a New Market Order
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
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("side", _buy_sell);
                    _params.Add("type", "MARKET");
                    _params.Add("quantity", quantity);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async($"/v1/me/sendchildorder", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _order = tradeClient.DeserializeObject<BPlaceOrderItem>(_json_value.Content);
                    {
                        _order.orderType = OrderType.Market;
                        //_order.price = price;
                        _order.quantity = quantity;
                        //_order.amount = (_order.quantity * _order.price).Normalize();
                        _order.fee = _order.amount * tradeClient.ExchangeInfo.Fees.trading.maker;
                        _order.sideType = sideType;
                        _order.timestamp = CUnixTime.NowMilli;
                        _order.symbol = _market.result.symbol;

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
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("symbol", _market.result.symbol);
                    _params.Add("orderId", Convert.ToInt64(order_id));

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                // If the parameters are correct, the status code will show 200 OK.
                var _json_value = await tradeClient.CallApiDelete1Async($"/v1/me/cancelchildorder", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _order = new BMyOrderItem
                    {
                        orderId = order_id,
                        symbol = _market.result.symbol,
                        timestamp = CUnixTime.NowMilli,

                        orderType = OrderType.Limit,
                        orderStatus = OrderStatus.Canceled,
                        sideType = sideType,

                        quantity = quantity,
                        price = price,
                        amount = quantity * price,
                        //fee = quantity * price * publicApi.ExchangeInfo.Fees.trading.maker
                    };

                    _result.result = _order;
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