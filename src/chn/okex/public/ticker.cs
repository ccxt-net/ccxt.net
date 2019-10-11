using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Public;
using System.Collections.Generic;

namespace CCXT.NET.OKEx.Public
{
    /// <summary>
    ///
    /// </summary>
    public class OTickers : OdinSdk.BaseLib.Coin.Public.Tickers, ITickers
    {
        /// <summary>
        ///
        /// </summary>
        public OTickers()
        {
            this.result = new List<OTickerItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "tickers")]
        public new List<OTickerItem> result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public long timestamp
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class OTicker : OdinSdk.BaseLib.Coin.Public.Ticker, ITicker
    {
        /// <summary>
        ///
        /// </summary>
        public OTicker()
        {
            this.result = new OTickerItem();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "ticker")]
        public new OTickerItem result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public long timestamp
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class OTickerItem : OdinSdk.BaseLib.Coin.Public.TickerItem, ITickerItem
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
        [JsonProperty(PropertyName = "date")]
        private long timeValue
        {
            set
            {
                timestamp = value * 1000;
            }
        }

        /// <summary>
        /// highest price for last 24H
        /// </summary>
        [JsonProperty(PropertyName = "high")]
        public override decimal highPrice
        {
            get;
            set;
        }

        /// <summary>
        /// lowest price for last 24H
        /// </summary>
        [JsonProperty(PropertyName = "low")]
        public override decimal lowPrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best bid (buy) price
        /// </summary>
        [JsonProperty(PropertyName = "buy")]
        public override decimal bidPrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best ask (sell) price
        /// </summary>
        [JsonProperty(PropertyName = "sell")]
        public override decimal askPrice
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
        /// volume of base currency traded for last 24 hours
        /// </summary>
        [JsonProperty(PropertyName = "vol")]
        public override decimal baseVolume
        {
            get;
            set;
        }
    }
}