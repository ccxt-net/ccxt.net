using CCXT.NET.OKEx.Public;
using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Public;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCXT.NET.OKEx
{
    /// <summary>
    ///
    /// </summary>
    public sealed class OKExClient : OdinSdk.BaseLib.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "OKEx";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public OKExClient(string division)
            : base(division)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        /// <param name="user_name">exchange's id or uuid for login</param>
        /// <param name="user_password">exchange's password for login</param>
        public OKExClient(string division, string connect_key, string secret_key, string user_name, string user_password)
            : base(division, connect_key, secret_key, user_name, user_password, authentication: true)
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
                            "CN",
                            "US"
                        },
                        Urls = new ExchangeUrls
                        {
                            logo = "https://user-images.githubusercontent.com/1294454/32552768-0d6dd3c6-c4a6-11e7-90f8-c043b64756a7.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://www.okex.com/api/v1" },
                                { "private", "https://www.okex.com/api/v1" },
                                { "trade", "https://www.okex.com/api/v1" },
                                { "web", "https://www.okex.com/v2" },
                                { "websocket", "wss://real.okex.com:10441/websocket" }
                            },
                            www = "https://www.okex.com",
                            doc = new List<string>
                            {
                                "https://github.com/okcoin-okex/API-docs-OKEx.com",
                                "https://www.okcoin.com/rest_getStarted.html",
                                "https://www.npmjs.com/package/okcoin.com"
                            },
                            fees = new List<string>
                            {
                                "https://www.okex.com/fees.html"
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
                            token = new ExchangeLimitCalled { rate = 60000 },           // 30 request per minute
                            @public = new ExchangeLimitCalled { rate = 2000 },
                            @private = new ExchangeLimitCalled { rate = 2000 },
                            trade = new ExchangeLimitCalled { rate = 2000 },
                            total = new ExchangeLimitCalled { rate = 2000 }
                        },
                        Fees = new MarketFees
                        {
                            trading = new MarketFee
                            {
                                tierBased = false,      // true for tier-based/progressive
                                percentage = false,     // fixed commission

                                maker = 0.59m / 100m,
                                taker = 0.59m / 100m
                            }
                        },
                        Timeframes = new Dictionary<string, string>
                        {
                            { "1m", "1min"},
                            { "3m", "3min"},
                            { "5m", "5min"},
                            { "15m", "15min"},
                            { "30m", "30min"},
                            { "1h", "1hour"},
                            { "2h", "2hour"},
                            { "4h", "4hour"},
                            { "6h", "6hour"},
                            { "12h", "12hour"},
                            { "1d", "1day"},
                            { "3d", "3day"}, // api document에는 있으나 오류남
                            { "1w", "1week"}
                        },
                        Options = new ExchangeOptions
                        {
                            fiats = new string[]
                            {
                                "USD", "CNY"
                            },
                            futures = new Dictionary<string, bool>
                            {
                                { "BCH", true },
                                { "BTC", true },
                                { "BTG", true },
                                { "EOS", true },
                                { "ETC", true },
                                { "ETH", true },
                                { "LTC", true },
                                { "NEO", true },
                                { "QTUM", true },
                                { "USDT", true },
                                { "XUC", true }
                            }
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
        /// <param name="market"></param>
        /// <param name="spot_url"></param>
        /// <param name="futures_url"></param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public (string endPoint, Dictionary<string, object> args) CheckFuturesUrl(IMarketItem market, string spot_url, string futures_url, Dictionary<string, object> args)
        {
            var _result = (endPoint: spot_url, args);

            var _omarket = market as OMarketItem;
            if (_omarket.future == true)
            {
                _result.endPoint = futures_url;

                if (_result.args.ContainsKey("contract_type") == false)
                    _result.args.Add("contract_type", "this_week");         //계약유형: this_week:금주, next_week:차주, month:당월, quarter:분기
            }

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<IRestRequest> CreatePostRequest(string endpoint, Dictionary<string, object> args = null)
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
        public new Dictionary<int, ErrorCode> ErrorMessages = new Dictionary<int, ErrorCode>
        {
            { 10000, ErrorCode.ExchangeError },             // "Required field, can not be null"
            { 10001, ErrorCode.DDoSProtection },            // "Request frequency too high to exceed the limit allowed"
            { 10005, ErrorCode.AuthenticationError },       // "'SecretKey' does not exist"
            { 10006, ErrorCode.AuthenticationError },       // "'Api_key' does not exist"
            { 10007, ErrorCode.AuthenticationError },       // "Signature does not match"
            { 1002, ErrorCode.InsufficientFunds },          // "The transaction amount exceed the balance"
            { 1003, ErrorCode.InvalidOrder },               // "The transaction amount is less than the minimum requirement"
            { 1004, ErrorCode.InvalidOrder },               // "The transaction amount is less than 0"
            { 1013, ErrorCode.InvalidOrder },               // no contract type (PR-1101)
            { 1027, ErrorCode.InvalidOrder },               // createLimitBuyOrder(symbol, 0, 0): Incorrect parameter may exceeded limit
            { 1050, ErrorCode.InvalidOrder },               // returned when trying to cancel an order that was filled or canceled previously
            { 1217, ErrorCode.InvalidOrder },               // "Order was sent at ±5% of the current market price. Please resend"
            { 10014, ErrorCode.InvalidOrder },              // "Order price must be between 0 and 1,000,000"
            { 1009, ErrorCode.OrderNotFound },              // for spot markets, cancelling closed order
            { 1019, ErrorCode.OrderNotFound },              // order closed? ("Undo order failed")
            { 1051, ErrorCode.OrderNotFound },              // for spot markets, cancelling "just closed" order
            { 10009, ErrorCode.OrderNotFound },             // for spot markets, "Order does not exist"
            { 20015, ErrorCode.OrderNotFound },             // for future markets
            { 10008, ErrorCode.ExchangeError },             // Illegal URL parameter
        };

        /// <summary>
        ///
        /// </summary>
        /// <param name="error_code"></param>
        /// <returns></returns>
        public new ErrorCode GetErrorMessage(int error_code)
        {
            return ErrorMessages.ContainsKey(error_code) == true
                                  ? ErrorMessages[error_code]
                                  : ErrorCode.ExchangeError;
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
                if (String.IsNullOrEmpty(response.Content) == false && response.Content[0] == '{')
                {
                    var _json_result = this.DeserializeObject<JToken>(response.Content);

                    var _error_code = ErrorCode.ExchangeError;
                    var _error_msg = response.Content;

                    var _json_error = _json_result.SelectToken("error_code");
                    if (_json_error != null)
                    {
                        var _error_number = _json_error.Value<int>();
                        if (ErrorMessages.ContainsKey(_error_number) == true)
                            _error_code = GetErrorMessage(_error_number);

                        _result.SetFailure(_error_msg, _error_code, _error_number);
                    }
                    else
                    {
                        var _json_return = _json_result.SelectToken("result");
                        if (_json_return != null)
                        {
                            if (_json_return.Value<bool>() == false)
                                _result.SetFailure(_error_msg, _error_code);
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