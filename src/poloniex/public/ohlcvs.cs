using Newtonsoft.Json;
using OdinSdk.BaseLib.Configuration;
using System;

namespace CCXT.NET.Poloniex.Public
{
    /// <summary>
    /// Represents a time frame of a market.
    /// </summary>
    public enum ChartPeriod : int
    {
        /// <summary>A time interval of 5 minutes.</summary>
        Minutes5 = 300,

        /// <summary>A time interval of 15 minutes.</summary>
        Minutes15 = 900,

        /// <summary>A time interval of 30 minutes.</summary>
        Minutes30 = 1800,

        /// <summary>A time interval of 2 hours.</summary>
        Hours2 = 7200,

        /// <summary>A time interval of 4 hours.</summary>
        Hours4 = 14400,

        /// <summary>A time interval of a day.</summary>
        Day = 86400
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IPOhlcvItem
    {
        /// <summary>
        /// 
        /// </summary>
        DateTime Time
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal Open
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal Close
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal High
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal Low
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal VolumeBase
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal VolumeQuote
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal WeightedAverage
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class POhlcvItem : IPOhlcvItem
    {
        [JsonProperty("date")]
        private ulong TimeInternal
        {
            set
            {
                Time = value.UnixTimeStampToDateTime();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Time
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("open")]
        public decimal Open
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("close")]
        public decimal Close
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("high")]
        public decimal High
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("low")]
        public decimal Low
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("volume")]
        public decimal VolumeBase
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("quoteVolume")]
        public decimal VolumeQuote
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("weightedAverage")]
        public decimal WeightedAverage
        {
            get; private set;
        }
    }
}