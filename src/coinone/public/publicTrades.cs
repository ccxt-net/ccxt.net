using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace CCXT.NET.Coinone.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class TradeData
    {
        /// <summary>
        /// KRW price.
        /// </summary>
        public decimal price;

        /// <summary>
        /// BTC quantity.
        /// </summary>
        public decimal qty;

        /// <summary>
        /// Timestamp.
        /// </summary>
        public long timestamp;

        /// <summary>
        /// 
        /// </summary>
        public TradeData()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="price"></param>
        /// <param name="qty"></param>
        /// <param name="timestamp"></param>
        [JsonConstructor]
        public TradeData(string price, string qty, string timestamp)
        {
            this.price = decimal.Parse(price, NumberStyles.Float);
            this.qty = Convert.ToDecimal(qty);
            this.timestamp = Convert.ToInt64(timestamp);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PublicTrades : CApiResult
    {
        /// <summary>
        /// trade array
        /// </summary>
        public List<TradeData> completeOrders;
    }
}