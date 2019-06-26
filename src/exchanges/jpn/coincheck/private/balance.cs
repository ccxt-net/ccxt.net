using CCXT.NET.Coin.Private;
using System.Collections.Generic;

namespace CCXT.NET.CoinCheck.Private
{
    /// <summary>
    ///
    /// </summary>
    public class CBalances : CCXT.NET.Coin.Private.Balances, IBalances
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
    public class CBalanceItem : CCXT.NET.Coin.Private.BalanceItem, IBalanceItem
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