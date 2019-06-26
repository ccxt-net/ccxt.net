using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CCXT.NET.Coin;
using CCXT.NET.Coin.Private;
using CCXT.NET.Coin.Types;
using CCXT.NET.Configuration;
using System;
using System.Collections.Generic;

namespace CCXT.NET.Zb.Private
{
    /// <summary>
    ///
    /// </summary>
    public class ZWithdraw : CCXT.NET.Coin.Private.Transfer, ITransfer
    {
        /// <summary>
        ///
        /// </summary>
        public ZWithdraw()
        {
            this.result = new ZWithdrawItem();
        }

        /// <summary>
        ///
        /// </summary>
        public new ZWithdrawItem result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public override string message
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        private int code
        {
            set
            {
                if (value == 1000)
                    success = true;
                else
                    success = false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        private string id
        {
            set
            {
                result.transactionId = value;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ZWithdraws : CCXT.NET.Coin.Private.Transfers, ITransfers
    {
        /// <summary>
        ///
        /// </summary>
        public ZWithdraws()
        {
            this.result = new List<ZWithdrawItem>();
        }

        /// <summary>
        ///
        /// </summary>
        public new List<ZWithdrawItem> result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public override ErrorCode errorCode
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originMsg")]
        public override string message
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        private JObject msg
        {
            set
            {
                success = value["isSuc"].Value<bool>();
                message = value["des"].Value<string>();
                result = value["datas"]["list"].ToObject<List<ZWithdrawItem>>();
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ZWithdrawItem : CCXT.NET.Coin.Private.TransferItem, ITransferItem
    {
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
        [JsonProperty(PropertyName = "fees")]
        public override decimal fee
        {
            get;
            set;
        }

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
        [JsonProperty(PropertyName = "manageTime")]
        public long manageTime
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private int status
        {
            set
            {
            }
        }

        /// <summary>
        /// SubmitTime
        /// </summary>
        [JsonProperty(PropertyName = "submitTime")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "toAddress")]
        public override string toAddress
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ZDeposits : CCXT.NET.Coin.Private.Transfers, ITransfers
    {
        /// <summary>
        ///
        /// </summary>
        public ZDeposits()
        {
            this.result = new List<ZDepositItem>();
        }

        /// <summary>
        ///
        /// </summary>
        public new List<ZDepositItem> result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public override ErrorCode errorCode
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originMsg")]
        public override string message
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        private JObject msg
        {
            set
            {
                success = value["isSuc"].Value<bool>();
                message = value["des"].Value<string>();
                result = value["datas"]["list"].ToObject<List<ZDepositItem>>();
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ZDepositItem : CCXT.NET.Coin.Private.TransferItem, ITransferItem
    {
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
        /// 블록체인에 등록 후 추가 블록 갯수
        /// </summary>
        [JsonProperty(PropertyName = "confirmTimes")]
        public override int confirmations
        {
            get;
            set;
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
        [JsonProperty(PropertyName = "description")]
        public string description
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "hash")]
        public override string transactionId
        {
            get;
            set;
        }

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
        [JsonProperty(PropertyName = "itransfer")]
        public int itransfer
        {
            get;
            set;
        }

        /// <summary>
        /// State(0 wait for confirmation, 1 recharge failure, 2 recharge success)
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private int status
        {
            set
            {
                transferType = value == 0 ? TransferType.Processing
                             : value == 1 ? TransferType.Rejected
                             : value == 2 ? TransferType.Done
                             : TransferType.Unknown;

                isCompleted = value == 2 ? true : false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "submit_time")]
        private DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }
    }
}