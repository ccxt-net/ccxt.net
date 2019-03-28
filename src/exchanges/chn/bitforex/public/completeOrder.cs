using Newtonsoft.Json;

using OdinSdk.BaseLib.Coin.Public;
using OdinSdk.BaseLib.Coin.Types;
using System;
using System.Collections.Generic;

namespace CCXT.NET.Bitforex.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class BCompleteOrders : OdinSdk.BaseLib.Coin.Public.CompleteOrders, ICompleteOrders
    {
        /// <summary>
        /// 
        /// </summary>
        public BCompleteOrders()
        {
            this.result = new List<BCompleteOrderItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new List<BCompleteOrderItem> result
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
    public class BCompleteOrderItem : OdinSdk.BaseLib.Coin.Public.CompleteOrderItem, ICompleteOrderItem
    {
        /// <summary>
        /// Transaction record id
        /// </summary>
        [JsonProperty(PropertyName = "tid")]
        public override string transactionId
        {
            get;
            set;
        }

        /// <summary>
        /// Deal time
        /// </summary>
        [JsonProperty(PropertyName = "time")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// sell or buy
        /// </summary>
        [JsonProperty(PropertyName = "direction")]
        private int sideValue
        {
            set
            {
                sideType = value == 1 ? SideType.Bid
                         : value == 2 ? SideType.Ask
                         : SideType.Unknown;
            }
        }

        /// <summary>
        /// The number of transactions
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// deal price
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public override decimal price
        {
            get;
            set;
        }
    }
}