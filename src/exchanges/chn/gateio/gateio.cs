using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using CCXT.NET.Shared.Coin;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace CCXT.NET.GateIO
{
    /// <summary>
    ///
    /// </summary>
    public sealed class GateIOClient : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "GateIO";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public GateIOClient(string division)
            : base(division)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public GateIOClient(string division, string connect_key, string secret_key)
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
                            "CN"
                        },
                        Urls = new ExchangeUrls
                        {
                            logo = "https://user-images.githubusercontent.com/1294454/31784029-0313c702-b509-11e7-9ccc-bc0da6a0e435.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://data.gate.io/api2" },
                                { "private", "https://data.gate.io/api2" },
                                { "trade", "https://data.gate.io/api2" },
                            },
                            www = "https://gate.io",
                            doc = new List<string>
                            {
                                "https://gate.io/api2"
                            },
                            fees = new List<string>
                            {
                                "https://gate.io/fee",
                                "https://support.gate.io/hc/en-us/articles/115003577673"
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
                            @public = new ExchangeLimitCalled { rate = 1000 },
                            @private = new ExchangeLimitCalled { rate = 1000 },
                            trade = new ExchangeLimitCalled { rate = 1000 },
                            total = new ExchangeLimitCalled { rate = 1000 }
                        },
                        Fees = new MarketFees
                        {
                            trading = new MarketFee
                            {
                                tierBased = true,      // true for tier-based/progressive
                                percentage = true,     // fixed commission

                                maker = 0.2m / 100m,
                                taker = 0.2m / 100m
                            }
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
                var _post_params = _request.Parameters.ToDictionary(p => p.Name, p => p.Value);

                var _post_data = ToQueryString(_post_params);
                {
                    var _signature = this.ConvertHexString(Encryptor.ComputeHash(Encoding.UTF8.GetBytes(_post_data))).Replace("-", "").ToLower();

                    _request.AddHeader("Sign", _signature);
                    _request.AddHeader("Key", ConnectKey);
                    _request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
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
                if (String.IsNullOrEmpty(response.Content) == false)
                {
                    var _json_result = this.DeserializeObject<JObject>(response.Content);

                    var _json_success = true;

                    if (_json_result.ContainsKey("result"))
                        _json_success = _json_result["result"].Value<bool>();
                    else
                        _json_success = _json_result.Count > 0;

                    if (_json_success != true)
                    {
                        var _error_code = ErrorCode.ExchangeError;
                        var _error_msg = _error_code.ToString();

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