# PowerShell script to create missing exchange implementations

$sourceDir = "D:\github.com\lisa3907\ccxt.net\src\exchanges"

# List of missing exchanges from ccxt.simple
$missingExchanges = @{
    "ae" = @("deribit")
    "au" = @("btcmarkets", "coinspot")
    "br" = @("foxbit", "mercado", "novadax")
    "bs" = @("fmfwio")
    "ca" = @("ndax", "timex")
    "cn" = @("bigone", "bingx", "bitget", "bybit", "coinex", "digifinex", "gate", "hashkey", "hitbtc", "htx", "kucoin", "kucoinfutures", "lbank", "mexc", "okx", "woo", "woofipro", "xt")
    "ee" = @("latoken")
    "eu" = @("bit2c", "bitopro", "bitvavo", "btcalpha", "btcturk", "coinmate", "exmo", "onetrading", "paymium", "wavesexchange", "whitebit", "yobit", "zonda")
    "gb" = @("bitteam", "blockchaincom", "coinmetro", "luno")
    "global" = @("coincatch", "defx", "hollaex", "myokx", "oceanex", "oxfun", "p2b", "tradeogre")
    "id" = @("indodax", "tokocrypto")
    "in" = @("bitbns", "modetrade")
    "jp" = @("bitbank", "btcbox", "zaif", "bittrade")
    "kr" = @("probit")
    "ky" = @("bitmart", "blofin")
    "lt" = @("cryptomus")
    "mt" = @("bequant")
    "mx" = @("bitso")
    "sg" = @("bitrue", "coinsph", "delta", "derive", "ellipx", "hibachi", "hyperliquid", "independentreserve")
    "us" = @("alpaca", "apex", "ascendex", "binancecoinm", "binanceus", "binanceusdm", "binance", "coinbaseadvanced", "coinbaseexchange", "coinbaseinternational", "coinbase", "crypto", "cryptocom", "krakenfutures", "okcoin", "okxus", "paradex", "phemex", "vertex")
}

function Create-ExchangeFile {
    param (
        [string]$countryCode,
        [string]$exchangeName,
        [string]$fileType,
        [string]$folderName,
        [string]$fileName
    )
    
    $exchangePath = Join-Path $sourceDir $countryCode $exchangeName $folderName
    $filePath = Join-Path $exchangePath $fileName
    
    if (!(Test-Path $exchangePath)) {
        New-Item -ItemType Directory -Path $exchangePath -Force | Out-Null
    }
    
    if (!(Test-Path $filePath)) {
        $capitalizedName = $exchangeName.Substring(0,1).ToUpper() + $exchangeName.Substring(1)
        $namespace = "CCXT.NET.$($countryCode.ToUpper()).$exchangeName"
        
        switch ($fileType) {
            "main" {
                $content = @"
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Configuration;

namespace $namespace
{
    /// <summary>
    /// $capitalizedName exchange implementation
    /// </summary>
    public sealed class ${exchangeName}Client : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        /// $capitalizedName API base URL
        /// </summary>
        public override string DealerUrl { get; set; } = "";

        /// <summary>
        /// $capitalizedName API base path
        /// </summary>
        public override string BaseUrl { get; set; } = "https://api.$exchangeName.com";

        /// <summary>
        /// Constructor
        /// </summary>
        public ${exchangeName}Client() : base("$exchangeName", 2)
        {
        }

        /// <summary>
        /// Constructor with configuration
        /// </summary>
        public ${exchangeName}Client(string _api_key, string _secret_key, string _user_id = "", string _user_password = "") 
            : base("$exchangeName", 2, _api_key, _secret_key, _user_id, _user_password, true)
        {
        }

        /// <summary>
        /// Constructor with exchange configuration
        /// </summary>
        public ${exchangeName}Client(ExchangeInfo _exchange_info) 
            : base(_exchange_info, 2, true)
        {
        }

        public override bool VerifySignature(IXApiClient _src_api_client, Languages _language)
        {
            return base.VerifySignature(_src_api_client, _language);
        }
    }
}
"@
            }
            "privateApi" {
                $content = @"
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Private;

namespace $namespace.Private
{
    /// <summary>
    /// $exchangeName private API
    /// </summary>
    public class PrivateApi : CCXT.NET.Shared.Coin.Private.PrivateApi, IPrivateApi
    {
        private readonly string __connect_key;
        private readonly string __secret_key;
        private readonly string __user_id;
        private readonly string __user_password;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrivateApi(string _connect_key, string _secret_key, string _user_id, string _user_password)
        {
            __connect_key = _connect_key;
            __secret_key = _secret_key;
            __user_id = _user_id;
            __user_password = _user_password;
        }
    }
}
"@
            }
            "publicApi" {
                $content = @"
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Public;

namespace $namespace.Public
{
    /// <summary>
    /// $exchangeName public API
    /// </summary>
    public class PublicApi : CCXT.NET.Shared.Coin.Public.PublicApi, IPublicApi
    {
        // TODO: Implement public API methods
    }
}
"@
            }
            "tradeApi" {
                $content = @"
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Trade;

namespace $namespace.Trade
{
    /// <summary>
    /// $exchangeName trade API
    /// </summary>
    public class TradeApi : CCXT.NET.Shared.Coin.Trade.TradeApi, ITradeApi
    {
        private readonly string __connect_key;
        private readonly string __secret_key;
        private readonly string __user_id;
        private readonly string __user_password;

        /// <summary>
        /// Constructor
        /// </summary>
        public TradeApi(string _connect_key, string _secret_key, string _user_id, string _user_password)
        {
            __connect_key = _connect_key;
            __secret_key = _secret_key;
            __user_id = _user_id;
            __user_password = _user_password;
        }
    }
}
"@
            }
            default {
                $classType = switch ($fileName) {
                    "balance.cs" { "Balance" }
                    "address.cs" { "Address" }
                    "transfer.cs" { "Transfer" }
                    "ticker.cs" { "Ticker" }
                    "orderBook.cs" { "OrderBook" }
                    "completeOrder.cs" { "CompleteOrder" }
                    "market.cs" { "Market" }
                    "ohlcv.cs" { "OHLCV" }
                    "order.cs" { "Order" }
                    "place.cs" { "Place" }
                    "trade.cs" { "Trade" }
                    default { "UnknownClass" }
                }
                
                $subNamespace = switch ($folderName) {
                    "private" { "Private" }
                    "public" { "Public" }
                    "trade" { "Trade" }
                    default { "" }
                }
                
                $content = @"
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.$subNamespace;

namespace $namespace.$subNamespace
{
    /// <summary>
    /// $exchangeName $classType implementation
    /// </summary>
    public class $classType
    {
        // TODO: Implement $classType methods
    }
}
"@
            }
        }
        
        Set-Content -Path $filePath -Value $content
        Write-Host "Created: $filePath" -ForegroundColor Green
    }
}

Write-Host "Starting missing exchange creation..." -ForegroundColor Yellow
Write-Host "============================================" -ForegroundColor Yellow

$totalCreated = 0
$totalSkipped = 0

foreach ($country in $missingExchanges.GetEnumerator()) {
    $countryCode = $country.Key
    Write-Host "`nProcessing country: $countryCode" -ForegroundColor Cyan
    
    foreach ($exchange in $country.Value) {
        $exchangePath = Join-Path $sourceDir $countryCode $exchange
        
        if (!(Test-Path $exchangePath)) {
            Write-Host "  Creating exchange: $exchange" -ForegroundColor White
            
            # Create main exchange file
            Create-ExchangeFile -countryCode $countryCode -exchangeName $exchange -fileType "main" -folderName "" -fileName "$exchange.cs"
            
            # Create private API files
            Create-ExchangeFile -countryCode $countryCode -exchangeName $exchange -fileType "privateApi" -folderName "private" -fileName "privateApi.cs"
            Create-ExchangeFile -countryCode $countryCode -exchangeName $exchange -fileType "default" -folderName "private" -fileName "balance.cs"
            Create-ExchangeFile -countryCode $countryCode -exchangeName $exchange -fileType "default" -folderName "private" -fileName "address.cs"
            Create-ExchangeFile -countryCode $countryCode -exchangeName $exchange -fileType "default" -folderName "private" -fileName "transfer.cs"
            
            # Create public API files
            Create-ExchangeFile -countryCode $countryCode -exchangeName $exchange -fileType "publicApi" -folderName "public" -fileName "publicApi.cs"
            Create-ExchangeFile -countryCode $countryCode -exchangeName $exchange -fileType "default" -folderName "public" -fileName "ticker.cs"
            Create-ExchangeFile -countryCode $countryCode -exchangeName $exchange -fileType "default" -folderName "public" -fileName "orderBook.cs"
            Create-ExchangeFile -countryCode $countryCode -exchangeName $exchange -fileType "default" -folderName "public" -fileName "completeOrder.cs"
            Create-ExchangeFile -countryCode $countryCode -exchangeName $exchange -fileType "default" -folderName "public" -fileName "market.cs"
            Create-ExchangeFile -countryCode $countryCode -exchangeName $exchange -fileType "default" -folderName "public" -fileName "ohlcv.cs"
            
            # Create trade API files
            Create-ExchangeFile -countryCode $countryCode -exchangeName $exchange -fileType "tradeApi" -folderName "trade" -fileName "tradeApi.cs"
            Create-ExchangeFile -countryCode $countryCode -exchangeName $exchange -fileType "default" -folderName "trade" -fileName "order.cs"
            Create-ExchangeFile -countryCode $countryCode -exchangeName $exchange -fileType "default" -folderName "trade" -fileName "place.cs"
            Create-ExchangeFile -countryCode $countryCode -exchangeName $exchange -fileType "default" -folderName "trade" -fileName "trade.cs"
            
            $totalCreated++
        } else {
            Write-Host "  Skipping: $exchange (already exists)" -ForegroundColor Gray
            $totalSkipped++
        }
    }
}

Write-Host "`n============================================" -ForegroundColor Yellow
Write-Host "Exchange creation completed!" -ForegroundColor Green
Write-Host "Total created: $totalCreated" -ForegroundColor White
Write-Host "Total skipped: $totalSkipped" -ForegroundColor White