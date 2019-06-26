using Newtonsoft.Json;
using CCXT.NET.Coin.Private;

namespace CCXT.NET.Bitflyer.Private
{
    /// <summary>
    ///
    /// </summary>
    public class BAddressItem : CCXT.NET.Coin.Private.AddressItem, IAddressItem
    {
        /// <summary>
        /// "NORMAL" for general deposit addresses.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string type
        {
            get;
            set;
        }

        /// <summary>
        /// "BTC" for Bitcoin addresses and "ETH" for Ethereum addresses.
        /// </summary>
        [JsonProperty(PropertyName = "currency_code")]
        public override string currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "address")]
        public override string address
        {
            get;
            set;
        }
    }
}