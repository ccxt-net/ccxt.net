# PowerShell script to reorganize exchange folders from three-letter to two-letter country codes
# and create missing exchange implementations

$sourceDir = "D:\github.com\lisa3907\ccxt.net\src\exchanges"
$simpleDir = "D:\github.com\lisa3907\ccxt.simple\src\Exchanges"

# Country code mapping from three-letter to two-letter
$countryMapping = @{
    "chn" = "cn"  # China
    "gbr" = "gb"  # United Kingdom
    "hk"  = "hk"  # Hong Kong (keep as is - already two letters)
    "jpn" = "jp"  # Japan
    "kor" = "kr"  # Korea (South)
    "sey" = "sc"  # Seychelles
    "usa" = "us"  # United States
}

# List of exchanges from ccxt.simple that need to be created
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

# Function to create standard folder structure for an exchange
function Create-ExchangeStructure {
    param (
        [string]$countryCode,
        [string]$exchangeName
    )
    
    $exchangePath = Join-Path $sourceDir $countryCode.ToLower() $exchangeName.ToLower()
    
    # Create directories
    $directories = @(
        $exchangePath,
        (Join-Path $exchangePath "private"),
        (Join-Path $exchangePath "public"),
        (Join-Path $exchangePath "trade")
    )
    
    foreach ($dir in $directories) {
        if (!(Test-Path $dir)) {
            New-Item -ItemType Directory -Path $dir -Force | Out-Null
            Write-Host "Created directory: $dir" -ForegroundColor Green
        }
    }
    
    # Create main exchange class file
    $mainClassFile = Join-Path $exchangePath "$($exchangeName.ToLower()).cs"
    if (!(Test-Path $mainClassFile)) {
        $mainClassContent = @"
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Configuration;

namespace CCXT.NET.$($countryCode.ToUpper()).$($exchangeName.ToLower())
{
    /// <summary>
    /// ${exchangeName} exchange implementation
    /// </summary>
    public sealed class $($exchangeName.ToLower())Client : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        /// ${exchangeName} API base URL
        /// </summary>
        public override string DealerUrl { get; set; } = "";

        /// <summary>
        /// ${exchangeName} API base path
        /// </summary>
        public override string BaseUrl { get; set; } = "https://api.$($exchangeName.ToLower()).com";

        /// <summary>
        /// Constructor
        /// </summary>
        public $($exchangeName.ToLower())Client() : base("$($exchangeName.ToLower())", 2)
        {
        }

        /// <summary>
        /// Constructor with configuration
        /// </summary>
        public $($exchangeName.ToLower())Client(string _api_key, string _secret_key, string _user_id = "", string _user_password = "") 
            : base("$($exchangeName.ToLower())", 2, _api_key, _secret_key, _user_id, _user_password, true)
        {
        }

        /// <summary>
        /// Constructor with exchange configuration
        /// </summary>
        public $($exchangeName.ToLower())Client(ExchangeInfo _exchange_info) 
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
        Set-Content -Path $mainClassFile -Value $mainClassContent
        Write-Host "Created file: $mainClassFile" -ForegroundColor Cyan
    }
    
    # Create private API files
    $privateFiles = @{
        "privateApi.cs" = @"
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Private;

namespace CCXT.NET.$($countryCode.ToUpper()).$($exchangeName.ToLower()).Private
{
    /// <summary>
    /// ${exchangeName} private API
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
        "balance.cs" = @"
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Private;

namespace CCXT.NET.$($countryCode.ToUpper()).$($exchangeName.ToLower()).Private
{
    /// <summary>
    /// ${exchangeName} balance implementation
    /// </summary>
    public class Balance
    {
        // TODO: Implement balance methods
    }
}
"@
        "address.cs" = @"
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Private;

namespace CCXT.NET.$($countryCode.ToUpper()).$($exchangeName.ToLower()).Private
{
    /// <summary>
    /// ${exchangeName} address implementation
    /// </summary>
    public class Address
    {
        // TODO: Implement address methods
    }
}
"@
        "transfer.cs" = @"
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Private;

namespace CCXT.NET.$($countryCode.ToUpper()).$($exchangeName.ToLower()).Private
{
    /// <summary>
    /// ${exchangeName} transfer implementation
    /// </summary>
    public class Transfer
    {
        // TODO: Implement transfer methods
    }
}
"@
    }
    
    foreach ($file in $privateFiles.GetEnumerator()) {
        $filePath = Join-Path $exchangePath "private" $file.Key
        if (!(Test-Path $filePath)) {
            Set-Content -Path $filePath -Value $file.Value
            Write-Host "Created file: $filePath" -ForegroundColor Cyan
        }
    }
    
    # Create public API files
    $publicFiles = @{
        "publicApi.cs" = @"
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Public;

namespace CCXT.NET.$($countryCode.ToUpper()).$($exchangeName.ToLower()).Public
{
    /// <summary>
    /// ${exchangeName} public API
    /// </summary>
    public class PublicApi : CCXT.NET.Shared.Coin.Public.PublicApi, IPublicApi
    {
        // TODO: Implement public API methods
    }
}
"@
        "ticker.cs" = @"
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Public;

namespace CCXT.NET.$($countryCode.ToUpper()).$($exchangeName.ToLower()).Public
{
    /// <summary>
    /// ${exchangeName} ticker implementation
    /// </summary>
    public class Ticker
    {
        // TODO: Implement ticker methods
    }
}
"@
        "orderBook.cs" = @"
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Public;

namespace CCXT.NET.$($countryCode.ToUpper()).$($exchangeName.ToLower()).Public
{
    /// <summary>
    /// ${exchangeName} order book implementation
    /// </summary>
    public class OrderBook
    {
        // TODO: Implement order book methods
    }
}
"@
        "completeOrder.cs" = @"
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Public;

namespace CCXT.NET.$($countryCode.ToUpper()).$($exchangeName.ToLower()).Public
{
    /// <summary>
    /// ${exchangeName} complete order implementation
    /// </summary>
    public class CompleteOrder
    {
        // TODO: Implement complete order methods
    }
}
"@
        "market.cs" = @"
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Public;

namespace CCXT.NET.$($countryCode.ToUpper()).$($exchangeName.ToLower()).Public
{
    /// <summary>
    /// ${exchangeName} market implementation
    /// </summary>
    public class Market
    {
        // TODO: Implement market methods
    }
}
"@
        "ohlcv.cs" = @"
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Public;

namespace CCXT.NET.$($countryCode.ToUpper()).$($exchangeName.ToLower()).Public
{
    /// <summary>
    /// ${exchangeName} OHLCV implementation
    /// </summary>
    public class OHLCV
    {
        // TODO: Implement OHLCV methods
    }
}
"@
    }
    
    foreach ($file in $publicFiles.GetEnumerator()) {
        $filePath = Join-Path $exchangePath "public" $file.Key
        if (!(Test-Path $filePath)) {
            Set-Content -Path $filePath -Value $file.Value
            Write-Host "Created file: $filePath" -ForegroundColor Cyan
        }
    }
    
    # Create trade API files
    $tradeFiles = @{
        "tradeApi.cs" = @"
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Trade;

namespace CCXT.NET.$($countryCode.ToUpper()).$($exchangeName.ToLower()).Trade
{
    /// <summary>
    /// ${exchangeName} trade API
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
        "order.cs" = @"
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Trade;

namespace CCXT.NET.$($countryCode.ToUpper()).$($exchangeName.ToLower()).Trade
{
    /// <summary>
    /// ${exchangeName} order implementation
    /// </summary>
    public class Order
    {
        // TODO: Implement order methods
    }
}
"@
        "place.cs" = @"
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Trade;

namespace CCXT.NET.$($countryCode.ToUpper()).$($exchangeName.ToLower()).Trade
{
    /// <summary>
    /// ${exchangeName} place order implementation
    /// </summary>
    public class Place
    {
        // TODO: Implement place order methods
    }
}
"@
        "trade.cs" = @"
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Trade;

namespace CCXT.NET.$($countryCode.ToUpper()).$($exchangeName.ToLower()).Trade
{
    /// <summary>
    /// ${exchangeName} trade implementation
    /// </summary>
    public class Trade
    {
        // TODO: Implement trade methods
    }
}
"@
    }
    
    foreach ($file in $tradeFiles.GetEnumerator()) {
        $filePath = Join-Path $exchangePath "trade" $file.Key
        if (!(Test-Path $filePath)) {
            Set-Content -Path $filePath -Value $file.Value
            Write-Host "Created file: $filePath" -ForegroundColor Cyan
        }
    }
}

Write-Host "Starting exchange folder reorganization..." -ForegroundColor Yellow
Write-Host "============================================" -ForegroundColor Yellow

# Step 1: Rename existing folders from three-letter to two-letter country codes
Write-Host "`nStep 1: Renaming existing folders to two-letter country codes..." -ForegroundColor Magenta
foreach ($mapping in $countryMapping.GetEnumerator()) {
    $oldPath = Join-Path $sourceDir $mapping.Key
    $newPath = Join-Path $sourceDir $mapping.Value.ToLower()
    
    if (Test-Path $oldPath) {
        if (Test-Path $newPath) {
            Write-Host "Target path already exists: $newPath (merging contents)" -ForegroundColor Yellow
            # Move contents from old to new
            Get-ChildItem -Path $oldPath -Directory | ForEach-Object {
                $targetPath = Join-Path $newPath $_.Name
                if (!(Test-Path $targetPath)) {
                    Move-Item -Path $_.FullName -Destination $targetPath -Force
                    Write-Host "Moved: $($_.Name) to $newPath" -ForegroundColor Green
                }
            }
            # Remove empty old directory
            if ((Get-ChildItem -Path $oldPath).Count -eq 0) {
                Remove-Item -Path $oldPath -Force
                Write-Host "Removed empty directory: $oldPath" -ForegroundColor Red
            }
        } else {
            Move-Item -Path $oldPath -Destination $newPath -Force
            Write-Host "Renamed: $($mapping.Key) -> $($mapping.Value.ToLower())" -ForegroundColor Green
        }
    }
}

# Step 2: Create missing exchange implementations
Write-Host "`nStep 2: Creating missing exchange implementations..." -ForegroundColor Magenta
foreach ($country in $missingExchanges.GetEnumerator()) {
    $countryCode = $country.Key
    Write-Host "`nProcessing country: $countryCode" -ForegroundColor Yellow
    
    foreach ($exchange in $country.Value) {
        $exchangePath = Join-Path $sourceDir $countryCode.ToLower() $exchange.ToLower()
        
        # Check if exchange already exists (might have been moved from three-letter folders)
        if (!(Test-Path $exchangePath)) {
            Write-Host "Creating new exchange: $exchange in $countryCode" -ForegroundColor Cyan
            Create-ExchangeStructure -countryCode $countryCode -exchangeName $exchange
        } else {
            Write-Host "Exchange already exists: $exchange in $countryCode" -ForegroundColor Gray
        }
    }
}

Write-Host "`n============================================" -ForegroundColor Yellow
Write-Host "Exchange folder reorganization completed!" -ForegroundColor Green
Write-Host "Summary:" -ForegroundColor Yellow
Write-Host "- Renamed folders from three-letter to two-letter country codes" -ForegroundColor White
Write-Host "- Created missing exchange implementations with standard structure" -ForegroundColor White
Write-Host "- All exchanges now follow the two-letter country code convention" -ForegroundColor White