using CCXT.NET.Configuration;
using CCXT.NET.Types;
using Newtonsoft.Json;

namespace CCXT.NET.Poloniex.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITradeOrder
    {
        /// <summary>
        /// 
        /// </summary>
        ulong IdOrder
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
    public class TradeOrder : ITradeOrder
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("orderNumber")]
        public ulong IdOrder
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