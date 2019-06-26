using Newtonsoft.Json;
using CCXT.NET.Coin.Private;

namespace CCXT.NET.Anxpro.Private
{
    /// <summary>
    ///
    /// </summary>
    public class AAddress : CCXT.NET.Coin.Private.Address, IAddress
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
    public class AAddressItem : CCXT.NET.Coin.Private.AddressItem, IAddressItem
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