using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Coin.Trade;
using CCXT.NET.Shared.Coin.Types;
using CCXT.NET.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Upbit.Trade
{
    /// <summary>
    /// UPBIT exchange's standardized trading API implementation
    /// Handles order placement, cancellation, and trade history queries
    /// Complies with CCXT.NET standard interface patterns
    /// Reference: https://docs.upbit.com/kr/reference
    /// API Version: v1.5.7 (100% 표준화 완성)
    /// </summary>
    public class TradeApi : CCXT.NET.Shared.Coin.Trade.TradeApi, ITradeApi
    {
        private readonly string __connect_key;
        private readonly string __secret_key;

        /// <summary>
        /// Initialize trading API with authentication credentials
        /// </summary>
        /// <param name="connect_key">UPBIT API access key</param>
        /// <param name="secret_key">UPBIT API secret key</param>
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
                    base.tradeClient = new UpbitClient("trade", __connect_key, __secret_key);

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
                    base.publicApi = new CCXT.NET.Upbit.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Fetch specific order details by order ID
        /// UPBIT API: GET /v1/order
        /// </summary>
        /// <param name="base_name">Base currency symbol (e.g., 'BTC')</param>
        /// <param name="quote_name">Quote currency symbol (e.g., 'KRW')</param>
        /// <param name="order_id">UPBIT order UUID</param>
        /// <param name="args">Additional parameters</param>
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
                    _params.Add("uuid", order_id);
                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiGet1Async("/order", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _order = tradeClient.DeserializeObject<UMyOrderItem>(_json_value.Content);
                    {
                        // Standardize amount calculation
                        _order.amount = _order.price * _order.quantity;
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
        /// Fetch open orders for specific market
        /// UPBIT API: GET /v1/orders (state=wait)
        /// </summary>
        /// <param name="base_name">Base currency symbol</param>
        /// <param name="quote_name">Quote currency symbol</param>
        /// <param name="args">Additional parameters including pagination</param>
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
                    _params.Add("market", _market.result.symbol);
                    _params.Add("state", "wait");
                    _params.Add("page", 1);
                    _params.Add("order_by", "asc");

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiGet1Async("/orders", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _orders = tradeClient.DeserializeObject<List<UMyOrderItem>>(_json_value.Content);
                    {
                        foreach (var _order in _orders)
                        {
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
        /// Fetch all open orders across all markets
        /// UPBIT API: GET /v1/orders (state=wait, no market filter)
        /// </summary>
        /// <param name="args">Additional parameters</param>
        /// <returns>Complete list of open orders</returns>
        public override async ValueTask<MyOrders> FetchAllOpenOrdersAsync(Dictionary<string, object> args = null)
        {
            var _result = new MyOrders();

            var _markets = await publicApi.LoadMarketsAsync();
            if (_markets.success)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("state", "wait");
                    _params.Add("page", 1);
                    _params.Add("order_by", "asc");

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiGet1Async("/orders", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _orders = tradeClient.DeserializeObject<List<UMyOrderItem>>(_json_value.Content);
                    {
                        foreach (var _order in _orders)
                        {
                            _order.amount = _order.price * _order.quantity;
                            _result.result.Add(_order);
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
        /// Fetch completed trades for specific market
        /// UPBIT API: GET /v1/orders (state=done)
        /// </summary>
        /// <param name="base_name">Base currency symbol</param>
        /// <param name="quote_name">Quote currency symbol</param>
        /// <param name="timeframe">Time frame (not used by UPBIT)</param>
        /// <param name="since">Filter trades since timestamp</param>
        /// <param name="limits">Maximum number of trades to return</param>
        /// <param name="args">Additional parameters</param>
        /// <returns>Historical trade execution data</returns>
        public override async ValueTask<MyTrades> FetchMyTradesAsync(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new MyTrades(base_name, quote_name);

            var _market = await publicApi.LoadMarketAsync(_result.marketId);
            if (_market.success)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("market", _market.result.symbol);
                    _params.Add("state", "done");
                    _params.Add("page", 1);
                    _params.Add("order_by", "desc"); // Most recent first

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiGet1Async("/orders", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _json_data = tradeClient.DeserializeObject<List<UMyTradeItem>>(_json_value.Content);
                    {
                        var _trades = _json_data
                            .Where(t => t.timestamp >= since)
                            .OrderByDescending(t => t.timestamp)
                            .Take(limits);

                        foreach (var _trade in _trades)
                        {
                            _trade.amount = _trade.price * _trade.quantity;
                            _result.result.Add(_trade);
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
        /// Place limit order (지정가 주문)
        /// UPBIT API: POST /v1/orders
        /// </summary>
        /// <param name="base_name">Base currency symbol</param>
        /// <param name="quote_name">Quote currency symbol</param>
        /// <param name="quantity">Order quantity</param>
        /// <param name="price">Order price</param>
        /// <param name="sideType">Buy (bid) or Sell (ask)</param>
        /// <param name="args">Additional order parameters</param>
        /// <returns>Created order information with UUID</returns>
        public override async ValueTask<MyOrder> CreateLimitOrderAsync(string base_name, string quote_name, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _market = await publicApi.LoadMarketAsync(_result.marketId);
            if (_market.success)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _side = sideType == SideType.Bid ? "bid" : "ask";

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("market", _market.result.symbol);
                    _params.Add("side", _side);
                    _params.Add("volume", quantity);
                    _params.Add("price", price);
                    _params.Add("ord_type", "limit");

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async("/orders", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _order = tradeClient.DeserializeObject<UPlaceOrderItem>(_json_value.Content);
                    {
                        _order.amount = _order.quantity * _order.price;
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
        /// Place market order (시장가 주문)
        /// UPBIT API: POST /v1/orders (ord_type=market)
        /// </summary>
        /// <param name="base_name">Base currency symbol</param>
        /// <param name="quote_name">Quote currency symbol</param>
        /// <param name="quantity">Order quantity</param>
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

                var _side = sideType == SideType.Bid ? "bid" : "ask";

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("market", _market.result.symbol);
                    _params.Add("side", _side);
                    _params.Add("ord_type", "market");

                    // For market orders, UPBIT uses different parameters:
                    // - bid: use 'price' (KRW amount to spend)
                    // - ask: use 'volume' (coin amount to sell)
                    if (sideType == SideType.Bid)
                    {
                        _params.Add("price", quantity); // KRW amount for market buy
                    }
                    else
                    {
                        _params.Add("volume", quantity); // Coin amount for market sell
                    }

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async("/orders", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _order = tradeClient.DeserializeObject<UPlaceOrderItem>(_json_value.Content);
                    {
                        _order.amount = _order.quantity * _order.price;
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
        /// Cancel existing order
        /// UPBIT API: DELETE /v1/order
        /// </summary>
        /// <param name="base_name">Base currency symbol</param>
        /// <param name="quote_name">Quote currency symbol</param>
        /// <param name="order_id">UPBIT order UUID to cancel</param>
        /// <param name="quantity">Original order quantity (not used by UPBIT)</param>
        /// <param name="price">Original order price (not used by UPBIT)</param>
        /// <param name="sideType">Original order side (not used by UPBIT)</param>
        /// <param name="args">Additional parameters</param>
        /// <returns>Cancelled order information</returns>
        public override async ValueTask<MyOrder> CancelOrderAsync(string base_name, string quote_name, string order_id, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _market = await publicApi.LoadMarketAsync(_result.marketId);
            if (_market.success)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("uuid", order_id);
                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiDelete1Async("/order", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _order = tradeClient.DeserializeObject<UPlaceOrderItem>(_json_value.Content);
                    {
                        _order.amount = _order.quantity * _order.price;
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
        /// Get order chance information (주문 가능 정보)
        /// UPBIT API: GET /v1/order/chance
        /// This method provides market information and account balance for trading
        /// </summary>
        /// <param name="base_name">Base currency symbol</param>
        /// <param name="quote_name">Quote currency symbol</param>
        /// <param name="args">Additional parameters</param>
        /// <returns>Order chance information including available balance and market status</returns>
        public async ValueTask<BoolResult> GetOrderChanceAsync(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new BoolResult();

            var _market = await publicApi.LoadMarketAsync($"{base_name}/{quote_name}");
            if (_market.success)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("market", _market.result.symbol);
                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiGet1Async("/order/chance", _params);
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                
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