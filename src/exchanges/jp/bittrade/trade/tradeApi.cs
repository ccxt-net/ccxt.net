using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Shared.Coin.Trade;

namespace CCXT.NET.JP.bittrade.Trade
{
    /// <summary>
    /// bittrade trade API
    /// </summary>
    public class TradeApi : CCXT.NET.Shared.Coin.Trade.TradeApi, ITradeApi
    {
        private readonly string __connect_key;
        private readonly string __secret_key;
        private readonly string __user_id;
        private readonly string __user_password;

        /// <summary>
        /// Constructor
        /// </summary>
        public TradeApi(string _connect_key, string _secret_key, string _user_id, string _user_password)
        {
            __connect_key = _connect_key;
            __secret_key = _secret_key;
            __user_id = _user_id;
            __user_password = _user_password;
        }
    }
}
