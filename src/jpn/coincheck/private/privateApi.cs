using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Private;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.CoinCheck.Private
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
                    base.privateClient = new CoinCheckClient("private", __connect_key, __secret_key);

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
                    base.publicApi = new CCXT.NET.CoinCheck.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Return your deposit address to make a new deposit.
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

                var _params = privateClient.MergeParamsAndArgs(args);

                var _json_value = await privateClient.CallApiGet1Async("/api/accounts", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _address = privateClient.DeserializeObject<CAddressItem>(_json_value.Content);
                    {
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
        /// 코인 출금
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
                    _params.Add("amount", quantity);
                    _params.Add("address", address);

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await privateClient.CallApiPost1Async("/api/send_money", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _withdraw = privateClient.DeserializeObject<CTransferItem>(_json_value.Content);
                    {
                        _withdraw.timestamp = CUnixTime.NowMilli;
                        _withdraw.transactionId = (_withdraw.timestamp * 1000).ToString();      // transactionId 없음

                        _withdraw.transactionType = TransactionType.Withdrawing;

                        _withdraw.currency = _currency_id.result;
                        _withdraw.toTag = tag;

                        _withdraw.confirmations = 0;
                        _withdraw.isCompleted = false;

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
        /// Used to retrieve your deposit history.
        /// Used to retrieve your withdrawal history.
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
                var _timestamp = privateClient.ExchangeInfo.GetTimestamp(timeframe);
                var _timeframe = privateClient.ExchangeInfo.GetTimeframe(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("currency", _currency_id.result);

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                // TransactionType.Deposit
                {
                    privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                    var _json_value = await privateClient.CallApiGet1Async("/api/deposit_money", _params);
#if RAWJSON
                    _result.rawJson += _json_value.Content;
#endif
                    var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _json_data = privateClient.DeserializeObject<CDeposits>(_json_value.Content);
                        if (_json_data.success == true)
                        {
                            var _deposits = _json_data.result
                                                        .Where(d => d.timestamp >= since)
                                                        .OrderByDescending(d => d.timestamp)
                                                        .Take(limits);

                            foreach (var _d in _deposits)
                            {
                                _d.transactionType = TransactionType.Deposit;

                                //_d.transferId = _d.timestamp.ToString();                  // transferId 있음
                                _d.transactionId = (_d.timestamp * 1000).ToString();      // transactionId 있음
                                _result.result.Add(_d);
                            }
                        }
                        else
                        {
                            _json_result.SetResult(_json_data);
                        }
                    }

                    _result.SetResult(_json_result);
                }

                // TransactionType.Withdrawal
                if (_result.success == true)
                {
                    privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                    var _json_value = await privateClient.CallApiGet1Async("/api/send_money", _params);
#if RAWJSON
                    _result.rawJson += _json_value.Content;
#endif
                    var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _json_data = privateClient.DeserializeObject<CWithdraws>(_json_value.Content);
                        if (_json_data.success == true)
                        {
                            var _withdraws = _json_data.result
                                                        .Where(w => w.timestamp >= since)
                                                        .OrderByDescending(w => w.timestamp)
                                                        .Take(limits);

                            foreach (var _w in _withdraws)
                            {
                                _w.transactionType = TransactionType.Withdraw;

                                //_w.transferId = _w.timestamp.ToString();                  // transferId 있음
                                _w.transactionId = (_w.timestamp * 1000).ToString();      // transactionId 없음
                                _result.result.Add(_w);
                            }
                        }
                        else
                        {
                            _json_result.SetResult(_json_data);
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
        /// 전체 계좌 조회
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

                var _params = privateClient.MergeParamsAndArgs(args);

                var _json_value = await privateClient.CallApiGet1Async("/api/accounts/balance", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<JObject>(_json_value.Content);
                    {
                        foreach (var _currency_id in _markets.CurrencyNames)
                        {
                            var _balance = new CBalanceItem();
                            _balance.currency = _currency_id.Value;

                            if (_json_data["success"].Value<bool>() == true)
                            {
                                if (_json_data.ContainsKey(_currency_id.Key))
                                    _balance.free = _json_data[_currency_id.Key].Value<decimal>();
                                if (_json_data.ContainsKey(_currency_id.Key))
                                    _balance.used = _json_data[$"{_currency_id.Key}_reserved"].Value<decimal>();
                                _balance.total = _balance.free + _balance.used;
                            }

                            _result.result.Add(_balance);
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