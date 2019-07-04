using CCXT.NET.Coin.Public;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CCXT.NET.Bitforex.Public
{
    /// <summary>
    ///
    /// </summary>
    public class BMarkets : CCXT.NET.Coin.Public.Markets, IMarkets
    {
        /// <summary>
        ///
        /// </summary>
        public BMarkets()
        {
            this.result = new List<BMarketItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "success")]
        public override bool success
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new List<BMarketItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BMarketItem : CCXT.NET.Coin.Public.MarketItem, IMarketItem
    {
        /// <summary>
        ///
        /// </summary>
        public BMarketItem()
        {
            this.precision = new MarketPrecision();
            this.limits = new MarketLimits
            {
                price = new MarketMinMax(),
                quantity = new MarketMinMax(),
                amount = new MarketMinMax()
            };
        }

        /// <summary>
        /// Transaction pair type
        /// </summary>
        [JsonProperty(PropertyName = "symbol")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// Price accuracy (several decimal places)
        /// </summary>
        [JsonProperty(PropertyName = "pricePrecision")]
        private int pricePrecision
        {
            set
            {
                precision.price = value;
            }
        }

        /// <summary>
        /// Quantity accuracy (several decimal places)
        /// </summary>
        [JsonProperty(PropertyName = "amountPrecision")]
        private int amountPrecision
        {
            set
            {
                precision.quantity = value;
            }
        }

        /// <summary>
        /// Minimum order quantity
        /// </summary>
        [JsonProperty(PropertyName = "minOrderAmount")]
        private decimal minOrderAmount
        {
            set
            {
                limits.quantity.min = value;
            }
        }
    }
}