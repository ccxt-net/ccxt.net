using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Private;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Zb.Private
{
    /// <summary>
    ///
    /// </summary>
    public class PrivateApi : OdinSdk.BaseLib.Coin.Private.PrivateApi, IPrivateApi
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
                    base.privateClient = new ZbClient("private", __connect_key, __secret_key);

                return base.privateClient;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override OdinSdk.BaseLib.Coin.Public.PublicApi publicApi
        {
            get
            {
                if (base.publicApi == null)
                    base.publicApi = new CCXT.NET.Zb.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Get a bitcoin/altcoin address for deposit. Returns a unique deposit address for your ANX account.
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Address> FetchAddress(string currency_name, Dictionary<string, object> args = null)
        {
            var _result = new Address();

            var _currency_id = await publicApi.LoadCurrencyId(currency_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("currency", _currency_id.result);
                    _params.Add("method", "getUserAddress");

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await privateClient.CallApiGet1Async($"/getUserAddress", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<ZAddress>(_json_value.Content);
                    {
                        var _address = _json_data.result;
                        _address.currency = currency_name;

                        _result.result = _address;
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
        /// Withdraw funds
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="address">coin address for send</param>
        /// <param name="tag">Secondary address identifier for coins like XRP,XMR etc.</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Transfer> CoinWithdraw(string currency_name, string address, string tag, decimal quantity, Dictionary<string, object> args = null)
        {
            var _result = new Transfer();

            var _currency_id = await publicApi.LoadCurrencyId(currency_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("amount", quantity);                // Withdrawal amount(maximum accurates to eight decimal places)
                    _params.Add("currency", _currency_id.result);      // asset being withdrawn
                    _params.Add("receiveAddr", address);                    // Deposit address (Must be a verified address, BTS should in the form of "account _ memo".
                    _params.Add("method", "withdraw");
                    //if (tag != null || tag != "")
                    //    _params.Add("addr-tag", tag);                // only in xrp，xem，bts，steem，eos，xmr

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await privateClient.CallApiGet1Async("/withdraw", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<ZWithdraw>(_json_value.Content);
                    {
                        var _now = CUnixTime.NowMilli;

                        var _withdraw = new ZWithdrawItem
                        {
                            transferId = _now.ToString(),                 // transferId 없음
                            transactionId = (_now * 1000).ToString(),      // transactionId 없음

                            timestamp = _now,

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
        /// Get status of recent deposits
        /// Get status of recent withdrawals
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limits">maximum number of items (optional): default 20</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Transfers> FetchTransfers(string currency_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new Transfers();

            var _currency_id = await publicApi.LoadCurrencyId(currency_name);
            if (_currency_id.success == true)
            {
                var _params = new Dictionary<string, object>();
                {
                    _params.Add("currency", _currency_id.result);
                    _params.Add("method", "getChargeRecord");
                    _params.Add("pageIndex", 1);
                    _params.Add("pageSize", 20);

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                // TransactionType.Deposit
                {
                    privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                    var _json_value = await privateClient.CallApiGet1Async("/getChargeRecord", _params);
#if RAWJSON
                    _result.rawJson += _json_value.Content;
#endif
                    var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _json_data = privateClient.DeserializeObject<ZDeposits>(_json_value.Content);
                        {
                            var _deposits = _json_data.result
                                                        .Where(d => d.timestamp >= since)
                                                        .OrderByDescending(d => d.timestamp)
                                                        .Take(limits);

                            foreach (var _d in _deposits)
                            {
#if RAWJSON
                                if (String.IsNullOrEmpty(_d.transactionId) == true)
                                    continue;
#endif
                                _d.transactionType = TransactionType.Deposit;
                                _d.currency = currency_name;

                                _result.result.Add(_d);
                            }
                        }
                    }

                    _result.SetResult(_json_result);
                }

                _params.Remove("method");
                _params.Add("method", "getWithdrawRecord");

                // TransactionType.Withdrawal
                if (_result.success == true)
                {
                    privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                    var _json_value = await privateClient.CallApiGet1Async("/getWithdrawRecord", _params);
#if RAWJSON
                    _result.rawJson += _json_value.Content;
#endif
                    var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _json_data = privateClient.DeserializeObject<ZWithdraws>(_json_value.Content);
                        {
                            var _withdraws = _json_data.result
                                                       .Where(w => w.timestamp >= since)
                                                       .OrderByDescending(w => w.timestamp)
                                                       .Take(limits);

                            foreach (var _w in _withdraws)
                            {
                                _w.transactionType = TransactionType.Withdraw;
                                _w.transactionId = (_w.timestamp * 1000).ToString();
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
        public override async ValueTask<Balances> FetchBalances(Dictionary<string, object> args = null)
        {
            var _result = new Balances();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("method", "getAccountInfo");

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await privateClient.CallApiGet1Async($"/getAccountInfo", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<ZBalances>(_json_value.Content);
                    {
                        foreach (var _currency_id in _markets.CurrencyNames)
                        {
                            var _balance = _json_data.result.Where(b => b.key == _currency_id.Key).SingleOrDefault();
                            {
                                var _balanceItem = new ZBalanceItem();
                                if (_balance != null)
                                {
                                    _balance.total = _balance.free + _balance.used;
                                    _balanceItem = _balance;
                                }

                                _balanceItem.currency = _currency_id.Value;

                                _result.result.Add(_balanceItem);
                            }
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