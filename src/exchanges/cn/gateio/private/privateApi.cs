using Newtonsoft.Json.Linq;
using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Coin.Private;
using CCXT.NET.Shared.Coin.Types;
using CCXT.NET.Shared.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCXT.NET.GateIO.Private
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
                    base.privateClient = new GateIOClient("private", __connect_key, __secret_key);

                return base.privateClient;
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
                    base.publicApi = new CCXT.NET.GateIO.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Since all BTC exchanges within Korbit are made internally, a BTC address does not need to be assigned to every user.
        /// However, to receive BTC from an outside source to your Korbit account, you can set up your BTC receiving address by using the following API.
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Address> CreateAddressAsync(string currency_name, Dictionary<string, object> args = null)
        {
            var _result = new Address();

            var _currency_id = await publicApi.LoadCurrencyIdAsync(currency_name);
            if (_currency_id.success)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("currency", currency_name);

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await privateClient.CallApiPost1Async("/1/private/newAddress", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _address = privateClient.DeserializeObject<GAddressItem>(_json_value.Content);
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
        /// Return your deposit address to make a new deposit.
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Address> FetchAddressAsync(string currency_name, Dictionary<string, object> args = null)
        {
            var _result = new Address();

            var _currency_id = await publicApi.LoadCurrencyIdAsync(currency_name);
            if (_currency_id.success)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("currency", currency_name);

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await privateClient.CallApiPost1Async("/1/private/depositAddress", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _address = privateClient.DeserializeObject<GAddressItem>(_json_value.Content);
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
        public override async ValueTask<Transfer> CoinWithdrawAsync(string currency_name, string address, string tag, decimal quantity, Dictionary<string, object> args = null)
        {
            var _result = new Transfer();

            var _currency_id = await publicApi.LoadCurrencyIdAsync(currency_name);
            if (_currency_id.success)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("amount", quantity);
                    _params.Add("currency", _currency_id.result);
                    _params.Add("address", address);

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await privateClient.CallApiPost1Async("/1/private/withdraw", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _json_data = privateClient.DeserializeObject<JObject>(_json_value.Content);
                    {
                        var _withdraw = new GTransferItem();
                        if (_json_data["result"].Value<bool>())
                        {
                            _withdraw.timestamp = CUnixTime.NowMilli;
                            _withdraw.transferId = _withdraw.timestamp.ToString();                    // transferId 없음
                            _withdraw.transactionId = (_withdraw.timestamp * 1000).ToString();      // transactionId 없음

                            _withdraw.transactionType = TransactionType.Withdrawing;

                            _withdraw.currency = _currency_id.result;
                            _withdraw.toAddress = address;
                            _withdraw.toTag = tag;

                            _withdraw.amount = quantity;
                            //_withdraw.fee = _json_data.fee;

                            _withdraw.confirmations = 0;
                            _withdraw.isCompleted = false;
                        }

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
        /// 전체 출금 조회
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
            if (_markets.success)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _timestamp = privateClient.ExchangeInfo.GetTimestamp(timeframe);
                var _timeframe = privateClient.ExchangeInfo.GetTimeframe(timeframe);

                var _params = new Dictionary<string, object>();
                {
                    _params.Add("start", since / 1000);
                    //_params.Add("end", since);

                    privateClient.MergeParamsAndArgs(_params, args);
                }

                var _json_value = await privateClient.CallApiPost1Async("/1/private/depositsWithdrawals", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _json_data = privateClient.DeserializeObject<GTransfers>(_json_value.Content);
                    {
                        if (_json_data.success)
                        {
                            foreach (var _d in _json_data.deposits)
                            {
                                var _deposit = new GTransferItem
                                {
                                    currency = _d.currency,
                                    fromAddress = _d.fromAddress,
                                    amount = _d.amount,
                                    transactionId = _d.transactionId,
                                    timestamp = _d.timestamp,
                                    transferType = _d.transferType
                                };

                                _deposit.transferId = _deposit.timestamp.ToString();                    // transferId 없음

                                _deposit.isCompleted = _deposit.transferType == TransferType.Done;
                                _deposit.transactionType = _deposit.transferType == TransferType.Done ? TransactionType.Deposit : TransactionType.Depositing;

                                _result.result.Add(_deposit);
                            }

                            foreach (var _w in _json_data.withdraws)
                            {
                                var _withdraw = new GTransferItem
                                {
                                    currency = _w.currency,
                                    toAddress = _w.toAddress,
                                    amount = _w.amount,
                                    transactionId = _w.transactionId,
                                    timestamp = _w.timestamp,
                                    transferType = _w.transferType
                                };

                                _withdraw.transferId = _withdraw.timestamp.ToString();                    // transferId 없음

                                _withdraw.isCompleted = _withdraw.transferType == TransferType.Done;
                                _withdraw.transactionType = _withdraw.transferType == TransferType.Done ? TransactionType.Withdraw : TransactionType.Withdrawing;

                                _result.result.Add(_withdraw);
                            }
                        }

                        //_result.result = _result.result
                        //                    .Where(t => t.timestamp >= since)
                        //                    .OrderByDescending(t => t.timestamp)
                        //                    .Take(limits);
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
        /// 전체 계좌 조회
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Balances> FetchBalancesAsync(Dictionary<string, object> args = null)
        {
            var _result = new Balances();

            var _markets = await publicApi.LoadMarketsAsync();
            if (_markets.success)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = privateClient.MergeParamsAndArgs(args);

                var _json_value = await privateClient.CallApiPost1Async("/1/private/balances", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _json_data = privateClient.DeserializeObject<GBalances>(_json_value.Content);
                    {
                        foreach (var _currency_id in _markets.CurrencyNames)
                        {
                            var _balance = new GBalanceItem();
                            _balance.currency = _currency_id.Value;

                            if (_json_data.success)
                            {
                                if (_json_data.available.ContainsKey(_currency_id.Value))
                                    _balance.free = _json_data.available[_currency_id.Value].Value<decimal>();
                                if (_json_data.locked.ContainsKey(_currency_id.Value))
                                    _balance.used = _json_data.locked[_currency_id.Value].Value<decimal>();
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