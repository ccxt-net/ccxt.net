using Newtonsoft.Json;

namespace CCXT.NET.Poloniex.Private
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserBalance
    {
        decimal QuoteAvailable
        {
            get;
        }

        decimal QuoteOnOrders
        {
            get;
        }

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
        [JsonProperty("available")]
        public decimal QuoteAvailable
        {
            get;
            private set;
        }

        [JsonProperty("onOrders")]
        public decimal QuoteOnOrders
        {
            get;
            private set;
        }

        [JsonProperty("btcValue")]
        public decimal BitcoinValue
        {
            get;
            private set;
        }
    }
}