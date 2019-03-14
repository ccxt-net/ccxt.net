using Newtonsoft.Json;
using OdinSdk.BaseLib.Configuration;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CCXT.NET.Huobi.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public class HPlaceOrder : OdinSdk.BaseLib.Coin.Trade.MyOrder, IMyOrder
    {
        /// <summary>
        /// 
        /// </summary>
        public HPlaceOrder()
        {
            this.result = new HPlaceOrderItem();
        }

        /// <summary>
        /// 
        /// </summary>
        public new HPlaceOrderItem result
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        private string data
        {
            set
            {
                result.orderId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string status
        {
            set
            {
                success = value == "ok" ? true : false;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HPlaceOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        /// 
        /// </summary>
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override SideType sideType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override OrderType orderType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override MakerType makerType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override OrderStatus orderStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// ISO 8601 datetime string with milliseconds
        /// </summary>
        public override string datetime
        {
            get
            {
                return CUnixTime.ConvertToUtcTimeMilli(timestamp).ToString("o");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override decimal amount
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override decimal filled
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override decimal cost
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override int count
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HCancelOrders : OdinSdk.BaseLib.Coin.Trade.MyOrders, IMyOrders
    {
        /// <summary>
        /// 
        /// </summary>
        public HCancelOrders()
        {
            this.result = new List<HCancelOrderItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        public new List<HCancelOrderItem> result
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
                if (value.ContainsKey("success"))
                {
                    foreach (var _s in value["success"].ToObject<JArray>())
                    {
                        var _cancelOrder = new HCancelOrderItem
                        {
                            orderId = _s.Value<string>(),
                            orderStatus = OrderStatus.Canceled,
                            timestamp = CUnixTime.NowMilli,
                        };

                        result.Add(_cancelOrder);
                    }
                }

                if (value.ContainsKey("failed"))
                {
                    foreach (var _s in value["failed"].ToObject<List<HCancelOrderItem>>())
                    {
                        _s.orderStatus = OrderStatus.Unknown;
                        result.Add(_s);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string status
        {
            set
            {
                success = value == "ok" ? true : false;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HCancelOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "err-msg")]
        public string err_msg
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "order-id")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "err-code")]
        public string err_code
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HCancelAllOrders : OdinSdk.BaseLib.Coin.Trade.MyOrders, IMyOrders
    {
        /// <summary>
        /// 
        /// </summary>
        public HCancelAllOrders()
        {
            this.result = new HHCancelAllOrdersItem();
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new HHCancelAllOrdersItem result
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string status
        {
            set
            {
                success = value == "ok" ? true : false;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HHCancelAllOrdersItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "success-count")]
        public int success_count
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "failed-count")]
        public int failed_count
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "next-id")]
        public string next_id
        {
            get;
            set;
        }
    }
}