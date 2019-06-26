using Newtonsoft.Json;
using CCXT.NET.Coin.Private;

namespace CCXT.NET.Poloniex.Private
{
    /// <summary>
    /// 거래소 회원 지갑 정보
    /// </summary>
    public class PBalanceItem : CCXT.NET.Coin.Private.BalanceItem, IBalanceItem
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
        [JsonProperty(PropertyName = "onOrders")]
        public override decimal used
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "btcValue")]
        public decimal btcValue
        {
            get;
            set;
        }
    }
}