using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Private;

namespace CCXT.NET.Anxpro.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class AAddress : OdinSdk.BaseLib.Coin.Private.Address, IAddress
    {
        /// <summary>
        /// 
        /// </summary>
        public AAddress()
        {
            this.result = new AAddressItem();
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new AAddressItem result
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AAddressItem : OdinSdk.BaseLib.Coin.Private.AddressItem, IAddressItem
    {
        /// <summary>
        /// 전자지갑 Address
        /// </summary>
        [JsonProperty(PropertyName = "addr")]
        public override string address
        {
            get;
            set;
        }
    }
}