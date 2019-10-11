using OdinSdk.BaseLib.Coin.Public;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CCXT.NET.Zb.Public
{
    /// <summary>
    ///
    /// </summary>
    public class ZOrderBooks : OdinSdk.BaseLib.Coin.Public.OrderBooks, IOrderBooks
    {
        /// <summary>
        ///
        /// </summary>
        public ZOrderBooks()
        {
            this.result = new ZOrderBook();
        }

        /// <summary>
        ///
        /// </summary>
        public new ZOrderBook result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ZOrderBook : OdinSdk.BaseLib.Coin.Public.OrderBook, IOrderBook
    {
        /// <summary>
        ///
        /// </summary>
        public ZOrderBook()
        {
            this.asks = new List<ZOrderBookItem>();
            this.bids = new List<ZOrderBookItem>();
        }

        /// <summary>
        ///
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
        [JsonProperty(PropertyName = "timestamp")]
        private long timeValue
        {
            set
            {
                timestamp = value * 1000;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originBids")]
        public new List<ZOrderBookItem> bids
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "bids")]
        private List<JArray> bidValue
        {
            set
            {
                foreach (var _bid in value)
                {
                    bids.Add(new ZOrderBookItem { price = _bid[0].Value<decimal>(), quantity = _bid[1].Value<decimal>() });
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originAsks")]
        public new List<ZOrderBookItem> asks
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "asks")]
        private List<JArray> askValue
        {
            set
            {
                foreach (var _ask in value)
                {
                    asks.Add(new ZOrderBookItem { price = _ask[0].Value<decimal>(), quantity = _ask[1].Value<decimal>() });
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ZOrderBookItem : OdinSdk.BaseLib.Coin.Public.OrderBookItem, IOrderBookItem
    {
        /// <summary>
        ///
        /// </summary>
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
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