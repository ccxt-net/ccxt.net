using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Private;
using OdinSdk.BaseLib.Coin.Public;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using CCXT.NET.Bitfinex.Public;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Bitfinex.Private
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
                    base.privateClient = new BitfinexClient("private", __connect_key, __secret_key);

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
                    base.publicApi = new CCXT.NET.Bitfinex.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Return your deposit address to make a new deposit.
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<Address> CreateAddress(string currency_name, Dictionary<string, object> args = null)
        {
            var _result = new Address();

            var _currency_nick = await publicApi.LoadCurrencyNick(currency_name);
            if (_currency_nick.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("method", _currency_nick.result);
                    _params.Add("wallet_name", "exchange");
                    _params.Add("renew", 1);

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await privateClient.CallApiPost1Async("/v1/deposit/new", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _address = privateClient.DeserializeObject<BAddressItem>(_json_value.Content);
                    {
                        _address.currency = currency_name;
                        if (_address.method == "ripple")
                        {
                            _address.tag = _address.address;
                            _address.address = _address.address_pool;
                        }

                        _result.result = _address;

                        if (_address.success == false)
                            _json_result.SetFailure("could not create address", ErrorCode.TooManyAddress);
                    }
                }

                _result.SetResult(_json_result);
            }
            else
            {
                _result.SetResult(_currency_nick);
            }

            return _result;
        }

        /// <summary>
        /// Return your deposit address to make a new deposit.
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<Address> FetchAddress(string currency_name, Dictionary<string, object> args = null)
        {
            var _result = new Address();

            var _currency_nick = await publicApi.LoadCurrencyNick(currency_name);
            if (_currency_nick.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("method", _currency_nick.result);
                    _params.Add("wallet_name", "exchange");
                    //_params.Add("renew", 0);

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await privateClient.CallApiPost1Async("/v1/deposit/new", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _address = privateClient.DeserializeObject<BAddressItem>(_json_value.Content);
                    {
                        _address.currency = currency_name;
                        if (_address.method == "ripple")
                        {
                            _address.tag = _address.address;
                            _address.address = _address.address_pool;
                        }

                        _result.result = _address;
                        _json_result.success = _address.success;
                    }
                }

                _result.SetResult(_json_result);
            }
            else
            {
                _result.SetResult(_currency_nick);
            }

            return _result;
        }

        /// <summary>
        /// Allow you to request a withdrawal from one of your wallet.
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

            var _currency_nick = await publicApi.LoadCurrencyNick(currency_name);
            if (_currency_nick.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("withdraw_type", _currency_nick.result);
                    _params.Add("walletselected", "exchange");
                    _params.Add("amount", quantity.ToString());
                    _params.Add("address", address);

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await privateClient.CallApiPost1Async("/v1/withdraw", _params);
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
                            isCompleted = _json_data.success,
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
                _result.SetResult(_currency_nick);
            }

            return _result;
        }

        /// <summary>
        /// View your past deposits/withdrawals.
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limits">You can set the maximum number of transactions you want to get with this parameter</param>
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
                    _params.Add("since", since / 1000);
                    _params.Add("limit", limits);

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await privateClient.CallApiPost1Async("/v1/history/movements", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<List<BTransferItem>>(_json_value.Content);
                    {
                        var _transfers = _json_data
                                                .Where(t => t.timestamp >= since)
                                                .OrderByDescending(t => t.timestamp)
                                                .Take(limits);

                        foreach (var _t in _transfers)
                        {
                            if (_t.transactionType == TransactionType.Withdraw)
                            {
                                _t.toAddress = _t.fromAddress;
                                _t.toTag = _t.fromTag;

                                _t.fromAddress = "";
                                _t.fromTag = "";
                            }

                            //_t.transferId = _t.timestamp.ToString();                  // transferId 있음
                            _t.transactionId = (_t.timestamp * 1000).ToString();

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
        /// See your balances
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

                var _json_value = await privateClient.CallApiPost1Async("/v1/balances", _params);
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
                            _balance.free = _balance.total - _balance.used;

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
        /// See your balances
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

                var _json_value = await privateClient.CallApiPost1Async("/v1/balances", _params);
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
                                _balance.free = _balance.total - _balance.used;

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