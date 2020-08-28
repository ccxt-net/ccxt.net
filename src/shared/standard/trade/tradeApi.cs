using CCXT.NET.Shared.Coin.Public;
using CCXT.NET.Shared.Coin.Types;
using CCXT.NET.Shared.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCXT.NET.Shared.Coin.Trade
{
    /// <summary>
    ///
    /// </summary>
    public interface ITradeApi
    {
        /// <summary>
        ///
        /// </summary>
        XApiClient tradeClient
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        PublicApi publicApi
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="order_id">Order number registered for sale or purchase</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        ValueTask<MyOrder> FetchMyOrderAsync(string base_name, string quote_name, string order_id, Dictionary<string, object> args);

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
        ValueTask<MyOrders> FetchMyOrdersAsync(string base_name, string quote_name, string timeframe, long since, int limits, Dictionary<string, object> args);

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        ///// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        ///// <param name="order_id">Order number registered for sale or purchase</param>
        ///// <param name="args">Add additional attributes for each exchange</param>
        ///// <returns></returns>
        //ValueTask<MyOrder> FetchOpenOrder(string base_name, string quote_name, string order_id, Dictionary<string, object> args);

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        ValueTask<MyOrders> FetchOpenOrdersAsync(string base_name, string quote_name, Dictionary<string, object> args);

        /// <summary>
        ///
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        ValueTask<MyOrders> FetchAllOpenOrdersAsync(Dictionary<string, object> args);

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        ValueTask<MyPositions> FetchOpenPositionsAsync(string base_name, string quote_name, Dictionary<string, object> args);

        /// <summary>
        ///
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        ValueTask<MyPositions> FetchAllOpenPositionsAsync(Dictionary<string, object> args);

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        ///// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        ///// <param name="transaction_id"></param>
        ///// <param name="timeframe">time frame interval (optional): default "1d"</param>
        ///// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        ///// <param name="limits">maximum number of items (optional): default 20</param>
        ///// <param name="args">Add additional attributes for each exchange</param>
        ///// <returns></returns>
        //ValueTask<MyTrades> FetchMyTrade(string base_name, string quote_name, string transaction_id, string timeframe, long since, int limits, Dictionary<string, object> args);

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
        ValueTask<MyTrades> FetchMyTradesAsync(string base_name, string quote_name, string timeframe, long since, int limits, Dictionary<string, object> args);

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="timeframe">time frame interval (optional): default "1d"</param>
        ///// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        ///// <param name="limits">maximum number of items (optional): default 20</param>
        ///// <param name="args">Add additional attributes for each exchange</param>
        ///// <returns></returns>
        //ValueTask<MyTrades> FetchAllMyTrades(string timeframe, long since, int limits, Dictionary<string, object> args);

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="price">price of coin</param>
        /// <param name="sideType">type of buy(bid) or sell(ask)</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        ValueTask<MyOrder> CreateOrderAsync(string base_name, string quote_name, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args);

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="price">price of coin</param>
        /// <param name="sideType">type of buy(bid) or sell(ask)</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        ValueTask<MyOrder> CreateLimitOrderAsync(string base_name, string quote_name, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args);

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="price">price of coin</param>
        /// <param name="sideType">type of buy(bid) or sell(ask)</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        ValueTask<MyOrder> CreateMarketOrderAsync(string base_name, string quote_name, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args);

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="order_id">Order number registered for sale or purchase</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="price">price of coin</param>
        /// <param name="sideType">type of buy(bid) or sell(ask)</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        ValueTask<MyOrder> CancelOrderAsync(string base_name, string quote_name, string order_id, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args);

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="order_ids"></param>
        /// <param name="args">Add additional attributes for each exchange</param>
        ValueTask<MyOrders> CancelOrdersAsync(string base_name, string quote_name, string[] order_ids, Dictionary<string, object> args);

        /// <summary>
        ///
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        ValueTask<MyOrders> CancelAllOrdersAsync(Dictionary<string, object> args);
    }

    /// <summary>
    ///
    /// </summary>
    public class TradeApi : ITradeApi
    {
        /// <summary>
        ///
        /// </summary>
        public virtual XApiClient tradeClient
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual PublicApi publicApi
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="order_id">Order number registered for sale or purchase</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async ValueTask<MyOrder> FetchMyOrderAsync(string base_name, string quote_name, string order_id = "", Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _currency_id = await publicApi.LoadCurrencyIdAsync(base_name);
            if (_currency_id.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new MyOrderItem();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return await Task.FromResult(_result);
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
        public virtual async ValueTask<MyOrders> FetchMyOrdersAsync(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new MyOrders(base_name, quote_name);

            var _currency_id = await publicApi.LoadCurrencyIdAsync(base_name);
            if (_currency_id.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _timeframe = tradeClient.ExchangeInfo.GetTimeframe(timeframe);
                var _timestamp = tradeClient.ExchangeInfo.GetTimestamp(timeframe);

                _result.result = new List<IMyOrderItem>();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async ValueTask<MyOrders> FetchOpenOrdersAsync(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new MyOrders(base_name, quote_name);

            var _currency_id = await publicApi.LoadCurrencyIdAsync(base_name);
            if (_currency_id.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new List<IMyOrderItem>();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async ValueTask<MyOrders> FetchAllOpenOrdersAsync(Dictionary<string, object> args = null)
        {
            var _result = new MyOrders();

            var _markets = await publicApi.LoadMarketsAsync();
            if (_markets.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new List<IMyOrderItem>();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_markets);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async ValueTask<MyPositions> FetchOpenPositionsAsync(string base_name, string quote_name, Dictionary<string, object> args)
        {
            var _result = new MyPositions(base_name, quote_name);

            var _market = await publicApi.LoadMarketAsync(_result.marketId ?? "");
            if (_market.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new List<IMyPositionItem>();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_market);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async ValueTask<MyPositions> FetchAllOpenPositionsAsync(Dictionary<string, object> args = null)
        {
            var _result = new MyPositions();

            var _markets = await publicApi.LoadMarketsAsync();
            if (_markets.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new List<IMyPositionItem>();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_markets);
            }

            return await Task.FromResult(_result);
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
        public virtual async ValueTask<MyTrades> FetchMyTradesAsync(string base_name, string quote_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new MyTrades(base_name, quote_name);

            var _currency_id = await publicApi.LoadCurrencyIdAsync(base_name);
            if (_currency_id.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _timeframe = tradeClient.ExchangeInfo.GetTimeframe(timeframe);
                var _timestamp = tradeClient.ExchangeInfo.GetTimestamp(timeframe);

                _result.result = new List<IMyTradeItem>();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="price">price of coin</param>
        /// <param name="sideType">type of buy(bid) or sell(ask)</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async ValueTask<MyOrder> CreateOrderAsync(string base_name, string quote_name, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _market = await publicApi.LoadMarketAsync(_result.marketId ?? "");
            if (_market.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new MyOrderItem
                {
                    timestamp = CUnixTime.NowMilli,
                    orderType = OrderType.Limit,
                    sideType = sideType,
                    orderStatus = OrderStatus.Unknown,
                    quantity = quantity,
                    price = price,
                    amount = quantity * price
                };

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_market);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="price">price of coin</param>
        /// <param name="sideType">type of buy(bid) or sell(ask)</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async ValueTask<MyOrder> CreateLimitOrderAsync(string base_name, string quote_name, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _market = await publicApi.LoadMarketAsync(_result.marketId ?? "");
            if (_market.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new MyOrderItem
                {
                    timestamp = CUnixTime.NowMilli,
                    orderType = OrderType.Limit,
                    sideType = sideType,
                    orderStatus = OrderStatus.Unknown,
                    quantity = quantity,
                    price = price,
                    amount = quantity * price
                };

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_market);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="price">price of coin</param>
        /// <param name="sideType">type of buy(bid) or sell(ask)</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async ValueTask<MyOrder> CreateMarketOrderAsync(string base_name, string quote_name, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _market = await publicApi.LoadMarketAsync(_result.marketId ?? "");
            if (_market.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new MyOrderItem
                {
                    timestamp = CUnixTime.NowMilli,
                    orderType = OrderType.Market,
                    sideType = sideType,
                    orderStatus = OrderStatus.Unknown,
                    quantity = quantity,
                    price = price,
                    amount = quantity * price
                };

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_market);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="order_id">Order number registered for sale or purchase</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="price">price of coin</param>
        /// <param name="sideType">type of buy(bid) or sell(ask)</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async ValueTask<MyOrder> CancelOrderAsync(string base_name, string quote_name, string order_id, decimal quantity, decimal price, SideType sideType, Dictionary<string, object> args = null)
        {
            var _result = new MyOrder(base_name, quote_name);

            var _currency_id = await publicApi.LoadCurrencyIdAsync(base_name);
            if (_currency_id.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new MyOrderItem
                {
                    orderId = order_id,
                    quantity = quantity,
                    price = price,
                    amount = quantity * price,
                    sideType = sideType
                };

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="order_ids"></param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async ValueTask<MyOrders> CancelOrdersAsync(string base_name, string quote_name, string[] order_ids, Dictionary<string, object> args = null)
        {
            var _result = new MyOrders(base_name, quote_name);

            var _currency_id = await publicApi.LoadCurrencyIdAsync(base_name);
            if (_currency_id.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new List<IMyOrderItem>();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async ValueTask<MyOrders> CancelAllOrdersAsync(Dictionary<string, object> args = null)
        {
            var _result = new MyOrders();

            var _markets = await publicApi.LoadMarketsAsync();
            if (_markets.success == true)
            {
                tradeClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new List<IMyOrderItem>();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_markets);
            }

            return await Task.FromResult(_result);
        }
    }
}