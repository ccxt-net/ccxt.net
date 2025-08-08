using System;
using System.Threading.Tasks;

namespace ccxt.samples.exchanges
{
    public static class KrakenSample
    {
        public static async Task Run()
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
    }
}