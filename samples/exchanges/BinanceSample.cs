using System;
using System.Linq;
using System.Threading.Tasks;

namespace ccxt.samples.exchanges
{
    public static class BinanceSample
    {
        public static async Task Run()
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
    }
}