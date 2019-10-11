using OdinSdk.BaseLib.Coin.Private;
using Newtonsoft.Json;

namespace CCXT.NET.GateIO.Private
{
    /// <summary>
    ///
    /// </summary>
    public class GAddressItem : OdinSdk.BaseLib.Coin.Private.AddressItem, IAddressItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public override bool success
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "addr")]
        public override string address
        {
            get;
            set;
        }
    }
}