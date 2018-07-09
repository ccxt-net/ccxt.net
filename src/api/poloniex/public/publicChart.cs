using Newtonsoft.Json;
using System;
using CCXT.NET.Configuration;

namespace CCXT.NET.Poloniex.Public
{
    /// <summary>Represents a time frame of a market.</summary>
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

    public interface IPublicChart
    {
        DateTime Time { get; }

        decimal Open { get; }
        decimal Close { get; }

        decimal High { get; }
        decimal Low { get; }

        decimal VolumeBase { get; }
        decimal VolumeQuote { get; }

        decimal WeightedAverage { get; }
    }

    public class PublicChart : IPublicChart
    {
        [JsonProperty("date")]
        private ulong TimeInternal
        {
            set { Time = value.UnixTimeStampToDateTime(); }
        }
        public DateTime Time { get; private set; }

        [JsonProperty("open")]
        public decimal Open { get; private set; }
        [JsonProperty("close")]
        public decimal Close { get; private set; }

        [JsonProperty("high")]
        public decimal High { get; private set; }
        [JsonProperty("low")]
        public decimal Low { get; private set; }

        [JsonProperty("volume")]
        public decimal VolumeBase { get; private set; }
        [JsonProperty("quoteVolume")]
        public decimal VolumeQuote { get; private set; }

        [JsonProperty("weightedAverage")]
        public decimal WeightedAverage { get; private set; }
    }
}