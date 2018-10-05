using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCXT.NET.Korbit.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class PrivateApi
    {
        private readonly string __connect_key;
        private readonly string __secret_key;
        private string __user_name;
        private string __user_password;

        /// <summary>
        /// 
        /// </summary>
        public PrivateApi(string connect_key, string secret_key, string user_name, string user_password)
        {
            __connect_key = connect_key;
            __secret_key = secret_key;

            __user_name = user_name;
            __user_password = user_password;
        }

        private KorbitClient __user_client = null;

        private KorbitClient UserClient
        {
            get
            {
                if (__user_client == null)
                    __user_client = new KorbitClient(__connect_key, __secret_key, __user_name, __user_password);
                return __user_client;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>Constants</returns>
        public async Task<KUserInfo> UserInfo()
        {
            return await UserClient.CallApiGetAsync<KUserInfo>("/v1/user/info");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task<KDeposit> UserAccounts()
        {
            return await UserClient.CallApiGetAsync<KDeposit>("/v1/user/accounts");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task<KBalances> UserBalances()
        {
            return await UserClient.CallApiGetAsync<KBalances>("/v1/user/balances");
        }

        /// <summary>
        /// Request BTC Withdrawal
        /// </summary>
        /// <param name="currency">A mandatory parameter. Currently only currency=“btc”, which means Bitcoin is supported.</param>
        /// <param name="amount">The amount of BTC to withdraw.</param>
        /// <param name="address">The BTC address to where the BTC is sent.</param>
        /// <param name="fee_priority">Optional parameter to select withdrawal fee. Set “normal” for fee 0.001 or “saver” for 0.0005. If it is not set, “normal” fee is applied. (Starting from 2017-03-17 2pm KST)</param>
        /// <returns></returns>
        public async Task<KWithdraw> Withdrawal(string currency, decimal amount, string address, decimal? fee_priority = null)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency", currency.ToLower());
                _params.Add("amount", amount);
                _params.Add("address", address);

                if (fee_priority != null)
                    _params.Add("fee_priority", fee_priority);
            }

            return await UserClient.CallApiPostAsync<KWithdraw>("/v1/user/coins/out", _params);
        }

        /// <summary>
        /// Cancel BTC Transfer Request
        /// </summary>
        /// <param name="currency">A mandatory parameter. Currently only currency=“btc”, which means Bitcoin is supported.</param>
        /// <param name="id">The unique ID of the BTC withdrawal request.</param>
        /// <returns></returns>
        public async Task<KWithdraw> CancelWithdrawal(string currency, string id)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency", currency.ToLower());
                _params.Add("id", id);
            }

            return await UserClient.CallApiGetAsync<KWithdraw>("/v1/user/coins/cancel", _params);
        }

        /// <summary>
        /// Query Status of BTC Deposit and Transfer
        /// You can query status of BTC deposits and transfers by using the following API
        /// </summary>
        /// <param name="currency">A mandatory parameter. Currently only currency=“btc”, which means Bitcoin is supported.</param>
        /// <param name="id">The unique ID of the BTC withdrawal request. If this parameter is not specified, the API responds with a pending BTC withdrawal request if any.</param>
        /// <returns></returns>
        public async Task<KCoinStatus> DepositsAndWithdrawals(string currency = "btc", string id = "")
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency", currency.ToLower());
                if (String.IsNullOrEmpty(id) == false)
                    _params.Add("id", id);
            }

            return await UserClient.CallApiGetAsync<KCoinStatus>("/v1/user/coins/status", _params);
        }

        /// <summary>
        /// 입출금 내역 조회
        /// </summary>
        /// <param name="currency">입출금 내역을 확인하고자 하는 거래 통화. 현재 KRW, BTC, ETH, ETC, XRP를 지원한다.</param>
        /// <param name="type">입출금의 종류로, 입금(deposit) 또는 출금(withdrawal)으로 파라미터를 설정할 수 있다. 기본값은 입출금(all)로, 입금 및 출금 내역을 모두 조회할 수 있다.</param>
        /// <param name="offset">전체 데이터 중 offset(기본값은 0)번째부터 데이터를 가져오도록 지정할 수 있다</param>
        /// <param name="limit">전체 데이터 중 limit(기본값은 40)개만 가져오도록 지정할 수 있다.</param>
        /// <returns></returns>
        public async Task<List<KTransfer>> Transfers(string currency, string type, int offset = 0, int limit = 40)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency", currency.ToLower());
                _params.Add("type", type);
                _params.Add("offset", offset);
                _params.Add("limit", limit);
            }

            return await UserClient.CallApiGetAsync<List<KTransfer>>("/v1/user/transfers", _params);
        }
    }
}