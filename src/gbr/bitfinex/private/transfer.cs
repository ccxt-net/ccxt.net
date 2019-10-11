using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Private;
using OdinSdk.BaseLib.Coin.Types;
using Newtonsoft.Json;

namespace CCXT.NET.Bitfinex.Private
{
    /// <summary>
    ///
    /// </summary>
    public class BTransfer : OdinSdk.BaseLib.Coin.Private.Transfer, ITransfer
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "withdrawal_id")]
        public string transferId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string status
        {
            set
            {
                message = value;

                if (message == "success")
                {
                    statusCode = 0;
                    errorCode = ErrorCode.Success;
                    success = true;
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BTransferItem : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
    {
        /// <summary>
        /// transaction id
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
        [JsonProperty(PropertyName = "txid")]
        public override string transactionId
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
        [JsonProperty(PropertyName = "method")]
        public string method
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string description
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "timestamp_created")]
        public decimal timestamp_created
        {
            get;
            set;
        }

        /// <summary>
        /// 64-bit Unix Timestamp in milliseconds since Epoch 1 Jan 1970
        /// </summary>
        [JsonProperty(PropertyName = "origin_timestamp")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        private decimal timeValue
        {
            set
            {
                timestamp = (long)(value * 1000);
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
                isCompleted = value == "COMPLETED";
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        private string typeValue
        {
            set
            {
                transactionType = TransactionTypeConverter.FromString(value);
            }
        }
    }
}