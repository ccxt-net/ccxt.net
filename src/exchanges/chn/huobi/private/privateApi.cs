using CCXT.NET.Coin;
using CCXT.NET.Coin.Private;
using CCXT.NET.Coin.Types;
using CCXT.NET.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Huobi.Private
{
    /// <summary>
    ///
    /// </summary>
    public class PrivateApi : CCXT.NET.Coin.Private.PrivateApi, IPrivateApi
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

        /// <summary>
        ///
        /// </summary>
        public override XApiClient privateClient
        {
            get
            {
                if (base.privateClient == null)
                    base.privateClient = new HuobiClient("private", __connect_key, __secret_key);

                return base.privateClient;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override CCXT.NET.Coin.Public.PublicApi publicApi
        {
            get
            {
                if (base.publicApi == null)
                    base.publicApi = new CCXT.NET.Huobi.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Withdraw funds
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="address">coin address for send</param>
        /// <param name="tag">Secondary address identifier for coins like XRP,XMR etc.</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<Transfer> CoinWithdraw(string currency_name, string address, string tag, decimal quantity, Dictionary<string, object> args = null)
        {
            var _result = new Transfer();

            var _currency_id = await publicApi.LoadCurrencyId(currency_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("currency", _currency_id.result);      // asset being withdrawn
                    _params.Add("address", address);                    // withdrawal key name, as set up on your account
                    _params.Add("amount", quantity);                // amount to withdraw, including fees
                    if (tag != null || tag != "")
                        _params.Add("addr-tag", tag);                // only in xrp，xem，bts，steem，eos，xmr

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await privateClient.CallApiPost1Async("/v1/dw/withdraw/api/create", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<HWithdraw>(_json_value.Content);
                    {
                        var _withdraw = new HTransferItem
                        {
                            transferId = _json_data.result.transferId,
                            transactionId = privateClient.GenerateNonceString(16),
                            timestamp = CUnixTime.NowMilli,

                            transactionType = TransactionType.Withdraw,

                            currency = currency_name,
                            toAddress = address,
                            toTag = tag,

                            amount = quantity,
                            fee = 0,

                            confirmations = 0,
                            isCompleted = true
                        };

                        _result.result = _withdraw;
                    }
                }

                _result.SetResult(_json_result);
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return _result;
        }

        /// <summary>
        /// Request withdrawal cancelation
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="transferId">The unique id of the withdrawal request specified</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<Transfer> CancelCoinWithdraw(string currency_name, string transferId, Dictionary<string, object> args = null)
        {
            var _result = new Transfer();

            var _currency_id = await publicApi.LoadCurrencyId(currency_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = privateClient.MergeParamsAndArgs(args);

                var _json_value = await privateClient.CallApiPost1Async($"/v1/dw/withdraw-virtual/{transferId}/cancel", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<HWithdraw>(_json_value.Content);
                    if (_json_data.success == true)
                    {
                        var _transfer = new HTransferItem
                        {
                            transferId = transferId,
                            timestamp = CUnixTime.NowMilli,

                            transactionType = TransactionType.Withdraw,

                            currency = currency_name,
                            toAddress = "",
                            toTag = "",

                            amount = 0.0m,
                            isCompleted = _json_data.success
                        };

                        _result.result = _transfer;
                    }
                    else
                    {
                        _json_result.SetFailure("Unknown reference id");
                    }
                }

                _result.SetResult(_json_result);
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return _result;
        }

        /// <summary>
        /// Get status of recent deposits
        /// Get status of recent withdrawals
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limits">maximum number of items (optional): default 20</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<Transfers> FetchTransfers(string currency_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new Transfers();

            var _currency_id = await publicApi.LoadCurrencyId(currency_name);
            if (_currency_id.success == true)
            {
                var _params = new Dictionary<string, object>();
                {
                    _params.Add("currency", _currency_id.result);
                    _params.Add("type", "deposit");

                    if (args.ContainsKey("account-id"))
                        _params.Add("from", args["account-id"]);

                    _params.Add("size", limits);

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                // TransactionType.Deposit
                {
                    privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                    var _json_value = await privateClient.CallApiGet1Async("/v1/query/deposit-withdraw", _params);
#if DEBUG
                    _result.rawJson += _json_value.Content;
#endif
                    var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _json_data = privateClient.DeserializeObject<HTransfers>(_json_value.Content);
                        {
                            var _deposits = _json_data.result
                                                        .Where(d => d.timestamp >= since)
                                                        .OrderByDescending(d => d.timestamp)
                                                        .Take(limits);

                            foreach (var _d in _deposits)
                            {
                                _d.transactionType = TransactionType.Deposit;
                                _d.currency = currency_name;

                                _result.result.Add(_d);
                            }
                        }
                    }

                    _result.SetResult(_json_result);
                }

                _params.Remove("type");
                _params.Add("type", "withdraw");

                // TransactionType.Withdrawal
                if (_result.success == true)
                {
                    privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                    var _json_value = await privateClient.CallApiGet1Async("/v1/query/deposit-withdraw", _params);
#if DEBUG
                    _result.rawJson += _json_value.Content;
#endif
                    var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _json_data = privateClient.DeserializeObject<HTransfers>(_json_value.Content);
                        {
                            var _withdraws = _json_data.result
                                                       .Where(w => w.timestamp >= since)
                                                       .OrderByDescending(w => w.timestamp)
                                                       .Take(limits);

                            foreach (var _w in _withdraws)
                            {
                                _w.transactionType = TransactionType.Withdraw;
                                _w.currency = currency_name;

                                _result.result.Add(_w);
                            }
                        }
                    }

                    _result.SetResult(_json_result);
                }
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return _result;
        }

        /// <summary>
        /// Get account balance
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<Balances> FetchBalances(Dictionary<string, object> args = null)
        {
            var _result = new Balances();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                if (args.ContainsKey("account-id") && args["account-id"].ToString() != "")
                {
                    privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                    var _params = privateClient.MergeParamsAndArgs(args);

                    var _json_value = await privateClient.CallApiGet1Async($"/v1/account/accounts/{args["account-id"].ToString()}/balance", _params);
#if DEBUG
                    _result.rawJson = _json_value.Content;
#endif
                    var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _json_data = privateClient.DeserializeObject<HBalances>(_json_value.Content);
                        {
                            foreach (var _currency_id in _markets.CurrencyNames)
                            {
                                var _balances = _json_data.result.Where(b => b.currency == _currency_id.Key);
                                foreach (var _balance in _balances)
                                {
                                    _balance.currency = _currency_id.Value;
                                    _balance.total = _balance.free + _balance.used;

                                    _result.result.Add(_balance);
                                }
                            }
                        }
                    }

                    _result.SetResult(_json_result);
                }
                else
                {
                    _result.SetFailure("account-id required. args[account-id]");
                }
            }
            else
            {
                _result.SetResult(_markets);
            }

            return _result;
        }

        /// <summary>
        /// Get account balance
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public async Task<HAccounts> GetAccounts(Dictionary<string, object> args = null)
        {
            var _result = new HAccounts();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = privateClient.MergeParamsAndArgs(args);

                var _json_value = await privateClient.CallApiGet1Async($"/v1/account/accounts", _params);
                //#if DEBUG
                //                _result.rawJson = _json_value.Content;
                //#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _accounts = privateClient.DeserializeObject<HAccounts>(_json_value.Content);

                    if (_accounts.success == true)
                        _result = _accounts;
                }

                _result.SetResult(_json_result);
            }
            else
            {
                _result.SetResult(_markets);
            }

            return _result;
        }
    }
}