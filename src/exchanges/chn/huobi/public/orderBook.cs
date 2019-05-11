using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using OdinSdk.BaseLib.Coin.Public;
using System.Collections.Generic;

namespace CCXT.NET.Huobi.Public
{
    /// <summary>
    ///
    /// </summary>
    public class HOrderBooks : OdinSdk.BaseLib.Coin.Public.OrderBooks, IOrderBooks
    {
        /// <summary>
        ///
        /// </summary>
        public HOrderBooks()
        {
            this.result = new HOrderBook();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "tick")]
        public new HOrderBook result
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
    public class HOrderBook : OdinSdk.BaseLib.Coin.Public.OrderBook, IOrderBook
    {
        /// <summary>
        ///
        /// </summary>
        public HOrderBook()
        {
            this.asks = new List<BOrderBookItem>();
            this.bids = new List<BOrderBookItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "ts")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originBids")]
        public new List<BOrderBookItem> bids
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
                foreach (var _b in value)
                {
                    bids.Add(new BOrderBookItem { price = _b[0].Value<decimal>(), quantity = _b[1].Value<decimal>() });
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originAsks")]
        public new List<BOrderBookItem> asks
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "asks")]
        public List<JArray> askValue
        {
            set
            {
                foreach (var _a in value)
                {
                    asks.Add(new BOrderBookItem { price = _a[0].Value<decimal>(), quantity = _a[1].Value<decimal>() });
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BOrderBookItem : OdinSdk.BaseLib.Coin.Public.OrderBookItem, IOrderBookItem
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