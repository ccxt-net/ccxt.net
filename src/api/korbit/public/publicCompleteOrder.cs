using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace CCXT.NET.Korbit.Public
{
    /// <summary>
    /// 체결 내역 ( List of Filled Orders )
    /// </summary>
    public class PublicCompleteOrder
    {
        /// <summary>
        /// Timestamp of last filled order.
        /// </summary>
        public long timestamp;

        /// <summary>
        /// Unique ID that identifies the transaction.
        /// </summary>
        public long tid;

        /// <summary>
        /// Transaction price in KRW.
        /// </summary>
        public decimal price;

        /// <summary>
        /// Transaction amount in BTC.
        /// </summary>
        public decimal amount;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="tid"></param>
        /// <param name="price"></param>
        /// <param name="amount"></param>
        [JsonConstructor]
        public PublicCompleteOrder(string timestamp, string tid, string price, string amount)
        {
            this.timestamp = Convert.ToInt64(timestamp);
            this.tid = Convert.ToInt64(tid);
            this.price = decimal.Parse(price, NumberStyles.Float);
            this.amount = decimal.Parse(amount, NumberStyles.Float);
        }
    }

    public class PublicCompleteOrders : List<PublicCompleteOrder>
    {
    }
}