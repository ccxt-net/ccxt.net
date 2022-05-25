using Newtonsoft.Json.Linq;
using CCXT.NET.Shared.Coin;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace CCXT.NET.Poloniex
{
    /// <summary>
    ///
    /// </summary>
    public sealed class PoloniexClient : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "Poloniex";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public PoloniexClient(string division)
            : base(division)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public PoloniexClient(string division, string connect_key, string secret_key)
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
                            logo = "https://user-images.githubusercontent.com/1294454/27766817-e9456312-5ee6-11e7-9b3c-b628ca5626a5.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://poloniex.com/public" },
                                { "private", "https://poloniex.com/tradingApi" },
                                { "trade", "https://poloniex.com/tradingApi" }
                            },
                            www = "https://poloniex.com",
                            doc = new List<string>
                            {
                                "https://poloniex.com/support/api/",
                                "http://pastebin.com/dMX7mZE0"
                            },
                            fees = new List<string>
                            {
                                "https://poloniex.com/fees"
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
                            useTotal = true,
                            token = new ExchangeLimitCalled { rate = 60000 },
                            @public = new ExchangeLimitCalled { rate = 1000 },
                            @private = new ExchangeLimitCalled { rate = 1000 },
                            trade = new ExchangeLimitCalled { rate = 1000 },
                            total = new ExchangeLimitCalled { rate = 500 }              // up to 6 calls per second
                        },
                        Fees = new MarketFees
                        {
                            trading = new MarketFee
                            {
                                tierBased = false,      // true for tier-based/progressive
                                percentage = false,     // fixed commission

                                maker = 0.15m / 100m,
                                taker = 0.25m / 100m
                            }
                        },
                        CurrencyIds = new Dictionary<string, string>
                        {
                            { "BTM", "Bitmark" },
                            { "STR", "XLM" },
                            { "BCC", "BTCtalkcoin" }
                        },
                        Timeframes = new Dictionary<string, string>
                        {
                            { "5m", "300"},
                            { "15m", "900"},
                            { "30m", "1800"},
                            { "2h", "7200"},
                            { "4h", "14400"},
                            { "1d", "86400"},
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
                var _nonce = GenerateOnlyNonce(16).ToString();

                _request.AddParameter("nonce", _nonce);

                var _post_params = _request.Parameters.ToDictionary(p => p.Name, p => p.Value);

                var _post_data = ToQueryString(_post_params);
                {
                    var _signature = this.ConvertHexString(Encryptor.ComputeHash(Encoding.UTF8.GetBytes(_post_data)));

                    _request.AddHeader("Sign", _signature);
                    _request.AddHeader("Key", ConnectKey);
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
                if (String.IsNullOrEmpty(response.Content) == false && response.Content != "[]")
                {
                    var _json_result = this.DeserializeObject<JToken>(response.Content);

                    var _json_error = _json_result.SelectToken("error");
                    if (_json_error == null)
                    {
                        var _json_success = _json_result.SelectToken("success");
                        if (_json_success != null && _json_success.Value<long>() != 1)
                        {
                            var _message = _result.message;

                            var _json_message = _json_result.SelectToken("response");
                            if (_json_message != null)
                                _message = _json_message.Value<string>();

                            _result.SetFailure(_message, ErrorCode.ResponseDataError);
                        }
                    }
                    else
                    {
                        _result.SetFailure(_json_error.Value<string>(), ErrorCode.ResponseDataError);
                    }
                }
                else
                {
                    _result.SetFailure(errorCode: ErrorCode.NotFoundData);
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