using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Public;

namespace CCXT.NET.Poloniex.Public
{
    /// <summary>
    ///
    /// </summary>
    public class PTickerItem : CCXT.NET.Shared.Coin.Public.TickerItem, ITickerItem
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
        /// highest price for last 24H
        /// </summary>
        [JsonProperty(PropertyName = "high24hr")]
        public override decimal highPrice
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
        /// current best bid (buy) price
        /// </summary>
        [JsonProperty(PropertyName = "highestBid")]
        public override decimal bidPrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best bid (buy) amount (may be missing or undefined)
        /// </summary>
        [JsonProperty(PropertyName = "bidQty")]
        public override decimal bidQuantity
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
        /// current best ask (sell) amount (may be missing or undefined)
        /// </summary>
        [JsonProperty(PropertyName = "askQty")]
        public override decimal askQuantity
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
        /// price of last trade (closing price for current period)
        /// </summary>
        [JsonProperty(PropertyName = "last")]
        public override decimal closePrice
        {
            get;
            set;
        }

        /// <summary>
        /// relative change, `(change/open) * 100`
        /// </summary>
        [JsonProperty(PropertyName = "percentChange")]
        public override decimal percentage
        {
            get;
            set;
        }

        /// <summary>
        /// volume of base currency traded for last 24 hours
        /// </summary>
        [JsonProperty(PropertyName = "quoteVolume")]
        public override decimal baseVolume
        {
            get;
            set;
        }

        /// <summary>
        /// volume of quote currency traded for last 24 hours
        /// </summary>
        [JsonProperty(PropertyName = "baseVolume")]
        public override decimal quoteVolume
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "frozen")]
        public bool frozen
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "isFrozen")]
        private byte frozenValue
        {
            set
            {
                frozen = value != 0;
            }
        }
    }
}