using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace CCXT.NET.Coinone.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class CTradeItem
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

        /// <summary>
        /// Timestamp.
        /// </summary>
        public long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CTradeItem()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="price"></param>
        /// <param name="qty"></param>
        /// <param name="timestamp"></param>
        [JsonConstructor]
        public CTradeItem(string price, string qty, string timestamp)
        {
            this.price = decimal.Parse(price, NumberStyles.Float);
            this.qty = Convert.ToDecimal(qty);
            this.timestamp = Convert.ToInt64(timestamp);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CTrades : CApiResult
    {
        /// <summary>
        /// trade array
        /// </summary>
        public List<CTradeItem> completeOrders
        {
            get;
            set;
        }
    }
}