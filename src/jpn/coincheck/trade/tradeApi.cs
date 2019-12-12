using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using OdinSdk.BaseLib.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.CoinCheck.Trade
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
                    base.tradeClient = new CoinCheckClient("trade", __connect_key, __secret_key);

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
                    base.publicApi = new CCXT.NET.CoinCheck.Public.PublicApi();

                return base.publicApi;
            }
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
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = tradeClient.MergeParamsAndArgs(args);

                var _json_value = await tradeClient.CallApiGet1Async("/api/exchange/orders/opens", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _orders = tradeClient.DeserializeObject<CMyOrders>(_json_value.Content);
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
        /// View your active positions.
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

                var _json_value = await tradeClient.CallApiGet1Async("/api/exchange/leverage/positions", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<CMyPositions>(_json_value.Content);
                    {
                        foreach (var _p in _json_data.result)
                        {
                            _p.orderType = OrderType.Position;
                            _p.sideType = _p.quantity < 0 ? SideType.Ask : SideType.Bid;
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
        ///
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

                var _params = tradeClient.MergeParamsAndArgs(args);

                var _json_value = await tradeClient.CallApiGet1Async("/api/exchange/orders/transactions", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = tradeClient.DeserializeObject<CMyTrades>(_json_value.Content);
                    {
                        var _trades = _json_data.result
                                                .Where(t => t.timestamp >= since)
                                                .OrderByDescending(t => t.timestamp)
                                                .Take(limits);

                        foreach (var _t in _trades)
                        {
                            _t.amount = _t.quantity * _t.price;
                            _t.fee = Math.Abs(_t.fee);
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
                    _params.Add("pair", _market.result.symbol);
                    _params.Add("order_type", _buy_sell);
                    _params.Add("rate", price);
                    _params.Add("amount", quantity);

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async($"/api/exchange/orders", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _order = tradeClient.DeserializeObject<CPlaceOrderItem>(_json_value.Content);
                    {
                        _order.orderType = OrderType.Limit;
                        _order.orderStatus = OrderStatus.Open;
                        _order.amount = (_order.quantity * _order.price).Normalize();
                        //_order.fee = quantity * price * publicApi.ExchangeInfo.Fees.trading.maker;

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
        /// Submit a new Order
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="quantity">Market, Spot, Sell. amount Order amount. ex) 0.1.
        /// In MarketBuy you have to specify JPY not BTC. market_buy_amount Market buy amount in JPY not BTC.ex) 10000</param>
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

                var _buy_sell = sideType == SideType.Bid ? "buy" : "sell";

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("pair", _market.result.symbol);
                    _params.Add("order_type", $"market_{_buy_sell}");
                    if (_buy_sell == "buy")
                        _params.Add("market_buy_amount", quantity.ToString());
                    else
                        _params.Add("amount", quantity.ToString());

                    tradeClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await tradeClient.CallApiPost1Async($"/api/exchange/orders", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _order = tradeClient.DeserializeObject<CPlaceOrderItem>(_json_value.Content);
                    {
                        _order.orderType = OrderType.Market;
                        _order.amount = (_order.quantity * _order.price).Normalize();
                        //_order.fee = _order.amount * tradeClient.ExchangeInfo.Fees.trading.maker;

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
        public override async ValueTask<MyOrder> CancelOrder(string base_name, string quote_name, string order_id, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _market = await publicApi.LoadMarket(_result.marketId);
            if (_market.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Trade);

                var _params = tradeClient.MergeParamsAndArgs(args);

                var _json_value = await tradeClient.CallApiDelete1Async($"/api/exchange/orders/{order_id}", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = tradeClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _order = tradeClient.DeserializeObject<CCancelOrderItem>(_json_value.Content);
                    {
                        _order.symbol = _market.result.symbol;
                        _order.orderType = OrderType.Limit;
                        _order.orderStatus = OrderStatus.Canceled;
                        _order.sideType = sideType;
                        _order.quantity = quantity;
                        _order.price = price;
                        _order.amount = (_order.quantity * _order.price).Normalize();
                        //_order.fee = quantity * price * publicApi.ExchangeInfo.Fees.trading.maker;
                        _order.timestamp = CUnixTime.NowMilli;
                    }

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