using OdinSdk.BaseLib.Coin.Public;
using OdinSdk.BaseLib.Coin.Types;
using Newtonsoft.Json;

namespace CCXT.NET.Bitstamp.Public
{
    /// <summary>
    /// recent trade data
    /// </summary>
    public class BCompleteOrderItem : OdinSdk.BaseLib.Coin.Public.CompleteOrderItem, ICompleteOrderItem
    {
        /// <summary>
        /// Transaction ID.
        /// </summary>
        [JsonProperty(PropertyName = "tid")]
        public override string transactionId
        {
            get;
            set;
        }

        /// <summary>
        /// BTC amount.
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
        [JsonProperty(PropertyName = "originAmount")]
        public override decimal amount
        {
            get;
            set;
        }

        /// <summary>
        /// 0 (buy) or 1 (sell).
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
        /// Unix timestamp date and time.
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        private long timeValue
        {
            set
            {
                timestamp = value * 1000;
            }
        }
    }
}