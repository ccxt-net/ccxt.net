using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCXT.NET.Coinone.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class PrivateApi
    {
        private readonly string __connect_key;
        private readonly string __secret_key;

        /// <summary>
        /// 
        /// </summary>
        public PrivateApi(string connect_key, string secret_key)
        {
            __connect_key = connect_key;
            __secret_key = secret_key;
        }

        private CoinOneClient __user_client = null;

        private CoinOneClient UserClient
        {
            get
            {
                if (__user_client == null)
                    __user_client = new CoinOneClient(__connect_key, __secret_key);
                return __user_client;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<CBalances> Balance()
        {
            return await UserClient.CallApiPostAsync<CBalances>("/v2/account/balance");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<CDailyBalances> DailyBalance()
        {
            return await UserClient.CallApiPostAsync<CDailyBalances>("/v2/account/daily_balance");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<CUserInfo> UserInfor()
        {
            return await UserClient.CallApiPostAsync<CUserInfo>("/v2/account/user_info");
        }

        /// <summary>
        /// Transaction_V2 - 2-Factor Authentication
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<CAuthNumber> AuthNumber(string type = "btc")
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("type", type);
            }

            return await UserClient.CallApiPostAsync<CAuthNumber>("/v2/transaction/auth_number/", _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="address"></param>
        /// <param name="auth_number"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public async Task<CAuthNumber> SendCoin(string currency, string address, int auth_number, decimal qty)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency", currency);
                _params.Add("address", address);
                _params.Add("auth_number", auth_number);
                _params.Add("qty", qty);
            }

            return await UserClient.CallApiPostAsync<CAuthNumber>("/v2/transaction/coin/", _params);
        }

        /// <summary>
        /// Coin Transactions History
        /// </summary>
        /// <param name="currency">Currency. Allowed values: btc, eth, etc</param>
        /// <returns></returns>
        public async Task<CTransactions> History(string currency = "btc")
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency", currency);
            }

            return await UserClient.CallApiPostAsync<CTransactions>("/v2/transaction/history/", _params);
        }
    }
}