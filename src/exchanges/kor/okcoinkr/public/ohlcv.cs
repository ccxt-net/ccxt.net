using CCXT.NET.Coin.Public;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CCXT.NET.OkCoinKr.Public
{
    /// <summary>
    ///
    /// </summary>
    public class OOHLCVs : CCXT.NET.Coin.Public.OHLCVs, IOHLCVs
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
        public new List<OOHLCVItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class OOHLCVItem : CCXT.NET.Coin.Public.OHLCVItem, IOHLCVItem
    {
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
        public int type
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "createDate")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// 시가
        /// </summary>
        [JsonProperty(PropertyName = "open")]
        public override decimal openPrice
        {
            get;
            set;
        }

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
        [JsonProperty(PropertyName = "close")]
        public override decimal closePrice
        {
            get;
            set;
        }
    }
}