using Newtonsoft.Json;
using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Configuration;
using System;
using System.Collections.Generic;

namespace CCXT.NET.BitMEX.Private
{
    /// <summary>
    ///
    /// </summary>
    public interface ITrollboxes : IApiResult<IList<TrollboxItem>>
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
    public class Trollboxes : ApiResult<IList<TrollboxItem>>, ITrollboxes
    {
        /// <summary>
        ///
        /// </summary>
        public Trollboxes()
        {
            this.result = new List<TrollboxItem>();
        }

#if DEBUG

        /// <summary>
        ///
        /// </summary>
        public string rawJson
        {
            get;
            set;
        }

#endif
    }

    /// <summary>
    ///
    /// </summary>
    public interface ITrollbox : IApiResult<ITrollboxItem>
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
    public class Trollbox : ApiResult<ITrollboxItem>, ITrollbox
    {
        /// <summary>
        ///
        /// </summary>
        public Trollbox()
        {
            this.result = new TrollboxItem();
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
    public interface ITrollboxItem
    {
        /// <summary>
        ///
        /// </summary>
        long id
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        int channelId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string lanaguage
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
        bool fromBot
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string html
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string message
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string user
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        bool isLeader
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class TrollboxItem : ITrollboxItem
    {
        /// <summary>
        ///
        /// </summary>
        public long id
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public int channelId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string lanaguage
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public long timestamp
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        private DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool fromBot
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string html
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string message
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string user
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool isLeader
        {
            get;
            set;
        }
    }
}