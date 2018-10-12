using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Private;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System;

namespace CCXT.NET.BitMEX.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class BTransferItem : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "transactID")]
        public override string transferId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "tx")]
        public override string transactionId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string account
        {
            get;
            set;
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
        public string transactType
        {
            set
            {
                transactionType = TransactionTypeConverter.FromString(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "transactStatus")]
        public string transactStatus
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
        [JsonProperty(PropertyName = "originTimestamp")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
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
        public decimal? walletBalance
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal? marginBalance
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string text
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime transactTime
        {
            get;
            set;
        }
    }
}