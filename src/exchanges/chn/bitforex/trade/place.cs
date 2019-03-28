using Newtonsoft.Json;

using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CCXT.NET.Bitforex.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public class BPlaceOrder : OdinSdk.BaseLib.Coin.Trade.MyOrder, IMyOrder
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
    public class BPlaceOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
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
    public class BCancelOrder : OdinSdk.BaseLib.Coin.Trade.MyOrder, IMyOrder
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
    public class BCancelOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class BCancelOrders : OdinSdk.BaseLib.Coin.Trade.MyOrder, IMyOrder
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
    public class BCancelOrdersItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
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