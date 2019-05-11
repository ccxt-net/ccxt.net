using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Private;

namespace CCXT.NET.GDAX.Private
{
    /// <summary>
    ///
    /// </summary>
    public class GTransfer : OdinSdk.BaseLib.Coin.Private.Transfer, ITransfer
    {
    }

    /// <summary>
    ///
    /// </summary>
    public class GTransferItem : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string transferId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override decimal amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override string currency
        {
            get;
            set;
        }
    }
}