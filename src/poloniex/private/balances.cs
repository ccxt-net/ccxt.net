using Newtonsoft.Json;

namespace CCXT.NET.Poloniex.Private
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPBalanceItem
    {
        /// <summary>
        /// 
        /// </summary>
        decimal QuoteAvailable
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal QuoteOnOrders
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal BitcoinValue
        {
            get; set;
        }
    }

    /// <summary>
    /// poloniex 거래소 회원 지갑 정보
    /// </summary>
    public class PBalanceItem : IPBalanceItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("available")]
        public decimal QuoteAvailable
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("onOrders")]
        public decimal QuoteOnOrders
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("btcValue")]
        public decimal BitcoinValue
        {
            get; set;
        }
    }
}