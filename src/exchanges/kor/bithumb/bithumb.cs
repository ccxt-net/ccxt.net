using CCXT.NET.Coin;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCXT.NET.Bithumb
{
    /// <summary>
    ///
    /// </summary>
    public sealed class BithumbClient : CCXT.NET.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "Bithumb";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public BithumbClient(string division)
            : base(division)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public BithumbClient(string division, string connect_key, string secret_key)
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
                            logo = "https://user-images.githubusercontent.com/1294454/30597177-ea800172-9d5e-11e7-804c-b9d4fa9b56b0.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://api.bithumb.com/public" },
                                { "private", "https://api.bithumb.com" },
                                { "trade", "https://api.bithumb.com" },
                                { "web", "https://www.bithumb.com" }
                            },
                            www = "https://www.bithumb.com",
                            doc = new List<string>
                            {
                                "https://www.bithumb.com/u1/US127"
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
                            token = new ExchangeLimitCalled { rate = 60000 },            // 120 request per minute
                            @public = new ExchangeLimitCalled { rate = 500 },
                            @private = new ExchangeLimitCalled { rate = 500 },
                            trade = new ExchangeLimitCalled { rate = 500 },
                            total = new ExchangeLimitCalled { rate = 500 }
                        },
                        Fees = new MarketFees
                        {
                            trading = new MarketFee
                            {
                                tierBased = false,      // true for tier-based/progressive
                                percentage = false,     // fixed commission

                                maker = 0.15m / 100m,
                                taker = 0.15m / 100m
                            }
                        },
                        Timeframes = new Dictionary<string, string>
                        {
                            { "1m", "01M"},
                            { "3m", "03M"},
                            { "5m", "05M"},
                            { "10m", "10M"},
                            { "30m", "30M"},
                            { "1h", "01H"},
                            { "6h", "06H"},
                            { "12h", "12H"},
                            { "1d", "24H"}
                        }
                    };
                }

                return base.ExchangeInfo;
            }
        }

        private HMACSHA512 __encryptor = null;

        /// <summary>
        ///
        /// </summary>
        public HMACSHA512 Encryptor
        {
            get
            {
                if (__encryptor == null)
                    __encryptor = new HMACSHA512(Encoding.UTF8.GetBytes(SecretKey));

                return __encryptor;
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

                _params.Add("endpoint", endpoint);

                var _post_data = ToQueryString2(_params);
                {
                    var _nonce = GenerateOnlyNonce(13).ToString();

                    var _sign_data = $"{endpoint}\0{_post_data}\0{_nonce}";
                    var _sign_hash = Encryptor.ComputeHash(Encoding.UTF8.GetBytes(_sign_data));

                    var _signature = Convert.ToBase64String(Encoding.UTF8.GetBytes(ConvertHexString(_sign_hash).ToLower()));
                    {
                        _request.AddHeader("Api-Key", ConnectKey);
                        _request.AddHeader("Api-Sign", _signature);
                        _request.AddHeader("Api-Nonce", _nonce);
                    }

                    _request.AddParameter(new Parameter
                    {
                        ContentType = "",
                        Name = "application/x-www-form-urlencoded",
                        Type = ParameterType.RequestBody,
                        Value = _post_data
                    });
                }
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        ///
        /// </summary>
        public new Dictionary<int, string> ErrorMessages = new Dictionary<int, string>
        {
            { 0,    "success" },
            { 5100, "bad request" },
            { 5200, "not member" },
            { 5300, "invalid apikey" },
            { 5302, "method not allowed" },
            { 5400, "database fail" },
            { 5500, "invalid parameter" },
            { 5600, "custom notice(output error messages in context)" },
            { 5900, "unknown error" }
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
                if (String.IsNullOrEmpty(response.Content) == false && (response.Content[0] == '{' || response.Content[0] == '['))
                {
                    var _json_result = this.DeserializeObject<JToken>(response.Content);

                    var _json_status = _json_result.SelectToken("status");
                    if (_json_status != null)
                    {
                        var _status_code = _json_status.Value<int>();
                        if (_status_code != 0)
                        {
                            var _message = GetErrorMessage(_status_code);

                            var _json_message = _json_result.SelectToken("message");
                            if (_json_message != null)
                                _message = _json_message.Value<string>();

                            _result.SetFailure(
                                    _message,
                                    ErrorCode.ResponseDataError,
                                    _status_code
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