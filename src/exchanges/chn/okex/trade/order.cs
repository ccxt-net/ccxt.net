using Newtonsoft.Json;
using CCXT.NET.Coin.Trade;
using CCXT.NET.Coin.Types;
using System.Collections.Generic;

namespace CCXT.NET.OKEx.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class OMyOrders : CCXT.NET.Coin.Trade.MyOrders, IMyOrders
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public override bool success
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "orders")]
        public new List<OMyOrderItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class OMyOrderItem : CCXT.NET.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        /// order ID
        /// </summary>
        [JsonProperty(PropertyName = "order_id")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// -1 = cancelled, 0 = unfilled, 1 = partially filled, 2 = fully filled, 4 = cancel request
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private int statusValue
        {
            set
            {
                orderStatus = value == -1 ? OrderStatus.Canceled
                            : value == 0 ? OrderStatus.Open
                            : value == 1 ? OrderStatus.Partially
                            : value == 2 ? OrderStatus.Closed
                            : value == 4 ? OrderStatus.Unknown
                            : OrderStatus.Unknown;
            }
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
        /// buy_market = market buy order, sell_market = market sell order
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        private string sideValue
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }

        /// <summary>
        /// order price
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// for limit orders, returns the order quantity.  For market orders, returns the filled quantity
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// average transaction price
        /// </summary>
        [JsonProperty(PropertyName = "avg_price")]
        public decimal avg_price
        {
            get;
            set;
        }

        /// <summary>
        /// filled quantity
        /// </summary>
        [JsonProperty(PropertyName = "deal_amount")]
        public override decimal filled
        {
            get;
            set;
        }

        /// <summary>
        /// order time
        /// </summary>
        [JsonProperty(PropertyName = "create_date")]
        public override long timestamp
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class OCancelOrderItem
    {
        /// <summary>
        /// ID of orders whose cancel request are successful, wait to be processed.(applicable to batch orders)
        /// </summary>
        [JsonProperty(PropertyName = "success")]
        public string success
        {
            get;
            set;
        }

        /// <summary>
        /// ID of orders whose cancel request are unsuccessful.(applicable to batch orders)
        /// </summary>
        [JsonProperty(PropertyName = "error")]
        public string failure
        {
            get;
            set;
        }
    }
}