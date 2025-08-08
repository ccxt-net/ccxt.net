using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCXT.NET.BitMEX.Trade;
using CCXT.NET.Shared.Coin.Types;

namespace ccxt.samples
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("CCXT.NET Sample Application");
            Console.WriteLine("===========================");
            Console.WriteLine("Select an exchange to test:");
            Console.WriteLine("1. Binance");
            Console.WriteLine("2. BitMEX");
            Console.WriteLine("3. Kraken");
            Console.WriteLine("4. Bittrex");
            Console.WriteLine("0. Exit");
            Console.WriteLine();

            Console.Write("Enter your choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await RunBinanceSample();
                    break;
                case "2":
                    await RunBitMEXSample();
                    break;
                case "3":
                    await RunKrakenSample();
                    break;
                case "4":
                    await RunBittrexSample();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        #region Binance Sample

        private static async Task RunBinanceSample()
        {
            Console.WriteLine("\n=== Binance Sample ===");
            
            var publicApi = new CCXT.NET.Binance.Public.PublicApi();

            Console.WriteLine("Fetching BTC/USD tickers...");
            var tickers = await publicApi.FetchTickersAsync();
            
            if (tickers.success)
            {
                var btcusdTickers = tickers.result.Where(t => t.symbol.ToUpper().Contains("BTCUSD"));

                foreach (var ticker in btcusdTickers)
                {
                    Console.WriteLine($"Symbol: {ticker.symbol}, Close Price: {ticker.closePrice}");
                }
            }
            else
            {
                Console.WriteLine($"Error: {tickers.message}");
            }
        }

        #endregion

        #region BitMEX Sample

        private static async Task RunBitMEXSample()
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
                    await RunBitMEXPublicSample(publicApi);
                    break;
                case "2":
                    await RunBitMEXTradingSample();
                    break;
                case "3":
                    await RunBitMEXBulkOrderSample();
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        private static async Task RunBitMEXPublicSample(CCXT.NET.BitMEX.Public.PublicApi publicApi)
        {
            Console.WriteLine("Fetching BTC/USD OHLCV data...");
            var ohlcvs = await publicApi.FetchOHLCVsAsync("btc", "usd");
            Console.WriteLine($"Fetched {ohlcvs.result.Count} OHLCV records");
        }

        private static async Task RunBitMEXTradingSample()
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

        private static async Task RunBitMEXBulkOrderSample()
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

        #endregion

        #region Kraken Sample

        private static async Task RunKrakenSample()
        {
            Console.WriteLine("\n=== Kraken Sample ===");
            Console.WriteLine("Select operation:");
            Console.WriteLine("1. Fetch OHLCV data");
            Console.WriteLine("2. Fetch Order Books");
            
            Console.Write("Enter your choice: ");
            var choice = Console.ReadLine();

            var publicApi = new CCXT.NET.Kraken.Public.PublicApi();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Fetching BTC/USD OHLCV data...");
                    var ohlcvs = await publicApi.FetchOHLCVsAsync("btc", "usd");
                    Console.WriteLine($"Fetched {ohlcvs.result.Count} OHLCV records");
                    break;
                case "2":
                    Console.WriteLine("Fetching BTC/USD order books...");
                    var books = await publicApi.FetchOrderBooksAsync("btc", "usd");
                    Console.WriteLine($"Fetched {books.result.asks.Count} asks in order book");
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        #endregion

        #region Bittrex Sample

        private static async Task RunBittrexSample()
        {
            Console.WriteLine("\n=== Bittrex Sample ===");
            
            var publicApi = new CCXT.NET.Bittrex.Public.PublicApi();

            Console.WriteLine("Fetching BTC tickers...");
            var tickers = await publicApi.FetchTickersAsync();
            
            if (tickers.success)
            {
                var btcTickers = tickers.result.Where(t => t.symbol.ToUpper().Contains("BTC"));

                foreach (var ticker in btcTickers)
                {
                    Console.WriteLine($"Symbol: {ticker.symbol}, Close Price: {ticker.closePrice}");
                }
            }
            else
            {
                Console.WriteLine($"Error: {tickers.message}");
            }
        }

        #endregion
    }
}