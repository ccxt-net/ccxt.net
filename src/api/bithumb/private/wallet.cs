namespace CCXT.NET.Bithumb.Private
{
    /// <summary>
    /// bithumb 거래소 회원 지갑 정보
    /// </summary>
    public class WalletItem
    {
        /// <summary>
        /// BTC, ETH, DASH, LTC, ETC, XRP
        /// </summary>
        public string currency
        {
            get;
            set;
        }

        /// <summary>
        /// 전자지갑 Address
        /// </summary>
        public string wallet_address
        {
            get;
            set;
        }
    }

    /// <summary>
    /// bithumb 거래소 회원 지갑 정보
    /// </summary>
    public class Wallet : ApiResult<WalletItem>
    {
        /// <summary>
        /// 
        /// </summary>
        public Wallet()
        {
            this.data = new WalletItem();
        }
    }
}