using System;
using System.Threading.Tasks;
using ccxt.samples.exchanges;

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
                    await BinanceSample.Run();
                    break;
                case "2":
                    await BitMEXSample.Run();
                    break;
                case "3":
                    await KrakenSample.Run();
                    break;
                case "4":
                    await BittrexSample.Run();
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
    }
}