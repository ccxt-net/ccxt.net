using Newtonsoft.Json;
using System.Collections.Generic;

namespace CCXT.NET.Shared.Coin.Private
{
    /// <summary>
    ///
    /// </summary>
    public interface IBalanceItem
    {
        /// <summary>
        ///
        /// </summary>
        string currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        decimal free
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        decimal used
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        decimal total
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BalanceItem : IBalanceItem
    {
        /// <summary>
        ///
        /// </summary>
        public virtual string currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual decimal free
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual decimal used
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual decimal total
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IBalance : IApiResult<IBalanceItem>
    {
#if DEBUG
        /// <summary>
        ///
        /// </summary>
        string rawJson
        {
            get;
            set;
        }
#endif
    }

    /// <summary>
    ///
    /// </summary>
    public class Balance : ApiResult<IBalanceItem>, IBalance
    {
        /// <summary>
        ///
        /// </summary>
        public Balance()
        {
            this.result = new BalanceItem();
        }
#if DEBUG
        /// <summary>
        ///
        /// </summary>
        [JsonIgnore]
        public virtual string rawJson
        {
            get;
            set;
        }
#endif
    }

    /// <summary>
    ///
    /// </summary>
    public interface IBalances : IApiResult<List<IBalanceItem>>
    {
#if DEBUG

        /// <summary>
        ///
        /// </summary>
        string rawJson
        {
            get;
            set;
        }

#endif
    }

    /// <summary>
    ///
    /// </summary>
    public class Balances : ApiResult<List<IBalanceItem>>, IBalances
    {
        /// <summary>
        ///
        /// </summary>
        public Balances()
        {
            this.result = new List<IBalanceItem>();
        }

#if DEBUG

        /// <summary>
        ///
        /// </summary>
        [JsonIgnore]
        public virtual string rawJson
        {
            get;
            set;
        }

#endif
    }
}