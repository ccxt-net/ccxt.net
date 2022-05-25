using Newtonsoft.Json.Linq;
using CCXT.NET.Shared.Coin;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCXT.NET.CEXIO
{
    /// <summary>
    ///
    /// </summary>
    public sealed class CexioClient : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "CEXIO";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public CexioClient(string division)
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
        public CexioClient(string division, string connect_key, string secret_key, string user_name, string user_password)
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
                            "GB",
                            "EU",
                            "CY",
                            "RU"
                        },
                        Urls = new ExchangeUrls
                        {
                            logo = "https://user-images.githubusercontent.com/1294454/27766442-8ddc33b0-5ed8-11e7-8b98-f786aef0f3c9.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://cex.io/api" },
                                { "private", "https://cex.io/api" },
                                { "trade", "https://cex.io/api" }
                            },
                            www = "https://cex.io",
                            doc = new List<string>
                            {
                                "https://cex.io/cex-api"
                            },
                            fees = new List<string>
                            {
                                "https://cex.io/fee-schedule",
                                "https://cex.io/limit-commissions"
                            }
                        },
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
                            token = new ExchangeLimitCalled { rate = 60000 },           // 40 request per minute
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

                                maker = 0.16m / 100m,
                                taker = 0.25m / 100m
                            }
                        },
                        Timeframes = new Dictionary<string, string>
                        {
                            {  "1m", "1m" },
                            {  "1h", "1h" },
                            {  "1d", "1d" }
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
                var _nonce = GenerateOnlyNonce(10).ToString();

                var _sign_data = $"{_nonce}{UserName}{ConnectKey}";
                var _signature = this.ConvertHexString(Encryptor.ComputeHash(Encoding.UTF8.GetBytes(_sign_data)));

                _request.AddParameter("key", ConnectKey);
                _request.AddParameter("signature", _signature);
                _request.AddParameter("nonce", _nonce);
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
                if (response.IsSuccessful == true)
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