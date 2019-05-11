using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using System;

namespace CCXT.NET.Bitfinex.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class BCancelOrders
    {
        /// <summary>
        ///
        /// </summary>
        public string result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BCancelAllOrders
    {
        /// <summary>
        ///
        /// </summary>
        public string result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BCancelOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        public string orderId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string status
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BMyOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
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
        [JsonProperty(PropertyName = "symbol")]
        public override string symbol
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
                base.orderStatus = (is_live == true && filled == 0) ? OrderStatus.Open
                                 : (is_live == true && filled > 0) ? OrderStatus.Partially
                                 : is_cancelled == true ? OrderStatus.Canceled
                                 : OrderStatus.Closed;

                return base.orderStatus;
            }
            set => base.orderStatus = value;
        }

        /// <summary>
        ///
        /// </summary>
        public long? cid
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime? cid_date
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string gid
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string exchange
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal avg_execution_price
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool is_live
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool is_cancelled
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool is_hidden
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string oco_order
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool was_forced
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal remaining_amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string src
        {
            get;
            set;
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
    }
}