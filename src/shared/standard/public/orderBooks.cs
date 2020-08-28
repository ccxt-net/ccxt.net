using Newtonsoft.Json;
using CCXT.NET.Shared.Configuration;
using System.Collections.Generic;

namespace CCXT.NET.Shared.Coin.Public
{
    /// <summary>
    /// interface item of orderbook
    /// </summary>
    public interface IOrderBookItem
    {
        /// <summary>
        /// quantity
        /// </summary>
        decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// unit price
        /// </summary>
        decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// quantity * price
        /// </summary>
        decimal amount
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
    }

    /// <summary>
    /// item of orderbook
    /// </summary>
    public class OrderBookItem : IOrderBookItem
    {
        /// <summary>
        /// quantity
        /// </summary>
        public virtual decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// price
        /// </summary>
        public virtual decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// amount (quantity * price)
        /// </summary>
        public virtual decimal amount
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
    }

    /// <summary>
    /// array of ask and bid order
    /// </summary>
    public interface IOrderBook
    {
        /// <summary>
        /// string symbol of the market ('BTCUSD', 'ETHBTC', ...)
        /// </summary>
        string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// 64-bit Unix Timestamp in milliseconds since Epoch 1 Jan 1970
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
        /// The default nonce is a 32-bit Unix Timestamp in seconds.
        /// </summary>
        long nonce
        {
            get;
            set;
        }

        /// <summary>
        /// buy array
        /// </summary>
        List<OrderBookItem> bids
        {
            get;
            set;
        }

        /// <summary>
        /// sell array
        /// </summary>
        List<OrderBookItem> asks
        {
            get;
            set;
        }
    }

    /// <summary>
    /// array of ask and bid order
    /// </summary>
    public class OrderBook : IOrderBook
    {
        /// <summary>
        /// string symbol of the market ('BTCUSD', 'ETHBTC', ...)
        /// </summary>
        public virtual string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// 64-bit Unix Timestamp in milliseconds since Epoch 1 Jan 1970
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
        /// The default nonce is a 32-bit Unix Timestamp in seconds.
        /// </summary>
        public virtual long nonce
        {
            get;
            set;
        }

        /// <summary>
        /// buy array
        /// </summary>
        public virtual List<OrderBookItem> bids
        {
            get;
            set;
        }

        /// <summary>
        /// sell array
        /// </summary>
        public virtual List<OrderBookItem> asks
        {
            get;
            set;
        }
    }

    /// <summary>
    /// key is marketId
    /// </summary>
    public interface IOrderBooks : IApiResult<IOrderBook>
    {
        /// <summary>
        /// string id of the market ('BTC/USD', 'ETH/BTC', ...)
        /// </summary>
        string marketId
        {
            get;
            set;
        }

#if RAWJSON

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
    /// array of ask and bid order
    /// </summary>
    public class OrderBooks : ApiResult<IOrderBook>, IOrderBooks
    {
        /// <summary>
        ///
        /// </summary>
        public OrderBooks()
        {
            this.result = new OrderBook();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        public OrderBooks(string base_name, string quote_name)
            : this()
        {
            this.marketId = this.MakeMarketId(base_name, quote_name);
        }

        /// <summary>
        /// string id of the market ('BTC/USD', 'ETH/BTC', ...)
        /// </summary>
        public virtual string marketId
        {
            get;
            set;
        }

#if RAWJSON

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