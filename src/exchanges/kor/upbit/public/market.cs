using Newtonsoft.Json;
using CCXT.NET.Coin.Public;

namespace CCXT.NET.Upbit.Public
{
    /// <summary>
    ///
    /// </summary>
    public class UMarketItem : CCXT.NET.Coin.Public.MarketItem, IMarketItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "market")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "english_name")]
        public override string baseLongName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string korean_name
        {
            get;
            set;
        }
    }
}