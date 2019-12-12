using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Private;
using OdinSdk.BaseLib.Coin.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.ItBit.Private
{
    /// <summary>
    ///
    /// </summary>
    public class PrivateApi : OdinSdk.BaseLib.Coin.Private.PrivateApi, IPrivateApi
    {
        private readonly string __connect_key;
        private readonly string __secret_key;
        private readonly string __user_id;
        private readonly string __wallet_id;

        /// <summary>
        ///
        /// </summary>
        public PrivateApi(string connect_key, string secret_key, string user_id, string wallet_id)
        {
            __connect_key = connect_key;
            __secret_key = secret_key;
            __user_id = user_id;
            __wallet_id = wallet_id;
        }

        /// <summary>
        ///
        /// </summary>
        public override XApiClient privateClient
        {
            get
            {
                if (base.privateClient == null)
                    base.privateClient = new ItbitClient("private", __connect_key, __secret_key);

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
                    base.publicApi = new CCXT.NET.ItBit.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Funding History for the specified wallet.
        /// </summary>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limits">You can set the maximum number of transactions you want to get with this parameter</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Transfers> FetchAllTransfers(string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new Transfers();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = privateClient.MergeParamsAndArgs(args);

                var _json_value = await privateClient.CallApiGet1Async($"/wallets/{__wallet_id}/funding_history", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<JObject>(_json_value.Content);
                    {
                        var _json_history = _json_data["fundingHistory"].Value<JArray>();

                        var _transfers = new List<TTransferItem>();
                        foreach (var _t in _json_history)
                        {
                            if (_t.SelectToken("destinationAddress") == null)
                                continue;

                            var _f = privateClient.DeserializeObject<TTransferItem>(_t.ToString());

                            _f.transferId = _f.timestamp.ToString();                    // transferId 없음
                            //_t.transactionId = (_t.timestamp * 1000).ToString();      // transactionId 있음

                            _transfers.Add(_f);
                        }

                        _result.result.AddRange(
                                    _transfers
                                        .Where(t => t.timestamp >= since)
                                        .OrderByDescending(t => t.timestamp)
                                        .Take(limits)
                                );
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
        /// Get information about all wallets in your account.
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Wallets> FetchWallets(string user_id, Dictionary<string, object> args = null)
        {
            var _result = new Wallets();

            privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);
            {
                var _params = new Dictionary<string, object>();
                {
                    _params.Add("userId", user_id);
                    _params.Add("page", 1);
                    _params.Add("perPage", 50);

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await privateClient.CallApiGet1Async("/wallets", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<List<TWalletItem>>(_json_value.Content);
                    {
                        foreach (var _w in _json_data)
                        {
                            foreach (var _balance in _w.balances)
                            {
                                var _market = publicApi.publicClient.ExchangeInfo.Markets.GetMarketByQuoteId(_balance.currency);
                                if (_market != null)
                                    _balance.currency = _market.quoteName;

                                _balance.used = _balance.total - _balance.free;

                                var _wallet = new WalletItem
                                {
                                    userId = _w.userId,

                                    walletId = _w.walletId,
                                    walletName = _w.walletName,

                                    timestamp = _w.timestamp,
                                    fee = _w.fee,

                                    balance = _balance
                                };

                                _result.result.Add(_wallet);
                            }
                        }
                    }
                }

                _result.SetResult(_json_result);
            }

            return _result;
        }

        /// <summary>
        /// Get the balance information for a specific currency in a wallet.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="args">Add additional attributes for each exchange: [walletId]</param>
        /// <returns></returns>
        public override async ValueTask<Balance> FetchBalance(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new Balance();

            var _currency_id = await publicApi.LoadCurrencyId(base_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _wallet_id = __wallet_id;

                var _params = new Dictionary<string, object>();
                {
                    privateClient.MergeParamsAndArgs(_params, args);

                    if (_params.ContainsKey("walletId") == true)
                    {
                        _wallet_id = _params["walletId"].ToString();
                        _params.Remove("walletId");
                    }
                }

                var _json_value = await privateClient.CallApiGet1Async($"/wallets/{_wallet_id}/balances/{_currency_id.result}", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _balance = privateClient.DeserializeObject<TBalanceItem>(_json_value.Content);
                    {
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
        /// Get wallet information.
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange: [walletId]</param>
        /// <returns></returns>
        public override async ValueTask<Balances> FetchBalances(Dictionary<string, object> args = null)
        {
            var _result = new Balances();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _wallet_id = __wallet_id;

                var _params = new Dictionary<string, object>();
                {
                    privateClient.MergeParamsAndArgs(_params, args);

                    if (_params.ContainsKey("walletId") == true)
                    {
                        _wallet_id = _params["walletId"].ToString();
                        _params.Remove("walletId");
                    }
                }

                var _json_value = await privateClient.CallApiGet1Async($"/wallets/{_wallet_id}", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<TWalletItem>(_json_value.Content);
                    {
                        foreach (var _currency_id in _markets.CurrencyNames)
                        {
                            var _balance = _json_data.balances.Where(b => b.currency == _currency_id.Key).FirstOrDefault();
                            if (_balance == null)
                                continue;

                            _balance.currency = _currency_id.Value;
                            _balance.used = _balance.total - _balance.free;

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