using Newtonsoft.Json;

namespace CCXT.NET.Poloniex.Private
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserBalance
    {
        /// <summary>
        /// 
        /// </summary>
        decimal QuoteAvailable
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal QuoteOnOrders
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal BitcoinValue
        {
            get;
        }
    }

    /// <summary>
    /// poloniex 거래소 회원 지갑 정보
    /// </summary>
    public class UserBalance : IUserBalance
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("available")]
        public decimal QuoteAvailable
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("onOrders")]
        public decimal QuoteOnOrders
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("btcValue")]
        public decimal BitcoinValue
        {
            get;
            private set;
        }
    }
}