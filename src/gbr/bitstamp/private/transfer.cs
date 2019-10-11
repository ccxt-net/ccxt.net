using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Private;

namespace CCXT.NET.Bitstamp.Private
{
    /// <summary>
    ///
    /// </summary>
    public class BWithdrawItem : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
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
    }

    /// <summary>
    ///
    /// </summary>
    public class BTransferItem : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
    {
        /// <summary>
        ///
        /// </summary>
        public decimal usd
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal eur
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal btc
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal btc_usd
        {
            get;
            set;
        }
    }
}