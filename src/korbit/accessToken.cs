using OdinSdk.BaseLib.Configuration;

namespace CCXT.NET.Korbit
{
    /// <summary>
    /// ACCESS TOKEN, REFRESH TOKEN
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// 
        /// </summary>
        public AccessToken()
        {
            timestamp = CUnixTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        public string token_type
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string access_token
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int expires_in
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string refresh_token
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
        /// 
        /// </summary>
        public bool CheckExpired()
        {
            return CUnixTime.Now > (timestamp + expires_in - 10);
        }
    }
}