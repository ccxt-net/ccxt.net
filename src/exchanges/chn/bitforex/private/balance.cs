using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Private;
using System.Collections.Generic;

namespace CCXT.NET.Bitforex.Private
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
        [JsonProperty(PropertyName = "data")]
        public new BBalanceItem result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "success")]
        public override bool success
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
        [JsonProperty(PropertyName = "data")]
        public new List<BBalanceItem> result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "success")]
        public override bool success
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BBalanceItem : OdinSdk.BaseLib.Coin.Private.BalanceItem, IBalanceItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        public override string currency
        {
            get;
            set;
        }

        /// <summary>
        /// Available assets
        /// </summary>
        [JsonProperty(PropertyName = "active")]
        public override decimal free
        {
            get;
            set;
        }

        /// <summary>
        /// Freezing assets
        /// </summary>
        [JsonProperty(PropertyName = "frozen")]
        public override decimal used
        {
            get;
            set;
        }

        /// <summary>
        /// Total assets
        /// </summary>
        [JsonProperty(PropertyName = "fix")]
        public override decimal total
        {
            get;
            set;
        }
    }
}