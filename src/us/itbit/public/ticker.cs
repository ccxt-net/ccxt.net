using Newtonsoft.Json;
using OdinSdk.BaseLib.Configuration;
using OdinSdk.BaseLib.Coin.Public;
using System;

namespace CCXT.NET.ItBit.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class TTickerItem : OdinSdk.BaseLib.Coin.Public.TickerItem, ITickerItem
    {
        /// <summary>
        /// string symbol of the market ('BTCUSD', 'ETHBTC', ...)
        /// </summary>
        [JsonProperty(PropertyName = "pair")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// highest price for last 24H
        /// </summary>
        [JsonProperty(PropertyName = "high24h")]
        public override decimal highPrice
        {
            get;
            set;
        }

        /// <summary>
        /// lowest price for last 24H
        /// </summary>
        [JsonProperty(PropertyName = "low24h")]
        public override decimal lowPrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best bid (buy) price
        /// </summary>
        [JsonProperty(PropertyName = "bid")]
        public override decimal bidPrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best bid (buy) amount (may be missing or undefined)
        /// </summary>
        [JsonProperty(PropertyName = "bidAmt")]
        public override decimal bidQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// current best ask (sell) price
        /// </summary>
        [JsonProperty(PropertyName = "ask")]
        public override decimal askPrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best ask (sell) amount (may be missing or undefined)
        /// </summary>
        [JsonProperty(PropertyName = "askAmt")]
        public override decimal askQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// volume weighted average price
        /// </summary>
        [JsonProperty(PropertyName = "vwap24h")]
        public override decimal vwap
        {
            get;
            set;
        }

        /// <summary>
        /// opening price before 24H
        /// </summary>
        [JsonProperty(PropertyName = "openToday")]
        public override decimal openPrice
        {
            get;
            set;
        }

        /// <summary>
        /// price of last trade (closing price for current period)
        /// </summary>
        [JsonProperty(PropertyName = "lastPrice")]
        public override decimal closePrice
        {
            get;
            set;
        }

        /// <summary>
        /// same as `close`, duplicated for convenience
        /// </summary>
        [JsonProperty(PropertyName = "endPrice")]
        public override decimal lastPrice
        {
            get
            {
                return closePrice;
            }
        }

        /// <summary>
        /// closing price for the previous period
        /// </summary>
        [JsonProperty(PropertyName = "prevClosePrice")]
        public override decimal prevPrice
        {
            get;
            set;
        }

        /// <summary>
        /// volume of base currency traded for last 24 hours
        /// </summary>
        [JsonProperty(PropertyName = "volume24h")]
        public override decimal baseVolume
        {
            get;
            set;
        }

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
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "serverTimeUTC")]
        private DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "lastAmt")]
        public decimal lastAmt
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "volumeToday")]
        public decimal volumeToday
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "highToday")]
        public decimal highToday
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "lowToday")]
        public decimal lowToday
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "vwapToday")]
        public decimal vwapToday
        {
            get;
            set;
        }
    }
}