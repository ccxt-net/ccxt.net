using System;
using System.Threading.Tasks;

namespace Binance
{
    class Program
    {
        private static int __step_no = 0;

        private static async Task Main(string[] args)
        {
            var _public_api = new CCXT.NET.Binance.Public.PublicApi();

            if (__step_no == 0 || __step_no == 1)
            {
                var _tickers = await _public_api.FetchTickersAsync();
                if (_tickers.success == true)
                {
                    foreach (var _t in _tickers.result)
                        Console.Out.WriteLine($"symbol: {_t.symbol}, closePrice: {_t.closePrice}");
                }
                else
                {
                    Console.Out.WriteLine($"error: {_tickers.message}");
                }
            }

            Console.Out.WriteLine("hit return to exit...");
            Console.ReadLine();
        }
    }
}