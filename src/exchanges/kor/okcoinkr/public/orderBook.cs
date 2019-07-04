using CCXT.NET.Coin.Public;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CCXT.NET.OkCoinKr.Public
{
    /// <summary>
    ///
    /// </summary>
    public class OOrderBook : CCXT.NET.Coin.Public.OrderBook, IOrderBook
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originBids")]
        public override List<IOrderBookItem> bids
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originAsks")]
        public override List<IOrderBookItem> asks
        {
            get;
            set;
        }

        /// <summary>
        /// [0] Ask price
        /// [1] Ask amount
        /// </summary>
        [JsonProperty(PropertyName = "asks")]
        private JArray asksValue
        {
            set
            {
                asks = new List<IOrderBookItem>();

                foreach (var _b in value)
                {
                    var _bid = new OrderBookItem()
                    {
                        price = _b[0].Value<decimal>(),
                        quantity = _b[1].Value<decimal>(),
                        count = 1
                    };

                    _bid.amount = _bid.quantity * _bid.price;
                    asks.Add(_bid);
                }
            }
        }

        /// <summary>
        /// [0] Bid price
        /// [1] Bid amount
        /// </summary>
        [JsonProperty(PropertyName = "bids")]
        public JArray bidsValue
        {
            set
            {
                bids = new List<IOrderBookItem>();

                foreach (var _a in value)
                {
                    var _ask = new OrderBookItem()
                    {
                        price = _a[0].Value<decimal>(),
                        quantity = _a[1].Value<decimal>(),
                        count = 1
                    };

                    _ask.amount = _ask.quantity * _ask.price;
                    bids.Add(_ask);
                }
            }
        }
    }
}