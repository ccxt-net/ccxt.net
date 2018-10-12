using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin;

namespace CCXT.NET.Coinone
{
    /// <summary>
    /// 
    /// </summary>
    public class CAuthNumber : OdinSdk.BaseLib.Coin.NameResult, INameResult
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