using Newtonsoft.Json;
using CCXT.NET.Configuration;

namespace CCXT.NET.Poloniex.Public
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPublicTicker
    {
        /// <summary>
        /// 
        /// </summary>
        decimal PriceLast
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal PriceChangePercentage
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal Volume24HourBase
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal Volume24HourQuote
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal OrderTopBuy
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal OrderTopSell
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal OrderSpread
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal OrderSpreadPercentage
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        bool IsFrozen
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PublicTicker : IPublicTicker
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("last")]
        public decimal PriceLast
        {
            get; internal set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("percentChange")]
        public decimal PriceChangePercentage
        {
            get; internal set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("baseVolume")]
        public decimal Volume24HourBase
        {
            get; internal set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("quoteVolume")]
        public decimal Volume24HourQuote
        {
            get; internal set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("highestBid")]
        public decimal OrderTopBuy
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("lowestAsk")]
        public decimal OrderTopSell
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal OrderSpread
        {
            get
            {
                return (OrderTopSell - OrderTopBuy).Normalize();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal OrderSpreadPercentage
        {
            get
            {
                return OrderTopSell / OrderTopBuy - 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("isFrozen")]
        internal byte IsFrozenInternal
        {
            set
            {
                IsFrozen = value != 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFrozen
        {
            get; private set;
        }
    }
}