using OdinSdk.BaseLib.Coin.Private;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using Newtonsoft.Json;
using System;

namespace CCXT.NET.Upbit.Private
{
    /// <summary>
    ///
    /// </summary>
    public class UTransferItem : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
    {
        /// <summary>
        /// 입출금 종류
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
        /// 출금의 고유 아이디
        /// </summary>
        [JsonProperty(PropertyName = "uuid")]
        public override string transferId
        {
            get;
            set;
        }

        /// <summary>
        /// 화폐를 의미하는 영문 대문자 코드
        /// </summary>
        public override string currency
        {
            get;
            set;
        }

        /// <summary>
        /// 출금의 트랜잭션 아이디
        /// </summary>
        [JsonProperty(PropertyName = "txid")]
        public override string transactionId
        {
            get;
            set;
        }

        /// <summary>
        /// 출금 상태
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        private string stateValue
        {
            set
            {
                transferType = TransferTypeConverter.FromString(value);
                isCompleted = transferType == TransferType.Done;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        private DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "done_at")]
        public DateTime? doneAt
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
        ///
        /// </summary>
        [JsonProperty(PropertyName = "krw_amount")]
        public decimal quoteAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 출금 유형: default(일반출금), internal(바로출금)
        /// </summary>
        [JsonProperty(PropertyName = "transaction_type")]
        public string withdrawType
        {
            get;
            set;
        }
    }
}