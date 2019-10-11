using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Private;
using OdinSdk.BaseLib.Coin.Types;
using Newtonsoft.Json;

namespace CCXT.NET.Korbit.Private
{
    /// <summary>
    ///
    /// </summary>
    public class KTransactionDetails
    {
        /// <summary>
        ///
        /// </summary>
        public string transaction_id
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string address
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string destiantion_tag
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string bank
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string account_number
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string owner
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class KTransfer : OdinSdk.BaseLib.Coin.Private.Transfer, ITransfer
    {
        /// <summary>
        ///
        /// </summary>
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
    /// Query Status of BTC Deposit and Transfer
    /// </summary>
    public class KTransferItem : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
    {
        /// <summary>
        /// The unique ID of BTC deposit or withdrawal request.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string transferId
        {
            get;
            set;
        }

        /// <summary>
        /// Unix timestamp in milliseconds by the time deposit or withdrawal request was created.
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        private long timeValue
        {
            set
            {
                timestamp = value;
            }
        }

        /// <summary>
        /// Current status of the order.
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string statusValue
        {
            set
            {
                this.isCompleted = value == "filled";
            }
        }

        /// <summary>
        /// The type of the transfer, which is either deposit or withdrawal.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        private string transactionValue
        {
            set
            {
                transactionType = TransactionTypeConverter.FromString(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "details")]
        private KTransactionDetails details
        {
            set
            {
                this.transactionId = value.transaction_id;

                if (this.transactionType == TransactionType.Deposit)
                {
                    this.fromAddress = value.address;
                    this.fromTag = value.destiantion_tag;
                }
                else
                {
                    this.toAddress = value.address;
                    this.toTag = value.destiantion_tag;
                }
            }
        }
    }
}