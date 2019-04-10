
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System.Collections.Generic;

namespace OdinSdk.BaseLib.Coin.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMyOrderItem
    {
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
        /// 
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
        MakerType makerType
        {
            get;
            set;
        }

        /// <summary>
        /// 
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
        decimal remaining
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

        /// <summary>
        /// 
        /// </summary>
        int count
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
    public class MyOrderItem : IMyOrderItem
    {
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
        public virtual MakerType makerType
        {
            get;
            set;
        }

        /// <summary>
        /// 
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
        public virtual decimal amount
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal filled
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal remaining
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

        /// <summary>
        /// 
        /// </summary>
        public virtual int count
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
    /// a market order by order-id
    /// </summary>
    public interface IMyOrder : IApiResult<IMyOrderItem>
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
    /// a market order by order-id
    /// </summary>
    public class MyOrder : ApiResult<IMyOrderItem>, IMyOrder
    {
        /// <summary>
        /// 
        /// </summary>
        public MyOrder()
        {
            this.result = new MyOrderItem();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        public MyOrder(string base_name, string quote_name)
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
    /// a market order list
    /// </summary>
    public interface IMyOrders : IApiResult<List<IMyOrderItem>>
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
    /// a market order list
    /// </summary>
    public class MyOrders : ApiResult<List<IMyOrderItem>>, IMyOrders
    {
        /// <summary>
        /// 
        /// </summary>
        public MyOrders()
        {
            this.result = new List<IMyOrderItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        public MyOrders(string base_name, string quote_name)
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