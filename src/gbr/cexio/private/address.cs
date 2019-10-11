using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Private;

namespace CCXT.NET.CEXIO.Private
{
    /// <summary>
    ///
    /// </summary>
    public class CAddressItem : OdinSdk.BaseLib.Coin.Private.AddressItem, IAddressItem
    {
        /// <summary>
        ///
        /// </summary>
        public string ok
        {
            set
            {
                success = value == "ok";
            }
        }

        /// <summary>
        /// 전자지갑 Address
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public override string address
        {
            get;
            set;
        }
    }
}