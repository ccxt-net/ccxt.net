using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Private;
using OdinSdk.BaseLib.Coin.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Bitflyer.Private
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
                    base.privateClient = new BitflyerClient("private", __connect_key, __secret_key);

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
                    base.publicApi = new CCXT.NET.Bitflyer.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Get Bitcoin/Ethereum Deposit Addresses
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

                var _params = privateClient.MergeParamsAndArgs(args);

                var _json_value = await privateClient.CallApiGet1Async("/v1/me/getaddresses", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _addresses = privateClient.DeserializeObject<List<BAddressItem>>(_json_value.Content);
                    {
                        foreach (var _address in _addresses)
                        {
                            var _market = _markets.GetMarketByBaseId(_address.currency);
                            if (_market != null)
                            {
                                _address.currency = _market.baseName;
                                _result.result.Add(_address);
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
        /// Get Bitcoin/Ether Deposit History
        /// Get Bitcoin/Ether Transaction History
        /// </summary>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limits">You can set the maximum number of transactions you want to get with this parameter</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<Transfers> FetchAllTransfers(string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new Transfers();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                var _timestamp = privateClient.ExchangeInfo.GetTimestamp(timeframe);
                var _timeframe = privateClient.ExchangeInfo.GetTimeframe(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    //var _till_time = CUnixTime.NowMilli;
                    //var _from_time = (since > 0) ? since : _till_time - _timestamp * 1000 * limits;     // 가져올 갯수 만큼 timeframe * limits 간격으로 데이터 양 계산

                    // bitflyer의 before, after는 id
                    //_params.Add("after", _from_time);
                    //_params.Add("before", _till_time);
                    _params.Add("count", limits);

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                // TransactionType.Deposit
                {
                    privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                    var _json_value = await privateClient.CallApiGet1Async("/v1/me/getcoinins", _params);
#if DEBUG
                    _result.rawJson += _json_value.Content;
#endif
                    var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _json_data = privateClient.DeserializeObject<List<BDepositItem>>(_json_value.Content);
                        {
                            var _deposits = _json_data
                                                        .Where(t => t.timestamp >= since)
                                                        .OrderByDescending(t => t.timestamp)
                                                        .Take(limits);

                            foreach (var _d in _deposits)
                            {
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

                    var _json_value = await privateClient.CallApiGet1Async("/v1/me/getcoinouts", _params);
#if DEBUG
                    _result.rawJson += _json_value.Content;
#endif
                    var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                    if (_json_result.success == true)
                    {
                        var _json_data = privateClient.DeserializeObject<List<BWithdrawItem>>(_json_value.Content);
                        {
                            var _withdraws = _json_data
                                                        .Where(t => t.timestamp >= since)
                                                        .OrderByDescending(t => t.timestamp)
                                                        .Take(limits);

                            foreach (var _w in _withdraws)
                            {
                                _result.result.Add(_w);
                            }
                        }
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
        /// Get Account Asset Balance
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

                var _params = privateClient.MergeParamsAndArgs(args);

                var _json_value = await privateClient.CallApiGet1Async("/v1/me/getbalance", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _balances = privateClient.DeserializeObject<List<BBalanceItem>>(_json_value.Content);
                    {
                        foreach (var _balance in _balances)
                        {
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
        /// Get Account Asset Balance
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

                var _json_value = await privateClient.CallApiGet1Async("/v1/me/getbalance", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<List<BBalanceItem>>(_json_value.Content);
                    {
                        foreach (var _currency_id in _markets.CurrencyNames)
                        {
                            var _balances = _json_data.Where(b => b.currency == _currency_id.Key);
                            foreach (var _balance in _balances)
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
    }
}