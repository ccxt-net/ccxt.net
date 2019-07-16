using CCXT.NET.Coin;
using CCXT.NET.Coin.Private;
using CCXT.NET.Coin.Types;
using CCXT.NET.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCXT.NET.Bitstamp.Private
{
    /// <summary>
    ///
    /// </summary>
    public class PrivateApi : CCXT.NET.Coin.Private.PrivateApi, IPrivateApi
    {
        private readonly string __connect_key;
        private readonly string __secret_key;
        private readonly string __user_name;
        private readonly string __user_passsword;

        /// <summary>
        ///
        /// </summary>
        public PrivateApi(string connect_key, string secret_key, string user_name, string user_password)
        {
            __connect_key = connect_key;
            __secret_key = secret_key;
            __user_name = user_name;
            __user_passsword = user_password;
        }

        /// <summary>
        ///
        /// </summary>
        public override XApiClient privateClient
        {
            get
            {
                if (base.privateClient == null)
                    base.privateClient = new BitstampClient("private", __connect_key, __secret_key, __user_name, __user_passsword);

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
                    base.publicApi = new CCXT.NET.Bitstamp.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// DEPOSIT ADDRESS
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

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _end_point = (currency_name == "BTC") ? "/bitcoin_deposit_address/" : $"/v2/{_currency_id.result}_address/";
                var _json_value = await privateClient.CallApiPost1Async(_end_point, _params);
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
        /// WITHDRAWAL
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="address">coin address for send</param>
        /// <param name="tag">Secondary address identifier for coins like XRP,XMR etc.</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="args">Add additional attributes for each exchange: [destination_tag]</param>
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
                    if (String.IsNullOrEmpty(tag) == false)
                        _params.Add("destination_tag", tag);
                    _params.Add("amount", quantity);

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _end_point = (currency_name == "BTC") ? "/bitcoin_withdrawal/" : $"/v2/{_currency_id.result}_withdrawal/";
                var _json_value = await privateClient.CallApiPost1Async(_end_point, _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<BWithdrawItem>(_json_value.Content);
                    if (String.IsNullOrEmpty(_json_data.transferId) == false)
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
                            isCompleted = true
                        };

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
        /// Funding History for the specified wallet.
        /// </summary>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limit">You can set the maximum number of transactions you want to get with this parameter</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<Transfers> FetchAllTransfers(string timeframe = "1d", long since = 0, int limit = 20, Dictionary<string, object> args = null)
        {
            var _result = new Transfers();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("offset", 0);
                    _params.Add("limit", limit);
                    _params.Add("sort", "desc");

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await privateClient.CallApiPost1Async("/v2/user_transactions/", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<List<JObject>>(_json_value.Content);
                    foreach (var _t in _json_data)
                    {
                        var _type = _t["type"].Value<int>();
                        if (_type != 0 && _type != 1)
                            continue;

                        var _currency = "";
                        var _amount = 0.0m;

                        foreach (var _c in _markets.CurrencyNames)
                        {
                            var _token = _t.SelectToken(_c.Key);
                            if (_token == null)
                                continue;

                            _amount = _token.Value<decimal>();
                            if (_amount != 0.0m)
                            {
                                _currency = _c.Value;
                                break;
                            }
                        }

                        if (String.IsNullOrEmpty(_currency) == true)
                            continue;

                        var _transaction_id = _t["id"].Value<string>();
                        var _transfer_id = _t["order_id"].Value<string>();

                        var _transaction_type = (_type == 0) ? TransactionType.Deposit : TransactionType.Withdraw;
                        var _to_address = "";
                        var _from_address = "";

                        if (_type == 0)
                        {
                            _transaction_type = TransactionType.Deposit;
                            _from_address = "undefined";
                        }
                        else
                        {
                            _transaction_type = TransactionType.Withdraw;
                            _to_address = "undefined";
                        }

                        var _fee = _t["fee"].Value<decimal>();
                        var _timestamp = CUnixTime.ConvertToUnixTimeMilli(_t["datetime"].Value<string>());

                        _result.result.Add(new BTransferItem
                        {
                            transactionId = _transaction_id,
                            transferId = _transfer_id,

                            transactionType = _transaction_type,
                            transferType = TransferType.Done,

                            fromAddress = _from_address,
                            toAddress = _to_address,

                            currency = _currency,
                            amount = _amount,
                            fee = _fee,
                            timestamp = _timestamp,

                            isCompleted = true
                        });
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
        /// ACCOUNT BALANCE
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<Balance> FetchBalance(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new Balance();

            var _market = await publicApi.LoadMarket(_result.MakeMarketId(base_name, quote_name));
            if (_market.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = privateClient.MergeParamsAndArgs(args);

                var _json_value = await privateClient.CallApiPost1Async($"/v2/balance/{_market.result.symbol}/", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _balances = privateClient.DeserializeObject<JToken>(_json_value.Content);
                    {
                        var _base_id = _market.result.baseId;
                        if (_balances.SelectToken($"{_base_id}_available") != null)
                        {
                            var _balance = new BBalanceItem
                            {
                                free = _balances[$"{_base_id}_available"].Value<decimal>(),
                                used = _balances[$"{_base_id}_reserved"].Value<decimal>()
                            };

                            _balance.currency = base_name;
                            _balance.total = _balance.free + _balance.used;

                            _result.result = _balance;
                        }
                    }
                }

                _result.SetResult(_json_result);
            }
            else
            {
                _result.SetResult(_market);
            }

            return _result;
        }

        /// <summary>
        /// ACCOUNT BALANCES
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

                var _json_value = await privateClient.CallApiPost1Async("/v2/balance/", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _balances = privateClient.DeserializeObject<JToken>(_json_value.Content);
                    {
                        foreach (var _currency_id in _markets.CurrencyNames)
                        {
                            if (_balances.SelectToken($"{_currency_id.Key}_available") != null)
                            {
                                var _balance = new BBalanceItem
                                {
                                    free = _balances[$"{_currency_id.Key}_available"].Value<decimal>(),
                                    used = _balances[$"{_currency_id.Key}_reserved"].Value<decimal>()
                                };

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