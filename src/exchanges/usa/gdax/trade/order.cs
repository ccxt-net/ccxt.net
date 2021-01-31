using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Trade;
using CCXT.NET.Shared.Coin.Types;
using CCXT.NET.Shared.Configuration;
using System;

namespace CCXT.NET.GDAX.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class GCancelOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        public string orderId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string status
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class GCancelAllOrders
    {
        /// <summary>
        ///
        /// </summary>
        public string result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class GMyOrderItem : CCXT.NET.Shared.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        ///
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
        [JsonProperty(PropertyName = "size")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "product_id")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "side")]
        private string sideValue
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }

        /// <summary>
        /// Self-trade prevention flag
        /// </summary>
        [JsonProperty(PropertyName = "stp")]
        public string stp
        {
            get;
            set;
        }

        /// <summary>
        /// quote currency (ex) USD
        /// </summary>
        [JsonProperty(PropertyName = "funds")]
        public decimal funds
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "specified_funds")]
        public decimal specified_funds
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        private string orderValue
        {
            set
            {
                orderType = OrderTypeConverter.FromString(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "post_only")]
        public bool post_only
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
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
        [JsonProperty(PropertyName = "done_at")]
        private DateTime done_at
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "done_reason")]
        public string done_reason
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "fill_fees")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "filled_size")]
        public override decimal filled
        {
            get;
            set;
        }

        /// <summary>
        /// the cumulative match size * price and is only present for orders placed after 2016-05-20.
        /// </summary>
        [JsonProperty(PropertyName = "executed_value")]
        public decimal executed_value
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string statusValue
        {
            set
            {
                orderStatus = OrderStatusConverter.FromString(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "settled")]
        public bool settled
        {
            get;
            set;
        }

        /// <summary>
        /// specified_funds/filled_size
        /// </summary>
        public override decimal price
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class GMyOrdersItem : CCXT.NET.Shared.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        ///
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
        [JsonProperty(PropertyName = "price")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "size")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "product_id")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "side")]
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
        [JsonProperty(PropertyName = "stp")]
        public string stp
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "funds")]
        public decimal funds
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "specified_funds")]
        public decimal specified_funds
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        private string orderValue
        {
            set
            {
                orderType = OrderTypeConverter.FromString(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "post_only")]
        public bool post_only
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
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
        [JsonProperty(PropertyName = "done_at")]
        private DateTime done_at
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "done_reason")]
        public string done_reason
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "fill_fees")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "filled_size")]
        public override decimal filled
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "executed_value")]
        public decimal executed_value
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string statusValue
        {
            set
            {
                orderStatus = OrderStatusConverter.FromString(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "settled")]
        public bool settled
        {
            get;
            set;
        }
    }
}