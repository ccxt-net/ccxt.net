using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Trade;
using CCXT.NET.Shared.Coin.Types;

namespace CCXT.NET.CEXIO.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class CMyTradeItem : CCXT.NET.Shared.Coin.Trade.MyTradeItem, IMyTradeItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "")]
        public override string tradeId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "")]
        public override decimal amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "")]
        private bool sideValue
        {
            set
            {
                sideType = (value) ? SideType.Bid : SideType.Ask;
            }
        }
    }
}