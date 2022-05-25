using Newtonsoft.Json.Linq;
using CCXT.NET.Shared.Coin;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCXT.NET.Kraken
{
    /// <summary>
    ///
    /// </summary>
    public sealed class KrakenClient : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "Kraken";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public KrakenClient(string division)
            : base(division)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public KrakenClient(string division, string connect_key, string secret_key)
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
                            "US"
                        },
                        Urls = new ExchangeUrls
                        {
                            logo = "https://user-images.githubusercontent.com/1294454/27766599-22709304-5ede-11e7-9de1-9f33732e1509.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://api.kraken.com" },
                                { "private", "https://api.kraken.com" },
                                { "trade", "https://api.kraken.com" },
                                { "zendesk", "https://support.kraken.com/hc/en-us/articles" }
                            },
                            www = "https://www.kraken.com",
                            doc = new List<string>
                            {
                                "https://www.kraken.com/en-us/help/api",
                                "https://github.com/nothingisdead/npm-kraken-api",
                                "https://bitbucket.org/arrivets/krakenapi"
                            },
                            fees = new List<string>
                            {
                                "https://www.kraken.com/en-us/help/fees"
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
                            @public = new ExchangeLimitCalled { rate = 3000 },
                            @private = new ExchangeLimitCalled { rate = 3000 },
                            trade = new ExchangeLimitCalled { rate = 3000 },
                            total = new ExchangeLimitCalled { rate = 2000 }
                        },
                        Fees = new MarketFees
                        {
                            trading = new MarketFee
                            {
                                tierBased = false,      // true for tier-based/progressive
                                percentage = false,     // fixed commission

                                maker = 0.26m / 100m,
                                taker = 0.16m / 100m
                            }
                        },
                        Timeframes = new Dictionary<string, string>
                        {
                            { "1m", "1"},
                            { "5m", "5"},
                            { "15m", "15"},
                            { "30m", "30"},
                            { "1h", "60"},
                            { "4h", "240"},
                            { "1d", "1440"},
                            { "7d", "10080"},
                            { "15d", "21600"}
                        }
                    };
                }

                return base.ExchangeInfo;
            }
        }

        private SHA256 __sha256 = null;

        /// <summary>
        ///
        /// </summary>
        public SHA256 Sha256Managed
        {
            get
            {
                if (__sha256 == null)
                    __sha256 = SHA256.Create();

                return __sha256;
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
                    __encryptor = new HMACSHA512(Convert.FromBase64String(SecretKey));

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
                var _nonce = GenerateOnlyNonce(16);
                _request.AddParameter("nonce", _nonce);

#if DEBUG
                Debug.WriteLine($"{this.DealerName}: nonce => '{_nonce}'");
#endif

                var _post_params = _request.Parameters.ToDictionary(p => p.Name, p => p.Value);

                var _post_data = ToQueryString(_post_params);
                {
                    var _sign_nonce = Sha256Managed.ComputeHash(Encoding.UTF8.GetBytes($"{_nonce}{_post_data}"));
                    var _sign_data = Encoding.UTF8.GetBytes(endpoint).Concat(_sign_nonce).ToArray();

                    var _signature = Convert.ToBase64String(Encryptor.ComputeHash(_sign_data));

                    _request.AddHeader("API-Key", ConnectKey);
                    _request.AddHeader("API-Sign", _signature);
                }

                _request.AddParameter("application/x-www-form-urlencoded", _post_data, ParameterType.RequestBody);
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        ///
        /// </summary>
        public new Dictionary<string, ErrorCode> ErrorMessages = new Dictionary<string, ErrorCode>
        {
            { "Invalid order", ErrorCode.InvalidOrder },
            { "Invalid nonce",  ErrorCode.InvalidNonce },
            { "Insufficient funds",  ErrorCode.InsufficientFunds },
            { "Cancel pending",  ErrorCode.CancelPending },
            { "Invalid arguments:volume",  ErrorCode.InvalidOrder },
            { "Too many address",  ErrorCode.TooManyAddress },
            { "Invalid amount",  ErrorCode.InvalidAmount },
            { "Unknown reference id",  ErrorCode.InvalidOrder },
            { "Unknown order",  ErrorCode.UnknownOrder }
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
                if (response.IsSuccessful == true)
                {
                    var _json_result = this.DeserializeObject<JToken>(response.Content);

                    var _json_error = _json_result.SelectToken("error");
                    if (_json_error != null && _json_error.Count() > 0)
                    {
                        var _message = "";
                        {
                            if (_json_error.GetType() == typeof(JArray))
                            {
                                if (_json_error.Count() > 0)
                                    _message = _json_error[0].Value<string>();
                            }
                            else if (_json_error.GetType() == typeof(JValue))
                            {
                                _message = _json_error.Value<string>();
                            }
                        }

                        if (String.IsNullOrEmpty(_message) == false)
                        {
                            var _error_code = ErrorMessages
                                                    .Where(e => _message.IndexOf(e.Key) >= 0)
                                                    .OrderByDescending(e => e.Key.Length)
                                                    .FirstOrDefault();

                            if (_error_code.Key != null)
                                _result.errorCode = _error_code.Value;
                            else
                                _result.errorCode = ErrorCode.ResponseDataError;

                            _result.SetFailure(
                                    _message,
                                    _result.errorCode
                                );
                        }
                        else
                        {
                            var _token_result = _json_result.SelectToken("result");
                            if (_token_result == null ||
                                    (
                                        _token_result != null &&
                                        _token_result.GetType() == typeof(JArray) &&
                                        _token_result.Count() <= 0
                                     )
                                )
                            {
                                _result.SetFailure(errorCode: ErrorCode.NotFoundData);
                            }
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