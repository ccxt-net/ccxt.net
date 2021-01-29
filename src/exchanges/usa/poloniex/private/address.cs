using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Private;

namespace CCXT.NET.Poloniex.Private
{
    /// <summary>
    ///
    /// </summary>
    public class PAddressItem : CCXT.NET.Shared.Coin.Private.AddressItem, IAddressItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "success")]
        private long successValue
        {
            set
            {
                this.success = value == 1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "response")]
        public override string address
        {
            get;
            set;
        }
    }
}