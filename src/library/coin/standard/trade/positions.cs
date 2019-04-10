using OdinSdk.BaseLib.Configuration;
using OdinSdk.BaseLib.Coin.Types;
using System.Collections.Generic;

namespace OdinSdk.BaseLib.Coin.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMyPositionItem
    {
        /// <summary>
        /// 
        /// </summary>
        string positionId
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

        /// <summary>
        /// 
        /// </summary>
        string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// buy/sell
        /// </summary>
        SideType sideType
        {
            get;
            set;
        }

        /// <summary>
        /// limit/market
        /// </summary>
        OrderType orderType
        {
            get;
            set;
        }

        /// <summary>
        /// open/closed
        /// </summary>
        OrderStatus orderStatus
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
        decimal liqPrice
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
        /// executedQty
        /// </summary>
        decimal filled
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal cost
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
    public class MyPositionItem : IMyPositionItem
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual string positionId
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

        /// <summary>
        /// 
        /// </summary>
        public virtual string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// buy/sell
        /// </summary>
        public virtual SideType sideType
        {
            get;
            set;
        }

        /// <summary>
        /// limit/market
        /// </summary>
        public virtual OrderType orderType
        {
            get;
            set;
        }

        /// <summary>
        /// open/closed
        /// </summary>
        public virtual OrderStatus orderStatus
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
        public virtual decimal liqPrice
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
        /// executedQty
        /// </summary>
        public virtual decimal filled
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal cost
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
    /// a market Position by Position-id
    /// </summary>
    public interface IMyPosition : IApiResult<IMyPositionItem>
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
    /// a market Position by Position-id
    /// </summary>
    public class MyPosition : ApiResult<IMyPositionItem>, IMyPosition
    {
        /// <summary>
        /// 
        /// </summary>
        public MyPosition()
        {
            this.result = new MyPositionItem();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        public MyPosition(string base_name, string quote_name)
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

    /// <summary>
    /// a market Position list
    /// </summary>
    public interface IMyPositions : IApiResult<List<IMyPositionItem>>
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
    /// a market Position list
    /// </summary>
    public class MyPositions : ApiResult<List<IMyPositionItem>>, IMyPositions
    {
        /// <summary>
        /// 
        /// </summary>
        public MyPositions()
        {
            this.result = new List<IMyPositionItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        public MyPositions(string base_name, string quote_name)
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