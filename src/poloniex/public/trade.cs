using OdinSdk.BaseLib.Coin.Types;
using Newtonsoft.Json;
using OdinSdk.BaseLib.Configuration;
using System;

namespace CCXT.NET.Poloniex.Public
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPTradeItem
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
        SideType Type
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal PricePerCoin
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal AmountQuote
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal AmountBase
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PTradeItem : IPTradeItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("date")]
        private string TimeInternal
        {
            set
            {
                Time = value.ParseDateTime();
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
        [JsonProperty("type")]
        private string TypeInternal
        {
            set
            {
                Type = SideTypeConverter.FromString(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SideType Type
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("rate")]
        public decimal PricePerCoin
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("amount")]
        public decimal AmountQuote
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("total")]
        public decimal AmountBase
        {
            get; private set;
        }
    }
}