using CCXT.NET.Coin.Types;
using CCXT.NET.Configuration;
using System.Collections.Generic;

namespace CCXT.NET.Coin.Trade
{
    /// <summary>
    ///
    /// </summary>
    public interface IMyTradeItem
    {
        /// <summary>
        ///
        /// </summary>
        string tradeId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// bid or ask
        /// </summary>
        SideType sideType
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        OrderType orderType
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// ISO 8601 datetime string with milliseconds
        /// </summary>
        string datetime
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        decimal price
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        decimal amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        decimal fee
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string orderId
        {
            get;
            set;
        }

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
    public class MyTradeItem : IMyTradeItem
    {
        /// <summary>
        ///
        /// </summary>
        public virtual string tradeId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual string symbol
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual SideType sideType
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual OrderType orderType
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// ISO 8601 datetime string with milliseconds
        /// </summary>
        public virtual string datetime
        {
            get
            {
                return CUnixTime.ConvertToUtcTimeMilli(timestamp).ToString("o");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual decimal price
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual decimal amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual decimal fee
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual string orderId
        {
            get;
            set;
        }

#if DEBUG

        /// <summary>
        ///
        /// </summary>
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
    public interface IMyTrades : IApiResult<List<IMyTradeItem>>
    {
        /// <summary>
        ///
        /// </summary>
        string marketId
        {
            get;
            set;
        }

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
    public class MyTrades : ApiResult<List<IMyTradeItem>>, IMyTrades
    {
        /// <summary>
        ///
        /// </summary>
        public MyTrades()
        {
            this.result = new List<IMyTradeItem>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        public MyTrades(string base_name, string quote_name)
                : this()
        {
            this.marketId = this.MakeMarketId(base_name, quote_name);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual string marketId
        {
            get;
            set;
        }

#if DEBUG

        /// <summary>
        ///
        /// </summary>
        public virtual string rawJson
        {
            get;
            set;
        }

#endif
    }
}