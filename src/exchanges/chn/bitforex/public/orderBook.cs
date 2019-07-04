using CCXT.NET.Coin.Public;
using CCXT.NET.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CCXT.NET.Bitforex.Public
{
    /// <summary>
    ///
    /// </summary>
    public class BOrderBooks : CCXT.NET.Coin.Public.OrderBooks, IOrderBooks
    {
        /// <summary>
        ///
        /// </summary>
        public BOrderBooks()
        {
            this.result = new BOrderBook();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new BOrderBook result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "success")]
        public override bool success
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BOrderBook : CCXT.NET.Coin.Public.OrderBook, IOrderBook
    {
        /// <summary>
        ///
        /// </summary>
        public BOrderBook()
        {
            this.asks = new List<BOrderBookItem>();
            this.bids = new List<BOrderBookItem>();
        }

        /// <summary>
        ///
        /// </summary>
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override string datetime
        {
            get
            {
                return CUnixTime.ConvertToUtcTimeMilli(timestamp).ToString("o");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override long nonce
        {
            get;
            set;
        }

        /// <summary>
        /// buy array
        /// </summary>
        [JsonProperty(PropertyName = "bids")]
        public new List<BOrderBookItem> bids
        {
            get;
            set;
        }

        /// <summary>
        /// sell array
        /// </summary>
        [JsonProperty(PropertyName = "asks")]
        public new List<BOrderBookItem> asks
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BOrderBookItem : CCXT.NET.Coin.Public.OrderBookItem, IOrderBookItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originAmount")]
        public override decimal amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override int count
        {
            get;
            set;
        }
    }
}