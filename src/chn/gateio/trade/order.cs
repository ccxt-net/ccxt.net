using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CCXT.NET.GateIO.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class GMyOrder : OdinSdk.BaseLib.Coin.Trade.MyOrder, IMyOrder
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
        [JsonProperty(PropertyName = "order")]
        public new GMyOrderItem result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "msg")]
        public override string message
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class GMyOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        /// 주문의 고유 아이디
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// 주문 상태
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        private string statusValue
        {
            set
            {
                orderStatus = OrderStatusConverter.FromString(value);
            }
        }

        /// <summary>
        /// 마켓의 유일키
        /// </summary>
        [JsonProperty(PropertyName = "pair")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// 주문 종류
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
        /// 주문 당시 화폐 가격
        /// </summary>
        [JsonProperty(PropertyName = "rate")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// 사용자가 입력한 주문 양
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "initial_rate")]
        public decimal initial_rate
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "initial_amount")]
        public decimal initial_amount
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class GOpenOrders : OdinSdk.BaseLib.Coin.Trade.MyOrder, IMyOrder
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
        public new List<GOpenOrderItem> result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public override string message
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class GOpenOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        /// 주문의 고유 아이디
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
        [JsonProperty(PropertyName = "orderNumber")]
        public string orderNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 주문 종류
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
        /// 주문 당시 화폐 가격
        /// </summary>
        [JsonProperty(PropertyName = "rate")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// 사용자가 입력한 주문 양
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "total")]
        public decimal total
        {
            get;
            set;
        }

        /// <summary>
        /// 마켓의 유일키
        /// </summary>
        [JsonProperty(PropertyName = "currencyPair")]
        public override string symbol
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
        private long timeValue
        {
            set
            {
                timestamp = value * 1000;
            }
        }

        /// <summary>
        /// 주문 상태
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        private string statusValue
        {
            set
            {
                orderStatus = OrderStatusConverter.FromString(value);
            }
        }
    }
}