using Newtonsoft.Json;
using OdinSdk.BaseLib.Configuration;
using System;

namespace CCXT.NET.Poloniex.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITrade : ITradeOrder
    {
        /// <summary>
        /// 
        /// </summary>
        DateTime Time
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Trade : TradeOrder, ITrade
    {
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
    }
}