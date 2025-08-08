using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCXT.NET.BitMEX.Trade;
using CCXT.NET.Shared.Coin.Types;

namespace ccxt.samples.exchanges
{
    public static class BitMEXSample
    {
        public static async Task Run()
        {
            Console.WriteLine("\n=== BitMEX Sample ===");
            Console.WriteLine("Select operation:");
            Console.WriteLine("1. Fetch OHLCV data (Public)");
            Console.WriteLine("2. Trading operations (Requires API key)");
            Console.WriteLine("3. Bulk order operations (Requires API key)");
            
            Console.Write("Enter your choice: ");
            var choice = Console.ReadLine();

            var publicApi = new CCXT.NET.BitMEX.Public.PublicApi();

            switch (choice)
            {
                case "1":
                    await RunPublicSample(publicApi);
                    break;
                case "2":
                    await RunTradingSample();
                    break;
                case "3":
                    await RunBulkOrderSample();
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        private static async Task RunPublicSample(CCXT.NET.BitMEX.Public.PublicApi publicApi)
        {
            Console.WriteLine("Fetching BTC/USD OHLCV data...");
            var ohlcvs = await publicApi.FetchOHLCVsAsync("btc", "usd");
            Console.WriteLine($"Fetched {ohlcvs.result.Count} OHLCV records");
        }

        private static async Task RunTradingSample()
        {
            Console.WriteLine("Note: This requires API credentials.");
            Console.Write("Enter API Key (or press Enter to skip): ");
            var apiKey = Console.ReadLine();
            
            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("Skipping trading sample - no API key provided.");
                return;
            }

            Console.Write("Enter API Secret: ");
            var apiSecret = Console.ReadLine();

            var tradeApi = new CCXT.NET.BitMEX.Trade.TradeApi(apiKey, apiSecret, is_live: false);

            decimal initialSellLimit = 60000m;
            decimal amendedSellLimit = 59500m;
            decimal orderQuantity = 100m;

            Console.WriteLine($"Placing sell limit order at {initialSellLimit}...");
            var limitOrder = await tradeApi.CreateLimitOrderAsync("BTC", "USD", orderQuantity, initialSellLimit, SideType.Ask);

            if (limitOrder.result.orderStatus == OrderStatus.Open)
            {
                Console.WriteLine($"Changing limit of the sell order to {amendedSellLimit}...");
                limitOrder = await tradeApi.UpdateOrder("BTC", "USD", limitOrder.result.orderId, 
                    limitOrder.result.quantity, amendedSellLimit, limitOrder.result.sideType);
            }

            if (limitOrder.result.orderStatus == OrderStatus.Open)
            {
                Console.WriteLine("Cancelling order...");
                limitOrder = await tradeApi.CancelOrderAsync("BTC", "USD", limitOrder.result.orderId, 
                    0m, 0m, SideType.Unknown);
            }

            if (limitOrder.result.orderStatus == OrderStatus.Canceled)
            {
                Console.WriteLine("Order successfully cancelled.");
            }
        }

        private static async Task RunBulkOrderSample()
        {
            Console.WriteLine("Note: This requires API credentials.");
            Console.Write("Enter API Key (or press Enter to skip): ");
            var apiKey = Console.ReadLine();
            
            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("Skipping bulk order sample - no API key provided.");
                return;
            }

            Console.Write("Enter API Secret: ");
            var apiSecret = Console.ReadLine();

            var tradeApi = new CCXT.NET.BitMEX.Trade.TradeApi(apiKey, apiSecret, is_live: false);

            decimal initialSellLimit = 60000m;
            decimal amendedSellLimit = 59500m;
            decimal orderQuantity = 100m;

            var order1 = new BBulkOrderItem(symbol: "XBTUSD", side: SideType.Ask, 
                orderType: OrderType.Limit, orderQty: orderQuantity, price: initialSellLimit);
            var order2 = new BBulkOrderItem(symbol: "XBTUSD", side: SideType.Ask, 
                orderType: OrderType.Limit, orderQty: orderQuantity * 2, price: initialSellLimit);
            var orders = new List<BBulkOrderItem> { order1, order2 };

            Console.WriteLine($"Placing sell limit bulk order at {initialSellLimit}...");
            var limitOrders = await tradeApi.CreateBulkOrder(orders);

            if (!limitOrders.success)
            {
                Console.Error.WriteLine($"Error {limitOrders.errorCode}, message: {limitOrders.message}");
                return;
            }

            if (limitOrders.result.All(x => x.orderStatus == OrderStatus.Open))
            {
                var orderUpdates = limitOrders.result.Select(x => new BBulkUpdateOrderItem
                {
                    orderID = x.orderId,
                    orderQty = x.quantity,
                    price = amendedSellLimit
                });

                Console.WriteLine($"Changing limit of the sell orders to {amendedSellLimit} in bulk...");
                limitOrders = await tradeApi.UpdateOrders(orderUpdates.ToList());
            }

            if (!limitOrders.success)
            {
                Console.Error.WriteLine($"Error {limitOrders.errorCode}, message: {limitOrders.message}");
                return;
            }

            if (limitOrders.result.All(x => x.orderStatus == OrderStatus.Open))
            {
                Console.WriteLine("Cancelling orders...");
                limitOrders = await tradeApi.CancelOrdersAsync("BTC", "USD", 
                    limitOrders.result.Select(x => x.orderId).ToArray());
            }

            if (limitOrders.result.All(x => x.orderStatus == OrderStatus.Canceled))
            {
                Console.WriteLine("Orders successfully bulk cancelled.");
            }

            if (!limitOrders.success)
            {
                Console.Error.WriteLine($"Error {limitOrders.errorCode}, message: {limitOrders.message}");
            }
        }
    }
}