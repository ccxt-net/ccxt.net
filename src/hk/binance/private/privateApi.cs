using Newtonsoft.Json.Linq;
using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Coin.Private;
using CCXT.NET.Shared.Coin.Types;
using CCXT.NET.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Binance.Private
{
    /// <summary>
    ///
    /// </summary>
    public class PrivateApi : CCXT.NET.Shared.Coin.Private.PrivateApi, IPrivateApi
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
                    base.privateClient = new BinanceClient("private", __connect_key, __secret_key);

                return base.privateClient;
            }
        }

        private BinanceClient __wapi_client = null;

        /// <summary>
        ///
        /// </summary>
        public BinanceClient priwapiClient
        {
            get
            {
                if (__wapi_client == null)
                    __wapi_client = new BinanceClient("wapi", __connect_key, __secret_key);

                return __wapi_client;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override CCXT.NET.Shared.Coin.Public.PublicApi publicApi
        {
            get
            {
                if (base.publicApi == null)
                    base.publicApi = new CCXT.NET.Binance.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Fetch deposit address.
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Address> FetchAddressAsync(string currency_name, Dictionary<string, object> args = null)
        {
            var _result = new Address();

            var _currency_id = await publicApi.LoadCurrencyIdAsync(currency_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("asset", _currency_id.result);

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await priwapiClient.CallApiGet1Async("/depositAddress.html", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = priwapiClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = priwapiClient.DeserializeObject<BAddressItem>(_json_value.Content);
                    {
                        _json_data.currency = currency_name;
                        _result.result = _json_data;
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
        /// Submit a withdraw request.
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="address">coin address for send</param>
        /// <param name="tag">Secondary address identifier for coins like XRP,XMR etc.</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="args">Add additional attributes for each exchange: name(description of address)</param>
        /// <returns></returns>
        public override async ValueTask<Transfer> CoinWithdrawAsync(string currency_name, string address, string tag, decimal quantity, Dictionary<string, object> args = null)
        {
            var _result = new Transfer();

            var _currency_id = await publicApi.LoadCurrencyIdAsync(currency_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("asset", _currency_id.result);

                    _params.Add("address", address);
                    if (String.IsNullOrEmpty(tag) == false)
                        _params.Add("addressTag", tag);

                    _params.Add("amount", quantity);
                    _params.Add("name", "crypto-coin exchange for .net api address");

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await priwapiClient.CallApiPost1Async($"/withdraw.html", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = priwapiClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = priwapiClient.DeserializeObject<BTransfer>(_json_value.Content);
                    if (_json_data.success == true)
                    {
                        var _withdraw = new BWithdrawItem
                        {
                            transferId = _json_data.transferId,
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
        /// Query Status of BTC Deposit and Transfer
        /// You can query status of BTC deposits and transfers by using the following API
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limits">maximum number of items (optional): default 20</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Transfers> FetchTransfersAsync(string currency_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new Transfers();

            var _currency_id = await publicApi.LoadCurrencyIdAsync(currency_name);
            if (_currency_id.success == true)
            {
                var _timestamp = privateClient.ExchangeInfo.GetTimestamp(timeframe);
                var _timeframe = privateClient.ExchangeInfo.GetTimeframe(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("asset", _currency_id.result);
                    _params.Add("startTime", since);

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                // TransactionType.Deposit
                {
                    privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                    var _json_value = await priwapiClient.CallApiGet1Async("/depositHistory.html", _params);
#if DEBUG
                    _result.rawJson += _json_value.Content;
#endif
                    var _json_result = priwapiClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _json_data = priwapiClient.DeserializeObject<BDeposits>(_json_value.Content);
                        if (_json_data.success == true)
                        {
                            var _deposits = _json_data.result
                                                      .Where(t => t.timestamp >= since)
                                                      .OrderByDescending(t => t.timestamp)
                                                      .Take(limits);

                            foreach (var _d in _deposits)
                            {
                                _d.transferId = _d.timestamp.ToString();                    // transferId 없음
                                //_d.transactionId = (_d.timestamp * 1000).ToString();      // transactionId 있음
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

                    var _json_value = await priwapiClient.CallApiGet1Async("/withdrawHistory.html", _params);
#if DEBUG
                    _result.rawJson += _json_value.Content;
#endif
                    var _json_result = priwapiClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _json_withdraws = priwapiClient.DeserializeObject<BWithdraws>(_json_value.Content);
                        {
                            var _withdraws = _json_withdraws.result
                                                        .Where(t => t.timestamp >= since)
                                                        .OrderByDescending(t => t.timestamp)
                                                        .Take(limits);

                            foreach (var _w in _withdraws)
                            {
                                //_w.transferId = _w.timestamp.ToString();                  // transferId 있음
                                //_w.transactionId = (_w.timestamp * 1000).ToString();      // transactionId 있음
                                _result.result.Add(_w);
                            }
                        }

                        if (_json_withdraws.success == false)
                            _json_result.SetResult(_json_withdraws);
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
        /// Fetch deposit history.
        /// Fetch withdraw history.
        /// </summary>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limits">You can set the maximum number of transactions you want to get with this parameter</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Transfers> FetchAllTransfersAsync(string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new Transfers();

            var _markets = await publicApi.LoadMarketsAsync();
            if (_markets.success == true)
            {
                var _timestamp = privateClient.ExchangeInfo.GetTimestamp(timeframe);
                var _timeframe = privateClient.ExchangeInfo.GetTimeframe(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    var _till_time = CUnixTime.NowMilli;
                    var _from_time = (since > 0) ? since : _till_time - _timestamp * 1000 * limits;     // 가져올 갯수 만큼 timeframe * limits 간격으로 데이터 양 계산

                    _params.Add("startTime", _from_time);
                    _params.Add("endTime", _till_time);

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                // TransactionType.Deposit
                {
                    privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                    var _json_value = await priwapiClient.CallApiGet1Async("/depositHistory.html", _params);
#if DEBUG
                    _result.rawJson += _json_value.Content;
#endif
                    var _json_result = priwapiClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _json_deposits = priwapiClient.DeserializeObject<BDeposits>(_json_value.Content);
                        {
                            var _deposits = _json_deposits.result
                                                        .Where(t => t.timestamp >= since)
                                                        .OrderByDescending(t => t.timestamp)
                                                        .Take(limits);

                            foreach (var _d in _deposits)
                            {
                                _d.transferId = _d.timestamp.ToString();                    // transferId 없음
                                //_d.transactionId = (_d.timestamp * 1000).ToString();      // transactionId 있음
                                _result.result.Add(_d);
                            }
                        }

                        if (_json_deposits.success == false)
                            _json_result.SetResult(_json_deposits);
                    }

                    _result.SetResult(_json_result);
                }

                // TransactionType.Withdrawal
                if (_result.success == true)
                {
                    privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                    var _json_value = await priwapiClient.CallApiGet1Async("/withdrawHistory.html", _params);
#if DEBUG
                    _result.rawJson += _json_value.Content;
#endif
                    var _json_result = priwapiClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _json_withdraws = priwapiClient.DeserializeObject<BWithdraws>(_json_value.Content);
                        {
                            var _withdraws = _json_withdraws.result
                                                        .Where(t => t.timestamp >= since)
                                                        .OrderByDescending(t => t.timestamp)
                                                        .Take(limits);

                            foreach (var _w in _withdraws)
                            {
                                //_w.transactionId = (_w.timestamp * 1000).ToString();      // transactionId 있음
                                //_w.transferId = _w.timestamp.ToString();                  // transferId 있음
                                _result.result.Add(_w);
                            }
                        }

                        if (_json_withdraws.success == false)
                            _json_result.SetResult(_json_withdraws);
                    }

                    _result.SetResult(_json_result);
                }
            }
            else
            {
                _result.SetResult(_markets);
            }

            return _result;
        }

        /// <summary>
        /// Get current account information.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Balance> FetchBalanceAsync(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new Balance();

            var _currency_id = await publicApi.LoadCurrencyIdAsync(base_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = privateClient.MergeParamsAndArgs(args);

                var _json_value = await privateClient.CallApiGet1Async("/account", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<JObject>(_json_value.Content);
                    {
                        var _balances = privateClient.DeserializeObject<List<BBalanceItem>>(_json_data["balances"].ToString());
                        foreach (var _balance in _balances)
                        {
                            if (_balance.currency.ToLower() != _currency_id.result.ToLower())
                                continue;

                            _balance.currency = base_name;
                            _balance.total = _balance.free + _balance.used;

                            _result.result = _balance;
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
        /// Get current account information.
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Balances> FetchBalancesAsync(Dictionary<string, object> args = null)
        {
            var _result = new Balances();

            var _markets = await publicApi.LoadMarketsAsync();
            if (_markets.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = privateClient.MergeParamsAndArgs(args);

                var _json_value = await privateClient.CallApiGet1Async("/account", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<JObject>(_json_value.Content);
                    {
                        var _json_balances = privateClient.DeserializeObject<List<BBalanceItem>>(_json_data["balances"].ToString());

                        foreach (var _currency_id in _markets.CurrencyNames)
                        {
                            var _balances = _json_balances.Where(b => b.currency == _currency_id.Key);

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
                _result.SetResult(_markets);
            }

            return _result;
        }
    }
}