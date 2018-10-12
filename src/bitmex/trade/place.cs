using Newtonsoft.Json;
using OdinSdk.BaseLib.Configuration;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using System;

namespace CCXT.NET.BitMEX.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public class BPlaceOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "orderID")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "clOrdID")]
        public string clientOrderId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string clOrdLinkID
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal account
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "symbol")]
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
        public decimal simpleOrderQty
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "orderQty")]
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
        public decimal displayQty
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal stopPx
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal pegOffsetValue
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string pegPriceType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string currency
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string settlCurrency
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "ordType")]
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
        [JsonProperty(PropertyName = "timeInForce")]
        public string timeInForce
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string execInst
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string contingencyType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string exDestination
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "ordStatus")]
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
        public string triggered
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool workingIndicator
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ordRejReason
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal simpleLeavesQty
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal leavesQty
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal simpleCumQty
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "cumQty")]
        public override decimal filled
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal avgPx
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string multiLegReportingType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string text
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime transactTime
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "originTimestamp")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        private DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }
    }
}