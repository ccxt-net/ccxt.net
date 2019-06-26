using Newtonsoft.Json;
using CCXT.NET.Coin.Private;
using System.Collections.Generic;

namespace CCXT.NET.ItBit.Private
{
    /// <summary>
    /// 거래소 회원 지갑 정보
    /// </summary>
    public class TWalletItem : CCXT.NET.Coin.Private.WalletItem, IWalletItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "userId")]
        public override string userId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string walletId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public override string walletName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "balances")]
        public List<TBalanceItem> balances
        {
            get;
            set;
        }
    }
}