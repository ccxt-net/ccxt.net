using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Private;

namespace CCXT.NET.CN.digifinex.Private
{
    /// <summary>
    /// digifinex private API
    /// </summary>
    public class PrivateApi : CCXT.NET.Shared.Coin.Private.PrivateApi, IPrivateApi
    {
        private readonly string __connect_key;
        private readonly string __secret_key;
        private readonly string __user_id;
        private readonly string __user_password;

        /// <summary>
        /// Constructor
        /// </summary>
        public PrivateApi(string _connect_key, string _secret_key, string _user_id, string _user_password)
        {
            __connect_key = _connect_key;
            __secret_key = _secret_key;
            __user_id = _user_id;
            __user_password = _user_password;
        }
    }
}
