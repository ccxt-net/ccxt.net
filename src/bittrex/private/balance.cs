using System.Collections.Generic;
using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Private;

namespace CCXT.NET.Bittrex.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class BBalance : OdinSdk.BaseLib.Coin.Private.Balance, IBalance
    {
        /// <summary>
        /// 
        /// </summary>
        public BBalance()
        {
            this.result = new BBalanceItem();
        }

        /// <summary>
        /// 
        /// </summary>
        public new BBalanceItem result
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BBalances : OdinSdk.BaseLib.Coin.Private.Balances, IBalances
    {
        /// <summary>
        /// 
        /// </summary>
        public BBalances()
        {
            this.result = new List<BBalanceItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        public new List<BBalanceItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 거래소 회원 지갑 정보
    /// </summary>
    public class BBalanceItem : OdinSdk.BaseLib.Coin.Private.BalanceItem, IBalanceItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Currency")]
        public override string currency
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Available")]
        public override decimal free
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Pending")]
        public override decimal used
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Balance")]
        public override decimal total
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "CryptoAddress")]
        public string address
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Requested")]
        public bool requested
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Uuid")]
        public string uuid
        {
            get;
            set;
        }
    }
}