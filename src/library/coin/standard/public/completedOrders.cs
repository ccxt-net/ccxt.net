using CCXT.NET.Coin.Types;
using CCXT.NET.Configuration;
using System.Collections.Generic;

namespace CCXT.NET.Coin.Public
{
    /// <summary>
    ///
    /// </summary>
    public interface ICompleteOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        string transactionId
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
        ///
        /// </summary>
        string datetime
        {
            get;
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
        /// market or limit
        /// </summary>
        OrderType orderType
        {
            get;
            set;
        }

        /// <summary>
        /// sell or buy
        /// </summary>
        SideType sideType
        {
            get;
            set;
        }

        /// <summary>
        /// maker or taker
        /// </summary>
        MakerType makerType
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        FillType fillType
        {
            get;
            set;
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
    public class CompleteOrderItem : ICompleteOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        public virtual string transactionId
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
        public virtual string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// market or limit
        /// </summary>
        public virtual OrderType orderType
        {
            get;
            set;
        }

        /// <summary>
        /// sell or buy
        /// </summary>
        public virtual SideType sideType
        {
            get;
            set;
        }

        /// <summary>
        /// maker or taker
        /// </summary>
        public virtual MakerType makerType
        {
            get;
            set;
        }

        /// <summary>
        /// fill or partial
        /// </summary>
        public virtual FillType fillType
        {
            get;
            set;
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
    public interface ICompleteOrders : IApiResult<List<ICompleteOrderItem>>
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
    public class CompleteOrders : ApiResult<List<ICompleteOrderItem>>, ICompleteOrders
    {
        /// <summary>
        ///
        /// </summary>
        public CompleteOrders()
        {
            this.result = new List<ICompleteOrderItem>();
        }

        /// <summary>
        ///
        /// </summary>
        public CompleteOrders(string base_name, string quote_name)
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