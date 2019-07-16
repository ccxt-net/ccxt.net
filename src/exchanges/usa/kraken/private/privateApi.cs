using CCXT.NET.Coin;
using CCXT.NET.Coin.Private;
using CCXT.NET.Coin.Types;
using CCXT.NET.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Kraken.Private
{
    /// <summary>
    ///
    /// </summary>
    public class PrivateApi : CCXT.NET.Coin.Private.PrivateApi, IPrivateApi
    {
        private readonly string __connect_key;
        private readonly string __secret_key;
        private readonly string __key_name;

        /// <summary>
        ///
        /// </summary>
        public PrivateApi(string connect_key, string secret_key, string key_name)
        {
            __connect_key = connect_key;
            __secret_key = secret_key;
            __key_name = key_name;

            this.depositMethods = new Dictionary<string, string>();
        }

        /// <summary>
        ///
        /// </summary>
        public override XApiClient privateClient
        {
            get
            {
                if (base.privateClient == null)
                    base.privateClient = new KrakenClient("private", __connect_key, __secret_key);

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
                    base.publicApi = new CCXT.NET.Kraken.Public.PublicApi();

                return base.publicApi;
            }
        }

        private Dictionary<string, string> depositMethods
        {
            get;
            set;
        }

        /// <summary>
        /// Get deposit methods
        /// </summary>
        /// <param name="currency_id"></param>
        /// <returns></returns>
        private async Task<NameResult> getDepositMethod(string currency_id)
        {
            var _result = new NameResult();

            if (depositMethods.ContainsKey(currency_id) == false)
            {
                var _params = new Dictionary<string, object>();
                {
                    _params.Add("asset", currency_id);
                }

#if DEBUG
                if (XApiClient.TestXUnitMode != XUnitMode.UseExchangeServer)
                {
                    _result.result = currency_id;
                    depositMethods.Add(currency_id, _result.result);
                }
                else
                {
#endif
                    var _json_value = await privateClient.CallApiPost1Async("/0/private/DepositMethods", _params);
#if DEBUG
                    _result.rawJson = _json_value.Content;
#endif
                    var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _json_data = privateClient.DeserializeObject<JToken>(_json_value.Content);
                        {
                            _result.result = _json_data["result"][0]["method"].Value<string>();

                            depositMethods.Add(currency_id, _result.result);
                        }
                    }

                    _result.SetResult(_json_result);
#if DEBUG
                }
#endif
            }
            else
            {
                _result.result = depositMethods[currency_id];

                _result.SetSuccess();
            }

            return _result;
        }

        /// <summary>
        /// Get deposit addresses
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<Address> CreateAddress(string currency_name, Dictionary<string, object> args = null)
        {
            var _result = new Address();

            var _currency_id = await publicApi.LoadCurrencyId(currency_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _deposit_method = await getDepositMethod(_currency_id.result);
                if (_deposit_method.success == true)
                {
                    var _params = new Dictionary<string, object>();
                    {
                        _params.Add("asset", _currency_id.result);
                        _params.Add("method", _deposit_method.result);
                        _params.Add("new", "true");

                        privateClient.MergeParamsAndArgs(_params, args);
                    }

                    var _json_value = await privateClient.CallApiPost1Async("/0/private/DepositAddresses", _params);
#if DEBUG
                    _result.rawJson = _json_value.Content;
#endif
                    var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _addresses = privateClient.DeserializeObject<KResponse<List<KAddressItem>>>(_json_value.Content);
                        {
                            var _address = _addresses.result.FirstOrDefault();
                            _address.currency = currency_name;

                            _result.result = _address;
                        }
                    }

                    _result.SetResult(_json_result);
                }
                else
                {
                    _result.SetResult(_deposit_method);
                }
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return _result;
        }

        /// <summary>
        /// Get deposit addresses
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<Address> FetchAddress(string currency_name, Dictionary<string, object> args = null)
        {
            var _result = new Address();

            var _currency_id = await publicApi.LoadCurrencyId(currency_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _deposit_method = await getDepositMethod(_currency_id.result);
                if (_deposit_method.success == true)
                {
                    var _params = new Dictionary<string, object>();
                    {
                        _params.Add("asset", _currency_id.result);
                        _params.Add("method", _deposit_method.result);

                        privateClient.MergeParamsAndArgs(_params, args);
                    }

                    var _json_value = await privateClient.CallApiPost1Async("/0/private/DepositAddresses", _params);
#if DEBUG
                    _result.rawJson = _json_value.Content;
#endif
                    var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _addresses = privateClient.DeserializeObject<KResponse<List<KAddressItem>>>(_json_value.Content);
                        {
                            var _address = _addresses.result.OrderBy(a => a.expiretm).FirstOrDefault();
                            _address.currency = currency_name;

                            _result.result = _address;
                        }
                    }

                    _result.SetResult(_json_result);
                }
                else
                {
                    _result.SetResult(_deposit_method);
                }
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return _result;
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
                    _params.Add("asset", _currency_id.result);      // asset being withdrawn
                    _params.Add("key", address);                    // withdrawal key name, as set up on your account
                    _params.Add("amount", quantity);                // amount to withdraw, including fees

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await privateClient.CallApiPost1Async("/0/private/Withdraw", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<KResponse<KTransferItem>>(_json_value.Content);
                    {
                        var _withdraw = new KTransferItem
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

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("asset", _currency_id.result);
                    _params.Add("refid", transferId);

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await privateClient.CallApiPost1Async("/0/private/WithdrawCancel", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<KWithdrawCancel>(_json_value.Content);
                    if (_json_data.result == true)
                    {
                        var _transfer = new KTransferItem
                        {
                            transferId = transferId,
                            timestamp = CUnixTime.NowMilli,

                            transactionType = TransactionType.Withdraw,

                            currency = currency_name,
                            toAddress = "",
                            toTag = "",

                            amount = 0.0m,
                            isCompleted = _json_data.result
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
        /// <param name="limit">maximum number of items (optional): default 20</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<Transfers> FetchTransfers(string currency_name, string timeframe = "1d", long since = 0, int limit = 20, Dictionary<string, object> args = null)
        {
            var _result = new Transfers();

            var _currency_id = await publicApi.LoadCurrencyId(currency_name);
            if (_currency_id.success == true)
            {
                var _deposit_method = await getDepositMethod(_currency_id.result);
                if (_deposit_method.success == true)
                {
                    var _params = new Dictionary<string, object>();
                    {
                        _params.Add("asset", _currency_id.result);
                        _params.Add("method", _deposit_method.result);

                        privateClient.MergeParamsAndArgs(_params, args);
                    }

                    // TransactionType.Deposit
                    {
                        privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                        var _json_value = await privateClient.CallApiPost1Async("/0/private/DepositStatus", _params);
#if DEBUG
                        _result.rawJson += _json_value.Content;
#endif
                        var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                        if (_json_result.success == true)
                        {
                            var _json_data = privateClient.DeserializeObject<KResponse<List<KTransferItem>>>(_json_value.Content);
                            {
                                var _deposits = _json_data.result
                                                            .Where(d => d.timestamp >= since)
                                                            .OrderByDescending(d => d.timestamp)
                                                            .Take(limit);

                                foreach (var _d in _deposits)
                                {
                                    _d.transactionType = TransactionType.Deposit;
                                    _d.currency = currency_name;

                                    //_d.transferId = _d.timestamp.ToString();                      // transferId 있음
                                    //_d.transactionId = (_d.timestamp * 1000).ToString();          // transactionId 있음

                                    _result.result.Add(_d);
                                }
                            }
                        }

                        _result.SetResult(_json_result);
                    }

                    // TransactionType.Withdrawal
                    if (_result.success == true)
                    {
                        privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                        var _json_value = await privateClient.CallApiPost1Async("/0/private/WithdrawStatus", _params);
#if DEBUG
                        _result.rawJson += _json_value.Content;
#endif
                        var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                        if (_json_result.success == true)
                        {
                            var _json_data = privateClient.DeserializeObject<KResponse<List<KTransferItem>>>(_json_value.Content);
                            {
                                var _withdraws = _json_data.result
                                                           .Where(w => w.timestamp >= since)
                                                           .OrderByDescending(w => w.timestamp)
                                                           .Take(limit);

                                foreach (var _w in _withdraws)
                                {
                                    _w.transactionType = TransactionType.Withdraw;
                                    _w.currency = currency_name;

                                    //_w.transferId = _w.timestamp.ToString();                      // transferId 있음
                                    //_w.transactionId = (_w.timestamp * 1000).ToString();          // transactionId 있음

                                    _result.result.Add(_w);
                                }
                            }
                        }

                        _result.SetResult(_json_result);
                    }
                }
                else
                {
                    _result.SetResult(_deposit_method);
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
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = privateClient.MergeParamsAndArgs(args);

                var _json_value = await privateClient.CallApiPost1Async("/0/private/Balance", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _balances = privateClient.DeserializeObject<KResponse<Dictionary<string, decimal>>>(_json_value.Content);
                    {
                        foreach (var _b in _balances.result)
                        {
                            // X-ISO4217-A3 standard currency codes
                            var _baseId = _b.Key;
                            if (_baseId.Substring(0, 1) == "X" || _baseId.Substring(0, 1) == "Z")
                                _baseId = _baseId.Substring(1, 3);

                            var _market = _markets.GetMarketByBaseId(_baseId);
                            var _base_name = _market != null ? _market.baseName : _baseId;

                            _result.result.Add(new BalanceItem
                            {
                                currency = _base_name,
                                free = _b.Value,
                                total = _b.Value
                            });
                        }
                    }
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