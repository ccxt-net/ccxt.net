using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Private;
using OdinSdk.BaseLib.Coin.Types;

namespace CCXT.NET.Gemini.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class GWithdrawItem : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
    {
        /// <summary>
        /// Standard string format of the transaction hash of the withdrawal transaction
        /// </summary>
        [JsonProperty(PropertyName = "txHash")]
        public override string transactionId
        {
            get;
            set;
        }

        /// <summary>
        /// Standard string format of the withdrawal destination address
        /// </summary>
        [JsonProperty(PropertyName = "destination")]
        public override string toAddress
        {
            get;
            set;
        }

        /// <summary>
        /// The withdrawal amount
        /// </summary>
        public override decimal amount
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GTransferItem : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
    {
        /// <summary>
        /// Transfer event id
        /// </summary>
        [JsonProperty(PropertyName = "eid")]
        public override string transferId
        {
            get;
            set;
        }

        /// <summary>
        /// Optional. When currency is a cryptocurrency, supplies the transaction hash when available.
        /// </summary>
        [JsonProperty(PropertyName = "txHash")]
        public override string transactionId
        {
            get;
            set;
        }

        /// <summary>
        /// Transfer type. Deposit or Withdrawal.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        private string type
        {
            set
            {
                transactionType = TransactionTypeConverter.FromString(value);
            }
        }

        /// <summary>
        /// Transfer status. Advanced or Complete.
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string status
        {
            set
            {
                transferType = TransferTypeConverter.FromString(value);
            }
        }

        /// <summary>
        /// The time that the trade was executed in milliseconds
        /// </summary>
        [JsonProperty(PropertyName = "timestampms")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// Currency code, see symbols
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        public override string currency
        {
            get;
            set;
        }

        /// <summary>
        /// Optional. When currency is a cryptocurrency, supplies the destination address when available.
        /// </summary>
        [JsonProperty(PropertyName = "destination")]
        public override string toAddress
        {
            get;
            set;
        }

        /// <summary>
        /// The transfer amount
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public override decimal amount
        {
            get;
            set;
        }

        /// <summary>
        /// Optional. When currency is a cryptocurrency, supplies the output index in the transaction when available.
        /// </summary>
        [JsonProperty(PropertyName = "outputIdx")]
        public long outputIdx
        {
            get;
            set;
        }

        /// <summary>
        /// Optional. When currency is a fiat currency, the method field will attempt to supply ACH or Wire.
        /// </summary>
        [JsonProperty(PropertyName = "method")]
        public string method
        {
            get;
            set;
        }

        /// <summary>
        /// Optional. Administrative field used to supply a reason for certain types of advances.
        /// </summary>
        [JsonProperty(PropertyName = "purpose")]
        public string purpose
        {
            get;
            set;
        }
    }
}