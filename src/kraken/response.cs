using Newtonsoft.Json.Linq;

namespace CCXT.NET.Kraken
{
    /// <summary>
    /// Response from Kraken API.
    /// </summary>
    /// <typeparam name="T">Type of result.</typeparam>
    public class KResponse<T>
    {
        /// <summary>
        /// Gets or sets errors of a request.
        /// </summary>
        public JArray error
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the result of a request.
        /// </summary>
        public T result
        {
            get;
            set;
        } // Nullable

        /// <summary>
        /// Gets or sets the raw Json result of a request.
        /// </summary>
        public string rawJson
        {
            get;
            set;
        }
    }
}