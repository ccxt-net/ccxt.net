using CCXT.NET.Coin;
using Newtonsoft.Json;

namespace CCXT.NET.Coinone
{
    /// <summary>
    ///
    /// </summary>
    public class CAuthNumber : CCXT.NET.Coin.NameResult, INameResult
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "errorCode")]
        public override int statusCode
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        private string messageValue
        {
            set
            {
                message = value;
                success = message == "success";
            }
        }
    }
}