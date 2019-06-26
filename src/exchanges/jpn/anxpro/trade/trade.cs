using Newtonsoft.Json;
using CCXT.NET.Coin.Trade;
using CCXT.NET.Coin.Types;
using System.Collections.Generic;

namespace CCXT.NET.Anxpro.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class AMyTrades : CCXT.NET.Coin.Trade.MyTrades, IMyTrades
    {
        /// <summary>
        ///
        /// </summary>
        public AMyTrades()
        {
            this.result = new List<AMyTradeItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        private string resultValue
        {
            set
            {
                message = value;
                success = message == "success";
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new List<AMyTradeItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class AMyTradeItem : CCXT.NET.Coin.Trade.MyTradeItem, IMyTradeItem
    {
        /// <summary>
        /// Trade id
        /// </summary>
        [JsonProperty(PropertyName = "tradeId")]
        public override string tradeId
        {
            get;
            set;
        }

        /// <summary>
        /// Order id
        /// </summary>
        [JsonProperty(PropertyName = "orderId")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// Time of the trade happened. Unix timestamp in millis
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "ccyPair")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// Amount of traded currency being executed
        /// </summary>
        [JsonProperty(PropertyName = "tradedCurrencyFillAmount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// Amount of settlement currency being executed
        /// </summary>
        [JsonProperty(PropertyName = "settlementCurrencyFillAmount")]
        public override decimal amount
        {
            get;
            set;
        }

        /// <summary>
        /// BUY/SELL
        /// </summary>
        [JsonProperty(PropertyName = "side")]
        private string sideValue
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }
    }
}