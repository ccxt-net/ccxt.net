using System.Collections.Generic;
using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Private;

namespace CCXT.NET.CoinCheck.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class CBalances : OdinSdk.BaseLib.Coin.Private.Balances, IBalances
    {
        /// <summary>
        /// 
        /// </summary>
        public CBalances()
        {
            this.result = new List<CBalanceItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        public new List<CBalanceItem> result
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool success
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CBalanceItem : OdinSdk.BaseLib.Coin.Private.BalanceItem, IBalanceItem
    {
        /// <summary>
        /// 
        /// </summary>
        public override string currency
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override decimal free
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override decimal used
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override decimal total
        {
            get;
            set;
        }
    }
}