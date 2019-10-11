using OdinSdk.BaseLib.Coin.Public;
using OdinSdk.BaseLib.Configuration;
using Newtonsoft.Json;
using System;

namespace CCXT.NET.GDAX.Public
{
    /// <summary>
    ///
    /// </summary>
    public class GTickerItem : OdinSdk.BaseLib.Coin.Public.TickerItem, ITickerItem
    {
        /// <summary>
        /// string symbol of the market ('BTCUSD', 'ETHBTC', ...)
        /// </summary>
        [JsonProperty(PropertyName = "symbol")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "time")]
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
        [JsonProperty(PropertyName = "bid")]
        public override decimal bidPrice
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
        /// price of last trade (closing price for current period)
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public override decimal closePrice
        {
            get;
            set;
        }

        /// <summary>
        /// volume of base currency traded for last 24 hours
        /// </summary>
        [JsonProperty(PropertyName = "volume")]
        public override decimal baseVolume
        {
            get;
            set;
        }
    }
}