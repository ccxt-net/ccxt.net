using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Configuration;
using Newtonsoft.Json;
using System;

namespace CCXT.NET.Bitstamp.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class BMyTradeItem : OdinSdk.BaseLib.Coin.Trade.MyTradeItem, IMyTradeItem
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