using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Public;
using CCXT.NET.Shared.Coin.Types;
using CCXT.NET.Shared.Configuration;
using System.Collections.Generic;

namespace CCXT.NET.Bithumb.Public
{
    /// <summary>
    /// BITHUMB market information data structures
    /// For standardized market data representation
    /// </summary>

    /// <summary>
    /// Market item for BITHUMB exchange
    /// </summary>
    public class BMarketItem : CCXT.NET.Shared.Coin.Public.MarketItem, IMarketItem
    {
        /// <summary>
        /// Market symbol in BITHUMB format (e.g., "BTC")
        /// </summary>
        public override string symbol { get; set; }

        /// <summary>
        /// Base currency ID (e.g., "BTC")
        /// </summary>
        public override string baseId { get; set; }

        /// <summary>
        /// Quote currency ID (always "KRW" for BITHUMB)
        /// </summary>
        public override string quoteId { get; set; } = "KRW";

        /// <summary>
        /// Base currency name (standardized)
        /// </summary>
        public override string baseName { get; set; }

        /// <summary>
        /// Quote currency name (always "KRW")
        /// </summary>
        public override string quoteName { get; set; } = "KRW";

        /// <summary>
        /// Market ID in format "BASE/QUOTE" (e.g., "BTC/KRW")
        /// </summary>
        public override string marketId { get; set; }

        /// <summary>
        /// Whether the market is currently active for trading
        /// </summary>
        public override bool active { get; set; } = true;

        /// <summary>
        /// Market precision settings
        /// </summary>
        public override MarketPrecision precision { get; set; }

        /// <summary>
        /// Market trading limits
        /// </summary>
        public override MarketLimits limits { get; set; }

        /// <summary>
        /// Minimum lot size for trading
        /// </summary>
        public override decimal lot { get; set; }

        /// <summary>
        /// Maker fee rate
        /// </summary>
        public override decimal makerFee { get; set; } = 0.15m / 100m;

        /// <summary>
        /// Taker fee rate
        /// </summary>
        public override decimal takerFee { get; set; } = 0.15m / 100m;
    }

    /// <summary>
    /// Collection of BITHUMB markets
    /// </summary>
    public class BMarkets : CCXT.NET.Shared.Coin.Public.Markets, IMarkets
    {
        /// <summary>
        /// Initialize BITHUMB markets collection
        /// </summary>
        public BMarkets()
        {
            this.result = new Dictionary<string, IMarketItem>();
        }
    }
}
