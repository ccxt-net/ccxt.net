using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CCXT.NET.Korbit.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class PublicOrder
    {
        /// <summary>
        /// 가격
        /// </summary>
        public decimal price;

        /// <summary>
        /// 미체결잔량합계
        /// </summary>
        public decimal quantity;

        /// <summary>
        /// 개별호가건수
        /// </summary>
        public decimal count;
    }

    /// <summary>
    /// 
    /// </summary>
    public class PublicOrderBook
    {
        /// <summary>
        /// Unix timestamp in milliseconds of the last placed order.
        /// </summary>
        public long timestamp;

        /// <summary>
        /// An array containing a list of ask prices. 
        /// Each order has two elements: price and the unfilled amount. 
        /// The third element is deprecated and is always 1.
        /// </summary>
        public JArray bids;

        /// <summary>
        /// An array containing a list of bid prices. 
        /// Each order has two elements: price and the unfilled amount. 
        /// The third element is deprecated and is always 1.
        /// </summary>
        public JArray asks;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<PublicOrder> GetBids()
        {
            var _result = new List<PublicOrder>();

            foreach (var b in bids)
            {
                var o = new PublicOrder()
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
        public List<PublicOrder> GetAsks()
        {
            var _result = new List<PublicOrder>();

            foreach (var a in asks)
            {
                var o = new PublicOrder()
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