using CCXT.NET.Coin.Public;
using CCXT.NET.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CCXT.NET.Bittrex.Public
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
        [JsonProperty(PropertyName = "result")]
        public new List<BOHLCVItem> result
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
        ///
        /// </summary>
        [JsonProperty(PropertyName = "O")]
        public override decimal openPrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "H")]
        public override decimal highPrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "L")]
        public override decimal lowPrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "C")]
        public override decimal closePrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "V")]
        public override decimal volume
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "T")]
        private DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }
    }
}