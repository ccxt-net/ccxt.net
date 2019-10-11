using OdinSdk.BaseLib.Coin.Public;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CCXT.NET.Kraken.Public
{
    /// <summary>
    ///
    /// </summary>
    public class KTickerItem : OdinSdk.BaseLib.Coin.Public.TickerItem, ITickerItem
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
        [JsonProperty(PropertyName = "h")]
        private List<decimal> highPriceList
        {
            set
            {
                this.highPrice = value[1];
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "l")]
        private List<decimal> lowPriceList
        {
            set
            {
                this.lowPrice = value[1];
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "b")]
        private List<decimal> bidPriceList
        {
            set
            {
                this.bidPrice = value[0];
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "a")]
        private List<decimal> askPriceList
        {
            set
            {
                this.askPrice = value[0];
            }
        }

        /// <summary>
        /// opening price before 24H
        /// </summary>
        [JsonProperty(PropertyName = "o")]
        public override decimal openPrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "c")]
        private List<decimal> closePriceList
        {
            set
            {
                this.closePrice = value[0];
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "v")]
        private List<decimal> volumeList
        {
            set
            {
                this.baseVolume = value[1];
            }
        }
    }
}