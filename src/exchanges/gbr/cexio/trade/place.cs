using CCXT.NET.Coin.Trade;
using CCXT.NET.Coin.Types;
using Newtonsoft.Json;

namespace CCXT.NET.CEXIO.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class CPlaceOrderItem : CCXT.NET.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "time")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        private string sideValue
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// pending amount (if partially executed)
        /// </summary>
        [JsonProperty(PropertyName = "pending")]
        public decimal pending
        {
            set
            {
                filled = quantity - value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override string symbol
        {
            get
            {
                return symbol1 + "/" + symbol2;
            }
            set => base.symbol = value;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "symbol1")]
        private string symbol1
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "symbol2")]
        private string symbol2
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "symbol1Amount")]
        public decimal symbol1Amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "symbol2Amount")]
        public decimal symbol2Amount
        {
            get;
            set;
        }
    }
}