using OdinSdk.BaseLib.Coin.Public;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace CCXT.NET.Anxpro.Public
{
    /// <summary>
    ///
    /// </summary>
    public class ATickers : OdinSdk.BaseLib.Coin.Public.Tickers, ITickers
    {
        /// <summary>
        ///
        /// </summary>
        public ATickers()
        {
            this.result = new Dictionary<string, ATickerItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new Dictionary<string, ATickerItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ATickerItem : OdinSdk.BaseLib.Coin.Public.TickerItem, ITickerItem
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
        /// volume weighted average price
        /// </summary>
        [JsonProperty(PropertyName = "vwapValue")]
        public override decimal vwap
        {
            get;
            set;
        }

        /// <summary>
        /// average price, `(last + open) / 2`
        /// </summary>
        [JsonProperty(PropertyName = "average")]
        public override decimal average
        {
            get;
            set;
        }

        /// <summary>
        /// volume of quote currency traded for last 24 hours
        /// </summary>
        public override decimal quoteVolume
        {
            get
            {
                return this.vwap * this.baseVolume;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "dataUpdateTime")]
        public long dataUpdateTime
        {
            set
            {
                timestamp = value / 1000;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "high")]
        public JObject highObject
        {
            set
            {
                this.highPrice = String.IsNullOrEmpty(value["value"].ToString()) == false ? value["value"].Value<decimal>() : 0m;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "low")]
        public JObject lowObject
        {
            set
            {
                this.lowPrice = String.IsNullOrEmpty(value["value"].ToString()) == false ? value["value"].Value<decimal>() : 0m;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "buy")]
        public JObject buyObject
        {
            set
            {
                this.bidPrice = String.IsNullOrEmpty(value["value"].ToString()) == false ? value["value"].Value<decimal>() : 0m;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "sell")]
        public JObject sellObject
        {
            set
            {
                this.askPrice = String.IsNullOrEmpty(value["value"].ToString()) == false ? value["value"].Value<decimal>() : 0m;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "vwap")]
        public JObject vwapObject
        {
            set
            {
                this.vwap = String.IsNullOrEmpty(value["value"].ToString()) == false ? value["value"].Value<decimal>() : 0m;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "last")]
        public JObject lastObject
        {
            set
            {
                this.closePrice = String.IsNullOrEmpty(value["value"].ToString()) == false ? value["value"].Value<decimal>() : 0m;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "avg")]
        public JObject avgObject
        {
            set
            {
                this.average = String.IsNullOrEmpty(value["value"].ToString()) == false ? value["value"].Value<decimal>() : 0m;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "vol")]
        public JObject volObject
        {
            set
            {
                this.baseVolume = String.IsNullOrEmpty(value["value"].ToString()) == false ? value["value"].Value<decimal>() : 0m;
            }
        }
    }
}