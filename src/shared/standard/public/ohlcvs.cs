using Newtonsoft.Json;
using CCXT.NET.Shared.Configuration;
using System.Collections.Generic;

namespace CCXT.NET.Shared.Coin.Public
{
    /// <summary>
    ///
    /// </summary>
    public interface IOHLCVItem
    {
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
        decimal openPrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        decimal highPrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        decimal lowPrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        decimal closePrice
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
        decimal volume
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        decimal vwap
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        long count
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class OHLCVItem : IOHLCVItem
    {
        /// <summary>
        ///
        /// </summary>
        public OHLCVItem()
        {
            this.count = 1;
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
        public virtual decimal openPrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual decimal highPrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual decimal lowPrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual decimal closePrice
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
        public virtual decimal volume
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual decimal vwap
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual long count
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IOHLCVs : IApiResult<List<IOHLCVItem>>
    {
        /// <summary>
        ///
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
    ///
    /// </summary>
    public class OHLCVs : ApiResult<List<IOHLCVItem>>, IOHLCVs
    {
        /// <summary>
        ///
        /// </summary>
        public OHLCVs()
        {
            this.result = new List<IOHLCVItem>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        public OHLCVs(string base_name, string quote_name)
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