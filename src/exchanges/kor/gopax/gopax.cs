using Newtonsoft.Json.Linq;
using CCXT.NET.Shared.Coin;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCXT.NET.GOPAX
{
    /// <summary>
    ///
    /// </summary>
    public sealed class GopaxClient : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "GOPAX";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public GopaxClient(string division)
            : base(division)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public GopaxClient(string division, string connect_key, string secret_key)
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
                            logo = "https://www.gopax.co.kr/images/brand/logo-primary.svg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://api.gopax.co.kr" },
                                { "private", "https://api.gopax.co.kr" },
                                { "trade", "https://api.gopax.co.kr" }
                            },
                            www = "https://www.gopax.co.kr/",
                            doc = new List<string>
                            {
                                "https://www.gopax.co.kr/API/",
                                "https://github.com/gopaxapi/gopax",
                                "https://gopaxapi.github.io/gopax/"
                            },
                            fees = new List<string>
                            {
                                "https://www.gopax.co.kr/feeinfo"
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
                            token = new ExchangeLimitCalled { rate = 60000 },         // 최근 1초의 구간 안에서 최대 20번의 API 호출이 가능합니다.
                            @public = new ExchangeLimitCalled { rate = 50 },
                            @private = new ExchangeLimitCalled { rate = 50 },
                            trade = new ExchangeLimitCalled { rate = 50 },
                            total = new ExchangeLimitCalled { rate = 50 }
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
                            { "1m", "1"},
                            { "5m", "5"},
                            { "30m", "30"},
                            { "1d", "1440"}
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
        public override async ValueTask<RestRequest> CreateGetRequestAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreateGetRequestAsync(endpoint, args);

            if (IsAuthentication == true)
            {
                var _nonce = GenerateOnlyNonce(13).ToString();

                var _post_data = "";    //ToQueryString(_request.Parameters);
                {
                    var _sign_data = $"{_nonce}{_request.Method}{endpoint}{_post_data}";
                    var _signature = Convert.ToBase64String(Encryptor.ComputeHash(Encoding.UTF8.GetBytes(_sign_data)));

                    _request.AddHeader("API-KEY", ConnectKey);
                    _request.AddHeader("SIGNATURE", _signature);
                    _request.AddHeader("NONCE", _nonce);
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
        public override async ValueTask<RestRequest> CreatePostRequestAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreatePostRequestAsync(endpoint, args);

            if (IsAuthentication == true)
            {
                var _nonce = GenerateOnlyNonce(13).ToString();

                var _post_data = "";    //ToQueryString(_request.Parameters);
                {
                    var _sign_data = $"{_nonce}{_request.Method}{endpoint}{_post_data}";
                    var _signature = Convert.ToBase64String(Encryptor.ComputeHash(Encoding.UTF8.GetBytes(_sign_data)));

                    _request.AddHeader("API-KEY", ConnectKey);
                    _request.AddHeader("SIGNATURE", _signature);
                    _request.AddHeader("NONCE", _nonce);
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
                var _nonce = GenerateOnlyNonce(13).ToString();

                var _post_data = "";    //ToQueryString(_request.Parameters);
                {
                    var _sign_data = $"{_nonce}{_request.Method}{endpoint}{_post_data}";
                    var _signature = Convert.ToBase64String(Encryptor.ComputeHash(Encoding.UTF8.GetBytes(_sign_data)));

                    _request.AddHeader("API-KEY", ConnectKey);
                    _request.AddHeader("SIGNATURE", _signature);
                    _request.AddHeader("NONCE", _nonce);
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

                    var _json_message = _json_result.SelectToken("errormsg");
                    if (_json_message != null)
                    {
                        _result.SetFailure(
                                _json_message.Value<string>(),
                                ErrorCode.ExchangeError
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