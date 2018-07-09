using CCXT.NET.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CCXT.NET.Poloniex.Private
{
    /// <summary>
    /// https://poloniex.com/
    /// </summary>
    public class PrivateApi
    {
        private string __connect_key;
        private string __secret_key;
        private string __end_point;

        /// <summary>
        /// 
        /// </summary>
        public PrivateApi(string connect_key, string secret_key, string end_point = "tradingApi")
        {
            __connect_key = connect_key;
            __secret_key = secret_key;

            __end_point = end_point;
        }

        private PoloniexClient __user_client = null;

        private PoloniexClient UserClient
        {
            get
            {
                if (__user_client == null)
                    __user_client = new PoloniexClient(__connect_key, __secret_key);
                return __user_client;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, UserBalance>> GetBalances()
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnCompleteBalances");
            }

            return await UserClient.CallApiPostAsync<Dictionary<string, UserBalance>>(__end_point, _params);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetDepositAddresses()
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnDepositAddresses");
            }

            return await UserClient.CallApiPostAsync<Dictionary<string, string>>(__end_point, _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <returns></returns>
        public async Task<IDepositWithdrawal> GetDepositsAndWithdrawals(DateTime start_time, DateTime end_time)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnDepositsWithdrawals");
                _params.Add("start", start_time.DateTimeToUnixTimeStamp());
                _params.Add("end", end_time.DateTimeToUnixTimeStamp());
            }

            return await UserClient.CallApiPostAsync<DepositWithdrawal>(__end_point, _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_name"></param>
        /// <returns></returns>
        public async Task<UserDepositAddress> GenerateNewDepositAddress(string currency_name)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "generateNewAddress");
                _params.Add("currency", currency_name);
            }

            return await UserClient.CallApiPostAsync<UserDepositAddress>(__end_point, _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_name"></param>
        /// <param name="amount"></param>
        /// <param name="address"></param>
        /// <param name="payment_id"></param>
        /// <returns></returns>
        public async Task<UserDepositAddress> Withdrawal(string currency_name, decimal amount, string address, string payment_id = null)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "withdraw");
                _params.Add("currency", currency_name);
                _params.Add("amount", amount);
                _params.Add("address", address);

                if (payment_id != null)
                    _params.Add("paymentId", payment_id);
            }

            return await UserClient.CallApiPostAsync<UserDepositAddress>(__end_point, _params);
        }
    }
}