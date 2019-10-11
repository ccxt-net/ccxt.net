using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin.Public;
using System.Collections.Generic;

namespace CCXT.NET.Zb.Public
{
    /// <summary>
    ///
    /// </summary>
    public class ZOHLCVs : OdinSdk.BaseLib.Coin.Public.OHLCVs, IOHLCVs
    {
        /// <summary>
        ///
        /// </summary>
        public ZOHLCVs()
        {
            this.result = new List<ZOHLCVItem>();
        }

        /// <summary>
        ///
        /// </summary>
        public new List<ZOHLCVItem> result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        private List<JArray> data
        {
            set
            {
                foreach (var _ohlcv in value)
                {
                    result.Add(new ZOHLCVItem
                    {
                        timestamp = _ohlcv[0].Value<long>(),
                        openPrice = _ohlcv[1].Value<decimal>(),
                        highPrice = _ohlcv[2].Value<decimal>(),
                        lowPrice = _ohlcv[3].Value<decimal>(),
                        closePrice = _ohlcv[4].Value<decimal>(),
                        volume = _ohlcv[5].Value<decimal>()
                    });
                }

                if (result.Count > 0)
                    success = true;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ZOHLCVItem : OdinSdk.BaseLib.Coin.Public.OHLCVItem, IOHLCVItem
    {
        /// <summary>
        ///
        /// </summary>
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override decimal openPrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override decimal highPrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override decimal lowPrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override decimal closePrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override decimal volume
        {
            get;
            set;
        }
    }
}