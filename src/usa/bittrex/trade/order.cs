using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Trade;
using CCXT.NET.Shared.Coin.Types;
using CCXT.NET.Shared.Configuration;
using System;
using System.Collections.Generic;

namespace CCXT.NET.Bittrex.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class BMyOrder : CCXT.NET.Shared.Coin.Trade.MyOrder, IMyOrder
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
        [JsonProperty(PropertyName = "result")]
        public new BMyOrderItem result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BMyOrderItem : CCXT.NET.Shared.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "Exchange")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "OrderUuid")]
        public override string orderId
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
        [JsonProperty(PropertyName = "CommissionReserved")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "QuantityRemaining")]
        public decimal filledValue
        {
            set
            {
                filled = quantity - value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "Sentinel")]
        public string Sentinel
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "IsOpen")]
        private bool statusValue
        {
            set
            {
                orderStatus = value == true ? OrderStatus.Open : OrderStatus.Closed;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "Opened")]
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
        [JsonProperty(PropertyName = "Type")]
        private string sideValue2
        {
            set
            {
                var _types = value.Split('_');

                orderType = OrderTypeConverter.FromString(_types[0]);
                sideType = SideTypeConverter.FromString(_types[1]);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BMyOrders : CCXT.NET.Shared.Coin.Trade.MyOrders, IMyOrders
    {
        /// <summary>
        ///
        /// </summary>
        public BMyOrders()
        {
            this.result = new List<BMyOrdersItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public new List<BMyOrdersItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BMyOrdersItem : CCXT.NET.Shared.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "OrderUuid")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "Exchange")]
        public override string symbol
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
        [JsonProperty(PropertyName = "Commission")]
        public override decimal fee
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
        public string AccountId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "QuantityRemaining")]
        public decimal filledValue
        {
            set
            {
                filled = quantity - value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public decimal Limit
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal ReserveRemaining
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal CommissionReserved
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal CommissionReserveRemaining
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string PricePerUnit
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime? Closed
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsOpen
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string Sentinel
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool CancelInitiated
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool ImmediateOrCancel
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsConditional
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string Condition
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string ConditionTarget
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "CommissionPaid")]
        private decimal CommissionPaid
        {
            set
            {
                fee = value;
            }
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
        [JsonProperty(PropertyName = "Opened")]
        private DateTime Opened
        {
            set
            {
                timeValue = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "Type")]
        private string sideValue
        {
            set
            {
                orderValue = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "OrderType")]
        private string orderValue
        {
            set
            {
                var _types = value.Split('_');

                orderType = OrderTypeConverter.FromString(_types[0]);
                sideType = SideTypeConverter.FromString(_types[1]);
            }
        }
    }
}