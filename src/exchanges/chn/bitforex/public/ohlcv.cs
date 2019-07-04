using CCXT.NET.Coin.Public;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CCXT.NET.Bitforex.Public
{
    /// <summary>
    ///
    /// </summary>
    public class BOHLCVs : CCXT.NET.Coin.Public.OHLCVs, IOHLCVs
    {
        /// <summary>
        ///
        /// </summary>
        public BOHLCVs()
        {
            this.result = new List<BOHLCVItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new List<BOHLCVItem> result
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
    public class BOHLCVItem : CCXT.NET.Coin.Public.OHLCVItem, IOHLCVItem
    {
        /// <summary>
        /// Timestamp, milliseconds
        /// </summary>
        [JsonProperty(PropertyName = "time")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// Opening price
        /// </summary>
        [JsonProperty(PropertyName = "open")]
        public override decimal openPrice
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
        /// Closing price
        /// </summary>
        [JsonProperty(PropertyName = "close")]
        public override decimal closePrice
        {
            get;
            set;
        }

        /// <summary>
        /// Volume
        /// </summary>
        [JsonProperty(PropertyName = "vol")]
        public override decimal volume
        {
            get;
            set;
        }
    }
}