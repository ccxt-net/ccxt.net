using Newtonsoft.Json;

using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;

namespace CCXT.NET.Zb.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class ZMyOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// Entrustment Order ID
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string orderId
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
        /// Status of hanging order (1: cancelled, 2: transaction completed, 0/3: pending transaction/pending transaction part)
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private int status
        {
            set
            {
                orderStatus = value == (0 | 3) ? OrderStatus.Open
                            : value == 1 ? OrderStatus.Canceled
                            : value == 2 ? OrderStatus.Closed
                            : OrderStatus.Unknown;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "total_amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// Transaction Volume
        /// </summary>
        [JsonProperty(PropertyName = "trade_amount")]
        public override decimal filled
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "trade_date")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// Total Transaction Value
        /// </summary>
        [JsonProperty(PropertyName = "trade_money")]
        public decimal tradeMoney
        {
            get;
            set;
        }

        /// <summary>
        /// Order Type 1/0[buy/sell]
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        private int type
        {
            set
            {
                sideType = value == 0 ? SideType.Ask
                         : value == 1 ? SideType.Bid
                         : SideType.Unknown;
            }
        }
    }
}