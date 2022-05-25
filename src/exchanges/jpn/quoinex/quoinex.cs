using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using CCXT.NET.Shared.Coin;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace CCXT.NET.Quoinex
{
    /// <summary>
    ///
    /// </summary>
    public sealed class QuoinexClient : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "Quoinex";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public QuoinexClient(string division)
            : base(division)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public QuoinexClient(string division, string connect_key, string secret_key)
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
                            "JP",
                            "SG",
                            "VN"
                        },
                        Urls = new ExchangeUrls
                        {
                            logo = "https://user-images.githubusercontent.com/1294454/35047114-0e24ad4a-fbaa-11e7-96a9-69c1a756083b.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://api.quoine.com" },
                                { "private", "https://api.quoine.com" },
                                { "trade", "https://api.quoine.com" }
                            },
                            www = "https://quoinex.com/",
                            doc = new List<string>
                            {
                                "https://developers.quoine.com",
                                "https://developers.quoine.com/v2",
                                "https://www.quoine.com/api/",
                            },
                            fees = new List<string>
                            {
                                "https://news.quoinex.com/fees/"
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

                                maker = 0.25m / 100m,   // https://news.quoinex.com/fees/
                                taker = 0.25m / 100m
                            }
                        }
                    };
                }

                return base.ExchangeInfo;
            }
        }

        private JwtHeader __jwt_header = null;

        /// <summary>
        ///
        /// </summary>
        public JwtHeader JwtHeader
        {
            get
            {
                if (__jwt_header == null)
                {
                    var _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
                    var _credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

                    __jwt_header = new JwtHeader(_credentials);
                }

                return __jwt_header;
            }
        }

        private JwtSecurityTokenHandler __jwt_handler = null;

        /// <summary>
        ///
        /// </summary>
        public JwtSecurityTokenHandler JwtHandler
        {
            get
            {
                if (__jwt_handler == null)
                    __jwt_handler = new JwtSecurityTokenHandler();

                return __jwt_handler;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<RestRequest> CreateGetRequestAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreateGetRequestAsync(endpoint, args);

            if (IsAuthentication == true)
            {
                var _nonce = GenerateOnlyNonce(13);

                var _post_params = _request.Parameters.ToDictionary(p => p.Name, p => p.Value);

                var _post_data = ToQueryString(_post_params);
                {
                    var _url = endpoint + (String.IsNullOrEmpty(_post_data) ? "" : "?" + _post_data);

                    var _payload = new JwtPayload
                        {
                            { "path", _url },
                            { "nonce", _nonce },
                            { "token_id", ConnectKey },
                            { "iat", _nonce / 1000 }
                        };

                    var _secToken = new JwtSecurityToken(this.JwtHeader, _payload);
                    var _signature = JwtHandler.WriteToken(_secToken);

                    _request.AddHeader("X-Quoine-API-Version", "2");
                    _request.AddHeader("X-Quoine-Auth", _signature);
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
        public override async ValueTask<RestRequest> CreatePutRequestAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreatePutRequestAsync(endpoint, args);

            if (IsAuthentication == true)
            {
                var _nonce = GenerateOnlyNonce(13);

                var _post_params = _request.Parameters.ToDictionary(p => p.Name, p => p.Value);

                var _post_data = ToQueryString(_post_params);
                {
                    var _url = endpoint + (String.IsNullOrEmpty(_post_data) ? "" : "?" + _post_data);

                    var _payload = new JwtPayload
                        {
                            { "path", _url },
                            { "nonce", _nonce },
                            { "token_id", ConnectKey },
                            { "iat", _nonce / 1000 }
                        };

                    var _secToken = new JwtSecurityToken(this.JwtHeader, _payload);
                    var _signature = JwtHandler.WriteToken(_secToken);

                    _request.AddHeader("X-Quoine-API-Version", "2");
                    _request.AddHeader("X-Quoine-Auth", _signature);
                }
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        ///
        /// </summary>
        public new Dictionary<string, ErrorCode> ErrorMessages = new Dictionary<string, ErrorCode>
        {
            { "Order not found", ErrorCode.OrderNotFound }
        };

        /// <summary>
        ///
        /// </summary>
        /// <param name="response">response value arrive from exchange's server</param>
        /// <returns></returns>
        public override BoolResult GetResponseMessage(RestResponse response = null)
        {
            var _result = new BoolResult();

            if (response != null)
            {
                if (response.IsSuccessful == false)
                {
                    if (String.IsNullOrEmpty(response.Content) == false && (response.Content[0] == '{' || response.Content[0] == '['))
                    {
                        var _json_result = this.DeserializeObject<JToken>(response.Content);

                        var _error_code = ErrorCode.ExchangeError;
                        var _error_msg = "";
                        {
                            var _json_message = _json_result.SelectToken("message");
                            var _json_errors = _json_result.SelectToken("errors");

                            if (_json_message != null)
                                _error_msg = _json_message.Value<string>();
                            else if (_json_errors != null)
                                _error_msg = _json_errors.Value<string>();
                        }

                        if (String.IsNullOrEmpty(_error_msg) == false)
                        {
                            if (ErrorMessages.ContainsKey(_error_msg) == true)
                                _error_code = ErrorMessages[_error_msg];

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