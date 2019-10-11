using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Public;

namespace CCXT.NET.Bitforex.Public
{
    /// <summary>
    /// Ticker Information
    /// </summary>
    public class BTicker : OdinSdk.BaseLib.Coin.Public.Ticker, ITicker
    {
        /// <summary>
        ///
        /// </summary>
        public BTicker()
        {
            this.result = new BTickerItem();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new BTickerItem result
        {
            get;
            set;
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
    }

    /// <summary>
    ///
    /// </summary>
    public class BTickerItem : OdinSdk.BaseLib.Coin.Public.TickerItem, ITickerItem
    {
        /// <summary>
        /// Server timestamp
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// Buy one price
        /// </summary>
        [JsonProperty(PropertyName = "buy")]
        public override decimal bidPrice
        {
            get;
            set;
        }

        /// <summary>
        /// Sell one price
        /// </summary>
        [JsonProperty(PropertyName = "sell")]
        public override decimal askPrice
        {
            get;
            set;
        }

        /// <summary>
        /// Latest price
        /// </summary>
        [JsonProperty(PropertyName = "last")]
        public override decimal closePrice
        {
            get;
            set;
        }

        /// <summary>
        /// Highest price
        /// </summary>
        [JsonProperty(PropertyName = "high")]
        public override decimal highPrice
        {
            get;
            set;
        }

        /// <summary>
        /// Lowest price
        /// </summary>
        [JsonProperty(PropertyName = "low")]
        public override decimal lowPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 24 hours trading volume
        /// </summary>
        [JsonProperty(PropertyName = "vol")]
        public override decimal baseVolume
        {
            get;
            set;
        }
    }
}