using CCXT.NET.Coin.Trade;
using CCXT.NET.Coin.Types;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CCXT.NET.Coinone.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class CMyOpenOrders : CCXT.NET.Coin.Trade.MyOrders, IMyOrders
    {
        /// <summary>
        /// 성공이면 “success”, 실패할 경우 에러 심블이 세팅된다.
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public override string message
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override bool success
        {
            get
            {
                base.success = message == "success";
                return base.success;
            }
            set => base.success = value;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "errorCode")]
        public override int statusCode
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public List<CMyOpenOrderItem> limitOrders
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class CMyOpenOrderItem : CCXT.NET.Coin.Trade.MyOrderItem, IMyOrderItem
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
        [JsonProperty(PropertyName = "qty")]
        public override decimal quantity
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
        ///
        /// </summary>
        [JsonProperty(PropertyName = "index")]
        public int index
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "feeRate")]
        public decimal feeRate
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
        ///
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        private string sideValue
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }
    }
}