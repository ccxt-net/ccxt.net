using Newtonsoft.Json;
using OdinSdk.BaseLib.Configuration;
using OdinSdk.BaseLib.Coin.Private;
using System;
using System.Collections.Generic;

namespace CCXT.NET.Bittrex.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class BWithdraw : OdinSdk.BaseLib.Coin.Private.Transfer, ITransfer
    {
        /// <summary>
        /// 
        /// </summary>
        public BWithdraw()
        {
            this.result = new BWithdrawItem();
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public new BWithdrawItem result
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BWithdrawItem : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "uuid")]
        public override string transferId
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BTransfers : OdinSdk.BaseLib.Coin.Private.Transfers, ITransfers
    {
        /// <summary>
        /// 
        /// </summary>
        public BTransfers()
        {
            this.result = new List<BTransferItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public new List<BTransferItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BTransfer : OdinSdk.BaseLib.Coin.Private.Transfer, ITransfer
    {
        /// <summary>
        /// 
        /// </summary>
        public BTransfer()
        {
            this.result = new BTransferItem();
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public new BTransferItem result
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
        [JsonProperty(PropertyName = "PaymentUuid")]
        public override string transferId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "TxId")]
        public override string transactionId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Currency")]
        public override string currency
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Address")]
        public override string toAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Amount")]
        public override decimal amount
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "TxCost")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Authorized")]
        public bool authorized
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "PendingPayment")]
        public bool pendingPayment
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "InvalidAddress")]
        public bool invalidAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Canceled")]
        private bool canceled
        {
            set
            {
                isCompleted = !value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Opened")]
        private DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }
    }
}