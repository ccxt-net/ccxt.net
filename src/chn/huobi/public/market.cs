using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Public;
using System.Collections.Generic;

namespace CCXT.NET.Huobi.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class HMarkets : OdinSdk.BaseLib.Coin.Public.Markets, IMarkets
    {
        /// <summary>
        /// 
        /// </summary>
        public HMarkets()
        {
            this.result = new List<HMarketItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new List<HMarketItem> result
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string status
        {
            set
            {
                success = value == "ok";
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HMarketItem : OdinSdk.BaseLib.Coin.Public.MarketItem, IMarketItem
    {
        /// <summary>
        /// 
        /// </summary>
        public HMarketItem()
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
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "symbol")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "base-currency")]
        public override string baseId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "quote-currency")]
        public override string quoteId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "amount-precision")]
        private int amountPrecision
        {
            set
            {
                this.precision.quantity = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "price-precision")]
        private int pricePrecision
        {
            set
            {
                this.precision.price = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "symbol-partition")]
        public string symbolPartition
        {
            get;
            set;
        }
    }
}