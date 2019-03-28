using Newtonsoft.Json;
using OdinSdk.BaseLib.Configuration;
using OdinSdk.BaseLib.Coin.Public;
using System;

namespace CCXT.NET.Bitflyer.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class BTickerItem : OdinSdk.BaseLib.Coin.Public.TickerItem, ITickerItem
    {
        /// <summary>
        /// string symbol of the market ('BTCUSD', 'ETHBTC', ...)
        /// </summary>
        [JsonProperty(PropertyName = "product_code")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// 64-bit Unix Timestamp in milliseconds since Epoch 1 Jan 1970
        /// </summary>
        [JsonProperty(PropertyName = "origin_timestamp")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        private DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }

        /// <summary>
        /// current best bid (buy) price
        /// </summary>
        [JsonProperty(PropertyName = "best_bid")]
        public override decimal bidPrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best bid (buy) amount (may be missing or undefined)
        /// </summary>
        [JsonProperty(PropertyName = "best_bid_size")]
        public override decimal bidQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// current best ask (sell) price
        /// </summary>
        [JsonProperty(PropertyName = "best_ask")]
        public override decimal askPrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best ask (sell) amount (may be missing or undefined)
        /// </summary>
        [JsonProperty(PropertyName = "best_ask_size")]
        public override decimal askQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// price of last trade (closing price for current period)
        /// </summary>
        [JsonProperty(PropertyName = "ltp")]
        public override decimal closePrice
        {
            get;
            set;
        }
        
        /// <summary>
        /// volume of base currency traded for last 24 hours
        /// </summary>
        [JsonProperty(PropertyName = "volume_by_product")]
        public override decimal baseVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "tick_id")]
        public long tick_id
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "total_bid_depth")]
        public decimal total_bid_depth
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "total_ask_depth")]
        public decimal total_ask_depth
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "volume")]
        public decimal volume
        {
            get;
            set;
        }
    }
}