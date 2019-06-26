using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CCXT.NET.Coin.Public;
using System.Collections.Generic;

namespace CCXT.NET.ItBit.Public
{
    /// <summary>
    ///
    /// </summary>
    public class TOrderBook : CCXT.NET.Coin.Public.OrderBook, IOrderBook
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [JsonProperty(PropertyName = "bids")]
        private List<JArray> bidsValue
        {
            set
            {
                this.bids = new List<IOrderBookItem>();

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

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [JsonProperty(PropertyName = "asks")]
        private List<JArray> asksValue
        {
            set
            {
                this.asks = new List<IOrderBookItem>();

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
        /// <returns></returns>
        [JsonProperty(PropertyName = "originBids")]
        public override List<IOrderBookItem> bids
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [JsonProperty(PropertyName = "originAsks")]
        public override List<IOrderBookItem> asks
        {
            get;
            set;
        }
    }
}