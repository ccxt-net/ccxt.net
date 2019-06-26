using Newtonsoft.Json;
using CCXT.NET.Coin.Public;
using System.Collections.Generic;

namespace CCXT.NET.GateIO.Public
{
    /// <summary>
    ///
    /// </summary>
    public class GMarkets : CCXT.NET.Coin.Public.Markets, IMarkets
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public override bool success
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "pairs")]
        public new List<Dictionary<string, GMarketItem>> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class GMarketItem : CCXT.NET.Coin.Public.MarketItem, IMarketItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "decimal_places")]
        public decimal decimal_places
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "min_amount")]
        public decimal min_amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "fee")]
        public decimal fee
        {
            get;
            set;
        }
    }
}