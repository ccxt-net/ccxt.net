using OdinSdk.BaseLib.Configuration;
using System.Collections.Generic;

namespace OdinSdk.BaseLib.Coin.Public
{
    /// <summary>
    ///
    /// </summary>
    public interface ITickerItem
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
        /// highest price for last 24H
        /// </summary>
        decimal highPrice
        {
            get;
            set;
        }

        /// <summary>
        /// lowest price for last 24H
        /// </summary>
        decimal lowPrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best bid (buy) price
        /// </summary>
        decimal bidPrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best bid (buy) amount (may be missing or undefined)
        /// </summary>
        decimal bidQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// current best ask (sell) price
        /// </summary>
        decimal askPrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best ask (sell) amount (may be missing or undefined)
        /// </summary>
        decimal askQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// volume weighted average price
        /// </summary>
        decimal vwap
        {
            get;
            set;
        }

        /// <summary>
        /// opening price before 24H
        /// </summary>
        decimal openPrice
        {
            get;
            set;
        }

        /// <summary>
        /// price of last trade (closing price for current period)
        /// </summary>
        decimal closePrice
        {
            get;
            set;
        }

        /// <summary>
        /// same as `close`, duplicated for convenience
        /// </summary>
        decimal lastPrice
        {
            get;
        }

        /// <summary>
        /// closing price for the previous period
        /// </summary>
        decimal prevPrice
        {
            get;
            set;
        }

        /// <summary>
        /// absolute change, `last - open`
        /// </summary>
        decimal changePrice
        {
            get;
        }

        /// <summary>
        /// relative change, `(change/open) * 100`
        /// </summary>
        decimal percentage
        {
            get;
        }

        /// <summary>
        /// average price, `(last + open) / 2`
        /// </summary>
        decimal average
        {
            get;
        }

        /// <summary>
        /// volume of base currency traded for last 24 hours
        /// </summary>
        decimal baseVolume
        {
            get;
            set;
        }

        /// <summary>
        /// volume of quote currency traded for last 24 hours
        /// </summary>
        decimal quoteVolume
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
    public class TickerItem : ITickerItem
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
        /// current best bid (buy) price
        /// </summary>
        public virtual decimal bidPrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best bid (buy) amount (may be missing or undefined)
        /// </summary>
        public virtual decimal bidQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// current best ask (sell) price
        /// </summary>
        public virtual decimal askPrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best ask (sell) amount (may be missing or undefined)
        /// </summary>
        public virtual decimal askQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// volume weighted average price (quoteVolume / baseVolume)
        /// </summary>
        public virtual decimal vwap
        {
            get;
            set;
        }

        /// <summary>
        /// opening price before 24H
        /// </summary>
        public virtual decimal openPrice
        {
            get;
            set;
        }

        /// <summary>
        /// price of last trade (closing price for current period)
        /// </summary>
        public virtual decimal closePrice
        {
            get;
            set;
        }

        /// <summary>
        /// highest price for last 24H
        /// </summary>
        public virtual decimal highPrice
        {
            get;
            set;
        }

        /// <summary>
        /// lowest price for last 24H
        /// </summary>
        public virtual decimal lowPrice
        {
            get;
            set;
        }

        /// <summary>
        /// same as `close`, duplicated for convenience
        /// </summary>
        public virtual decimal lastPrice
        {
            get
            {
                return closePrice;
            }
        }

        /// <summary>
        /// closing price for the previous period
        /// </summary>
        public virtual decimal prevPrice
        {
            get;
            set;
        }

        /// <summary>
        /// absolute change, `last - open`
        /// </summary>
        public virtual decimal changePrice
        {
            get;
            set;
        }

        /// <summary>
        /// relative change, `(changePrice / openPrice) * 100`
        /// </summary>
        public virtual decimal percentage
        {
            get;
            set;
        }

        /// <summary>
        /// average price, `(lastPrice + openPrice) / 2`
        /// </summary>
        public virtual decimal average
        {
            get;
            set;
        }

        /// <summary>
        /// volume of base currency traded for last 24 hours
        /// </summary>
        public virtual decimal baseVolume
        {
            get;
            set;
        }

        /// <summary>
        /// volume of quote currency traded for last 24 hours
        /// </summary>
        public virtual decimal quoteVolume
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
    public interface ITicker : IApiResult<ITickerItem>
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
    public class Ticker : ApiResult<ITickerItem>, ITicker
    {
        /// <summary>
        ///
        /// </summary>
        public Ticker()
        {
            this.result = new TickerItem();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        public Ticker(string base_name, string quote_name)
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
    ///
    /// </summary>
    public interface ITickerItems : IApiResult<List<ITickerItem>>
    {
    }

    /// <summary>
    ///
    /// </summary>
    public class TickerItems : ApiResult<List<ITickerItem>>, ITickerItems
    {
        /// <summary>
        ///
        /// </summary>
        public TickerItems()
        {
            this.result = new List<ITickerItem>();
        }
    }

    /// <summary>
    /// key is marketId
    /// </summary>
    public interface ITickers : IApiResult<List<ITickerItem>>
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
    public class Tickers : ApiResult<List<ITickerItem>>, ITickerItems
    {
        /// <summary>
        ///
        /// </summary>
        public Tickers()
        {
            this.result = new List<ITickerItem>();
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