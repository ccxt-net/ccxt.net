using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CCXT.NET.Korbit.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class KOrderBookItem
    {
        /// <summary>
        /// 가격
        /// </summary>
        public decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// 미체결잔량합계
        /// </summary>
        public decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// 개별호가건수
        /// </summary>
        public decimal count
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class KOrderBooks
    {
        /// <summary>
        /// Unix timestamp in milliseconds of the last placed order.
        /// </summary>
        public long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// An array containing a list of ask prices. 
        /// Each order has two elements: price and the unfilled amount. 
        /// The third element is deprecated and is always 1.
        /// </summary>
        public JArray bids
        {
            get;
            set;
        }

        /// <summary>
        /// An array containing a list of bid prices. 
        /// Each order has two elements: price and the unfilled amount. 
        /// The third element is deprecated and is always 1.
        /// </summary>
        public JArray asks
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<KOrderBookItem> GetBids()
        {
            var _result = new List<KOrderBookItem>();

            foreach (var b in bids)
            {
                var o = new KOrderBookItem()
                {
                    price = b[0].Value<decimal>(),
                    quantity = b[1].Value<decimal>(),
                    count = b[2].Value<decimal>()
                };

                _result.Add(o);
            }

            return _result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<KOrderBookItem> GetAsks()
        {
            var _result = new List<KOrderBookItem>();

            foreach (var a in asks)
            {
                var o = new KOrderBookItem()
                {
                    price = a[0].Value<decimal>(),
                    quantity = a[1].Value<decimal>(),
                    count = a[2].Value<decimal>()
                };

                _result.Add(o);
            }

            return _result;
        }
    }
}