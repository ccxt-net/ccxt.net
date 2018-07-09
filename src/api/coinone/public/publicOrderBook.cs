using System.Collections.Generic;

namespace CCXT.NET.Coinone.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class OrderData
    {
        /// <summary>
        /// KRW price.
        /// </summary>
        public decimal price;

        /// <summary>
        /// BTC quantity.
        /// </summary>
        public decimal qty;
    }

    /// <summary>
    /// 
    /// </summary>
    public class PublicOrderBook : CApiResult
    {
        /// <summary>
        /// Ask List
        /// </summary>
        public List<OrderData> ask;

        /// <summary>
        /// Bid List
        /// </summary>
        public List<OrderData> bid;
    }
}