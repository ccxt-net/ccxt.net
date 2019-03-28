using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Private;
using OdinSdk.BaseLib.Coin.Types;

using System;
using System.Collections.Generic;

namespace CCXT.NET.GateIO.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class GTransfers
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public bool success
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "deposits")]
        public List<GDepositItem> deposits
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "withdraws")]
        public List<GWithdrawItem> withdraws
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string message
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GDepositItem : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
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
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public override decimal amount
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
        /// Unix timestamp in milliseconds by the time deposit or withdrawal request was created.
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
                transferType = TransferTypeConverter.FromString(value);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GWithdrawItem : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
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
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public override decimal amount
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
        /// Unix timestamp in milliseconds by the time deposit or withdrawal request was created.
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
                transferType = TransferTypeConverter.FromString(value);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GTransferItem : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
    {
        
    }
}