using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using CCXT.NET.Kraken.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Kraken.Trade
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
                    base.tradeClient = new KrakenClient("trade", __connect_key, __secret_key);

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
                    base.publicApi = new CCXT.NET.Kraken.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Query orders info
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
                    _params.Add("txid", order_id);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async("/0/private/QueryOrders", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<KResponse<Dictionary<string, KMyOrderItem>>>(_json_value.Content);
                    {
                        var _order = _json_data.result.FirstOrDefault();
                        if (_order.Value != null)
                        {
                            _order.Value.orderId = _order.Key;

                            _order.Value.price = _order.Value.descr.price != 0.0m ? _order.Value.descr.price : _order.Value.price;
                            _order.Value.amount = _order.Value.quantity * _order.Value.price;

                            _result.marketId = _market.result.marketId;
                            _result.result = _order.Value;
                        }
                        else
                        {
                            _json_result.SetFailure(errorCode: ErrorCode.OrderNotFound);
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
        /// Get orders
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limits">maximum number of items (optional): default 20</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<MyOrders> FetchMyOrders(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new MyOrders(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _kmarket = _market.result as KMarketItem;

                var _params = tradeClient.MergeParamsAndArgs(args);

                // open orders
                {
                    var _json_value = await tradeClient.CallApiPost1Async("/0/private/OpenOrders", _params);
#if DEBUG
                    _result.rawJson += _json_value.Content;
#endif
                    var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _json_data = tradeClient.DeserializeObject<KResponse<KMyOpenOrders>>(_json_value.Content);
                        {
                            var _orders = _json_data.result.open
                                                    .Where(o => o.Value.symbol == _kmarket.altname && o.Value.timestamp >= since)
                                                    .OrderByDescending(o => o.Value.timestamp)
                                                    .Take(limits);

                            foreach (var _o in _orders)
                            {
                                _o.Value.orderId = _o.Key;

                                _o.Value.price = _o.Value.descr.price != 0.0m ? _o.Value.descr.price : _o.Value.price;
                                _o.Value.amount = _o.Value.price * _o.Value.quantity;

                                _result.result.Add(_o.Value);
                            }
                        }
                    }

                    _result.SetResult(_json_result);
                }

                // closed orders
                if (_result.success == true)
                {
                    var _json_value = await tradeClient.CallApiPost1Async("/0/private/ClosedOrders", _params);
#if DEBUG
                    _result.rawJson += _json_value.Content;
#endif
                    var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _json_data = tradeClient.DeserializeObject<KResponse<KMyClosedOrders>>(_json_value.Content);
                        {
                            var _orders = _json_data.result.closed
                                                    .Where(o => o.Value.symbol == _kmarket.altname && o.Value.timestamp >= since)
                                                    .OrderByDescending(o => o.Value.timestamp)
                                                    .Take(limits);

                            foreach (var _o in _orders)
                            {
                                _o.Value.orderId = _o.Key;

                                _o.Value.price = _o.Value.descr.price != 0.0m ? _o.Value.descr.price : _o.Value.price;
                                _o.Value.amount = _o.Value.price * _o.Value.quantity;

                                _result.result.Add(_o.Value);
                            }
                        }
                    }

                    _result.SetResult(_json_result);
                }
            }
            else
            {
                _result.SetResult(_market);
            }

            return _result;
        }

        /// <summary>
        /// Get open orders
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="args">userref = restrict results to given user reference id (optional)</param>
        /// <returns></returns>
        public override async Task<MyOrders> FetchOpenOrders(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new MyOrders(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _kmarket = _market.result as KMarketItem;

                var _params = tradeClient.MergeParamsAndArgs(args);

                var _json_value = await tradeClient.CallApiPost1Async("/0/private/OpenOrders", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<KResponse<KMyOpenOrders>>(_json_value.Content);
                    {
                        var _orders = _json_data.result.open
                                                     .Where(o => o.Value.symbol == _kmarket.altname)
                                                     .OrderByDescending(o => o.Value.timestamp);

                        foreach (var _o in _orders)
                        {
                            _o.Value.orderId = _o.Key;

                            _o.Value.price = _o.Value.descr.price != 0.0m ? _o.Value.descr.price : _o.Value.price;
                            _o.Value.amount = _o.Value.price * _o.Value.quantity;

                            _result.result.Add(_o.Value);
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
        /// Get all open orders
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

                var _json_value = await tradeClient.CallApiPost1Async("/0/private/OpenOrders", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<KResponse<KMyOpenOrders>>(_json_value.Content);
                    {
                        var _orders = _json_data.result.open
                                                .OrderByDescending(o => o.Value.timestamp);

                        foreach (var _o in _orders)
                        {
                            _o.Value.orderId = _o.Key;

                            var _market = _markets.result.Values.Where(m => (m as KMarketItem).altname == _o.Value.symbol).SingleOrDefault();
                            if (_market != null)
                                _o.Value.symbol = _market.symbol;

                            _o.Value.price = _o.Value.descr.price != 0.0m ? _o.Value.descr.price : _o.Value.price;
                            _o.Value.amount = _o.Value.price * _o.Value.quantity;

                            _result.result.Add(_o.Value);
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
        /// Get open positions
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<MyPositions> FetchAllOpenPositions(Dictionary<string, object> args = null)
        {
            var _result = new MyPositions();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = tradeClient.MergeParamsAndArgs(args);

                var _json_value = await tradeClient.CallApiPost1Async("/0/private/OpenPositions", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<KResponse<Dictionary<string, KMyPositionItem>>>(_json_value.Content);
                    {
                        var _positions = _json_data.result
                                            .OrderByDescending(o => o.Value.timestamp);

                        foreach (var _p in _positions)
                        {
                            _p.Value.positionId = _p.Key;
                            _p.Value.orderType = OrderType.Position;

                            _p.Value.amount = _p.Value.cost + _p.Value.fee;
                            _p.Value.price = _p.Value.amount / Math.Abs(_p.Value.quantity);

                            _result.result.Add(_p.Value);
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
        /// Get trades history
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

                var _params = tradeClient.MergeParamsAndArgs(args);

                var _json_value = await tradeClient.CallApiPost1Async("/0/private/TradesHistory", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<KResponse<KMyTrades>>(_json_value.Content);
                    {
                        var _trades = _json_data.result.trades
                                                .OrderByDescending(o => o.Value.timestamp);

                        foreach (var _t in _trades)
                        {
                            _t.Value.tradeId = _t.Key;
                            _t.Value.amount = _t.Value.quantity * _t.Value.price;

                            _result.result.Add(_t.Value);
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
        /// create new limit order
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
                    _params.Add("pair", _market.result.symbol);
                    _params.Add("type", _buy_sell);
                    _params.Add("ordertype", "limit");
                    _params.Add("price", price);  // (optional.  dependent upon ordertype)
                    //_params.Add("price2", "?"); // secondary price (optional.  dependent upon ordertype)
                    _params.Add("volume", quantity); // order volume in lots

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async($"/0/private/AddOrder", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<KResponse<KMyPlaceOrder>>(_json_value.Content);
                    if (_json_data.result.transactionIds != null)
                    {
                        var _order = new KMyPlaceOrderItem
                        {
                            orderId = "",
                            symbol = _market.result.symbol,

                            timestamp = CUnixTime.NowMilli,
                            orderType = OrderType.Limit,
                            sideType = sideType,
                            orderStatus = OrderStatus.Open,

                            quantity = quantity,
                            price = price,
                            amount = quantity * price
                        };

                        foreach (var _p in _json_data.result.transactionIds)
                        {
                            if (String.IsNullOrEmpty(_order.orderId) == false)
                                _order.orderId += ",";
                            _order.orderId += _p;
                        }

                        _result.message = _json_data.result.description.order;
                        _result.result = _order;
                    }
                    else
                    {
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

        /// <summary>
        /// create new market order
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

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("pair", _market.result.symbol);
                    _params.Add("type", _buy_sell);
                    _params.Add("ordertype", "market");
                    //_params.Add("price", price);  // (optional.  dependent upon ordertype)
                    //_params.Add("price2", "?"); // secondary price (optional.  dependent upon ordertype)
                    _params.Add("volume", quantity); // order volume in lots

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async($"/0/private/AddOrder", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<KResponse<KMyPlaceOrder>>(_json_value.Content);
                    {
                        foreach (var _p in _json_data.result.transactionIds)
                        {
                            var _order = new KMyPlaceOrderItem
                            {
                                orderId = _p,
                                symbol = _market.result.symbol,

                                timestamp = CUnixTime.NowMilli,
                                orderType = OrderType.Market,
                                sideType = sideType,

                                quantity = quantity,
                                price = price,
                                amount = quantity * price
                            };

                            _result.result = _order;
                            break;
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
        /// Cancel open order
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
                    _params.Add("txid", order_id);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async($"/0/private/CancelOrder", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<KResponse<KMyCancelOrder>>(_json_value.Content);
                    if (_json_data.result.count > 0)
                    {
                        var _order = new KMyPlaceOrderItem
                        {
                            orderId = order_id,
                            symbol = _market.result.symbol,
                            orderType = OrderType.Unknown,
                            sideType = sideType,
                            timestamp = CUnixTime.NowMilli,
                            quantity = quantity,
                            price = price,
                            amount = quantity * price,
                            count = _json_data.result.count,
                            pending = _json_data.result.pending
                        };

                        _result.result = _order;
                    }
                    else
                    {
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