using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Public;
using System.Globalization;

namespace CCXT.NET.Quoinex.Public
{
    /// <summary>
    ///
    /// </summary>
    public class QTickerItem : OdinSdk.BaseLib.Coin.Public.TickerItem, ITickerItem
    {
        /// <summary>
        /// string symbol of the market ('BTCUSD', 'ETHBTC', ...)
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// current best bid (buy) price
        /// </summary>
        [JsonProperty(PropertyName = "market_bid")]
        public override decimal bidPrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best ask (sell) price
        /// </summary>
        [JsonProperty(PropertyName = "market_ask")]
        public override decimal askPrice
        {
            get;
            set;
        }

        /// <summary>
        /// price of last trade (closing price for current period)
        /// </summary>
        [JsonProperty(PropertyName = "last_traded_price")]
        public override decimal closePrice
        {
            get;
            set;
        }

        /// <summary>
        /// volume of base currency traded for last 24 hours
        /// </summary>
        [JsonProperty(PropertyName = "volume_24h")]
        public override decimal baseVolume
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "base_currency")]
        public string base_currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "quoted_currency")]
        public string quoted_currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "high_market_ask")]
        private string high_market_ask
        {
            set
            {
                this.highPrice = decimal.Parse(value, NumberStyles.Float);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "low_market_bid")]
        private string low_market_bid
        {
            set
            {
                this.lowPrice = decimal.Parse(value, NumberStyles.Float);
            }
        }
    }
}