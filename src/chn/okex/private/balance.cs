using OdinSdk.BaseLib.Coin.Private;
using Newtonsoft.Json;

namespace CCXT.NET.OKEx.Private
{
    /// <summary>
    /// 거래소 회원 지갑 정보
    /// </summary>
    public class OBalanceItem : OdinSdk.BaseLib.Coin.Private.BalanceItem, IBalanceItem
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