using Newtonsoft.Json;

using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using System;
using System.Collections.Generic;

namespace CCXT.NET.Bitforex.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public class BMyOrder : OdinSdk.BaseLib.Coin.Trade.MyOrder, IMyOrder
    {
        /// <summary>
        /// 
        /// </summary>
        public BMyOrder()
        {
            this.result = new BMyOrderItem();
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new BMyOrderItem result
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
    public class BMyOrders : OdinSdk.BaseLib.Coin.Trade.MyOrders, IMyOrders
    {
        /// <summary>
        /// 
        /// </summary>
        public BMyOrders()
        {
            this.result = new List<BMyOrderItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new List<BMyOrderItem> result
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
    public class BMyOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        /// Transaction pairs such as coin-usd-btc, coin-usd-eth, etc.
        /// </summary>
        [JsonProperty(PropertyName = "symbol")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// Average transaction price
        /// </summary>
        [JsonProperty(PropertyName = "avgPrice")]
        public decimal avgPrice
        {
            get;
            set;
        }

        /// <summary>
        /// Order creation time
        /// </summary>
        [JsonProperty(PropertyName = "createTime")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// last update time
        /// </summary>
        [JsonProperty(PropertyName = "lastTime")]
        public long lastTime
        {
            get;
            set;
        }

        /// <summary>
        /// The number of transactions
        /// </summary>
        [JsonProperty(PropertyName = "dealAmount")]
        public decimal dealAmount
        {
            get;
            set;
        }

        /// <summary>
        /// Order quantity
        /// </summary>
        [JsonProperty(PropertyName = "orderAmount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// Order ID
        /// </summary>
        [JsonProperty(PropertyName = "orderId")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// Lower unit price
        /// </summary>
        [JsonProperty(PropertyName = "orderPrice")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// Order Status: 0 Not Closed, 1 Part Transaction, 2 All Transaction, 3 Partial Deal Canceled, 4 All Revoked
        /// </summary>
        [JsonProperty(PropertyName = "orderState")]
        private int orderStatusValue
        {
            set
            {
                orderStatus = value == 0 ? OrderStatus.Open
                            : value == 1 ? OrderStatus.Partially
                            : value == 2 ? OrderStatus.Closed
                            : value == 3 ? OrderStatus.Canceled // Partial Deal Canceled ?
                            : value == 4 ? OrderStatus.Canceled
                            : OrderStatus.Unknown;
            }
        }

        /// <summary>
        /// Fees
        /// </summary>
        [JsonProperty(PropertyName = "tradeFee")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// Trading methods: 1 buy, 2 sell
        /// </summary>
        [JsonProperty(PropertyName = "tradeType")]
        public override SideType sideType
        {
            get;
            set;
        }
    }
}