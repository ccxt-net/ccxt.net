using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin.Public;
using System.Collections.Generic;

namespace CCXT.NET.Bittrex.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class BOrderBooks : OdinSdk.BaseLib.Coin.Public.OrderBooks, IOrderBooks
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
        public new BOrderBook result
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BOrderBook : OdinSdk.BaseLib.Coin.Public.OrderBook, IOrderBook
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "buy")]
        private List<JToken> bidsValue
        {
            set
            {
                this.bids = new List<IOrderBookItem>();

                foreach (var _bid in value)
                {
                    var _b = new OrderBookItem
                    {
                        price = _bid["Rate"].Value<decimal>(),
                        quantity = _bid["Quantity"].Value<decimal>(),
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
        [JsonProperty(PropertyName = "sell")]
        private List<JToken> asksValue
        {
            set
            {
                this.asks = new List<IOrderBookItem>();

                foreach (var _ask in value)
                {
                    var _a = new OrderBookItem
                    {
                        price = _ask["Rate"].Value<decimal>(),
                        quantity = _ask["Quantity"].Value<decimal>(),
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
    }
}