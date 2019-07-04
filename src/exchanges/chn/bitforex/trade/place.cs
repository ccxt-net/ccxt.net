using CCXT.NET.Coin.Trade;
using CCXT.NET.Coin.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CCXT.NET.Bitforex.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class BPlaceOrder : CCXT.NET.Coin.Trade.MyOrder, IMyOrder
    {
        /// <summary>
        ///
        /// </summary>
        public BPlaceOrder()
        {
            this.result = new BPlaceOrderItem();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new BPlaceOrderItem result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "success")]
        public override bool success
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BPlaceOrderItem : CCXT.NET.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        /// Order ID
        /// </summary>
        [JsonProperty(PropertyName = "orderId")]
        public override string orderId
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BCancelOrder : CCXT.NET.Coin.Trade.MyOrder, IMyOrder
    {
        /// <summary>
        ///
        /// </summary>
        public BCancelOrder()
        {
            this.result = new BCancelOrderItem();
        }

        /// <summary>
        ///
        /// </summary>
        public new BCancelOrderItem result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "success")]
        public override bool success
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public bool data
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BCancelOrderItem : CCXT.NET.Coin.Trade.MyOrderItem, IMyOrderItem
    {
    }

    /// <summary>
    ///
    /// </summary>
    public class BCancelOrders : CCXT.NET.Coin.Trade.MyOrder, IMyOrder
    {
        /// <summary>
        ///
        /// </summary>
        public BCancelOrders()
        {
            this.result = new List<BCancelOrdersItem>();
        }

        /// <summary>
        ///
        /// </summary>
        public new List<BCancelOrdersItem> result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "success")]
        public override bool success
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        private JObject data
        {
            set
            {
                var _success = value["success"].Value<string>().Split(',');
                foreach (var _s in _success)
                {
                    result.Add(new BCancelOrdersItem { orderId = _s, orderStatus = OrderStatus.Canceled });
                }
                var _error = value["error"].ToObject<List<BCancelOrdersItem>>();
                foreach (var _e in _error)
                {
                    _e.orderStatus = OrderStatus.Unknown;
                    result.Add(_e);
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BCancelOrdersItem : CCXT.NET.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "orderId")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string code
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string message
        {
            get;
            set;
        }
    }
}