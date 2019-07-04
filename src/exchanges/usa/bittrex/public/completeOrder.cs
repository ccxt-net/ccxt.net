using CCXT.NET.Coin.Public;
using CCXT.NET.Coin.Types;
using CCXT.NET.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CCXT.NET.Bittrex.Public
{
    /// <summary>
    ///
    /// </summary>
    public class BCompleteOrders : CCXT.NET.Coin.Public.CompleteOrders, ICompleteOrders
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
        [JsonProperty(PropertyName = "result")]
        public new List<BCompleteOrderItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Used to retrieve the latest trades that have occured for a specific market.
    /// </summary>
    public class BCompleteOrderItem : CCXT.NET.Coin.Public.CompleteOrderItem, ICompleteOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public override string transactionId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "Quantity")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "Price")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "Total")]
        public override decimal amount
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
        [JsonProperty(PropertyName = "TimeStamp")]
        private DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "OrderType")]
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
        [JsonProperty(PropertyName = "FillType")]
        private string fillValue
        {
            set
            {
                fillType = FillTypeConverter.FromString(value);
            }
        }
    }
}