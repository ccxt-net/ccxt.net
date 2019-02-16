using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;

namespace CCXT.NET.Gemini.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public class GPlaceOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        /// The order id
        /// </summary>
        [JsonProperty(PropertyName = "order_id")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// An optional client-specified order id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string clientOrderId
        {
            get;
            set;
        }

        /// <summary>
        /// The symbol of the order
        /// </summary>
        [JsonProperty(PropertyName = "symbol")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// Will always be "gemini"
        /// </summary>
        [JsonProperty(PropertyName = "exchange")]
        public string exchange
        {
            get;
            set;
        }

        /// <summary>
        /// The average price at which this order as been executed so far. 0 if the order has not been executed at all.
        /// </summary>
        [JsonProperty(PropertyName = "avg_execution_price")]
        public decimal avg_execution_price
        {
            get;
            set;
        }

        /// <summary>
        /// Either "buy" or "sell".
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
        /// Description of the order: exchange limit, auction-only exchange limit, market buy, market sell, indication-of-interest
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        private string orderValue
        {
            set
            {
                orderType = OrderTypeConverter.FromString(value);
            }
        }

        /// <summary>
        /// The timestamp the order was submitted in milliseconds.
        /// </summary>
        [JsonProperty(PropertyName = "timestampms")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// true if the order is active on the book (has remaining quantity and has not been canceled)
        /// </summary>
        [JsonProperty(PropertyName = "is_live")]
        public bool is_live
        {
            get;
            set;
        }

        /// <summary>
        /// true if the order has been canceled. Note the spelling, "cancelled" instead of "canceled". This is for compatibility reasons.
        /// </summary>
        [JsonProperty(PropertyName = "is_cancelled")]
        public bool is_cancelled
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "is_hidden")]
        public bool is_hidden
        {
            get;
            set;
        }

        /// <summary>
        /// Will always be false.
        /// </summary>
        [JsonProperty(PropertyName = "was_forced")]
        public bool was_forced
        {
            get;
            set;
        }

        /// <summary>
        /// The amount of the order that has been filled.
        /// </summary>
        [JsonProperty(PropertyName = "executed_amount")]
        public override decimal filled
        {
            get;
            set;
        }

        /// <summary>
        /// The amount of the order that has not been filled.
        /// </summary>
        [JsonProperty(PropertyName = "remaining_amount")]
        public override decimal remaining
        {
            get;
            set;
        }

        /// <summary>
        /// An array containing at most one supported order execution option. 
        /// "maker-or-cancel"	, "immediate-or-cancel"	, "auction-only", "indication-of-interest"
        /// </summary>
        [JsonProperty(PropertyName = "options")]
        public JArray options
        {
            get;
            set;
        }

        /// <summary>
        /// The price the order was issued at
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// The originally submitted amount of the order.
        /// </summary>
        [JsonProperty(PropertyName = "original_amount")]
        public override decimal quantity
        {
            get;
            set;
        }
    }
}