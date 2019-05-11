using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Public;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace CCXT.NET.Poloniex.Public
{
    /// <summary>
    ///
    /// </summary>
    public class POrderBook : OdinSdk.BaseLib.Coin.Public.OrderBook, IOrderBook
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "bids")]
        private List<string[]> bidsValue
        {
            set
            {
                this.bids = ParseOrders(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "asks")]
        private List<string[]> asksValue
        {
            set
            {
                this.asks = ParseOrders(value);
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

        /// <summary>
        ///
        /// </summary>
        public int isFrozen
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public long seq
        {
            get;
            set;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private List<IOrderBookItem> ParseOrders(List<string[]> orders)
        {
            var _result = new List<IOrderBookItem>();

            foreach (var _order in orders)
            {
                var _o = new OrderBookItem
                {
                    price = decimal.Parse(_order[0], NumberStyles.Float),
                    quantity = decimal.Parse(_order[1], NumberStyles.Float),
                    count = 1
                };

                _o.amount = _o.quantity * _o.price;
                _result.Add(_o);
            }

            return _result;
        }
    }
}