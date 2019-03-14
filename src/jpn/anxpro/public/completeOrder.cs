using Newtonsoft.Json;
using OdinSdk.BaseLib.Configuration;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Coin.Public;
using System;

namespace CCXT.NET.Anxpro.Public
{
    /// <summary>
    /// recent trade data 
    /// </summary>
    public class ACompleteOrderItem : OdinSdk.BaseLib.Coin.Public.CompleteOrderItem, ICompleteOrderItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string transactionId
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
        [JsonProperty(PropertyName = "exec_date")]
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
        [JsonProperty(PropertyName = "side")]
        private string sideValue
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }
    }
}