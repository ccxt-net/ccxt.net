using Newtonsoft.Json;
using CCXT.NET.Coin.Private;

namespace CCXT.NET.OKEx.Private
{
    /// <summary>
    /// 거래소 회원 지갑 정보
    /// </summary>
    public class OBalanceItem : CCXT.NET.Coin.Private.BalanceItem, IBalanceItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "free")]
        public override decimal free
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "freezed")]
        public override decimal used
        {
            get;
            set;
        }
    }
}