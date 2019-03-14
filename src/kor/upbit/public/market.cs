using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Public;

namespace CCXT.NET.Upbit.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class UMarketItem : OdinSdk.BaseLib.Coin.Public.MarketItem, IMarketItem
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