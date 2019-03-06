using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Private;

namespace CCXT.NET.Gemini.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class GAddressItem : OdinSdk.BaseLib.Coin.Private.AddressItem, IAddressItem
    {
        /// <summary>
        /// Currency code
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        public override string currency
        {
            get;
            set;
        }

        /// <summary>
        /// String representation of the new cryptocurrency address.
        /// </summary>
        [JsonProperty(PropertyName = "address")]
        public override string address
        {
            get;
            set;
        }

        /// <summary>
        /// Optional. if you provided a label when requesting the address, it will be echoed back here.
        /// </summary>
        [JsonProperty(PropertyName = "label")]
        public string label
        {
            get;
            set;
        }
    }
}