using Newtonsoft.Json;
using CCXT.NET.Coin.Public;
using CCXT.NET.Configuration;
using System;
using System.Collections.Generic;

namespace CCXT.NET.Bittrex.Public
{
    /// <summary>
    ///
    /// </summary>
    public class BTickerItems : CCXT.NET.Coin.Public.TickerItems, ITickerItems
    {
        /// <summary>
        ///
        /// </summary>
        public BTickerItems()
        {
            this.result = new List<BTickerItem>();
        }

        /// <summary>
        ///
        /// </summary>
        public new List<BTickerItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BTickerItem : CCXT.NET.Coin.Public.TickerItem, ITickerItem
    {
        /// <summary>
        /// string symbol of the market ('BTCUSD', 'ETHBTC', ...)
        /// </summary>
        [JsonProperty(PropertyName = "MarketName")]
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
        [JsonProperty(PropertyName = "TimeStamp")]
        private DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }

        /// <summary>
        /// highest price for last 24H
        /// </summary>
        [JsonProperty(PropertyName = "High")]
        public override decimal highPrice
        {
            get;
            set;
        }

        /// <summary>
        /// lowest price for last 24H
        /// </summary>
        [JsonProperty(PropertyName = "Low")]
        public override decimal lowPrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best bid (buy) price
        /// </summary>
        [JsonProperty(PropertyName = "Bid")]
        public override decimal bidPrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best ask (sell) price
        /// </summary>
        [JsonProperty(PropertyName = "Ask")]
        public override decimal askPrice
        {
            get;
            set;
        }

        /// <summary>
        /// volume weighted average price
        /// </summary>
        [JsonProperty(PropertyName = "vwap")]
        public override decimal vwap
        {
            get
            {
                base.vwap = (this.baseVolume != 0) ? this.quoteVolume / this.baseVolume : 0m;
                return base.vwap;
            }
            set => base.vwap = value;
        }

        /// <summary>
        /// opening price before 24H
        /// </summary>
        [JsonProperty(PropertyName = "PrevDay")]
        public override decimal openPrice
        {
            get;
            set;
        }

        /// <summary>
        /// price of last trade (closing price for current period)
        /// </summary>
        [JsonProperty(PropertyName = "Last")]
        public override decimal closePrice
        {
            get;
            set;
        }

        /// <summary>
        /// volume of base currency traded for last 24 hours
        /// </summary>
        [JsonProperty(PropertyName = "Volume")]
        public override decimal baseVolume
        {
            get;
            set;
        }

        /// <summary>
        /// volume of quote currency traded for last 24 hours
        /// </summary>
        [JsonProperty(PropertyName = "BaseVolume")]
        public override decimal quoteVolume
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "OpenBuyOrders")]
        public int OpenBuyOrders
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "OpenSellOrders")]
        public int OpenSellOrders
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "Created")]
        public DateTime Created
        {
            get;
            set;
        }
    }
}