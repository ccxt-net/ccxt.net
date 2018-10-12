using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CCXT.NET.BitMEX
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BitmexClient : OdinSdk.BaseLib.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        /// 
        /// </summary>
        public override string DealerName { get; set; } = "BitMEX";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public BitmexClient(string division)
            : base(division)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public BitmexClient(string division, string connect_key, string secret_key)
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
                            "SC" // Seychelles
                        },
                        Urls = new ExchangeUrls
                        {
                            logo = "https://user-images.githubusercontent.com/1294454/27766319-f653c6e6-5ed4-11e7-933d-f0bc3699ae8f.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://www.bitmex.com" },
                                { "private", "https://www.bitmex.com" },
                                { "trade", "https://www.bitmex.com" },
                                { "test.public", "https://testnet.bitmex.com" },
                                { "test.private", "https://testnet.bitmex.com" },
                                { "test.trade", "https://testnet.bitmex.com" }
                            },
                            www = "https://www.bitmex.com",
                            doc = new List<string>
                            {
                                "https://www.bitmex.com/app/apiOverview",
                                "https://github.com/BitMEX/api-connectors/tree/master/official-http"
                            },
                            fees = new List<string>
                            {
                                "https://www.bitmex.com/app/fees"
                            }
                        },
                        AmountMultiplier = new Dictionary<string, decimal>
                        {
                            { "XBTUSD", 100000000.0m },
                            { "ETHUSD", 1000000.0m }
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
                            useTotal = true,
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
                                tierBased = false,          // true for tier-based/progressive
                                percentage = false,         // fixed commission
                                
                                maker = -0.0250m / 100m,    // https://www.bitmex.com/app/fees
                                taker = 0.0750m / 100m
                            }
                        },
                        Timeframes = new Dictionary<string, string>
                        {
                            { "1m", "1m"},
                            { "5m", "5m"},
                            { "1h", "1h"},
                            { "1d", "1d" }
                        }
                    };
                }

                return base.ExchangeInfo;
            }
        }

        private HMACSHA256 __encryptor = null;

        /// <summary>
        /// 
        /// </summary>
        public HMACSHA256 Encryptor
        {
            get
            {
                if (__encryptor == null)
                    __encryptor = new HMACSHA256(Encoding.UTF8.GetBytes(SecretKey));

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
            var _request = await base.CreatePostRequest(endpoint);

            if (IsAuthentication == true)
            {
                //var _post_data = ToQueryString(_request.Parameters);
                //_request.Parameters.Clear();

                var _nonce = (GenerateOnlyNonce(10) + 3600).ToString();
                //if (args != null && args.Count > 0)
                //    endpoint += $"?{_post_data}";

                var _json_body = "";
                if (args != null && args.Count > 0)
                {
                    _json_body = Regex.Unescape(this.SerializeObject(args, Formatting.None));

                    _request.AddParameter(new Parameter
                    {
                        ContentType = "",
                        Name = "application/json",
                        Type = ParameterType.RequestBody,
                        Value = _json_body
                    });

                    //_request.Resource += $"?{_post_data}";
                }

                var _signature = await CreateSignature(_request.Method, endpoint, _nonce, _json_body);
                {
                    _request.AddHeader("api-expires", _nonce);
                    _request.AddHeader("api-key", ConnectKey);
                    _request.AddHeader("api-signature", _signature);
                }
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<IRestRequest> CreateGetRequest(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreateGetRequest(endpoint, args);

            if (IsAuthentication == true)
            {
                var _post_data = ToQueryString(_request.Parameters);
                //_request.Parameters.Clear();

                var _nonce = (GenerateOnlyNonce(10) + 3600).ToString();
                if (args != null && args.Count > 0)
                    endpoint += $"?{_post_data}";

                var _signature = await CreateSignature(_request.Method, endpoint, _nonce);
                {
                    _request.AddHeader("api-expires", _nonce);
                    _request.AddHeader("api-key", ConnectKey);
                    _request.AddHeader("api-signature", _signature);
                }
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<IRestRequest> CreateDeleteRequest(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreateDeleteRequest(endpoint);

            if (IsAuthentication == true)
            {
                //var _post_data = ToQueryString(_request.Parameters);
                //_request.Parameters.Clear();

                var _nonce = (GenerateOnlyNonce(10) + 3600).ToString();
                //if (args != null && args.Count > 0)
                //    endpoint += $"?{_post_data}";

                var _json_body = "";
                if (args != null && args.Count > 0)
                {
                    _json_body = Regex.Unescape(this.SerializeObject(args, Formatting.None));

                    _request.AddParameter(new Parameter
                    {
                        ContentType = "",
                        Name = "application/json",
                        Type = ParameterType.RequestBody,
                        Value = _json_body
                    });

                    //_request.Resource += $"?{_post_data}";
                }

                var _signature = await CreateSignature(_request.Method, endpoint, _nonce, _json_body);
                {
                    _request.AddHeader("api-expires", _nonce);
                    _request.AddHeader("api-key", ConnectKey);
                    _request.AddHeader("api-signature", _signature);
                }
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateSignature(Method verb, string endpoint, string nonce, string json_body = "")
        {
            return await Task.FromResult
                    (
                        this.ConvertHexString
                        (
                            Encryptor.ComputeHash
                            (
                                Encoding.UTF8.GetBytes($"{verb}{endpoint}{nonce}{json_body}")
                            )
                        )
                        .ToLower()
                    );
        }

        /// <summary>
        /// 
        /// </summary>
        public new Dictionary<string, ErrorCode> ErrorMessages = new Dictionary<string, ErrorCode>
        {
            { "Not Found", ErrorCode.OrderNotFound },
            { "Invalid API Key.", ErrorCode.AuthenticationError },
            { "Access Denied", ErrorCode.PermissionDenied }
        };

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
                if (response.IsSuccessful == false) // (int) StatusCode >= 200 && (int) StatusCode <= 299 && ResponseStatus == ResponseStatus.Completed;
                {
                    if ((int)response.StatusCode != 429)
                    {
                        if (String.IsNullOrEmpty(response.Content) == false && response.Content[0] == '{')
                        {
                            var _json_result = this.DeserializeObject<JToken>(response.Content);

                            var _json_error = _json_result.SelectToken("error");
                            if (_json_error != null)
                            {
                                var _json_message = _json_error.SelectToken("message");
                                if (_json_message != null)
                                {
                                    var _error_code = ErrorCode.ExchangeError;

                                    var _error_msg = _json_message.Value<string>();
                                    if (String.IsNullOrEmpty(_error_msg) == false)
                                    {
                                        if (ErrorMessages.ContainsKey(_error_msg) == true)
                                            _error_code = ErrorMessages[_error_msg];
                                    }
                                    else
                                    {
                                        _error_msg = response.Content;
                                    }

                                    _result.SetFailure(_error_msg, _error_code);
                                }
                            }
                        }
                    }
                    else
                    {
                        _result.SetFailure(
                                response.ErrorMessage ?? response.StatusDescription,
                                ErrorCode.DDoSProtection,
                                (int)response.StatusCode,
                                false
                            );
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