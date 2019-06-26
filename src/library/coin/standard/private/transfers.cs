using CCXT.NET.Coin.Types;
using CCXT.NET.Configuration;
using System.Collections.Generic;

namespace CCXT.NET.Coin.Private
{
    /// <summary>
    ///
    /// </summary>
    public interface ITransferItem
    {
        /// <summary>
        /// transaction id
        /// </summary>
        string transferId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string transactionId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        TransactionType transactionType
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        TransferType transferType
        {
            get;
            set;
        }

        /// <summary>
        /// 64-bit Unix Timestamp in milliseconds since Epoch 1 Jan 1970
        /// </summary>
        long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// ISO 8601 datetime string with milliseconds
        /// </summary>
        string datetime
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        string currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string fromAddress
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string fromTag
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string toAddress
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string toTag
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        decimal amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// 블록체인에 등록 후 추가 블록 갯수
        /// </summary>
        int confirmations
        {
            get;
            set;
        }

        /// <summary>
        /// 출금 또는 입금시 오류 또는 취소시 false
        /// </summary>
        bool isCompleted
        {
            get;
            set;
        }

        ///// <summary>
        /////
        ///// </summary>
        //string ipAddress
        //{
        //    get;
        //    set;
        //}

#if DEBUG
        /// <summary>
        ///
        /// </summary>
        string rawJson
        {
            get;
            set;
        }
#endif
    }

    /// <summary>
    ///
    /// </summary>
    public class TransferItem : ITransferItem
    {
        /// <summary>
        ///
        /// </summary>
        public virtual string transferId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual string transactionId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual TransactionType transactionType
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual TransferType transferType
        {
            get;
            set;
        }

        /// <summary>
        /// Unix timestamp in milliseconds by the time deposit or withdrawal request was created.
        /// </summary>
        public virtual long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// ISO 8601 datetime string with milliseconds
        /// </summary>
        public virtual string datetime
        {
            get
            {
                return CUnixTime.ConvertToUtcTimeMilli(timestamp).ToString("o");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual string currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual string fromAddress
        {
            get;
            set;
        }

        /// <summary>
        /// like ripple need destination tag
        /// </summary>
        public virtual string fromTag
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual string toAddress
        {
            get;
            set;
        }

        /// <summary>
        /// like ripple need destination tag
        /// </summary>
        public virtual string toTag
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual decimal amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// 블록체인에 등록 후 추가 블록 갯수
        /// </summary>
        public virtual int confirmations
        {
            get;
            set;
        }

        /// <summary>
        /// 출금 또는 입금시 오류 또는 취소시 false
        /// </summary>
        public virtual bool isCompleted
        {
            get;
            set;
        }

        ///// <summary>
        /////
        ///// </summary>
        //public virtual string ipAddress
        //{
        //    get;
        //    set;
        //}

#if DEBUG
        /// <summary>
        ///
        /// </summary>
        public virtual string rawJson
        {
            get;
            set;
        }
#endif
    }

    /// <summary>
    ///
    /// </summary>
    public interface ITransfer : IApiResult<ITransferItem>
    {
#if DEBUG
        /// <summary>
        ///
        /// </summary>
        string rawJson
        {
            get;
            set;
        }
#endif
    }

    /// <summary>
    ///
    /// </summary>
    public class Transfer : ApiResult<ITransferItem>, ITransfer
    {
        /// <summary>
        ///
        /// </summary>
        public Transfer()
        {
            this.result = new TransferItem();
        }

#if DEBUG
        /// <summary>
        ///
        /// </summary>
        public virtual string rawJson
        {
            get;
            set;
        }
#endif
    }

    /// <summary>
    ///
    /// </summary>
    public interface ITransfers : IApiResult<List<ITransferItem>>
    {
#if DEBUG
        /// <summary>
        ///
        /// </summary>
        string rawJson
        {
            get;
            set;
        }
#endif
    }

    /// <summary>
    ///
    /// </summary>
    public class Transfers : ApiResult<List<ITransferItem>>, ITransfers
    {
        /// <summary>
        ///
        /// </summary>
        public Transfers()
        {
            this.result = new List<ITransferItem>();
        }

#if DEBUG
        /// <summary>
        ///
        /// </summary>
        public virtual string rawJson
        {
            get;
            set;
        }
#endif
    }
}