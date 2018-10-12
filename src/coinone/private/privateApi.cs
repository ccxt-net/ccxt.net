using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Private;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Coinone.Private
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
                    base.privateClient = new CoinoneClient("private", __connect_key, __secret_key);

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
                    base.publicApi = new CCXT.NET.Coinone.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// 
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
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _timestamp = privateClient.ExchangeInfo.GetTimestamp(timeframe);
                var _timeframe = privateClient.ExchangeInfo.GetTimeframe(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("currency", _currency_id.result);

                    if (args != null)
                    {
                        foreach (var _a in args)
                        {
                            if (_params.ContainsKey(_a.Key) == true)
                                _params.Remove(_a.Key);

                            _params.Add(_a.Key, _a.Value);
                        }
                    }
                }

                var _json_value = await privateClient.CallApiPost1Async("/v2/transaction/history/", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<JObject>(_json_value.Content);

                    var _json_trxs = privateClient.DeserializeObject<List<CTransferItem>>(_json_data["transactions"].ToString());
                    {
                        var _transfers = _json_trxs
                                                .Where(t => t.timestamp >= since)
                                                .OrderByDescending(t => t.timestamp)
                                                .Take(limits);

                        foreach (var _t in _transfers)
                        {
                            _t.currency = currency_name;

                            if (_t.transactionType == TransactionType.Deposit || _t.transactionType == TransactionType.Withdraw)
                                _t.isCompleted = true;

                            _t.transferId = _t.timestamp.ToString();                    // transferId 없음
                            //_t.transactionId = (_t.timestamp * 1000).ToString();      // transactionId 있음

                            _result.result.Add(_t);
                        }
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
        /// 
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<Addresses> FetchAddresses(Dictionary<string, object> args = null)
        {
            var _result = new Addresses();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = new Dictionary<string, object>();
                {
                    if (args != null)
                    {
                        foreach (var _a in args)
                        {
                            if (_params.ContainsKey(_a.Key) == true)
                                _params.Remove(_a.Key);

                            _params.Add(_a.Key, _a.Value);
                        }
                    }
                }

                var _json_value = await privateClient.CallApiPost1Async("/v2/account/deposit_address", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<JObject>(_json_value.Content);
                    {
                        var _addresses = privateClient.DeserializeObject<Dictionary<string, string>>(_json_data["walletAddress"].ToString());

                        foreach (var _address in _addresses)
                        {
                            if (String.IsNullOrEmpty(_address.Value) == true)
                                continue;

                            var _market = _markets.GetMarketByBaseId(_address.Key);
                            if (_market != null)
                            {
                                var _tag = _addresses.ContainsKey($"{_address.Key}_tag")
                                         ? _addresses[$"{_address.Key}_tag"]
                                         : "";

                                _result.result.Add(new CAddressItem
                                {
                                    currency = _market.baseName,
                                    address = _address.Value,
                                    tag = _tag
                                });
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

        /// <summary>
        /// 
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
                    _params.Add("address", address);
                    _params.Add("qty", quantity);
                    _params.Add("type", "trade");

                    if (args != null)
                    {
                        //    _params.Add("auth_number", args["auth_number"]);     // 2-Factor Authentication number.
                        //    _params.Add("from_address", args["from_address"]);

                        foreach (var _a in args)
                        {
                            if (_params.ContainsKey(_a.Key) == true)
                                _params.Remove(_a.Key);

                            _params.Add(_a.Key, _a.Value);
                        }
                    }
                }

                var _json_value = await privateClient.CallApiPost1Async($"/v2/transaction/{_currency_id.result}/", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<CTransfer>(_json_value.Content);
                    if (_json_data.success == true)
                    {
                        var _withdraw = new CTransferItem
                        {
                            transferId = privateClient.GenerateNonceString(13),
                            transactionId = privateClient.GenerateNonceString(16),
                            timestamp = CUnixTime.NowMilli,

                            transactionType = TransactionType.Withdraw,

                            currency = currency_name,
                            toAddress = address,
                            toTag = tag,

                            amount = quantity,
                            fee = 0,

                            confirmations = 0,
                            isCompleted = _json_data.success
                        };

                        _result.result = _withdraw;
                    }
                    else
                    {
                        _json_result.SetFailure(_json_data.message);
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
        /// 
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<Balance> FetchBalance(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new Balance();

            var _currency_id = await publicApi.LoadCurrencyId(base_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = new Dictionary<string, object>();
                {
                    if (args != null)
                    {
                        foreach (var _a in args)
                        {
                            if (_params.ContainsKey(_a.Key) == true)
                                _params.Remove(_a.Key);

                            _params.Add(_a.Key, _a.Value);
                        }
                    }
                }

                var _json_value = await privateClient.CallApiPost1Async("/v2/account/balance", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _balances = privateClient.DeserializeObject<Dictionary<string, JToken>>(_json_value.Content);
                    {
                        foreach (var _b in _balances)
                        {
                            if (_b.Value.GetType() != typeof(JObject))
                                continue;

                            var _balance = privateClient.DeserializeObject<CBalanceItem>(_b.Value.ToString());
                            _balance.currency = _b.Key;

                            if (_balance.currency.ToLower() != _currency_id.result.ToLower())
                                continue;

                            _balance.currency = base_name;
                            _balance.used = _balance.total - _balance.free;

                            _result.result= _balance;
                            break;
                        }
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
        /// 
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

                var _params = new Dictionary<string, object>();
                {
                    if (args != null)
                    {
                        foreach (var _a in args)
                        {
                            if (_params.ContainsKey(_a.Key) == true)
                                _params.Remove(_a.Key);

                            _params.Add(_a.Key, _a.Value);
                        }
                    }
                }

                var _json_value = await privateClient.CallApiPost1Async("/v2/account/balance", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _balances = privateClient.DeserializeObject<Dictionary<string, JToken>>(_json_value.Content);
                    {
                        foreach (var _currency_id in _markets.CurrencyNames)
                        {
                            if (_balances.ContainsKey(_currency_id.Key) == false)
                                continue;

                            var _balance = privateClient.DeserializeObject<CBalanceItem>(_balances[_currency_id.Key].ToString());
                            if (_balance != null)
                            {
                                _balance.currency = _currency_id.Value;
                                _balance.used = _balance.total - _balance.free;

                                _result.result.Add(_balance);
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

        /// <summary>
        /// Transaction_V2 - 2-Factor Authentication
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<CAuthNumber> GetAuthNumber(string currency_name, Dictionary<string, object> args = null)
        {
            var _result = new CAuthNumber();

            var _currency_id = await publicApi.LoadCurrencyId(currency_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("type", _currency_id.result);

                    if (args != null)
                    {
                        foreach (var _a in args)
                        {
                            if (_params.ContainsKey(_a.Key) == true)
                                _params.Remove(_a.Key);

                            _params.Add(_a.Key, _a.Value);
                        }
                    }
                }

                var _json_value = await privateClient.CallApiPost1Async("/v2/transaction/auth_number/", _params);

                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                //if (_json_result.success == true)
                {
                    // auth number를 넘겨 받아야 하지만 현재는 핸폰 SMS로 전달 되고 있음
                    var _auth_number = privateClient.GenerateNonceString(16, 6);
                    _result.result = _auth_number;
                }

                _result.SetResult(_json_result);
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return _result;
        }
    }
}