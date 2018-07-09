using Newtonsoft.Json;

namespace CCXT.NET.Poloniex.Private
{
    public interface IUserDepositAddress
    {
        bool IsGenerationSuccessful { get; }

        string Address { get; }
    }

    public class UserDepositAddress : IUserDepositAddress
    {
        [JsonProperty("success")]
        private byte IsGenerationSuccessfulInternal
        {
            set { IsGenerationSuccessful = value == 1; }
        }
        public bool IsGenerationSuccessful { get; private set; }

        [JsonProperty("response")]
        public string Address { get; private set; }
    }
}