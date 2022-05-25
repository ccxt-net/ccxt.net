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

namespace CCXT.NET.Upbit
{
    /// <summary>
    ///
    /// </summary>
    public sealed class UpbitClient : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "Upbit";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public UpbitClient(string division)
            : base(division)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public UpbitClient(string division, string connect_key, string secret_key)
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
                            logo = "",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://api.upbit.com/v1" },
                                { "private", "https://api.upbit.com/v1" },
                                { "trade", "https://api.upbit.com/v1" },
                                { "public.old", "https://crix-api-endpoint.upbit.com/v1/crix" },
                                { "private.old", "https://ccx.upbit.com/api/v1" },
                                { "trade.old", "https://ccx.upbit.com/api/v1" },
                                { "dunamu", "https://quotation-api-cdn.dunamu.com/v1/forex" },
                                { "ccx", "https://ccx.upbit.com/api/v1" },
                                { "cs3", "https://s3.ap-northeast-2.amazonaws.com" }
                            },
                            www = "https://upbit.com",
                            doc = new List<string>
                            {
                                "https://docs.upbit.com/",
                                "https://upbit.com"
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
                            token = new ExchangeLimitCalled { rate = 60000 },           // 40 requests per minute
                            @public = new ExchangeLimitCalled { rate = 1500 },
                            @private = new ExchangeLimitCalled { rate = 1500 },
                            trade = new ExchangeLimitCalled { rate = 1500 },
                            total = new ExchangeLimitCalled { rate = 1500 }
                        },
                        Fees = new MarketFees
                        {
                            trading = new MarketFee
                            {
                                tierBased = false,      // true for tier-based/progressive
                                percentage = false,     // fixed commission

                                maker = 0.05m / 100m,
                                taker = 0.05m / 100m
                            }
                        },
                        Timeframes = new Dictionary<string, string>
                        {
                            { "1m","minutes" },
                            { "3m","minutes" },
                            { "5m","minutes" },
                            { "10m","minutes" },
                            { "15m","minutes" },
                            { "30m","minutes" },
                            { "60m","minutes" },
                            { "240m","minutes" },
                            { "1d","days" },
                            { "1w","weeks" },
                            { "1M","months" },
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
        public override async ValueTask<RestRequest> CreatePostRequestAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreatePostRequestAsync(endpoint, args);

            if (IsAuthentication == true)
            {
                var _nonce = GenerateOnlyNonce(13);

                var _post_params = _request.Parameters.ToDictionary(p => p.Name, p => p.Value);

                var _post_data = ToQueryString(_post_params);
                {
                    var _payload = new JwtPayload
                        {
                            { "access_key", ConnectKey },
                            { "nonce", _nonce },
                            { "query", _post_data }
                        };

                    var _secToken = new JwtSecurityToken(this.JwtHeader, _payload);
                    var _signature = JwtHandler.WriteToken(_secToken);

                    _request.AddHeader("Authorization", $"Bearer {_signature}");
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
        public override async ValueTask<RestRequest> CreateGetRequestAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreateGetRequestAsync(endpoint, args);

            if (IsAuthentication == true)
            {
                var _nonce = GenerateOnlyNonce(13);

                var _post_params = _request.Parameters.ToDictionary(p => p.Name, p => p.Value);

                var _post_data = ToQueryString(_post_params);
                {
                    var _payload = new JwtPayload
                        {
                            { "access_key", ConnectKey },
                            { "nonce", _nonce },
                            { "query", _post_data }
                        };

                    var _secToken = new JwtSecurityToken(this.JwtHeader, _payload);
                    var _signature = JwtHandler.WriteToken(_secToken);

                    _request.AddHeader("Authorization", $"Bearer {_signature}");
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
        public override async ValueTask<RestRequest> CreateDeleteRequestAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreateDeleteRequestAsync(endpoint, args);

            if (IsAuthentication == true)
            {
                var _nonce = GenerateOnlyNonce(13);

                var _post_params = _request.Parameters.ToDictionary(p => p.Name, p => p.Value);
                
                var _post_data = ToQueryString(_post_params);
                {
                    var _payload = new JwtPayload
                        {
                            { "access_key", ConnectKey },
                            { "nonce", _nonce },
                            { "query", _post_data }
                        };

                    var _secToken = new JwtSecurityToken(this.JwtHeader, _payload);
                    var _signature = JwtHandler.WriteToken(_secToken);

                    _request.AddHeader("Authorization", $"Bearer {_signature}");
                }
            }

            return await Task.FromResult(_request);
        }

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
                if (String.IsNullOrEmpty(response.Content) == false && response.Content[0] == '{')
                {
                    var _json_result = this.DeserializeObject<JToken>(response.Content);

                    //{"error":{"name":"V1::Exceptions::OrderNotFound","message":"주문을 찾지 못했습니다.","dialog":"client","origin":"member126085 order_uuid:4e493427-0ba8-4bd0-b2f1-0170ae978209"}}
                    var _json_error = _json_result.SelectToken("error");
                    if (_json_error != null)
                    {
                        var _error_code = ErrorCode.ExchangeError;
                        var _error_msg = _json_error["message"].Value<string>();

                        var _json_name = _json_error["name"];
                        if (_json_name != null)
                        {
                            var _names = _json_name.Value<string>().Split(new string[] { "::" }, StringSplitOptions.None);
                            if (_names.Length > 2)
                            {
                                var _error_enum = Enum.Parse(typeof(ErrorCode), _names[2]);
                                if (_error_enum != null)
                                    _error_code = (ErrorCode)_error_enum;
                            }
                        }

                        _result.SetFailure(_error_msg, _error_code);
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