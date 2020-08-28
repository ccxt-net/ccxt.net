using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Public;

namespace CCXT.NET.Binance.Public
{
    /// <summary>
    /// 4hr ticker price change statistics (GET /api/v1/ticker/24hr)
    /// </summary>
    public class BTickerItem : CCXT.NET.Shared.Coin.Public.TickerItem, ITickerItem
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
        /// 64-bit Unix Timestamp in milliseconds since Epoch 1 Jan 1970
        /// </summary>
        [JsonProperty(PropertyName = "closeTime")]
        public override long timestamp
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
        [JsonProperty(PropertyName = "weightedAvgPrice")]
        public override decimal vwap
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
        /// absolute change, `last - open`
        /// </summary>
        [JsonProperty(PropertyName = "priceChange")]
        public override decimal changePrice
        {
            get;
            set;
        }

        /// <summary>
        /// relative change, `(change/open) * 100`
        /// </summary>
        [JsonProperty(PropertyName = "priceChangePercent")]
        public override decimal percentage
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

        /// <summary>
        ///
        /// </summary>
        public decimal lastQty
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public long openTime
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public long firstId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public long lastId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public long count
        {
            get;
            set;
        }
    }
}