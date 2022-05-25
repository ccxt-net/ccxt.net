using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Extension;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCXT.NET.GDAX
{
    /// <summary>
    ///
    /// </summary>
    public sealed class GdaxClient : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "GDAX";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public GdaxClient(string division)
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
        public GdaxClient(string division, string connect_key, string secret_key, string user_name, string user_password)
            : base(division, connect_key, secret_key, user_name, user_password, true)
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
                            "US"
                        },
                        Urls = new ExchangeUrls
                        {
                            logo = "https://user-images.githubusercontent.com/1294454/27766527-b1be41c6-5edb-11e7-95f6-5b496c469e2c.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://api.gdax.com" },
                                { "private", "https://api.gdax.com" },
                                { "trade", "https://api.gdax.com" },
                                { "test", "https://api-public.sandbox.gdax.com" }
                            },
                            www = "https://www.gdax.com",
                            doc = new List<string>
                            {
                                "https://docs.gdax.com"
                            },
                            fees = new List<string>
                            {
                                "https://www.gdax.com/fees",
                                "https://support.gdax.com/customer/en/portal/topics/939402-depositing-and-withdrawing-funds/articles"
                            }
                        },
                        RequiredCredentials = new RequiredCredentials
                        {
                            apikey = true,
                            secret = true,
                            uid = false,
                            login = false,
                            password = true,
                            twofa = false
                        },
                        LimitRate = new ExchangeLimitRate
                        {
                            useTotal = false,
                            token = new ExchangeLimitCalled { rate = 60000 },           //
                            @public = new ExchangeLimitCalled { rate = 334 },           // We throttle public endpoints by IP: 3 requests per second, up to 6 requests per second in bursts.
                            @private = new ExchangeLimitCalled { rate = 200 },          // We throttle private endpoints by user ID: 5 requests per second, up to 10 requests per second in bursts.
                            trade = new ExchangeLimitCalled { rate = 200 },
                            total = new ExchangeLimitCalled { rate = 20 }               // The FIX API throttles the number of incoming messages to 50 commands per second.
                        },
                        Fees = new MarketFees
                        {
                            trading = new MarketFee
                            {
                                tierBased = true,      // true for tier-based/progressive
                                percentage = true,     // fixed commission

                                maker = 0.0m / 100m,
                                taker = 0.3m / 100m
                            }
                        },
                        Timeframes = new Dictionary<string, string>
                        {
                            { "1m", "60"},
                            { "5m", "300"},
                            { "15m", "900"},
                            { "1h", "3600"},
                            { "6h", "21600"},
                            { "1d", "86400"},
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
        public override async ValueTask<RestRequest> CreatePostRequestAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreatePostRequestAsync(endpoint, args);

            if (IsAuthentication == true)
            {
                var _params = new Dictionary<string, object>();
                {
                    foreach (var _param in _request.Parameters)
                        _params.Add(_param.Name, _param.Value);

                    _request.RemoveParameters();
                }

                var _nonce = GenerateOnlyNonce(16).ToString();

                var _json_body = this.SerializeObject(_params, Formatting.None);
                {
                    var _sign_data = $"{_nonce}{_request.Method}{endpoint}{_json_body}";
                    var _signature = this.ConvertHexString(Encryptor.ComputeHash(Encoding.UTF8.GetBytes(_sign_data)));

                    _request.AddHeader("CB-ACCESS-KEY", ConnectKey);
                    _request.AddHeader("CB-ACCESS-SIGN", _signature.ToLower());
                    _request.AddHeader("CB-ACCESS-TIMESTAMP", _nonce);
                    _request.AddHeader("CB-ACCESS-PASSPHRASE", base.UserPassword);
                }

                _request.AddParameter("application/json", _json_body, ParameterType.RequestBody);
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
                if (response.IsSuccessful == true)
                {
                    var _json_result = this.DeserializeObject<JToken>(response.Content);

                    var _json_message = _json_result.SelectToken("message");
                    if (_json_message != null)
                    {
                        _result.SetFailure(
                                _json_message.Value<string>(),
                                ErrorCode.ResponseDataError
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