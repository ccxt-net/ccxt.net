using OdinSdk.BaseLib.Coin;

namespace CCXT.NET.Bithumb.Private
{
    /// <summary>
    /// bithumb 거래소 회원 지갑 정보
    /// </summary>
    public class BWalletItem
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
    public class BWallet : ApiResult<BWalletItem>
    {
        /// <summary>
        /// 
        /// </summary>
        public BWallet()
        {
            this.result = new BWalletItem();
        }
    }
}