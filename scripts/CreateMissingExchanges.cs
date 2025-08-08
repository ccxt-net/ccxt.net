using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class CreateMissingExchanges
{
    static void Main()
    {
        string sourceDir = @"D:\github.com\lisa3907\ccxt.net\src\exchanges";
        
        // Dictionary of missing exchanges from ccxt.simple
        var missingExchanges = new Dictionary<string, List<string>>
        {
            ["ae"] = new List<string> { "deribit" },
            ["au"] = new List<string> { "btcmarkets", "coinspot" },
            ["br"] = new List<string> { "foxbit", "mercado", "novadax" },
            ["bs"] = new List<string> { "fmfwio" },
            ["ca"] = new List<string> { "ndax", "timex" },
            ["cn"] = new List<string> { "bigone", "bingx", "bitget", "bybit", "coinex", "digifinex", "gate", "hashkey", "hitbtc", "htx", "kucoin", "kucoinfutures", "lbank", "mexc", "okx", "woo", "woofipro", "xt" },
            ["ee"] = new List<string> { "latoken" },
            ["eu"] = new List<string> { "bit2c", "bitopro", "bitvavo", "btcalpha", "btcturk", "coinmate", "exmo", "onetrading", "paymium", "wavesexchange", "whitebit", "yobit", "zonda" },
            ["gb"] = new List<string> { "bitteam", "blockchaincom", "coinmetro", "luno" },
            ["global"] = new List<string> { "coincatch", "defx", "hollaex", "myokx", "oceanex", "oxfun", "p2b", "tradeogre" },
            ["id"] = new List<string> { "indodax", "tokocrypto" },
            ["in"] = new List<string> { "bitbns", "modetrade" },
            ["jp"] = new List<string> { "bitbank", "btcbox", "zaif", "bittrade" },
            ["kr"] = new List<string> { "probit" },
            ["ky"] = new List<string> { "bitmart", "blofin" },
            ["lt"] = new List<string> { "cryptomus" },
            ["mt"] = new List<string> { "bequant" },
            ["mx"] = new List<string> { "bitso" },
            ["sg"] = new List<string> { "bitrue", "coinsph", "delta", "derive", "ellipx", "hibachi", "hyperliquid", "independentreserve" },
            ["us"] = new List<string> { "alpaca", "apex", "ascendex", "binancecoinm", "binanceus", "binanceusdm", "binance", "coinbaseadvanced", "coinbaseexchange", "coinbaseinternational", "coinbase", "crypto", "cryptocom", "krakenfutures", "okcoin", "okxus", "paradex", "phemex", "vertex" }
        };

        int totalCreated = 0;
        int totalSkipped = 0;

        Console.WriteLine("Starting exchange creation process...");
        Console.WriteLine("============================================");

        foreach (var country in missingExchanges)
        {
            string countryCode = country.Key;
            Console.WriteLine($"\nProcessing country: {countryCode.ToUpper()}");
            
            foreach (string exchange in country.Value)
            {
                string exchangePath = Path.Combine(sourceDir, countryCode, exchange);
                
                if (!Directory.Exists(exchangePath))
                {
                    Console.WriteLine($"  Creating: {exchange}");
                    CreateExchangeStructure(sourceDir, countryCode, exchange);
                    totalCreated++;
                }
                else
                {
                    Console.WriteLine($"  Skipping: {exchange} (already exists)");
                    totalSkipped++;
                }
            }
        }

        Console.WriteLine("\n============================================");
        Console.WriteLine("Exchange creation completed!");
        Console.WriteLine($"Total created: {totalCreated}");
        Console.WriteLine($"Total skipped: {totalSkipped}");
    }

    static void CreateExchangeStructure(string sourceDir, string countryCode, string exchangeName)
    {
        string exchangePath = Path.Combine(sourceDir, countryCode, exchangeName);
        
        // Create directories
        Directory.CreateDirectory(exchangePath);
        Directory.CreateDirectory(Path.Combine(exchangePath, "private"));
        Directory.CreateDirectory(Path.Combine(exchangePath, "public"));
        Directory.CreateDirectory(Path.Combine(exchangePath, "trade"));
        
        // Create main exchange class file
        string mainClassFile = Path.Combine(exchangePath, $"{exchangeName}.cs");
        File.WriteAllText(mainClassFile, GenerateMainClass(countryCode, exchangeName));
        
        // Create private API files
        CreatePrivateFiles(exchangePath, countryCode, exchangeName);
        
        // Create public API files
        CreatePublicFiles(exchangePath, countryCode, exchangeName);
        
        // Create trade API files
        CreateTradeFiles(exchangePath, countryCode, exchangeName);
    }

    static string GenerateMainClass(string countryCode, string exchangeName)
    {
        string capitalizedName = char.ToUpper(exchangeName[0]) + exchangeName.Substring(1);
        return $@"using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Configuration;

namespace CCXT.NET.{countryCode.ToUpper()}.{exchangeName}
{{
    /// <summary>
    /// {capitalizedName} exchange implementation
    /// </summary>
    public sealed class {exchangeName}Client : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {{
        /// <summary>
        /// {capitalizedName} API base URL
        /// </summary>
        public override string DealerUrl {{ get; set; }} = """";

        /// <summary>
        /// {capitalizedName} API base path
        /// </summary>
        public override string BaseUrl {{ get; set; }} = ""https://api.{exchangeName}.com"";

        /// <summary>
        /// Constructor
        /// </summary>
        public {exchangeName}Client() : base(""{exchangeName}"", 2)
        {{
        }}

        /// <summary>
        /// Constructor with configuration
        /// </summary>
        public {exchangeName}Client(string _api_key, string _secret_key, string _user_id = """", string _user_password = """") 
            : base(""{exchangeName}"", 2, _api_key, _secret_key, _user_id, _user_password, true)
        {{
        }}

        /// <summary>
        /// Constructor with exchange configuration
        /// </summary>
        public {exchangeName}Client(ExchangeInfo _exchange_info) 
            : base(_exchange_info, 2, true)
        {{
        }}

        public override bool VerifySignature(IXApiClient _src_api_client, Languages _language)
        {{
            return base.VerifySignature(_src_api_client, _language);
        }}
    }}
}}";
    }

    static void CreatePrivateFiles(string exchangePath, string countryCode, string exchangeName)
    {
        var privateFiles = new Dictionary<string, string>
        {
            ["privateApi.cs"] = GeneratePrivateApi(countryCode, exchangeName),
            ["balance.cs"] = GenerateBalanceFile(countryCode, exchangeName),
            ["address.cs"] = GenerateAddressFile(countryCode, exchangeName),
            ["transfer.cs"] = GenerateTransferFile(countryCode, exchangeName)
        };

        foreach (var file in privateFiles)
        {
            string filePath = Path.Combine(exchangePath, "private", file.Key);
            File.WriteAllText(filePath, file.Value);
        }
    }

    static string GeneratePrivateApi(string countryCode, string exchangeName)
    {
        return $@"using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Private;

namespace CCXT.NET.{countryCode.ToUpper()}.{exchangeName}.Private
{{
    /// <summary>
    /// {exchangeName} private API
    /// </summary>
    public class PrivateApi : CCXT.NET.Shared.Coin.Private.PrivateApi, IPrivateApi
    {{
        private readonly string __connect_key;
        private readonly string __secret_key;
        private readonly string __user_id;
        private readonly string __user_password;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrivateApi(string _connect_key, string _secret_key, string _user_id, string _user_password)
        {{
            __connect_key = _connect_key;
            __secret_key = _secret_key;
            __user_id = _user_id;
            __user_password = _user_password;
        }}
    }}
}}";
    }

    static string GenerateBalanceFile(string countryCode, string exchangeName)
    {
        return $@"using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Private;

namespace CCXT.NET.{countryCode.ToUpper()}.{exchangeName}.Private
{{
    /// <summary>
    /// {exchangeName} balance implementation
    /// </summary>
    public class Balance
    {{
        // TODO: Implement balance methods
    }}
}}";
    }

    static string GenerateAddressFile(string countryCode, string exchangeName)
    {
        return $@"using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Private;

namespace CCXT.NET.{countryCode.ToUpper()}.{exchangeName}.Private
{{
    /// <summary>
    /// {exchangeName} address implementation
    /// </summary>
    public class Address
    {{
        // TODO: Implement address methods
    }}
}}";
    }

    static string GenerateTransferFile(string countryCode, string exchangeName)
    {
        return $@"using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Private;

namespace CCXT.NET.{countryCode.ToUpper()}.{exchangeName}.Private
{{
    /// <summary>
    /// {exchangeName} transfer implementation
    /// </summary>
    public class Transfer
    {{
        // TODO: Implement transfer methods
    }}
}}";
    }

    static void CreatePublicFiles(string exchangePath, string countryCode, string exchangeName)
    {
        var publicFiles = new Dictionary<string, string>
        {
            ["publicApi.cs"] = GeneratePublicApi(countryCode, exchangeName),
            ["ticker.cs"] = GenerateTickerFile(countryCode, exchangeName),
            ["orderBook.cs"] = GenerateOrderBookFile(countryCode, exchangeName),
            ["completeOrder.cs"] = GenerateCompleteOrderFile(countryCode, exchangeName),
            ["market.cs"] = GenerateMarketFile(countryCode, exchangeName),
            ["ohlcv.cs"] = GenerateOhlcvFile(countryCode, exchangeName)
        };

        foreach (var file in publicFiles)
        {
            string filePath = Path.Combine(exchangePath, "public", file.Key);
            File.WriteAllText(filePath, file.Value);
        }
    }

    static string GeneratePublicApi(string countryCode, string exchangeName)
    {
        return $@"using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Public;

namespace CCXT.NET.{countryCode.ToUpper()}.{exchangeName}.Public
{{
    /// <summary>
    /// {exchangeName} public API
    /// </summary>
    public class PublicApi : CCXT.NET.Shared.Coin.Public.PublicApi, IPublicApi
    {{
        // TODO: Implement public API methods
    }}
}}";
    }

    static string GenerateTickerFile(string countryCode, string exchangeName)
    {
        return $@"using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Public;

namespace CCXT.NET.{countryCode.ToUpper()}.{exchangeName}.Public
{{
    /// <summary>
    /// {exchangeName} ticker implementation
    /// </summary>
    public class Ticker
    {{
        // TODO: Implement ticker methods
    }}
}}";
    }

    static string GenerateOrderBookFile(string countryCode, string exchangeName)
    {
        return $@"using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Public;

namespace CCXT.NET.{countryCode.ToUpper()}.{exchangeName}.Public
{{
    /// <summary>
    /// {exchangeName} order book implementation
    /// </summary>
    public class OrderBook
    {{
        // TODO: Implement order book methods
    }}
}}";
    }

    static string GenerateCompleteOrderFile(string countryCode, string exchangeName)
    {
        return $@"using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Public;

namespace CCXT.NET.{countryCode.ToUpper()}.{exchangeName}.Public
{{
    /// <summary>
    /// {exchangeName} complete order implementation
    /// </summary>
    public class CompleteOrder
    {{
        // TODO: Implement complete order methods
    }}
}}";
    }

    static string GenerateMarketFile(string countryCode, string exchangeName)
    {
        return $@"using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Public;

namespace CCXT.NET.{countryCode.ToUpper()}.{exchangeName}.Public
{{
    /// <summary>
    /// {exchangeName} market implementation
    /// </summary>
    public class Market
    {{
        // TODO: Implement market methods
    }}
}}";
    }

    static string GenerateOhlcvFile(string countryCode, string exchangeName)
    {
        return $@"using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Public;

namespace CCXT.NET.{countryCode.ToUpper()}.{exchangeName}.Public
{{
    /// <summary>
    /// {exchangeName} OHLCV implementation
    /// </summary>
    public class OHLCV
    {{
        // TODO: Implement OHLCV methods
    }}
}}";
    }

    static void CreateTradeFiles(string exchangePath, string countryCode, string exchangeName)
    {
        var tradeFiles = new Dictionary<string, string>
        {
            ["tradeApi.cs"] = GenerateTradeApi(countryCode, exchangeName),
            ["order.cs"] = GenerateOrderFile(countryCode, exchangeName),
            ["place.cs"] = GeneratePlaceFile(countryCode, exchangeName),
            ["trade.cs"] = GenerateTradeFile(countryCode, exchangeName)
        };

        foreach (var file in tradeFiles)
        {
            string filePath = Path.Combine(exchangePath, "trade", file.Key);
            File.WriteAllText(filePath, file.Value);
        }
    }

    static string GenerateTradeApi(string countryCode, string exchangeName)
    {
        return $@"using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Trade;

namespace CCXT.NET.{countryCode.ToUpper()}.{exchangeName}.Trade
{{
    /// <summary>
    /// {exchangeName} trade API
    /// </summary>
    public class TradeApi : CCXT.NET.Shared.Coin.Trade.TradeApi, ITradeApi
    {{
        private readonly string __connect_key;
        private readonly string __secret_key;
        private readonly string __user_id;
        private readonly string __user_password;

        /// <summary>
        /// Constructor
        /// </summary>
        public TradeApi(string _connect_key, string _secret_key, string _user_id, string _user_password)
        {{
            __connect_key = _connect_key;
            __secret_key = _secret_key;
            __user_id = _user_id;
            __user_password = _user_password;
        }}
    }}
}}";
    }

    static string GenerateOrderFile(string countryCode, string exchangeName)
    {
        return $@"using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Trade;

namespace CCXT.NET.{countryCode.ToUpper()}.{exchangeName}.Trade
{{
    /// <summary>
    /// {exchangeName} order implementation
    /// </summary>
    public class Order
    {{
        // TODO: Implement order methods
    }}
}}";
    }

    static string GeneratePlaceFile(string countryCode, string exchangeName)
    {
        return $@"using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Trade;

namespace CCXT.NET.{countryCode.ToUpper()}.{exchangeName}.Trade
{{
    /// <summary>
    /// {exchangeName} place order implementation
    /// </summary>
    public class Place
    {{
        // TODO: Implement place order methods
    }}
}}";
    }

    static string GenerateTradeFile(string countryCode, string exchangeName)
    {
        return $@"using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Trade;

namespace CCXT.NET.{countryCode.ToUpper()}.{exchangeName}.Trade
{{
    /// <summary>
    /// {exchangeName} trade implementation
    /// </summary>
    public class Trade
    {{
        // TODO: Implement trade methods
    }}
}}";
    }
}