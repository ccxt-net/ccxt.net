using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Public;

namespace CCXT.NET.OkCoinKr.Public
{
    /// <summary>
    ///
    /// </summary>
    public class OTicker : CCXT.NET.Shared.Coin.Public.Ticker, ITicker
    {
        /// <summary>
        ///
        /// </summary>
        public long timestamp
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "ticker")]
        public new OTickerItem result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        private long date
        {
            set
            {
                timestamp = value * 1000;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class OTickerItem : CCXT.NET.Shared.Coin.Public.TickerItem, ITickerItem
    {
        /// <summary>
        /// 고가
        /// </summary>
        [JsonProperty(PropertyName = "high")]
        public override decimal highPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 저가
        /// </summary>
        [JsonProperty(PropertyName = "low")]
        public override decimal lowPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 종가
        /// </summary>
        [JsonProperty(PropertyName = "last")]
        public override decimal closePrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "vol")]
        public decimal closeVolume
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "buy")]
        public override decimal bidPrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "sell")]
        public override decimal askPrice
        {
            get;
            set;
        }
    }
}