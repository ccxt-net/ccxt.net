using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Private;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.BitMEX.Private
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
        public PrivateApi(string connect_key, string secret_key, bool is_live = true)
        {
            __connect_key = connect_key;
            __secret_key = secret_key;

            IsLive = is_live;
        }

        private bool IsLive
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override XApiClient privateClient
        {
            get
            {
                if (base.privateClient == null)
                {
                    var _division = (IsLive == false ? "test." : "") + "private";
                    base.privateClient = new BitmexClient(_division, __connect_key, __secret_key);
                }

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
                    base.publicApi = new CCXT.NET.BitMEX.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Get a deposit address.
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

                var _json_value = await privateClient.CallApiGet1Async("/api/v1/user/depositAddress", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _address = _json_value.Content;
                    {
                        _result.result.currency = currency_name;
                        _result.result.address = _address;
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
        /// Request a withdrawal to an external wallet.
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
                    _params.Add("currency", _currency_id.result);
                    _params.Add("amount", quantity);
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

                var _json_value = await privateClient.CallApiPost1Async("/api/v1/user/requestWithdrawal", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<BTransferItem>(_json_value.Content);
                    if (String.IsNullOrEmpty(_json_data.transferId) == false)
                    {
                        var _withdraw = new BTransferItem
                        {
                            transferId = _json_data.transferId,
                            transactionId = _json_data.transactionId,
                            timestamp = _json_data.timestamp,

                            transactionType = TransactionType.Withdraw,

                            currency = _json_data.currency,
                            toAddress = _json_data.toAddress,
                            toTag = tag,

                            amount = _json_data.amount,
                            fee = _json_data.fee,

                            confirmations = 0,
                            isCompleted = true
                        };

                        _withdraw.account = _json_data.account;
                        _withdraw.transferType = _json_data.transferType;
                        _withdraw.text = _json_data.text;
                        _withdraw.transactTime = _json_data.transactTime;

                        _result.result = _withdraw;
                    }
                    else
                    {
                        _json_result.SetFailure();
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
        /// Get a history of all of your wallet transactions (deposits, withdrawals, PNL).
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

                var _json_value = await privateClient.CallApiGet1Async("/api/v1/user/walletHistory", _params);
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
                            _t.toAddress = String.IsNullOrEmpty(_t.toAddress) == false ? _t.toAddress : "undefined";

                            if (_t.transactionType == TransactionType.Deposit)
                            {
                                _t.fromAddress = _t.toAddress;
                                _t.fromTag = _t.toTag;

                                _t.toAddress = "";
                                _t.toTag = "";
                            }

                            _t.transferType = TransferTypeConverter.FromString(_t.transactStatus);
                            _t.isCompleted = (_t.transferType == TransferType.Done);

                            _t.walletBalance = (_t.walletBalance ?? 0m) * 0.00000001m;
                            _t.marginBalance = (_t.marginBalance ?? 0m) * 0.00000001m;
                            _t.amount = _t.amount * 0.00000001m;
                            _t.fee = _t.fee * 0.00000001m;

                            if (_t.timestamp == 0)
                                _t.timestamp = CUnixTime.NowMilli;

                            //_t.transferId = _t.timestamp.ToString();              // transferId 있음
                            _t.transactionId = (_t.timestamp * 1000).ToString();    // transactionId 없음

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
        /// Get your account's margin status.
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

                var _json_value = await privateClient.CallApiGet1Async("/api/v1/user/margin", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _balance = privateClient.DeserializeObject<BBalanceItem>(_json_value.Content);
                    {
                        if (base_name.ToUpper() == "BTC")
                        {
                            var _multiplier = publicApi.publicClient.ExchangeInfo.GetAmountMultiplier("XBTUSD", 1.0m);

                            _balance.free = _balance.free / _multiplier;
                            _balance.total = _balance.total / _multiplier;
                        }

                        _balance.currency = base_name;
                        _balance.used = _balance.total - _balance.free;

                        _result.result = _balance;
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
        /// Get your account's margin status.
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
                    _params.Add("currency", "all");

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

                var _json_value = await privateClient.CallApiGet1Async("/api/v1/user/margin", _params);
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
                                if (_currency_id.Value == "BTC")
                                {
                                    var _multiplier = publicApi.publicClient.ExchangeInfo.GetAmountMultiplier("XBTUSD", 1.0m);

                                    _balance.free = _balance.free / _multiplier;
                                    _balance.total = _balance.total / _multiplier;
                                }

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
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="channelId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<Trollboxe> SendChatMessage(string message, int channelId, Dictionary<string, object> args = null)
        {
            var _result = new Trollboxe();

            privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);
            {
                var _params = new Dictionary<string, object>();
                {
                    _params.Add("message", message);
                    _params.Add("channelID", channelId);

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

                var _json_value = await privateClient.CallApiPost1Async("/api/v1/chat", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _chatting = privateClient.DeserializeObject<TrollboxItem>(_json_value.Content);
                    {
                        _result.result = _chatting;
                    }
                }

                _result.SetResult(_json_result);
            }

            return _result;
        }
    }
}