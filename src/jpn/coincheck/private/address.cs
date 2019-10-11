using OdinSdk.BaseLib.Coin.Private;
using Newtonsoft.Json;

namespace CCXT.NET.CoinCheck.Private
{
    /// <summary>
    ///
    /// </summary>
    public class CAddressItem : OdinSdk.BaseLib.Coin.Private.AddressItem, IAddressItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "success")]
        public override bool success
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string id
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public string email
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "identity_status")]
        public string identity_status
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "bitcoin_address")]
        public override string address
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "lending_leverage")]
        public decimal lending_leverage
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "taker_fee")]
        public decimal taker_fee
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "maker_fee")]
        public decimal maker_fee
        {
            get;
            set;
        }
    }
}