using CCXT.NET.Coin.Private;
using Newtonsoft.Json;

namespace CCXT.NET.ItBit.Private
{
    /// <summary>
    /// 거래소 회원 지갑 정보
    /// </summary>
    public class TBalanceItem : CCXT.NET.Coin.Private.BalanceItem, IBalanceItem
    {
        /// <summary>
        ///
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
        [JsonProperty(PropertyName = "availableBalance")]
        public override decimal free
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "totalBalance")]
        public override decimal total
        {
            get;
            set;
        }
    }
}