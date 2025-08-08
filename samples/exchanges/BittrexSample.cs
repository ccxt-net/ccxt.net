using System;
using System.Linq;
using System.Threading.Tasks;

namespace ccxt.samples.exchanges
{
    public static class BittrexSample
    {
        public static async Task Run()
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
    }
}