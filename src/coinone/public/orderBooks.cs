using System.Collections.Generic;

namespace CCXT.NET.Coinone.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class COrderBookItem
    {
        /// <summary>
        /// KRW price.
        /// </summary>
        public decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// BTC quantity.
        /// </summary>
        public decimal qty
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class COrderBooks : CApiResult
    {
        /// <summary>
        /// Ask List
        /// </summary>
        public List<COrderBookItem> ask
        {
            get;
            set;
        }

        /// <summary>
        /// Bid List
        /// </summary>
        public List<COrderBookItem> bid
        {
            get;
            set;
        }
    }
}