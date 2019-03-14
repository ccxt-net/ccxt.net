using Newtonsoft.Json;
using OdinSdk.BaseLib.Configuration;
using OdinSdk.BaseLib.Coin.Public;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CCXT.NET.Huobi.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class HTickers : OdinSdk.BaseLib.Coin.Public.Tickers, ITickers
    {
        /// <summary>
        /// 
        /// </summary>
        public HTickers()
        {
            this.result = new List<HTickerItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new List<HTickerItem> result
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string status
        {
            set
            {
                success = value == "ok";
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HTickersItem : OdinSdk.BaseLib.Coin.Public.TickerItem, ITickerItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "open")]
        public override decimal openPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "close")]
        public override decimal closePrice
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "low")]
        public override decimal lowPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "high")]
        public override decimal highPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public decimal amount
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "count")]
        public int count
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "vol")]
        public override decimal baseVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "symbol")]
        public override string symbol
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HTicker : OdinSdk.BaseLib.Coin.Public.Ticker, ITicker
    {
        /// <summary>
        /// 
        /// </summary>
        public HTicker()
        {
            this.result = new HTickerItem();
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "tick")]
        public new HTickerItem result
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "ts")]
        private long timestamp
        {
            set
            {
                result.timestamp = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string status
        {
            set
            {
                success = value == "ok";
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HTickerItem : OdinSdk.BaseLib.Coin.Public.TickerItem, ITickerItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "close")]
        public override decimal closePrice
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "open")]
        public override decimal openPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "high")]
        public override decimal highPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "low")]
        public override decimal lowPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public decimal amount
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "count")]
        public int count
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "vol")]
        public override decimal baseVolume
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "ask")]
        private JArray ask
        {
            set
            {
                askPrice = value[0].Value<decimal>();
                askQuantity = value[1].Value<decimal>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "bid")]
        private JArray bid
        {
            set
            {
                bidPrice = value[0].Value<decimal>();
                bidQuantity = value[1].Value<decimal>();
            }
        }
    }
}