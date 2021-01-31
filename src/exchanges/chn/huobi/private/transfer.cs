using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Private;
using CCXT.NET.Shared.Coin.Types;
using CCXT.NET.Shared.Configuration;
using System.Collections.Generic;

namespace CCXT.NET.Huobi.Private
{
    /// <summary>
    ///
    /// </summary>
    public class HWithdraw : CCXT.NET.Shared.Coin.Private.Transfer, ITransfer
    {
        /// <summary>
        ///
        /// </summary>
        public HWithdraw()
        {
            this.result = new HWithdrawItem();
        }

        /// <summary>
        ///
        /// </summary>
        public new HWithdrawItem result
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
                success = value == "ok" ? true : false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        private string data
        {
            set
            {
                result.transferId = value;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class HWithdrawItem : CCXT.NET.Shared.Coin.Private.TransferItem, ITransferItem
    {
        /// <summary>
        ///
        /// </summary>
        public override string transferId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override string transactionId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override TransactionType transactionType
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override TransferType transferType
        {
            get;
            set;
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
        /// ISO 8601 datetime string with milliseconds
        /// </summary>
        public override string datetime
        {
            get
            {
                return CUnixTime.ConvertToUtcTimeMilli(timestamp).ToString("o");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override string currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override string fromAddress
        {
            get;
            set;
        }

        /// <summary>
        /// like ripple need destination tag
        /// </summary>
        public override string fromTag
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override string toAddress
        {
            get;
            set;
        }

        /// <summary>
        /// like ripple need destination tag
        /// </summary>
        public override string toTag
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
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// 블록체인에 등록 후 추가 블록 갯수
        /// </summary>
        public override int confirmations
        {
            get;
            set;
        }

        /// <summary>
        /// 출금 또는 입금시 오류 또는 취소시 false
        /// </summary>
        public override bool isCompleted
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class HTransfers : CCXT.NET.Shared.Coin.Private.Transfers, ITransfers
    {
        /// <summary>
        ///
        /// </summary>
        public HTransfers()
        {
            this.result = new List<HTransferItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new List<HTransferItem> result
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
                success = value == "ok" ? true : false;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class HTransferItem : CCXT.NET.Shared.Coin.Private.TransferItem, ITransferItem
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
        [JsonProperty(PropertyName = "type")]
        private string type
        {
            set
            {
                transactionType = TransactionTypeConverter.FromString(value);
            }
        }

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
        [JsonProperty(PropertyName = "tx-hash")]
        public override string transactionId
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
        [JsonProperty(PropertyName = "address")]
        private string address
        {
            set
            {
                if (transactionType == TransactionType.Deposit || transactionType == TransactionType.Depositing)
                    fromAddress = value;
                else
                    toAddress = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "address-tag")]
        public string tag
        {
            set
            {
                if (transactionType == TransactionType.Deposit || transactionType == TransactionType.Depositing)
                    fromTag = value;
                else
                    fromAddress = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "fee")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// withdraw states - submitted, reexamine, canceled, pass, reject, pre-transfer, wallet-transfer, wallet-reject, confirmed, confirm-error, repealed
        /// deposit states - unknown, confirming, confirmed, safe, orphan
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        private string state
        {
            set
            {
                transferType = TransferTypeConverter.FromString(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "created-at")]
        public long createdTimestamp
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "updated-at")]
        public override long timestamp
        {
            get;
            set;
        }
    }
}