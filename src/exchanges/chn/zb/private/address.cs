using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Coin.Private;

namespace CCXT.NET.Zb.Private
{
    /// <summary>
    ///
    /// </summary>
    public class ZAddress : CCXT.NET.Shared.Coin.Private.Address, IAddress
    {
        /// <summary>
        ///
        /// </summary>
        public ZAddress()
        {
            this.result = new ZAddressItem();
        }

        /// <summary>
        ///
        /// </summary>
        public new ZAddressItem result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public override ErrorCode errorCode
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originMsg")]
        public override string message
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        private JObject msg
        {
            set
            {
                success = value["isSuc"].Value<bool>();
                message = value["des"].Value<string>();
                result.address = value["datas"]["key"].Value<string>();
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ZAddressItem : CCXT.NET.Shared.Coin.Private.AddressItem, IAddressItem
    {
        /// <summary>
        ///
        /// </summary>
        public override string currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override string address
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override string tag
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override bool success
        {
            get;
            set;
        }
    }
}