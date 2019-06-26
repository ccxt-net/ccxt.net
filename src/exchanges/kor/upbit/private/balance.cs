using Newtonsoft.Json;
using CCXT.NET.Coin.Private;

namespace CCXT.NET.Upbit.Private
{
    /// <summary>
    /// Upbit 거래소 회원 지갑 정보
    /// </summary>
    public class UBalanceItem : CCXT.NET.Coin.Private.BalanceItem, IBalanceItem
    {
        /// <summary>
        /// 화폐를 의미하는 영문 대문자 코드
        /// </summary>
        public override string currency
        {
            get;
            set;
        }

        /// <summary>
        /// 주문가능 금액/수량
        /// </summary>
        [JsonProperty(PropertyName = "balance")]
        public override decimal free
        {
            get;
            set;
        }

        /// <summary>
        /// 주문 중 묶여있는 금액/수량
        /// </summary>
        [JsonProperty(PropertyName = "locked")]
        public override decimal used
        {
            get;
            set;
        }

        /// <summary>
        /// 매수평균가
        /// </summary>
        [JsonProperty(PropertyName = "avg_krw_buy_price")]
        public decimal averagePrice
        {
            get;
            set;
        }

        /// <summary>
        /// 매수평균가 수정 여부
        /// </summary>
        [JsonProperty(PropertyName = "modified")]
        public bool modified
        {
            get;
            set;
        }
    }
}