using CCXT.NET.Coin.Private;
using Newtonsoft.Json;

namespace CCXT.NET.Quoinex.Private
{
    /// <summary>
    /// 거래소 회원 지갑 정보
    /// </summary>
    public class QBalanceItem : CCXT.NET.Coin.Private.BalanceItem, IBalanceItem
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
        [JsonProperty(PropertyName = "balance")]
        public override decimal free
        {
            get;
            set;
        }
    }
}