using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Public;

namespace CCXT.NET.Korbit.Public
{
    /// <summary>
    /// 체결 내역 ( List of Filled Orders )
    /// </summary>
    public class KCompleteOrderItem : OdinSdk.BaseLib.Coin.Public.CompleteOrderItem, ICompleteOrderItem
    {
        /// <summary>
        /// Unique ID that identifies the transaction.
        /// </summary>
        [JsonProperty(PropertyName = "tid")]
        public override string transactionId
        {
            get;
            set;
        }

        /// <summary>
        /// Transaction amount in BTC.
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originAmount")]
        public override decimal amount
        {
            get;
            set;
        }
    }
}