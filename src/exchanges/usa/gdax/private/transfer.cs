using CCXT.NET.Coin.Private;
using Newtonsoft.Json;

namespace CCXT.NET.GDAX.Private
{
    /// <summary>
    ///
    /// </summary>
    public class GTransfer : CCXT.NET.Coin.Private.Transfer, ITransfer
    {
    }

    /// <summary>
    ///
    /// </summary>
    public class GTransferItem : CCXT.NET.Coin.Private.TransferItem, ITransferItem
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