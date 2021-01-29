using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CCXT.NET.Shared.Coin.Public;
using System.Collections.Generic;

namespace CCXT.NET.OKEx.Public
{
    /// <summary>
    ///
    /// </summary>
    public class OMarkets : CCXT.NET.Shared.Coin.Public.Markets, IMarkets
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "detailMsg")]
        public override string message
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string msg
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        private int statusValue
        {
            set
            {
                statusCode = value;
                success = statusCode == 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new List<OMarketItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class OMarketItem : CCXT.NET.Shared.Coin.Public.MarketItem, IMarketItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "baseCurrency")]
        public int baseNumericId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "quoteCurrency")]
        public int quoteNumericId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public JToken callNoCancelSwitchTime
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public int collect
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public JToken continuousSwitchTime
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool isMarginOpen
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public int listDisplay
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal marginRiskPreRatio
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal marginRiskRatio
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public int marketFrom
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal maxMarginLeverage
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public int maxPriceDigit
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public int maxSizeDigit
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public JToken mergeTypes
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal minTradeSize
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public int online
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public int productId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal quoteIncrement
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public int quotePrecision
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public int sort
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public int tradingMode
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string type
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool spot
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool future
        {
            get;
            set;
        }
    }
}