using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCXT.NET.BitMEX.Trade;
using CCXT.NET.Shared.Coin.Types;

namespace bitmex
{
    class Program
    {
        private static int __step_no = 2;

        private static readonly string API_KEY = "N5lkLW2eUsVF3CxaxpaM5ry7";    // Put your API key
        private static readonly string API_SECRET = "ORKlDys1wouXV84H_G_H5zxN_VrF8IsUf_MNYux1B4GFfHJd"; // Put your API secret;
        private static decimal initial_sell_limit = 60000m;
        private static decimal amended_sell_limit = 59500m;
        private static decimal order_quantity = 100m;

        private static async Task<int> Main(string[] args)
        {
            var _public_api = new CCXT.NET.BitMEX.Public.PublicApi();
            var _private_api = new CCXT.NET.BitMEX.Private.PrivateApi(API_KEY, API_SECRET, is_live: false);
            var _trade_api = new CCXT.NET.BitMEX.Trade.TradeApi(API_KEY, API_SECRET, is_live: false);

            if (__step_no == 0)
            {
                var _ohlcvs = await _public_api.FetchOHLCVsAsync("btc", "usd");
                Console.WriteLine(_ohlcvs.result.Count);
            }

            if (__step_no == 1)
            {
                Console.Out.WriteLine($"Placing sell limit order at {initial_sell_limit}...");
                var _limit_order = await _trade_api.CreateLimitOrderAsync("BTC", "USD", order_quantity, initial_sell_limit, SideType.Ask);

                if (_limit_order.result.orderStatus == OrderStatus.Open)
                {
                    Console.Out.WriteLine($"Changing limit of the sell order to {amended_sell_limit}...");
                    _limit_order = await _trade_api.UpdateOrder("BTC", "USD", _limit_order.result.orderId, _limit_order.result.quantity, amended_sell_limit, _limit_order.result.sideType);
                }

                if (_limit_order.result.orderStatus == OrderStatus.Open)
                {
                    Console.Out.WriteLine("Cancelling order...");
                    _limit_order  = await _trade_api.CancelOrderAsync("BTC", "USD", _limit_order.result.orderId, 0m, 0m, SideType.Unknown);
                }
                if (_limit_order.result.orderStatus == OrderStatus.Canceled)
                {
                    Console.Out.WriteLine("Order successfully cancelled.");
                }
            }
            if (__step_no == 2)
            {
                var _order1 = new BBulkOrderItem(symbol: "XBTUSD", side: SideType.Ask, orderType: OrderType.Limit, orderQty: order_quantity, price: initial_sell_limit);
                var _order2 = new BBulkOrderItem(symbol: "XBTUSD", side: SideType.Ask, orderType: OrderType.Limit, orderQty: order_quantity * 2, price: initial_sell_limit);
                var _orders = new List<BBulkOrderItem> { _order1, _order2 };
                Console.Out.WriteLine($"Placing sell limit bulk order at {initial_sell_limit}...");
                var _limit_orders = await _trade_api.CreateBulkOrder(_orders);
                if (!_limit_orders.success)
                {
                    Console.Error.WriteLine($"Error {_limit_orders.errorCode}, message: {_limit_orders.message}");
                    return _limit_orders.statusCode;
                }

                if (_limit_orders.result.All(x => x.orderStatus == OrderStatus.Open))
                //if (_limit_order.result.orderStatus == OrderStatus.Open)
                {
                    var _order_updates = _limit_orders.result.Select(x => new BBulkUpdateOrderItem
                    {
                        orderID = x.orderId,
                        orderQty = x.quantity,
                        price = amended_sell_limit
                    });
                    Console.Out.WriteLine($"Changing limit of the sell orders to {amended_sell_limit} in bulk...");
                    _limit_orders = await _trade_api.UpdateOrders(_order_updates.ToList());
                }
                if (!_limit_orders.success)
                {
                    Console.Error.WriteLine($"Error {_limit_orders.errorCode}, message: {_limit_orders.message}");
                    return _limit_orders.statusCode;
                }

                if (_limit_orders.result.All(x => x.orderStatus == OrderStatus.Open))
                {
                    Console.Out.WriteLine("Cancelling orders...");
                    _limit_orders = await _trade_api.CancelOrdersAsync("BTC", "USD", _limit_orders.result.Select(x => x.orderId).ToArray());
                }
                if (_limit_orders.result.All(x => x.orderStatus == OrderStatus.Canceled))
                {
                    Console.Out.WriteLine("Orders successfully bulk cancelled.");
                }
                if (!_limit_orders.success)
                {
                    Console.Error.WriteLine($"Error {_limit_orders.errorCode}, message: {_limit_orders.message}");
                    return _limit_orders.statusCode;
                }
            }

            Console.Out.WriteLine("hit return to exit...");
            Console.ReadLine();
            return 0;
        }
    }
}
