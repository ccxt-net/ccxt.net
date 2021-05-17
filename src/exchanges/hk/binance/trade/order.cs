using CCXT.NET.Shared.Coin.Trade;
using CCXT.NET.Shared.Coin.Types;
using Newtonsoft.Json;

namespace CCXT.NET.Binance.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class BMyOrderItem : CCXT.NET.Shared.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "orderId")]
        public long orderNumber
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "clientOrderId")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long orderListId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "origQty")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "executedQty")]
        public override decimal filled
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "cummulativeQuoteQty")]
        public override decimal amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string timeInForce
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
        public decimal stopPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal icebergQty
        {
            get;
            set;
        }
        
        /// <summary>
        ///
        /// </summary>
        public long updateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool isWorking
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal origQuoteOrderQty
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "side")]
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
        [JsonProperty(PropertyName = "status")]
        private string statusValue
        {
            set
            {
                orderStatus = OrderStatusConverter.FromString(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        private string orderValue
        {
            set
            {
                orderType = OrderTypeConverter.FromString(value);
            }
        }
    }
}