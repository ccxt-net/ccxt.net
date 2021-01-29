using System;
using System.Threading.Tasks;

namespace kraken
{
    class Program
    {
        private static int __step_no = 0;

        private static async Task Main(string[] args)
        {
            var _public_api = new CCXT.NET.Kraken.Public.PublicApi();

            if (__step_no == 0)
            {
                var _ohlcvs = await _public_api.FetchOHLCVsAsync("btc", "usd");
                Console.WriteLine(_ohlcvs.result.Count);
            }

            Console.Out.WriteLine("hit return to exit...");
            Console.ReadLine();
        }
    }
}
