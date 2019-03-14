using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Configuration;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using System;

namespace CCXT.NET.ItBit.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public class TMyOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
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
        [JsonProperty(PropertyName = "walletId")]
        public string walletId
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
        [JsonProperty(PropertyName = "instrument")]
        public override string symbol
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
        /// order amount
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
        public decimal displayAmount
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
        [JsonProperty(PropertyName = "volumeWeightedAveragePrice")]
        public decimal vwap
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "amountFilled")]
        public override decimal filled
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "createdTime")]
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
        [JsonProperty(PropertyName = "status")]
        private string statusValue
        {
            set
            {
                orderStatus = OrderStatusConverter.FromString(value);
            }
        }

        /// <summary>
        /// order currency
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        public string currency
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public JObject metadata
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string clientOrderIdentifier
        {
            get;
            set;
        }
    }
}