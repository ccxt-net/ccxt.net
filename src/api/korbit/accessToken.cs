using OdinSdk.BaseLib.Configuration;

namespace CCXT.NET.Korbit
{
    /// <summary>
    /// ACCESS TOKEN, REFRESH TOKEN
    /// </summary>
    public class AccessToken
    {
        public AccessToken()
        {
            timestamp = CUnixTime.Now;
        }

        public string token_type
        {
            get;
            set;
        }

        public string access_token
        {
            get;
            set;
        }

        public int expires_in
        {
            get;
            set;
        }

        public string refresh_token
        {
            get;
            set;
        }

        public long timestamp
        {
            get;
            set;
        }

        public bool CheckExpired()
        {
            return CUnixTime.Now > (timestamp + expires_in - 10);
        }
    }
}