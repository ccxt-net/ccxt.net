using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Trade;
using System.Collections.Generic;

namespace CCXT.NET.OkCoinKr.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class OCancelOrders : CCXT.NET.Shared.Coin.Trade.MyOrders, IMyOrders
    {
        /// <summary>
        ///
        /// </summary>
        public OCancelOrders()
        {
            this.result = new List<OPlaceOrderItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "success")]
        private string succesOrderIds
        {
            set
            {
                foreach (var _o in value.Split(','))
                {
                    this.result.Add(new OPlaceOrderItem
                    {
                        orderId = _o,
                        success = true
                    });
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "error")]
        private string errorOrderIds
        {
            set
            {
                foreach (var _o in value.Split(','))
                {
                    this.result.Add(new OPlaceOrderItem
                    {
                        orderId = _o,
                        success = false
                    });
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public new List<OPlaceOrderItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class OPlaceOrder : CCXT.NET.Shared.Coin.Trade.MyOrder, IMyOrder
    {
        /// <summary>
        ///
        /// </summary>
        public OPlaceOrder()
        {
            this.result = new OPlaceOrderItem();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "order_id")]
        private string orderId
        {
            set
            {
                this.result.orderId = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        private bool resultValue
        {
            set
            {
                success = value;
                result.success = success;
                message = success == true ? "success" : "failure";
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originResult")]
        public new OPlaceOrderItem result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class OPlaceOrderItem : CCXT.NET.Shared.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        public bool success
        {
            get;
            set;
        }
    }
}