using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CCXT.NET.Coin;
using CCXT.NET.Coin.Private;

namespace CCXT.NET.Bithumb.Private
{
    /// <summary>
    ///
    /// </summary>
    public class BBalance : CCXT.NET.Coin.Private.Balance, IBalance
    {
        /// <summary>
        ///
        /// </summary>
        public BBalance()
        {
            this.result = new BBalanceItem();
        }

        /// <summary>
        /// 결과 상태 코드 (정상 : 0000, 정상이외 코드는 에러 코드 참조)
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public override int statusCode
        {
            get => base.statusCode;
            set
            {
                base.statusCode = value;

                if (statusCode == 0)
                {
                    message = "success";
                    errorCode = ErrorCode.Success;
                    success = true;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public new BBalanceItem result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public JToken data
        {
            get;
            set;
        }
    }

    /// <summary>
    /// bithumb 거래소 회원 지갑 정보
    /// </summary>
    public class BBalanceItem : CCXT.NET.Coin.Private.BalanceItem, IBalanceItem
    {
        /// <summary>
        ///
        /// </summary>
        public decimal misu
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal xcoin_last
        {
            get;
            set;
        }
    }
}