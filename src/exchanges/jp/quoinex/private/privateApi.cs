using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Coin.Private;
using CCXT.NET.Shared.Coin.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Quoinex.Private
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
                    base.privateClient = new QuoinexClient("private", __connect_key, __secret_key);

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
                    base.publicApi = new CCXT.NET.Quoinex.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// Get Crypto Accounts
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Addresses> FetchAddressesAsync(Dictionary<string, object> args = null)
        {
            var _result = new Addresses();

            var _markets = await publicApi.LoadMarketsAsync();
            if (_markets.success)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = privateClient.MergeParamsAndArgs(args);

                var _json_value = await privateClient.CallApiGet1Async("/crypto_accounts", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _addresses = privateClient.DeserializeObject<List<QAddressItem>>(_json_value.Content);
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
        /// Get Account Balance
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Balance> FetchBalanceAsync(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new Balance();

            var _currency_id = await publicApi.LoadCurrencyIdAsync(base_name);
            if (_currency_id.success)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = privateClient.MergeParamsAndArgs(args);

                var _json_value = await privateClient.CallApiGet1Async("/accounts/balance", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _balances = privateClient.DeserializeObject<List<QBalanceItem>>(_json_value.Content);
                    {
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
        /// Get all Account Balances
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

                var _json_value = await privateClient.CallApiGet1Async("/accounts/balance", _params);
#if DEBUG
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success)
                {
                    var _json_data = privateClient.DeserializeObject<List<QBalanceItem>>(_json_value.Content);
                    {
                        foreach (var _currency_id in _markets.CurrencyNames)
                        {
                            var _balances = _json_data.Where(b => b.currency == _currency_id.Key);
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