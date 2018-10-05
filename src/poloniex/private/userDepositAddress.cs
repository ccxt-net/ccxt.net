using Newtonsoft.Json;

namespace CCXT.NET.Poloniex.Private
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserDepositAddress
    {
        /// <summary>
        /// 
        /// </summary>
        bool IsGenerationSuccessful
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        string Address
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserDepositAddress : IUserDepositAddress
    {
        [JsonProperty("success")]
        private byte IsGenerationSuccessfulInternal
        {
            set
            {
                IsGenerationSuccessful = value == 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsGenerationSuccessful
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("response")]
        public string Address
        {
            get; private set;
        }
    }
}