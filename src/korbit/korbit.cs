using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET.Korbit
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class KorbitClient : OdinSdk.BaseLib.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        /// 
        /// </summary>
        public override string DealerName { get; set; } = "Korbit";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public KorbitClient(string division)
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
        public KorbitClient(string division, string connect_key, string secret_key, string user_name, string user_password)
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
                            "KR"
                        },
                        Urls = new ExchangeUrls
                        {
                            logo = "https://d3esrl798jsx2v.cloudfront.net/share/logo/logo-landing.png",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://api.korbit.co.kr/v1" },
                                { "private", "https://api.korbit.co.kr/v1" },
                                { "trade", "https://api.korbit.co.kr/v1" }
                            },
                            www = "https://www.korbit.co.kr/",
                            doc = new List<string>
                            {
                                "https://apidocs.korbit.co.kr/"
                            },
                            fees = new List<string>
                            {
                                "https://www.korbit.co.kr/orders/trading_volumes"
                            }
                        },
                        RequiredCredentials = new RequiredCredentials
                        {
                            apikey = true,
                            secret = true,
                            uid = false,
                            login = true,
                            password = true,
                            twofa = false
                        },
                        LimitRate = new ExchangeLimitRate
                        {
                            useTotal = false,
                            token = new ExchangeLimitCalled { rate = 60000 },          // Creating / refreshing access token calls are limited to 60 calls per 60 minutes
                            @public = new ExchangeLimitCalled { rate = 1000 },         // Ticker calls are limited to 60 calls per 60 seconds
                            @private = new ExchangeLimitCalled { rate = 1000 },
                            trade = new ExchangeLimitCalled { rate = 1000 },
                            total = new ExchangeLimitCalled { rate = 84 }              // All other calls combined, are limited to 12 calls per 1 second.
                        },
                        Fees = new MarketFees
                        {
                            trading = new MarketFee
                            {
                                tierBased = false,      // true for tier-based/progressive
                                percentage = false,     // fixed commission

                                maker = 0.08m / 100m,
                                taker = 0.20m / 100m
                            }
                        }
                    };
                }

                return base.ExchangeInfo;
            }
        }

        private static AccessToken __access_token = new AccessToken();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AccessToken> GetAccessToken()
        {
            if (__access_token.success == true)
            {
                if (__access_token.CheckExpired() == true)
                    __access_token = await GetAccessToken("refresh_token", __access_token.refreshToken);
            }

            if (__access_token.success == false)
            {
                __access_token = await GetAccessToken("password");
                if (__access_token.success == false)
                    throw new ArgumentException(__access_token.message);
            }

            return __access_token;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AccessToken> GetAccessToken(string grant_type, string refresh_token = "")
        {
            var _result = new AccessToken();

            var _params = new Dictionary<string, object>();
            {
                _params.Add("client_id", ConnectKey);
                _params.Add("client_secret", SecretKey);

                _params.Add("grant_type", grant_type);

                if (grant_type == "password")
                {
                    _params.Add("username", UserName);
                    _params.Add("password", UserPassword);
                }
                else
                    _params.Add("refresh_token", refresh_token);
            }

            var _request = await base.CreatePostRequest("/oauth2/access_token", _params);
            {
                var _access_token = await base.RestExecuteAsync(_request);
                if (_access_token.IsSuccessful == true)
                {
                    _result = this.DeserializeObject<AccessToken>(_access_token.Content);
                    _result.SetSuccess();
                }
                else
                {
                    _result.SetFailure(
                            $"get access token error: {_access_token.Content}, grant_type: {grant_type}, refresh_token: {refresh_token}"
                        );
                }
            }

            return _result;
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
                var _nonce = GenerateOnlyNonce(13);

                var _access_token = await GetAccessToken();
                if (_access_token != null)
                    _request.AddHeader("Authorization", $"{_access_token.tokenType} {_access_token.accessToken}");

                _request.AddParameter("nonce", _nonce);
            }

            return _request;
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
                var _access_token = await GetAccessToken();
                if (_access_token != null)
                    _request.AddHeader("Authorization", $"{_access_token.tokenType} {_access_token.accessToken}");
            }

            return _request;
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
                    if (String.IsNullOrEmpty(response.Content) == false && response.Content != "[]")
                    {
                        var _json_result = this.DeserializeObject<JToken>(response.Content);

                        var _json_status = _json_result.SelectToken("status");
                        if (_json_status != null)
                        {
                            var _json_message = _json_status.Value<string>();
                            if (_json_message != "success")
                            {
                                _result.SetFailure(
                                        _json_message,
                                        ErrorCode.ResponseDataError
                                    );
                            }
                        }
                    }
                    else
                    {
                        _result.SetFailure(errorCode: ErrorCode.NotFoundData);
                    }
                }

                if (_result.success == true && response.IsSuccessful == false)
                {
                    var _message = response.ErrorMessage ?? response.StatusDescription;

                    var _warning = response.Headers.Where(h => h.Name.ToLower() == "warning").SingleOrDefault();
                    if (_warning != null)
                        _message = _warning.Value.ToString();

                    _result.SetFailure(
                            _message,
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