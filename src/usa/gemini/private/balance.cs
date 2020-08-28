using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Private;

namespace CCXT.NET.Gemini.Private
{
    /// <summary>
    /// 거래소 회원 지갑 정보
    /// </summary>
    public class GBalanceItem : CCXT.NET.Shared.Coin.Private.BalanceItem, IBalanceItem
    {
        /// <summary>
        /// The current balance
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public override decimal total
        {
            get;
            set;
        }

        /// <summary>
        /// The amount that is available to trade
        /// </summary>
        [JsonProperty(PropertyName = "available")]
        public override decimal free
        {
            get;
            set;
        }

        /// <summary>
        /// The amount that is available to withdraw
        /// </summary>
        [JsonProperty(PropertyName = "availableForWithdrawal")]
        public decimal availableForWithdrawal
        {
            get;
            set;
        }

        /// <summary>
        /// Currency code
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        public override string currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string type
        {
            get;
            set;
        }
    }
}