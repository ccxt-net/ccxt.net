using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Private;

namespace CCXT.NET.GDAX.Private
{
    /// <summary>
    ///
    /// </summary>
    public class GTransfer : CCXT.NET.Shared.Coin.Private.Transfer, ITransfer
    {
    }

    /// <summary>
    ///
    /// </summary>
    public class GTransferItem : CCXT.NET.Shared.Coin.Private.TransferItem, ITransferItem
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