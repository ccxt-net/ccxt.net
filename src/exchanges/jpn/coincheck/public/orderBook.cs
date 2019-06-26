using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using CCXT.NET.Coin.Public;
using System.Collections.Generic;

namespace CCXT.NET.CoinCheck.Public
{
    /// <summary>
    ///
    /// </summary>
    public class COrderBook : CCXT.NET.Coin.Public.OrderBook, IOrderBook
    {
        /// <summary>
        ///
        /// </summary>
        public COrderBook()
        {
            this.asks = new List<COrderBookItem>();
            this.bids = new List<COrderBookItem>();
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
        [JsonProperty(PropertyName = "originBids")]
        public new List<COrderBookItem> bids
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originAsks")]
        public new List<COrderBookItem> asks
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "bids")]
        private List<JArray> bidsValue
        {
            set
            {
                foreach (var _bid in value)
                {
                    bids.Add(new COrderBookItem { price = _bid[0].Value<decimal>(), quantity = _bid[1].Value<decimal>(), count = 1 });
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "asks")]
        private List<JArray> asksValue
        {
            set
            {
                foreach (var _ask in value)
                {
                    asks.Add(new COrderBookItem { price = _ask[0].Value<decimal>(), quantity = _ask[1].Value<decimal>(), count = 1 });
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class COrderBookItem : CCXT.NET.Coin.Public.OrderBookItem, IOrderBookItem
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