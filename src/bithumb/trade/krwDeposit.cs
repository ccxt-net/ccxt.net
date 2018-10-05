using OdinSdk.BaseLib.Coin;

namespace CCXT.NET.Bithumb.Trade
{
    /// <summary>
    /// bithumb 회원 btc 출금(회원등급에 따른 BTC 출금)
    /// </summary>
    public class BKrwDeposit : ApiResult
    {
        /// <summary>
        /// 가상계좌번호
        /// </summary>
        public string account
        {
            get;
            set;
        }

        /// <summary>
        /// 신한은행(은행명)
        /// </summary>
        public string bank
        {
            get;
            set;
        }

        /// <summary>
        /// 비티씨코리아닷컴(입금자명)
        /// </summary>
        public string bankuser
        {
            get;
            set;
        }
    }
}