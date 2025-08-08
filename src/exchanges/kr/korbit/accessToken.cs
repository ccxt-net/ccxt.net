using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Configuration;
using Newtonsoft.Json;

namespace CCXT.NET.Korbit
{
    /// <summary>
    /// access token, refresh token
    /// </summary>
    public class AccessToken : ApiResult
    {
        /// <summary>
        ///
        /// </summary>
        public AccessToken()
        {
            timestamp = CUnixTime.NowMilli;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "token_type")]
        public string tokenType
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "access_token")]
        public string accessToken
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "expires_in")]
        public int expiresIn
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "refresh_token")]
        public string refreshToken
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// ISO 8601 datetime string with milliseconds
        /// </summary>
        public string datetime
        {
            get
            {
                return CUnixTime.ConvertToUtcTimeMilli(timestamp).ToString("o");
            }
        }

        /// <summary>
        /// 10분 전 갱신
        /// </summary>
        public bool CheckExpired()
        {
            return CUnixTime.Now >= (timestamp / 1000 + expiresIn - (60 * 10));
        }
    }
}