using Newtonsoft.Json.Linq;
using CCXT.NET.Shared.Coin;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCXT.NET.Bitstamp
{
    /// <summary>
    ///
    /// </summary>
    public sealed class BitstampClient : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "Bitstamp";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public BitstampClient(string division)
            : base(division)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        /// <param name="user_name"></param>
        /// <param name="user_password"></param>
        public BitstampClient(string division, string connect_key, string secret_key, string user_name, string user_password)
            : base(division, connect_key, secret_key, user_name, user_password, authentication: true)
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
                            "GB"
                        },
                        Urls = new ExchangeUrls
                        {
                            logo = "https://user-images.githubusercontent.com/1294454/27786377-8c8ab57e-5fe9-11e7-8ea4-2b05b6bcceec.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://www.bitstamp.net/api" },
                                { "private", "https://www.bitstamp.net/api" },
                                { "trade", "https://www.bitstamp.net/api" }
                            },
                            www = "https://www.bitstamp.net",
                            doc = new List<string>
                            {
                                "https://www.bitstamp.net/api"
                            }
                        },
                        NonceStyle = NonceStyle.UnixMilliseconds,
                        RequiredCredentials = new RequiredCredentials
                        {
                            apikey = true,
                            secret = true,
                            uid = true,
                            login = false,
                            password = false,
                            twofa = false
                        },
                        LimitRate = new ExchangeLimitRate
                        {
                            useTotal = false,
                            token = new ExchangeLimitCalled { rate = 60000 },           // 60 request per minute
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

                                maker = 0.25m / 100m,
                                taker = 0.25m / 100m
                            }
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
                var _nonce = GenerateOnlyNonce(13);
                var _sign_data = $"{_nonce}{UserName}{ConnectKey}";

                var _signature = this.ConvertHexString(Encryptor.ComputeHash(Encoding.UTF8.GetBytes(_sign_data)));
                {
                    _request.AddParameter("key", ConnectKey);
                    _request.AddParameter("signature", _signature.ToUpper());
                    _request.AddParameter("nonce", _nonce);
                }
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        ///
        /// </summary>
        public new Dictionary<string, ErrorCode> ErrorMessages = new Dictionary<string, ErrorCode>
        {
            { "No permission found", ErrorCode.PermissionDenied },
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
                if (String.IsNullOrEmpty(response.Content) == false && (response.Content[0] == '{' || response.Content[0] == '['))
                {
                    var _json_result = this.DeserializeObject<JToken>(response.Content);

                    var _error_code = ErrorCode.ExchangeError;
                    var _error_msg = response.Content;

                    var _json_error = _json_result.SelectToken("error");
                    if (_json_error != null)
                    {
                        var _error = _json_error.Value<string>();
                        if (String.IsNullOrEmpty(_error) == false)
                        {
                            if (ErrorMessages.ContainsKey(_error) == true)
                                _error_code = ErrorMessages[_error];

                            _error_msg = _error;
                        }

                        _result.SetFailure(_error_msg, _error_code);
                    }

                    if (_result.success == true)
                    {
                        var _json_status = _json_result.SelectToken("status");
                        if (_json_status != null)
                        {
                            var _status = _json_status.Value<string>();
                            if (_status == "error")
                            {
                                var _json_code = _json_result.SelectToken("code");
                                if (_json_code != null)
                                {
                                    var _code = _json_code.Value<string>();
                                    if (_code == "API0005")
                                    {
                                        _error_code = ErrorCode.AuthenticationError;
                                        _error_msg = "invalid signature, use the uid for the main account if you have subaccounts";
                                    }
                                }

                                _result.SetFailure(_error_msg, _error_code);
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