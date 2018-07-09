using System.Collections.Generic;

namespace CCXT.NET.Coinone.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class TransactionData
    {
        /// <summary>
        /// Transaction ID.
        /// </summary>
        public string txid
        {
            get;
            set;
        }

        /// <summary>
        /// Transaction type. send: "send", receive: "receive".
        /// </summary>
        public string type
        {
            get;
            set;
        }

        /// <summary>
        /// From address.
        /// </summary>
        public string from
        {
            get;
            set;
        }

        /// <summary>
        /// To address.
        /// </summary>
        public string to
        {
            get;
            set;
        }

        /// <summary>
        /// Confirmations.
        /// </summary>
        public int confirmations
        {
            get;
            set;
        }

        /// <summary>
        /// Transaction quantity.
        /// </summary>
        public decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// Timestamp.
        /// </summary>
        public long timestamp
        {
            get;
            set;
        }
    }

    public class UserHistory : CApiResult
    {
        /// <summary>
        /// 
        /// </summary>
        public List<TransactionData> transactions
        {
            get;
            set;
        }
    }
}