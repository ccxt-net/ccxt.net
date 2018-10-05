using OdinSdk.BaseLib.Coin;

namespace CCXT.NET.Bithumb.Private
{
    /// <summary>
    /// bithumb 거래소 회원 지갑 정보
    /// </summary>
    public class BAccountItem
    {
        /// <summary>
        /// 회원가입 일시 Timestamp
        /// </summary>
        public long created
        {
            get;
            set;
        }

        /// <summary>
        /// 회원ID
        /// </summary>
        public string account_id
        {
            get;
            set;
        }

        /// <summary>
        /// 거래 수수료
        /// </summary>
        public decimal trade_fee
        {
            get;
            set;
        }

        /// <summary>
        /// 1Currency 잔액 (BTC, ETH, DASH, LTC, ETC, XRP)
        /// </summary>
        public decimal balance
        {
            get;
            set;
        }
    }

    /// <summary>
    /// bithumb 거래소 회원 지갑 정보
    /// </summary>
    public class BAccount : ApiResult<BAccountItem>
    {
        /// <summary>
        /// 
        /// </summary>
        public BAccount()
        {
            this.result = new BAccountItem();
        }
    }
}