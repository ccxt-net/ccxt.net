using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Private;

namespace CCXT.NET.Bitfinex.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class BAddressItem : OdinSdk.BaseLib.Coin.Private.AddressItem, IAddressItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public string result
        {
            set
            {
                this.success = value == "success";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "method")]
        public string method
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "address_pool")]
        public string address_pool
        {
            get;
            set;
        }
    }
}