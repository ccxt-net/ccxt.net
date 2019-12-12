using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Private;
using OdinSdk.BaseLib.Coin.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.OkCoinKr.Private
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
                    base.privateClient = new OkCoinKrClient("private", __connect_key, __secret_key);

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
                    base.publicApi = new CCXT.NET.OkCoinKr.Public.PublicApi();

                return base.publicApi;
            }
        }

        /// <summary>
        /// 전체 계좌 조회
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<Balances> FetchBalances(Dictionary<string, object> args = null)
        {
            var _result = new Balances();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _params = privateClient.MergeParamsAndArgs(args);

                var _json_value = await privateClient.CallApiPost1Async("/v1/userinfo.do", _params);
#if RAWJSON
                _result.rawJson = _json_value.Content;
#endif
                var _json_result = privateClient.GetResponseMessage(_json_value.Response);
                if (_json_result.success == true)
                {
                    var _json_data = privateClient.DeserializeObject<OBalances>(_json_value.Content);
                    {
                        foreach (var _currency_id in _markets.CurrencyNames)
                        {
                            //var _borrow = _json_data.info.funds.borrow.Where(b => b.Key == _currency_id.Key).SingleOrDefault();
                            var _free = _json_data.info.funds.free.Where(b => b.Key == _currency_id.Key).SingleOrDefault();
                            var _freezed = _json_data.info.funds.freezed.Where(b => b.Key == _currency_id.Key).SingleOrDefault();

                            var _balance = new OBalanceItem
                            {
                                currency = _currency_id.Value,
                                free = _free.Value,
                                used = _freezed.Value,
                                total = _free.Value + _freezed.Value
                            };

                            if (_balance.total > 0)
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