using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Public;
using System.Collections.Generic;

namespace CCXT.NET.CoinCheck.Public
{
    /// <summary>
    ///
    /// </summary>
    public class CMarkets : CCXT.NET.Shared.Coin.Public.Markets, IMarkets
    {
        /// <summary>
        ///
        /// </summary>
        public CMarkets()
        {
            this.result = new List<CMarketItem>();
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
        public new List<CMarketItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class CMarketItem : CCXT.NET.Shared.Coin.Public.MarketItem, IMarketItem
    {
        /// <summary>
        ///
        /// </summary>
        public CMarketItem()
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