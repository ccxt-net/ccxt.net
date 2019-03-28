using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Public;
using System;

namespace CCXT.NET.GateIO.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class GTickerItem : OdinSdk.BaseLib.Coin.Public.TickerItem, ITickerItem
    {
        /// <summary>
        /// volume of quote currency traded for last 24 hours
        /// </summary>
        [JsonProperty(PropertyName = "quoteVolume")]
        public override decimal quoteVolume
        {
            get;
            set;
        }

        /// <summary>
        /// volume of base currency traded for last 24 hours
        /// </summary>
        [JsonProperty(PropertyName = "baseVolume")]
        public override decimal baseVolume
        {
            get;
            set;
        }

        /// <summary>
        /// current best bid (buy) price
        /// </summary>
        [JsonProperty(PropertyName = "highestBid")]
        public override decimal bidPrice
        {
            get;
            set;
        }

        /// <summary>
        /// highest price for last 24H
        /// </summary>
        [JsonProperty(PropertyName = "high24hr")]
        public override decimal highPrice
        {
            get;
            set;
        }

        /// <summary>
        /// price of last trade (closing price for current period)
        /// </summary>
        [JsonProperty(PropertyName = "last")]
        public override decimal closePrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best ask (sell) price
        /// </summary>
        [JsonProperty(PropertyName = "lowestAsk")]
        public override decimal askPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "elapsed")]
        public string elapsed
        {
            get;
            set;
        }

        /// <summary>
        /// lowest price for last 24H
        /// </summary>
        [JsonProperty(PropertyName = "low24hr")]
        public override decimal lowPrice
        {
            get;
            set;
        }

        /// <summary>
        /// relative change, `(changePrice / openPrice) * 100`
        /// </summary>
        [JsonProperty(PropertyName = "percentChange")]
        public override decimal percentage
        {
            get;
            set;
        }
    }
}