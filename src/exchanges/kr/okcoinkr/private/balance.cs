using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Private;
using System.Collections.Generic;

namespace CCXT.NET.OkCoinKr.Private
{
    /// <summary>
    ///
    /// </summary>
    public class OBalances : CCXT.NET.Shared.Coin.Private.Balances, IBalances
    {
        /// <summary>
        ///
        /// </summary>
        public OBalances()
        {
            this.result = new List<IBalanceItem>();
        }

        /// <summary>
        /// true (성공)
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        private bool resultValue
        {
            set
            {
                success = value;
                message = success ? "success" : "failure";
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "info")]
        public OBalanceInfo info
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class OBalanceInfo
    {
        /// <summary>
        ///
        /// </summary>
        public OBalanceFunds funds
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class OBalanceFunds
    {
        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, decimal> borrow
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, decimal> free
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, decimal> freezed
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class OBalanceItem : CCXT.NET.Shared.Coin.Private.BalanceItem, IBalanceItem
    {
    }
}