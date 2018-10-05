using Newtonsoft.Json;
using System.Collections.Generic;

namespace CCXT.NET.Poloniex.Private
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDepositWithdrawal
    {
        /// <summary>
        /// 
        /// </summary>
        IList<Deposit> Deposits
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        IList<Withdrawal> Withdrawals
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DepositWithdrawal : IDepositWithdrawal
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("deposits")]
        public IList<Deposit> Deposits
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("withdrawals")]
        public IList<Withdrawal> Withdrawals
        {
            get; private set;
        }
    }
}