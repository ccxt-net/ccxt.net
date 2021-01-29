using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Trade;
using CCXT.NET.Shared.Configuration;
using System;

namespace CCXT.NET.Bitstamp.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class BMyTradeItem : CCXT.NET.Shared.Coin.Trade.MyTradeItem, IMyTradeItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string tradeId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "order_id")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// Transaction type: 0 - deposit; 1 - withdrawal; 2 - market trade; 14 - sub account transfer.
        /// </summary>
        public int type
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "datetime")]
        private DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }
    }
}