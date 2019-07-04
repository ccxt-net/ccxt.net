using CCXT.NET.Coin.Trade;
using CCXT.NET.Coin.Types;
using Newtonsoft.Json;

namespace CCXT.NET.Bitfinex.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class BPlaceOrderItem : CCXT.NET.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        ///
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
        private decimal timeValue
        {
            set
            {
                timestamp = (long)(value * 1000);
            }
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
        [JsonProperty(PropertyName = "original_amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "executed_amount")]
        public override decimal filled
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override OrderStatus orderStatus
        {
            get
            {
                base.orderStatus = statusLive == true ? OrderStatus.Open
                                 : statusCanceled == true ? OrderStatus.Canceled
                                 : OrderStatus.Closed;

                return base.orderStatus;
            }
            set => base.orderStatus = value;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "is_live")]
        private bool statusLive
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "is_cancelled")]
        private bool statusCanceled
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        private string orderValue
        {
            set
            {
                orderType = OrderTypeConverter.FromString(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "side")]
        private string sideValue
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }
    }
}