using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Private;

namespace CCXT.NET.Poloniex.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class PWithdraw : OdinSdk.BaseLib.Coin.Private.Transfer, ITransfer
    {
        /// <summary>
        /// 
        /// </summary>
        public PWithdraw()
        {
            this.result = new PTransferItem();
        }

        [JsonProperty(PropertyName = "response")]
        private string response
        {
            set
            {
                message = value;
                success = message.IndexOf("Withdrew") >= 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public new PTransferItem result
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PTransferItem : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
    {
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
        [JsonProperty(PropertyName = "withdrawalNumber")]
        public override string transferId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "address")]
        public override string fromAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "originTimestamp")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
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
                var _states = value.Split(' ');
                if (_states.Length > 1)
                    transactionId = _states[1];

                isCompleted = value.IndexOf("COMPLETE") >= 0;
            }
        }
    }
}