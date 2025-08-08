using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CCXT.NET.Shared.Coin.Public;
using System.Collections.Generic;

namespace CCXT.NET.GateIO.Public
{
    /// <summary>
    ///
    /// </summary>
    public class GOrderBook : CCXT.NET.Shared.Coin.Public.OrderBook, IOrderBook
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "elapsed")]
        public string elapsed
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originAsks")]
        public override List<OrderBookItem> asks
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
                this.asks = new List<OrderBookItem>();

                foreach (var _ask in value)
                {
                    var _a = new OrderBookItem
                    {
                        price = _ask[0].Value<decimal>(),
                        quantity = _ask[1].Value<decimal>(),
                        count = 1
                    };

                    _a.amount = _a.quantity * _a.price;
                    this.asks.Add(_a);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originBids")]
        public override List<OrderBookItem> bids
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
                this.bids = new List<OrderBookItem>();

                foreach (var _bid in value)
                {
                    var _b = new OrderBookItem
                    {
                        price = _bid[0].Value<decimal>(),
                        quantity = _bid[1].Value<decimal>(),
                        count = 1
                    };

                    _b.amount = _b.quantity * _b.price;
                    this.bids.Add(_b);
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class GOrderBookItem : CCXT.NET.Shared.Coin.Public.OrderBookItem, IOrderBookItem
    {
    }
}