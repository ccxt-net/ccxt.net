using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Private;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Bithumb.Private
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
                    base.privateClient = new BithumbClient("private", __connect_key, __secret_key);

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
                    base.publicApi = new CCXT.NET.Bithumb.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// bithumb 거래소 회원 입금 주소
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

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("currency", _currency_id.result); //default : BTC

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

                var _json_value = await privateClient.CallApiPost1Async("/info/wallet_address", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<BAddress>(_json_value.Content);
                    if (_json_data.success == true)
                        _result.result = _json_data.result;
                    else
                        _json_result.SetResult(_json_data);
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
        /// bithumb 회원 Currency 출금
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="address">coin address for send</param>
        /// <param name="tag">Secondary address identifier for coins like XRP,XMR etc.</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="args">Add additional attributes for each exchange: [destination]</param>
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
                    _params.Add("currency", _currency_id.result);
                    _params.Add("units", quantity);
                    _params.Add("address", address);

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

                var _json_value = await privateClient.CallApiPost1Async($"/trade/btc_withdrawal", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<BTransfer>(_json_value.Content);
                    if (_json_data.success == true)
                    {
                        var _withdraw = new BTransferItem
                        {
                            transferId = privateClient.GenerateNonceString(16),
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
                        _json_result.SetResult(_json_data);
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
        /// bithumb 회원 KRW 출금 신청
        /// </summary>
        /// <param name="currency_name">quote coin name</param>
        /// <param name="bank_name">은행코드_은행명</param>
        /// <param name="account">출금계좌번호</param>
        /// <param name="amount">출금 금액</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<Transfer> FiatWithdraw(string currency_name, string bank_name, string account, decimal amount, Dictionary<string, object> args = null)
        {
            var _result = new Transfer();

            var _currency_id = await publicApi.LoadCurrencyId(currency_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("bank", bank_name);
                    _params.Add("account", account);
                    _params.Add("price", amount);

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

                var _json_value = await privateClient.CallApiPost1Async($"/trade/krw_withdrawal", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<BTransfer>(_json_value.Content);
                    if (_json_data.success == true)
                    {
                        var _withdraw = new BTransferItem
                        {
                            transferId = privateClient.GenerateNonceString(16),
                            transactionId = privateClient.GenerateNonceString(16),
                            timestamp = CUnixTime.NowMilli,

                            transactionType = TransactionType.Withdraw,

                            currency = currency_name,
                            toAddress = bank_name,
                            toTag = account,

                            amount = amount,
                            fee = 0,

                            confirmations = 0,
                            isCompleted = _json_data.success
                        };

                        _result.result = _withdraw;
                    }
                    else
                    {
                        _json_result.SetResult(_json_data);
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
        /// 회원 거래 내역
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
                    _params.Add("offset", since);
                    _params.Add("count", limits);
                    _params.Add("searchGb", 0);     // 0 : 전체, 1 : 구매완료, 2 : 판매완료, 3 : 출금중, 4 : 입금, 5 : 출금, 9 : KRW입금중

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

                var _json_value = await privateClient.CallApiPost1Async("/info/user_transactions", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<BTransfers>(_json_value.Content);
                    if (_json_data.success == true)
                    {
                        var _transfers = _json_data.result
                                                    .Where(t => t.timestamp >= since && (t.transactionType == TransactionType.Deposit || t.transactionType == TransactionType.Withdraw))
                                                    .OrderByDescending(t => t.timestamp)
                                                    .Take(limits);

                        foreach (var _t in _transfers)
                        {
                            if (_t.transactionType == TransactionType.Deposit)
                                _t.fromAddress = "undefined";
                            else if (_t.transactionType == TransactionType.Withdraw)
                                _t.toAddress = "undefined";

                            _t.currency = currency_name;

                            _t.transferId = _t.timestamp.ToString();                      // transferId 없음
                            _t.transactionId = (_t.timestamp * 1000).ToString();          // transactionId 없음
                            _t.isCompleted = true;

                            _result.result.Add(_t);
                        }
                    }
                    else
                    {
                        _json_result.SetResult(_json_data);
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
        /// Get information about a wallet in your account.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<Wallet> FetchWallet(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new Wallet();

            var _currency_id = await publicApi.LoadCurrencyId(base_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

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

                var _json_value = await privateClient.CallApiPost1Async("/info/account", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<BWallet>(_json_value.Content);
                    if (_json_data.success == true)
                    {
                        var _balance =_json_data.result.balance;
                        {
                            _balance.currency = base_name;
                            _balance.total = _balance.free + _balance.used;
                        }

                        _result.result = _json_data.result;
                    }
                    else
                    {
                        _json_result.SetResult(_json_data);
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
        /// bithumb 거래소 회원 지갑 정보
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

                var _json_value = await privateClient.CallApiPost1Async("/info/balance", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<BBalance>(_json_value.Content);
                    if (_json_data.success == true)
                    {
                        var _base_id = _currency_id.result;

                        if (_json_data.data.SelectToken($"total_{_base_id}") != null)
                        {
                            var _balance = new BBalanceItem()
                            {
                                currency = base_name,

                                free = _json_data.data[$"available_{_base_id}"].Value<decimal>(),
                                used = _json_data.data[$"in_use_{_base_id}"].Value<decimal>(),
                                total = _json_data.data[$"total_{_base_id}"].Value<decimal>(),
                                
                                misu = _json_data.data[$"misu_{_base_id}"].Value<decimal>(),
                                xcoin_last = _json_data.data[$"xcoin_last"].Value<decimal>()
                            };

                            _result.result = _balance;
                        }
                    }
                    else
                    {
                        _json_result.SetResult(_json_data);
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
        /// bithumb 거래소 회원 지갑 정보
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
                    _params.Add("currency", "ALL");

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

                var _json_value = await privateClient.CallApiPost1Async("/info/balance", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<BBalance>(_json_value.Content);
                    if (_json_data.success == true)
                    {
                        foreach (var _coin_id in _markets.CurrencyNames)
                        {
                            var _base_id = _coin_id.Key.ToLower();
                            if (_json_data.data.SelectToken($"total_{_base_id}") == null)
                                continue;

                            var _balance = new BBalanceItem()
                            {
                                currency = _coin_id.Value,

                                free = _json_data.data[$"available_{_base_id}"].Value<decimal>(),
                                used = _json_data.data[$"in_use_{_base_id}"].Value<decimal>(),
                                total = _json_data.data[$"total_{_base_id}"].Value<decimal>(),

                                misu = _json_data.data[$"misu_{_base_id}"].Value<decimal>(),
                                xcoin_last = (_base_id != "krw") ? _json_data.data[$"xcoin_last_{_base_id}"].Value<decimal>() : 0
                            };

                            _result.result.Add(_balance);
                        }
                    }
                    else
                    {
                        _json_result.SetResult(_json_data);
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