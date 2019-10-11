using OdinSdk.BaseLib.Coin.Private;
using OdinSdk.BaseLib.Coin.Types;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CCXT.NET.OKEx.Private
{
    /// <summary>
    ///
    /// </summary>
    public class OTransfer : OdinSdk.BaseLib.Coin.Private.Transfer, ITransfer
    {
        /// <summary>
        ///
        /// </summary>
        public OTransfer()
        {
            this.result = new OTransferItem();
        }

        /// <summary>
        /// withdrawal request ID
        /// </summary>
        [JsonProperty(PropertyName = "withdraw_id")]
        public string transferId
        {
            get;
            set;
        }

        /// <summary>
        /// true for success, false for error
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public override bool success
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class OTransferItem : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
    {
    }

    /// <summary>
    ///
    /// </summary>
    public class ODeposits : OdinSdk.BaseLib.Coin.Private.Transfers, ITransfers
    {
        /// <summary>
        ///
        /// </summary>
        public ODeposits()
        {
            this.result = new List<BDepositItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "records")]
        public new List<BDepositItem> result
        {
            get;
            set;
        }

        /// <summary>
        /// Pairs like : ltc_usd etc_usd
        /// </summary>
        [JsonProperty(PropertyName = "symbol")]
        public string symbol
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BDepositItem : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
    {
        /// <summary>
        /// address
        /// </summary>
        [JsonProperty(PropertyName = "addr")]
        public override string fromAddress
        {
            get;
            set;
        }

        /// <summary>
        /// account name
        /// </summary>
        [JsonProperty(PropertyName = "account")]
        public string account
        {
            get;
            set;
        }

        /// <summary>
        /// amount
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public override decimal amount
        {
            get;
            set;
        }

        /// <summary>
        /// bank
        /// </summary>
        [JsonProperty(PropertyName = "bank")]
        public string bank
        {
            get;
            set;
        }

        /// <summary>
        /// benificiary address
        /// </summary>
        [JsonProperty(PropertyName = "benificiary_addr")]
        public string benificiary_addr
        {
            get;
            set;
        }

        /// <summary>
        /// withdraw amount after fee deduction
        /// </summary>
        [JsonProperty(PropertyName = "transaction_value")]
        public decimal transaction_value
        {
            get;
            set;
        }

        /// <summary>
        /// commission fee
        /// </summary>
        [JsonProperty(PropertyName = "fee")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// Unix timestamp in milliseconds by the time deposit or withdrawal request was created.
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// recharge status (-1:Failure;0:Wait Confirmation;1:Complete)
        /// </summary>
        private int status
        {
            set
            {
                transactionType = value == (-1 | 1) ? TransactionType.Deposit
                                : value == 0 ? TransactionType.Depositing
                                : TransactionType.Unknown;

                transferType = value == -1 ? TransferType.Rejected
                             : value == 0 ? TransferType.Processing
                             : value == 1 ? TransferType.Done
                             : TransferType.Unknown;

                isCompleted = value == 1 ? true : false;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class OWithdraws : OdinSdk.BaseLib.Coin.Private.Transfers, ITransfers
    {
        /// <summary>
        ///
        /// </summary>
        public OWithdraws()
        {
            this.result = new List<OWithdrawItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "records")]
        public new List<OWithdrawItem> result
        {
            get;
            set;
        }

        /// <summary>
        /// btc, ltc, eth, etc, bch, usdt
        /// </summary>
        [JsonProperty(PropertyName = "symbol")]
        public string symbol
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class OWithdrawItem : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
    {
        /// <summary>
        /// address
        /// </summary>
        [JsonProperty(PropertyName = "addr")]
        public override string toAddress
        {
            get;
            set;
        }

        /// <summary>
        /// account name
        /// </summary>
        [JsonProperty(PropertyName = "account")]
        public string account
        {
            get;
            set;
        }

        /// <summary>
        /// amount
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public override decimal amount
        {
            get;
            set;
        }

        /// <summary>
        /// bank
        /// </summary>
        [JsonProperty(PropertyName = "bank")]
        public string bank
        {
            get;
            set;
        }

        /// <summary>
        /// benificiary address
        /// </summary>
        [JsonProperty(PropertyName = "benificiary_addr")]
        public string benificiary_addr
        {
            get;
            set;
        }

        /// <summary>
        /// withdraw amount after fee deduction
        /// </summary>
        [JsonProperty(PropertyName = "transaction_value")]
        public decimal transaction_value
        {
            get;
            set;
        }

        /// <summary>
        /// commission fee
        /// </summary>
        [JsonProperty(PropertyName = "fee")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// Unix timestamp in milliseconds by the time deposit or withdrawal request was created.
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// withdrawal status (-3:Revoked;-2:Cancelled;-1:Failure;0:Pending;1:Pending;2:Complete;3:Email Confirmation;4:Verifying5:Wait Confirmation)
        /// </summary>
        private int status
        {
            set
            {
                transactionType = value == (-3 | -2 | -1 | 2) ? TransactionType.Deposit
                                : value == (0 | 1 | 3 | 4 | 5) ? TransactionType.Depositing
                                : TransactionType.Unknown;

                transferType = value == (-3 | -2) ? TransferType.Canceled
                             : value == -1 ? TransferType.Rejected
                             : value == (0 | 1 | 3 | 4 | 5) ? TransferType.Processing
                             : value == 2 ? TransferType.Done
                             : TransferType.Unknown;

                isCompleted = value == 2 ? true : false;
            }
        }
    }
}