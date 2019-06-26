using Newtonsoft.Json;
using CCXT.NET.Coin.Private;
using CCXT.NET.Coin.Types;
using CCXT.NET.Configuration;
using System;

namespace CCXT.NET.Bitflyer.Private
{
    /// <summary>
    ///
    /// </summary>
    public class BWithdraw : CCXT.NET.Coin.Private.Transfer, ITransfer
    {
        /// <summary>
        ///
        /// </summary>
        public override bool success
        {
            get
            {
                base.success = String.IsNullOrEmpty(transferId) == false;
                return base.success;
            }
            set => base.success = value;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "message_id")]
        public string transferId
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BDepositItem : CCXT.NET.Coin.Private.TransferItem, ITransferItem
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
        [JsonProperty(PropertyName = "order_id")]
        public string orderId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "currency_code")]
        public override string currency
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
        [JsonProperty(PropertyName = "address")]
        public override string fromAddress
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "tx_hash")]
        public override string transactionId
        {
            get;
            set;
        }

        /// <summary>
        /// If the Bitcoin deposit is being processed, it will be listed as "PENDING".
        /// If the deposit has been completed, it will be listed as "COMPLETED".
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string status
        {
            set
            {
                transactionType = value == "COMPLETED" ? TransactionType.Deposit : value == "PENDING" ? TransactionType.Depositing : TransactionType.Unknown;
                isCompleted = value == "COMPLETED";
            }
        }

        /// <summary>
        /// Unix timestamp in milliseconds by the time deposit or withdrawal request was created.
        /// </summary>
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "event_date")]
        private DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BWithdrawItem : CCXT.NET.Coin.Private.TransferItem, ITransferItem
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
        [JsonProperty(PropertyName = "order_id")]
        public string orderId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "currency_code")]
        public override string currency
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
        [JsonProperty(PropertyName = "address")]
        public override string toAddress
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "tx_hash")]
        public override string transactionId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "additional_fee")]
        public decimal additional_fee
        {
            get;
            set;
        }

        /// <summary>
        /// If the remittance is being processed, it will be listed as "PENDING".
        /// If the remittance has been completed, it will be listed as "COMPLETED".
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string status
        {
            set
            {
                transactionType = value == "COMPLETED" ? TransactionType.Withdraw : value == "PENDING" ? TransactionType.Withdrawing : TransactionType.Unknown;
                isCompleted = value == "COMPLETED";
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "event_date")]
        private DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }
    }
}