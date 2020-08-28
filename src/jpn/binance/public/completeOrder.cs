using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Public;

namespace CCXT.NET.Binance.Public
{
    /// <summary>
    /// recent trade data
    /// </summary>
    public class BCompleteOrderItem : CCXT.NET.Shared.Coin.Public.CompleteOrderItem, ICompleteOrderItem
    {
        /// <summary>
        /// Aggregate tradeId
        /// </summary>
        [JsonProperty(PropertyName = "a")]
        public override string transactionId
        {
            get;
            set;
        }

        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonProperty(PropertyName = "T")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// Quantity
        /// </summary>
        [JsonProperty(PropertyName = "q")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// Price
        /// </summary>
        [JsonProperty(PropertyName = "p")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// First tradeId
        /// </summary>
        [JsonProperty(PropertyName = "f")]
        public long firstTradeId
        {
            get;
            set;
        }

        /// <summary>
        /// Last tradeId
        /// </summary>
        [JsonProperty(PropertyName = "l")]
        public long lastTradeId
        {
            get;
            set;
        }

        /// <summary>
        /// Was the buyer the maker?
        /// </summary>
        [JsonProperty(PropertyName = "m")]
        public bool isBuyerMaker
        {
            get;
            set;
        }

        /// <summary>
        /// Was the trade the best price match?
        /// </summary>
        [JsonProperty(PropertyName = "M")]
        public bool isBestMatch
        {
            get;
            set;
        }
    }
}