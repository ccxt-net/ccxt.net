using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Coin.Trade;
using CCXT.NET.Shared.Coin.Types;
using CCXT.NET.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Bithumb.Trade
{
    /// <summary>
    /// BITHUMB exchange's standardized trading API implementation
    /// Handles order placement, cancellation, and trade history queries
    /// Complies with CCXT.NET standard interface patterns
    /// Reference: https://apidocs.bithumb.com/reference
    /// API Version: v1.0 (100% 표준화 완성)
    /// </summary>
    public class TradeApi : CCXT.NET.Shared.Coin.Trade.TradeApi, ITradeApi
    {
        private readonly string __connect_key;
        private readonly string __secret_key;

        /// <summary>
        /// Initialize trading API with authentication credentials
        /// </summary>
        /// <param name="connect_key">BITHUMB API access key</param>
        /// <param name="secret_key">BITHUMB API secret key</param>
        public TradeApi(string connect_key, string secret_key)
        {
            __connect_key = connect_key;
            __secret_key = secret_key;
        }

        /// <summary>
        /// Authenticated client for trading operations
        /// </summary>
        public override XApiClient tradeClient
        {
            get
            {
                if (base.tradeClient == null)
                    base.tradeClient = new BithumbClient("trade", __connect_key, __secret_key);

                return base.tradeClient;
            }
        }

        /// <summary>
        /// Public API instance for market data validation
        /// </summary>
        public override CCXT.NET.Shared.Coin.Public.PublicApi publicApi
        {
            get
            {
                if (base.publicApi == null)
                    base.publicApi = new CCXT.NET.Bithumb.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Fetch specific order details by order ID
        /// BITHUMB API: POST /info/order_detail
        /// </summary>
        /// <param name="base_name">Base currency symbol (e.g., 'BTC')</param>
        /// <param name="quote_name">Quote currency symbol (e.g., 'KRW')</param>
        /// <param name="order_id">BITHUMB order ID</param>
        /// <param name="args">Additional parameters (require) type</param>
        /// <returns>Order details including status and fill information</returns>
        public override async ValueTask<MyOrder> FetchMyOrderAsync(string base_name, string quote_name, string order_id, Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _market = await publicApi.LoadMarketAsync(_result.marketId);
            if (_market.success)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("currency", _market.result.symbol);
                    _params.Add("order_id", order_id);
                    _params.Add("type", "");

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async("/info/order_detail", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _json_data = tradeClient.DeserializeObject<BMyOrders>(_json_value.Content);
                    if (_json_data.success)
                    {
                        var _type = (_params.ContainsKey("type")) ? _params["type"].ToString() : "";

                        var _order = new BMyOrderItem
                        {
                            orderId = order_id,

                            symbol = _market.result.symbol,
                            payment_currency = quote_name,

                            sideType = SideTypeConverter.FromString(_type),
                            makerType = MakerType.Unknown,
                            orderStatus = OrderStatus.Closed,
                            orderType = OrderType.Limit,

                            timestamp = CUnixTime.NowMilli
                        };

                        foreach (var _o in _json_data.result)
                        {
                            //if (String.IsNullOrEmpty(_order.contract_id))
                            //    _order.contract_id = _o.contract_id;

                            if (_order.sideType != _o.sideType)
                                continue;

                            if (_order.timestamp > _o.timestamp)
                                _order.timestamp = _o.timestamp;

                            _order.quantity += _o.quantity;
                            _order.fee += _o.fee;
                            _order.amount += _o.amount;

                            _order.count++;
                        }

                        _order.price = _order.amount / _order.quantity;

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
        /// Get all account orders; active, canceled, or filled.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limits">maximum number of items (optional): default 20</param>
        /// <param name="args">Add additional attributes for each exchange: (require) order_id, type</param>
        /// <returns></returns>
        public override async ValueTask<MyOrders> FetchMyOrdersAsync(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new MyOrders(base_name, quote_name);

            var _market = await publicApi.LoadMarketAsync(_result.marketId);
            if (_market.success)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _timeframe = tradeClient.ExchangeInfo.GetTimeframe(timeframe);
                var _timestamp = tradeClient.ExchangeInfo.GetTimestamp(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("currency", _market.result.symbol);
                    _params.Add("order_id", "");
                    _params.Add("type", "");
                    _params.Add("after", since);
                    _params.Add("count", limits);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value_detail = await tradeClient.CallApiPost1Async("/info/order_detail", _params);
#if DEBUG
                _result.rawJson += _json_value_detail.Content;
#endif
                var _json_result_detail = tradeClient.GetResponseMessage(_json_value_detail.Response);
                if (_json_result_detail.success)
                {
                    var _json_data_detail = tradeClient.DeserializeObject<BMyOrders>(_json_value_detail.Content);
                    if (_json_data_detail.success)
                    {
                        //var _order_id = (_params.ContainsKey("order_id")) ? _params["order_id"].ToString() : "";

                        foreach (var _o in _json_data_detail.result)
                        {
                            _o.orderId = _o.timestamp.ToString();

                            _o.makerType = MakerType.Unknown;
                            _o.orderStatus = OrderStatus.Closed;
                            _o.orderType = OrderType.Limit;

                            _o.count++;

                            _result.result.Add(_o);
                        }

                        _result.SetResult(_json_result_detail);
                    }
                    else
                    {
                        _json_result_detail.SetResult(_json_data_detail);
                    }
                }

                var _json_value_orders = await tradeClient.CallApiPost1Async("/info/orders", _params);
#if DEBUG
                _result.rawJson += _json_value_orders.Content;
#endif
                var _json_result_orders = tradeClient.GetResponseMessage(_json_value_orders.Response);
                if (_json_result_orders.success)
                {
                    var _json_data_orders = tradeClient.DeserializeObject<BMyOpenOrders>(_json_value_orders.Content);
                    if (_json_data_orders.success)
                    {
                        foreach (var _o in _json_data_orders.result)
                        {
#if DEBUG
                            if (String.IsNullOrEmpty(_o.orderId))
                                continue;
#endif

                            _o.symbol = _market.result.symbol;

                            _o.orderType = OrderType.Limit;
                            _o.makerType = MakerType.Maker;

                            _o.filled = _o.quantity - _o.remaining;
                            _o.amount = _o.quantity * _o.price;

                            _o.count++;

                            _result.result.Add(_o);
                        }

                        _result.SetResult(_json_result_orders);
                    }
                    else
                    {
                        _json_result_orders.SetResult(_json_data_orders);
                    }
                }

                if (_json_result_detail.success == false && _json_result_orders.success == false)
                    _result.SetResult(_json_result_detail);
            }
            else
            {
                _result.SetResult(_market);
            }

            return _result;
        }

        /// <summary>
        /// Fetch open orders for specific market
        /// BITHUMB API: POST /info/orders
        /// </summary>
        /// <param name="base_name">Base currency symbol</param>
        /// <param name="quote_name">Quote currency symbol</param>
        /// <param name="args">Additional parameters</param>
        /// <returns>List of open orders waiting for execution</returns>
        public override async ValueTask<MyOrders> FetchOpenOrdersAsync(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new MyOrders(base_name, quote_name);

            var _market = await publicApi.LoadMarketAsync(_result.marketId);
            if (_market.success)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = new Dictionary<string, object>();
                {
                    var _since = 0; // CUnixTime.ConvertToUnixTimeMilli(CUnixTime.UtcNow.AddYears(-1));
                    var _limits = 100;

                    _params.Add("currency", _market.result.symbol);
                    _params.Add("after", _since);
                    _params.Add("count", _limits);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async("/info/orders", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _json_data = tradeClient.DeserializeObject<BMyOpenOrders>(_json_value.Content);
                    if (_json_data.success)
                    {
                        foreach (var _o in _json_data.result)
                        {
                            if (OrderStatusConverter.IsAlive(_o.orderStatus) == false)
                                continue;

                            _o.symbol = _market.result.symbol;

                            _o.orderType = OrderType.Limit;
                            _o.makerType = MakerType.Maker;

                            _o.filled = _o.quantity - _o.remaining;
                            _o.amount = _o.quantity * _o.price;

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
        public override async ValueTask<MyTrades> FetchMyTradesAsync(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new MyTrades(base_name, quote_name);

            var _market = await publicApi.LoadMarketAsync(_result.marketId);
            if (_market.success)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _timeframe = tradeClient.ExchangeInfo.GetTimeframe(timeframe);
                var _timestamp = tradeClient.ExchangeInfo.GetTimestamp(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("currency", _market.result.symbol);
                    _params.Add("offset", 0);
                    _params.Add("count", limits);
                    _params.Add("searchGb", 0);     // 0 : 전체, 1 : 구매완료, 2 : 판매완료, 3 : 출금중, 4 : 입금, 5 : 출금, 9 : KRW입금중

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async("/info/user_transactions", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _json_data = tradeClient.DeserializeObject<BMyTrades>(_json_value.Content);
                    if (_json_data.success)
                    {
                        var _orders = _json_data.result
                                                .Where(o => o.timestamp >= since && (o.sideType == SideType.Ask || o.sideType == SideType.Bid))
                                                .OrderByDescending(t => t.timestamp)
                                                .Take(limits);

                        foreach (var _o in _orders)
                        {
                            _o.symbol = _market.result.symbol;

                            _o.amount = Math.Abs(_o.amount);
                            if (_o.quantity != 0.0m)
                                _o.price = _o.amount / _o.quantity;

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
        /// Place limit order (지정가 주문)
        /// BITHUMB API: POST /trade/place
        /// </summary>
        /// <param name="base_name">Base currency symbol</param>
        /// <param name="quote_name">Quote currency symbol</param>
        /// <param name="quantity">Order quantity</param>
        /// <param name="price">Order price</param>
        /// <param name="sideType">Buy (bid) or Sell (ask)</param>
        /// <param name="args">Additional order parameters</param>
        /// <returns>Created order information with order ID</returns>
        public override async ValueTask<MyOrder> CreateLimitOrderAsync(string base_name, string quote_name, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _market = await publicApi.LoadMarketAsync(_result.marketId);
            if (_market.success)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _buy_sell = sideType == SideType.Bid ? "bid" : "ask";

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("order_currency", _market.result.baseId);
                    _params.Add("payment_currency", _market.result.quoteId);
                    _params.Add("units", quantity);
                    _params.Add("price", price);
                    _params.Add("type", _buy_sell);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async("/trade/place", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _json_data = tradeClient.DeserializeObject<BPlaceOrders>(_json_value.Content);
                    if (_json_data.success)
                    {
                        var _limit_order = new BPlaceOrderItem
                        {
                            orderId = _json_data.orderId,
                            sideType = sideType,
                            orderType = OrderType.Limit,
                            orderStatus = OrderStatus.Open,
                            timestamp = Convert.ToInt64(_json_data.orderId),

                            price = price,
                            quantity = quantity,
                            amount = quantity * price,
                            //fee = quantity * price * publicApi.ExchangeInfo.Fees.trading.maker,

                            filled = 0m,
                            cost = 0m
                        };

                        foreach (var _trade in _json_data.data)
                        {
                            _limit_order.filled += _trade.quantity;
                            _limit_order.cost += _trade.amount + _trade.fee;
                        }

                        _result.result = _limit_order;
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
        /// Place market order (시장가 주문)
        /// BITHUMB API: POST /trade/market_buy or /trade/market_sell
        /// </summary>
        /// <param name="base_name">Base currency symbol</param>
        /// <param name="quote_name">Quote currency symbol</param>
        /// <param name="quantity">Order quantity (coin amount for both buy and sell)</param>
        /// <param name="price">Not used for market orders (ignored)</param>
        /// <param name="sideType">Buy (bid) or Sell (ask)</param>
        /// <param name="args">Additional order parameters</param>
        /// <returns>Created market order information</returns>
        public override async ValueTask<MyOrder> CreateMarketOrderAsync(string base_name, string quote_name, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _market = await publicApi.LoadMarketAsync(_result.marketId);
            if (_market.success)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _buy_sell = sideType == SideType.Bid ? "buy" : "sell";

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("currency", _market.result.baseId);
                    _params.Add("units", quantity);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async($"/trade/market_{_buy_sell}", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _json_data = tradeClient.DeserializeObject<BPlaceOrders>(_json_value.Content);
                    if (_json_data.success)
                    {
                        var _market_order = new BPlaceOrderItem
                        {
                            orderId = _json_data.orderId,
                            sideType = sideType,
                            orderType = OrderType.Market,
                            orderStatus = OrderStatus.Open,
                            timestamp = Convert.ToInt64(_json_data.orderId),

                            // For market orders, price is determined by market
                            price = 0m,
                            quantity = quantity,
                            amount = 0m, // Will be calculated from filled trades
                            //fee = quantity * price * publicApi.ExchangeInfo.Fees.trading.maker,

                            filled = 0m,
                            cost = 0m
                        };

                        foreach (var _trade in _json_data.data)
                        {
                            _market_order.filled += _trade.quantity;
                            _market_order.cost += _trade.amount + _trade.fee;
                        }

                        _result.result = _market_order;
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
        /// Cancel existing order
        /// BITHUMB API: POST /trade/cancel
        /// </summary>
        /// <param name="base_name">Base currency symbol</param>
        /// <param name="quote_name">Quote currency symbol</param>
        /// <param name="order_id">BITHUMB order ID to cancel</param>
        /// <param name="quantity">Original order quantity (not used by BITHUMB)</param>
        /// <param name="price">Original order price (not used by BITHUMB)</param>
        /// <param name="sideType">Original order side (bid/ask)</param>
        /// <param name="args">Additional parameters</param>
        /// <returns>Cancelled order information</returns>
        public override async ValueTask<MyOrder> CancelOrderAsync(string base_name, string quote_name, string order_id, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _market = await publicApi.LoadMarketAsync(_result.marketId);
            if (_market.success)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _buy_sell = sideType == SideType.Bid ? "bid" : "ask";

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("currency", _market.result.symbol);
                    _params.Add("order_id", order_id);
                    _params.Add("type", _buy_sell);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async("/trade/cancel", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _json_data = tradeClient.DeserializeObject<BPlaceOrder>(_json_value.Content);
                    if (_json_data.success)
                    {
                        var _cancel_order = new BPlaceOrderItem
                        {
                            orderId = order_id,
                            symbol = _market.result.symbol,

                            orderStatus = OrderStatus.Canceled,
                            sideType = sideType,

                            quantity = quantity,
                            price = price,
                            amount = quantity * price
                        };

                        _result.result = _cancel_order;
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
    }
}