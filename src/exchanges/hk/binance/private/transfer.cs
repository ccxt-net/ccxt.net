using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Private;
using CCXT.NET.Shared.Coin.Types;
using System.Collections.Generic;

namespace CCXT.NET.Binance.Private
{
    /// <summary>
    ///
    /// </summary>
    public class BDeposits : CCXT.NET.Shared.Coin.Private.Transfers, ITransfers
    {
        /// <summary>
        ///
        /// </summary>
        public BDeposits()
        {
            this.result = new List<BDepositItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "depositList")]
        public new List<BDepositItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BDepositItem : CCXT.NET.Shared.Coin.Private.TransferItem, ITransferItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "txId")]
        public override string transactionId
        {
            get;
            set;
        }

        /// <summary>
        /// Unix timestamp in milliseconds by the time deposit or withdrawal request was created.
        /// </summary>
        [JsonProperty(PropertyName = "insertTime")]
        public override long timestamp
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
        [JsonProperty(PropertyName = "address")]
        public override string fromAddress
        {
            get;
            set;
        }

        /// <summary>
        /// like ripple need destination tag
        /// </summary>
        [JsonProperty(PropertyName = "addressTag")]
        public override string fromTag
        {
            get;
            set;
        }

        /// <summary>
        /// 0(0:pending,1:success)
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private int typeStatus
        {
            set
            {
                transactionType = (value == 1) ? TransactionType.Deposit
                                : (value == 0) ? TransactionType.Depositing
                                : TransactionType.Unknown;

                isCompleted = (value == 1);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BTransfer : CCXT.NET.Shared.Coin.Private.Transfer, ITransfer
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "msg")]
        public override string message
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override bool success
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string transferId
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BWithdraws : CCXT.NET.Shared.Coin.Private.Transfers, ITransfers
    {
        /// <summary>
        ///
        /// </summary>
        public BWithdraws()
        {
            this.result = new List<BWithdrawItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "withdrawList")]
        public new List<BWithdrawItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BWithdrawItem : CCXT.NET.Shared.Coin.Private.TransferItem, ITransferItem
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
        [JsonProperty(PropertyName = "txId")]
        public override string transactionId
        {
            get;
            set;
        }

        /// <summary>
        /// Unix timestamp in milliseconds by the time deposit or withdrawal request was created.
        /// </summary>
        [JsonProperty(PropertyName = "applyTime")]
        public override long timestamp
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
        [JsonProperty(PropertyName = "address")]
        public override string toAddress
        {
            get;
            set;
        }

        /// <summary>
        /// like ripple need destination tag
        /// </summary>
        [JsonProperty(PropertyName = "addressTag")]
        public override string toTag
        {
            get;
            set;
        }

        /// <summary>
        /// 0(0:Email Sent,1:Cancelled 2:Awaiting Approval 3:Rejected 4:Processing 5:Failure 6Completed)
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private int typeStatus
        {
            set
            {
                transactionType = (value == 1 || value == 3 || value == 5 || value == 6) ? TransactionType.Withdraw
                                : (value == 0 || value == 2 || value == 4) ? TransactionType.Withdrawing
                                : TransactionType.Unknown;

                isCompleted = (value == 6);
            }
        }
    }
}