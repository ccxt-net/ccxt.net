using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CCXT.NET.Coin;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCXT.NET.Coinone
{
    /// <summary>
    ///
    /// </summary>
    public sealed class CoinoneClient : CCXT.NET.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "Coinone";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public CoinoneClient(string division)
            : base(division)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public CoinoneClient(string division, string connect_key, string secret_key)
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
                            logo = "https://user-images.githubusercontent.com/1294454/38003300-adc12fba-323f-11e8-8525-725f53c4a659.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://api.coinone.co.kr" },
                                { "private", "https://api.coinone.co.kr" },
                                { "trade", "https://api.coinone.co.kr" }
                            },
                            www = "https://coinone.co.kr",
                            doc = new List<string>
                            {
                                "https://doc.coinone.co.kr"
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
                            @public = new ExchangeLimitCalled { rate = 667 },          // 90 requests per minute
                            @private = new ExchangeLimitCalled { rate = 667 },         // 6 requests per second
                            trade = new ExchangeLimitCalled { rate = 667 },
                            total = new ExchangeLimitCalled { rate = 667 }
                        },
                        Fees = new MarketFees
                        {
                            trading = new MarketFee
                            {
                                tierBased = false,      // true for tier-based/progressive
                                percentage = false,     // fixed commission

                                maker = 0.1m / 100m,
                                taker = 0.1m / 100m
                            }
                        },
                        Timeframes = new Dictionary<string, string>
                        {
                            { "1h", "hour"},
                            { "1d", "day"}
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
                    __encryptor = new HMACSHA512(Encoding.UTF8.GetBytes(SecretKey.ToUpper()));

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

                var _nonce = GenerateOnlyNonce(13).ToString();

                _params.Add("access_token", ConnectKey);
                _params.Add("nonce", _nonce);

                var _post_data = this.SerializeObject(_params, Formatting.None);
                {
                    var _sign_data = Convert.ToBase64String(Encoding.UTF8.GetBytes(_post_data));
                    var _signature = this.ConvertHexString(Encryptor.ComputeHash(Encoding.UTF8.GetBytes(_sign_data)));

                    _request.AddHeader("X-COINONE-PAYLOAD", _sign_data);
                    _request.AddHeader("X-COINONE-SIGNATURE", _signature.ToLower());
                }
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        ///
        /// </summary>
        public new Dictionary<int, string> ErrorMessages = new Dictionary<int, string>
        {
             { 0, "success" },
             { 4, "blocked user access" },
             { 11, "access token is missing" },
             { 12, "invalid access token" },
             { 40, "invalid api permission" },
             { 50, "authenticate error" },
             { 51, "invalid api" },
             { 52, "deprecated api" },
             { 53, "two factor auth fail" },
             { 100, "session expired" },
             { 101, "invalid format" },
             { 102, "id is not exist" },
             { 103, "lack of balance" },
             { 104, "order id is not exist" },
             { 105, "price is not correct" },
             { 106, "locking error" },
             { 107, "parameter error" },
             { 111, "order id is not exist" },
             { 112, "cancel failed" },
             { 113, "quantity is too low(eth, etc > 0.01)" },
             { 120, "v2 api payload is missing" },
             { 121, "v2 api signature is missing" },
             { 122, "v2 api nonce is missing" },
             { 123, "v2 api signature is not correct" },
             { 130, "v2 api nonce value must be a positive integer" },
             { 131, "v2 api nonce is must be bigger then last nonce" },
             { 132, "v2 api body is corrupted" },
             { 141, "too many limit orders" },
             { 150, "it's v1 api. v2 access token is not acceptable" },
             { 151, "it's v2 api. v1 access token is not acceptable" },
             { 200, "wallet error" },
             { 202, "limitation error" },
             { 210, "limitation error" },
             { 220, "limitation error" },
             { 221, "limitation error" },
             { 310, "mobile auth error" },
             { 311, "need mobile auth" },
             { 312, "name is not correct" },
             { 330, "phone number error" },
             { 404, "page not found error" },
             { 405, "server error" },
             { 444, "locking error" },
             { 500, "email error" },
             { 501, "email error" },
             { 777, "mobile auth error" },
             { 778, "phone number error" },
             { 779, "address error" },
             { 1202, "app not found" },
             { 1203, "already registered" },
             { 1204, "invalid access" },
             { 1205, "api key error" },
             { 1206, "user not found" },
             { 1207, "user not found" },
             { 1208, "user not found" },
             { 1209, "user not found" }
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
                    if (response.Content.Length > 0 && response.Content[0] == '{')
                    {
                        var _json_result = this.DeserializeObject<JObject>(response.Content);

                        var _json_code = _json_result.SelectToken("errorCode");
                        if (_json_code != null)
                        {
                            var _status_code = _json_code.Value<int>();
                            if (_status_code != 0)
                            {
                                _result.SetFailure(
                                        GetErrorMessage(_status_code),
                                        ErrorCode.ResponseDataError,
                                        _status_code
                                    );
                            }
                        }
                    }
                    else
                    {
                        _result.SetFailure(errorCode: ErrorCode.ResponseDataError);
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