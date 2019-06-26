using Newtonsoft.Json.Linq;
using CCXT.NET.Coin;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCXT.NET.OkCoinKr
{
    /// <summary>
    ///
    /// </summary>
    public sealed class OkCoinKrClient : CCXT.NET.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "OkCoinKr";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public OkCoinKrClient(string division)
            : base(division)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public OkCoinKrClient(string division, string connect_key, string secret_key)
            : base(division, connect_key, secret_key, authentication: true)
        {
        }

        /// <summary>
        /// information of exchange for trading
        /// </summary>
        public override ExchangeInfo ExchangeInfo
        {
            get
            {
                if (base.ExchangeInfo == null)
                {
                    base.ExchangeInfo = new ExchangeInfo(this.DealerName)
                    {
                        Countries = new List<string>
                        {
                            "KR"
                        },
                        Urls = new ExchangeUrls
                        {
                            logo = "https://user-images.githubusercontent.com/1294454/27766791-89ffb502-5ee5-11e7-8a5b-c5950b68ac65.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://www.okcoinkr.com/api" },
                                { "private", "https://www.okcoinkr.com/api" },
                                { "trade", "https://www.okcoinkr.com/api" },
                                { "web", "https://www.okcoinkr.com/v2" }
                            },
                            www = "https://www.okcoinkr.com",
                            doc = new List<string>
                            {
                                "https://okcoinkr.zendesk.com"
                            }
                        },
                        RequiredCredentials = new RequiredCredentials
                        {
                            apikey = true,
                            secret = true,
                            uid = false,
                            login = false,
                            password = false,
                            twofa = false
                        },
                        LimitRate = new ExchangeLimitRate
                        {
                            useTotal = false,
                            token = new ExchangeLimitCalled { rate = 60000 },
                            @public = new ExchangeLimitCalled { rate = 1000 },
                            @private = new ExchangeLimitCalled { rate = 1000 },
                            trade = new ExchangeLimitCalled { rate = 1000 },
                            total = new ExchangeLimitCalled { rate = 1000 }     // up to 3000 requests per 5 minutes ≈ 600 requests per minute ≈ 10 requests per second ≈ 100 ms
                        },
                        Fees = new MarketFees
                        {
                            trading = new MarketFee
                            {
                                tierBased = false,      // true for tier-based/progressive
                                percentage = false,     // fixed commission

                                maker = 0.2m / 100m,
                                taker = 0.2m / 100m
                            }
                        },
                        Timeframes = new Dictionary<string, string>
                        {
                            { "1m","1min" },
                            { "3m","3min" },
                            { "5m","5min" },
                            { "15m","15min" },
                            { "30m","30min" },
                            { "1h","1hour" },
                            { "2h","2hour" },
                            { "4h","4hour" },
                            { "6h","6hour" },
                            { "12h","12hour" },
                            { "1d","1day" },
                            { "3d","3day" },
                            { "1w","1week" }
                        }
                    };
                }

                return base.ExchangeInfo;
            }
        }

        private MD5CryptoServiceProvider __md5crypto = null;

        /// <summary>
        ///
        /// </summary>
        public MD5CryptoServiceProvider MD5Crypto
        {
            get
            {
                if (__md5crypto == null)
                    __md5crypto = new MD5CryptoServiceProvider();

                return __md5crypto;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<IRestRequest> CreatePostRequest(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreatePostRequest(endpoint, args);

            if (IsAuthentication == true)
            {
                var _params = new Dictionary<string, object>();
                {
                    foreach (var _param in _request.Parameters)
                        _params.Add(_param.Name, _param.Value);

                    _request.Parameters.Clear();
                }

                _params.Add("api_key", ConnectKey);

                var _post_data = ToQueryString(_params);
                {
                    _post_data = String.Join("&", new SortedSet<string>(_post_data.Split('&'), StringComparer.Ordinal));

                    // secret key must be at the end of query
                    var _sign_data = _post_data + $"&secret_key={SecretKey}";

                    var _signature = this.ConvertHexString(MD5Crypto.ComputeHash(Encoding.UTF8.GetBytes(_sign_data)));
                    _post_data += $"&sign={_signature.ToUpper()}";
                }

                _request.AddParameter(new Parameter
                {
                    ContentType = "",
                    Name = "application/x-www-form-urlencoded",
                    Type = ParameterType.RequestBody,
                    Value = _post_data
                });
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        ///
        /// </summary>
        public new Dictionary<int, string> ErrorMessages = new Dictionary<int, string>
        {
            { 10000, "Required field, can not be null" },
            { 10001, "Request frequency too high to exceed the limit allowed" },
            { 10002, "System error" },
            { 10003, "Not in reqest list, please try again later" },
            { 10004, "This IP is not allowed to access" },

            { 1009, "No order" },
        };

        /// <summary>
        ///
        /// </summary>
        /// <param name="error_code"></param>
        /// <returns></returns>
        public override string GetErrorMessage(int error_code)
        {
            return ErrorMessages.ContainsKey(error_code) == true
                                  ? ErrorMessages[error_code]
                                  : "failure";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="response">response value arrive from exchange's server</param>
        /// <returns></returns>
        public override BoolResult GetResponseMessage(IRestResponse response = null)
        {
            var _result = new BoolResult();

            if (response != null)
            {
                if (response.IsSuccessful == true)
                {
                    var _json_result = this.DeserializeObject<JToken>(response.Content);

                    var _json_error = _json_result.SelectToken("error_code");
                    if (_json_error != null)
                    {
                        var _error_code = _json_error.Value<int>();
                        if (_error_code != 0)
                        {
                            _result.SetFailure(
                                      GetErrorMessage(_error_code),
                                      ErrorCode.ResponseDataError,
                                      _error_code
                                  );
                        }
                    }
                }

                if (_result.success == true && response.IsSuccessful == false)
                {
                    _result.SetFailure(
                            response.ErrorMessage ?? response.StatusDescription,
                            ErrorCode.ResponseRestError,
                            (int)response.StatusCode,
                            false
                        );
                }
            }

            return _result;
        }
    }
}