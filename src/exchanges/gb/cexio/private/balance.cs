using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Private;

namespace CCXT.NET.CEXIO.Private
{
    /// <summary>
    /// 거래소 회원 지갑 정보
    /// </summary>
    public class CBalanceItem : CCXT.NET.Shared.Coin.Private.BalanceItem, IBalanceItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "available")]
        public override decimal free
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "orders")]
        public override decimal used
        {
            get;
            set;
        }
    }
}