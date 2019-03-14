using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Private;

namespace CCXT.NET.Kraken.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class KTransferItem : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "refid")]
        public override string transferId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "txid")]
        public override string transactionId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "asset")]
        public override string currency
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "info")]
        public override string toAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string method
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string aclass
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "time")]
        private long timeValue
        {
            set
            {
                timestamp = value * 1000;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string status
        {
            set
            {
                isCompleted = value.ToLower() == "success";
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class KWithdrawCancel : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public bool result
        {
            get;
            set;
        }
    }
}