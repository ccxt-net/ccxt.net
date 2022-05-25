using Newtonsoft.Json.Linq;
using CCXT.NET.Shared.Coin;
using RestSharp;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace CCXT.NET.Bitflyer
{
    /// <summary>
    ///
    /// </summary>
    public sealed class BitflyerClient : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "Bitflyer";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public BitflyerClient(string division)
            : base(division)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public BitflyerClient(string division, string connect_key, string secret_key)
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
                            "JP"
                        },
                        Urls = new ExchangeUrls
                        {
                            logo = "https://user-images.githubusercontent.com/1294454/28051642-56154182-660e-11e7-9b0d-6042d1e6edd8.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://api.bitflyer.jp" },
                                { "private", "https://api.bitflyer.jp" },
                                { "trade", "https://api.bitflyer.jp" }
                            },
                            www = "https://bitflyer.jp",
                            doc = new List<string>
                            {
                                "https://lightning.bitflyer.jp/docs?lang=en",
                                "https://bitflyer.jp/API"
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
                                tierBased = false,      // true for tier-based/progressive
                                percentage = false,     // fixed commission

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
                var _nonce = GenerateOnlyNonce(16).ToString();

                var _post_params = _request.Parameters.ToDictionary(p => p.Name, p => p.Value);

                var _post_data = ToQueryString(_post_params);
                {
                    var _sign_data = $"{_nonce}{_request.Method}{endpoint}{_post_data}";
                    var _signature = this.ConvertHexString(Encryptor.ComputeHash(Encoding.UTF8.GetBytes(_sign_data)));

                    _request.AddHeader("ACCESS-KEY", ConnectKey);
                    _request.AddHeader("ACCESS-TIMESTAMP", _nonce);
                    _request.AddHeader("ACCESS-SIGN", _signature);
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
                if (response.IsSuccessful == true)
                {
                    if (response.Content != "")
                    {
                        var _json_result = this.DeserializeObject<JToken>(response.Content);

                        var _json_message = _json_result.SelectToken("error_message");
                        if (_json_message != null)
                        {
                            _result.SetFailure(
                                      _json_message.Value<string>(),
                                      ErrorCode.ResponseDataError
                                  );
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