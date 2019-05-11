using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;

namespace CCXT.NET.Gemini.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class GMyTradeItem : OdinSdk.BaseLib.Coin.Trade.MyTradeItem, IMyTradeItem
    {
        /// <summary>
        /// The price that the execution happened at
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// The quantity that was executed
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// The time that the trade happened in milliseconds
        /// </summary>
        [JsonProperty(PropertyName = "timestampms")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// Will be either "Buy" or "Sell", indicating the side of the original order
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
        /// If true, this order was the taker in the trade
        /// </summary>
        [JsonProperty(PropertyName = "aggressor")]
        private bool aggressor
        {
            get;
            set;
        }

        /// <summary>
        /// Currency that the fee was paid in
        /// </summary>
        [JsonProperty(PropertyName = "fee_currency")]
        public string fee_currency
        {
            get;
            set;
        }

        /// <summary>
        /// The amount charged
        /// </summary>
        [JsonProperty(PropertyName = "fee_amount")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// Unique identifier for the trade
        /// </summary>
        [JsonProperty(PropertyName = "tid")]
        public override string tradeId
        {
            get;
            set;
        }

        /// <summary>
        /// The order that this trade executed against
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
        [JsonProperty(PropertyName = "client_order_id")]
        public string client_order_id
        {
            get;
            set;
        }

        /// <summary>
        /// True if the trade was filled at an auction
        /// </summary>
        [JsonProperty(PropertyName = "is_auction_fill")]
        public bool is_auction_fill
        {
            get;
            set;
        }

        /// <summary>
        /// Optional Will only be present if the trade is broken. See Break Types below for more information.
        /// </summary>
        [JsonProperty(PropertyName = "break")]
        public string breakType
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
    }
}