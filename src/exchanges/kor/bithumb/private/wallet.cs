using CCXT.NET.Coin;
using CCXT.NET.Coin.Private;
using Newtonsoft.Json;

namespace CCXT.NET.Bithumb.Private
{
    /// <summary>
    ///
    /// </summary>
    public class BWallet : CCXT.NET.Coin.Private.Wallet, IWallet
    {
        /// <summary>
        ///
        /// </summary>
        public BWallet()
        {
            this.result = new BWalletItem();
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
        [JsonProperty(PropertyName = "data")]
        public new BWalletItem result
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 거래소 회원 지갑 정보
    /// </summary>
    public class BWalletItem : CCXT.NET.Coin.Private.WalletItem, IWalletItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "created")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "account_id")]
        public override string userId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "trade_fee")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originBalance")]
        public override BalanceItem balance
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "balance")]
        private decimal balanceValue
        {
            set
            {
                balance = new BBalanceItem
                {
                    free = value
                };
            }
        }
    }
}