using CCXT.NET.Coin;
using CCXT.NET.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CCXT.NET.Huobi
{
    /// <summary>
    ///
    /// </summary>
    public sealed class HuobiClient : CCXT.NET.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "Huobi";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public HuobiClient(string division)
            : base(division)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public HuobiClient(string division, string connect_key, string secret_key)
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
                            logo = "https://user-images.githubusercontent.com/1294454/27766569-15aa7b9a-5edd-11e7-9e7f-44791f4ee49c.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://api.huobi.pro" },
                                { "private", "https://api.huobi.pro" },
                                { "trade", "https://api.huobi.pro" }
                            },
                            www = "https://www.huobi.com",
                            doc = new List<string>
                            {
                                "https://github.com/huobiapi/API_Docs_en/wiki"
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
                            token = new ExchangeLimitCalled { rate = 60000 },
                            @public = new ExchangeLimitCalled { rate = 1000 },
                            @private = new ExchangeLimitCalled { rate = 1000 },
                            trade = new ExchangeLimitCalled { rate = 1000 },
                            total = new ExchangeLimitCalled { rate = 1000 }     // up to 3000 requests per 5 minutes ≈ 600 requests per minute ≈ 10 requests per second ≈ 100 ms
                        },
                        Fees = new MarketFees
                        {
                            trading = new MarketFee
                            {
                                tierBased = false,      // true for tier-based/progressive
                                percentage = false,     // fixed commission

                                maker = 0.2m / 100m,
                                taker = 0.2m / 100m
                            }
                        },
                        Timeframes = new Dictionary<string, string>
                        {
                            { "1m","1min" },
                            { "5m","5min" },
                            { "15m","15min" },
                            { "30m","30min" },
                            { "1h","60min" },
                            { "1d","1day" },
                            { "1w","1week" },
                            { "1M","1mon" },
                            { "1y","1year" }
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
        /// <param name="str"></param>
        /// <returns></returns>
        public string UrlEncode(string str)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in str)
            {
                if (HttpUtility.UrlEncode(c.ToString(), Encoding.UTF8).Length > 1)
                {
                    builder.Append(HttpUtility.UrlEncode(c.ToString(), Encoding.UTF8).ToUpper());
                }
                else
                {
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="method"></param>
        /// <param name="host"></param>
        /// <param name="resourcePath"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string GetSignatureStr(Method method, string host, string resourcePath, string parameters)
        {
            var sign = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append(method.ToString().ToUpper()).Append("\n")
                .Append(host).Append("\n")
                .Append(resourcePath).Append("\n");

            var paraArray = parameters.Split('&');
            List<string> parametersList = new List<string>();
            foreach (var item in paraArray)
            {
                parametersList.Add(item);
            }
            parametersList.Sort(delegate (string s1, string s2)
            {
                return string.CompareOrdinal(s1, s2);
            });
            foreach (var item in parametersList)
            {
                sb.Append(item).Append("&");
            }
            sign = sb.ToString().TrimEnd('&');

            sign = Convert.ToBase64String(Encryptor.ComputeHash(Encoding.UTF8.GetBytes(sign)));
            return UrlEncode(sign);
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
                var _params = new Dictionary<string, object>();
                {
                    _params.Add("AccessKeyId", ConnectKey);
                    _params.Add("SignatureMethod", "HmacSHA256");
                    _params.Add("SignatureVersion", 2);
                    _params.Add("Timestamp", CUnixTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"));
                }

                var _post_data = ToQueryString(_params);
                var _signature = GetSignatureStr(Method.POST, ApiUrl.Replace("https://", ""), endpoint, _post_data);

                _post_data += "&Signature=" + _signature;

                _request.AddHeader("Content-Type", "application/json");
                _request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36");
                _request.Resource += $"?{_post_data}";
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async Task<IRestRequest> CreateGetRequest(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreateGetRequest($"{endpoint}", args);

            if (IsAuthentication == true)
            {
                var _params = new Dictionary<string, object>();
                {
                    _params.Add("AccessKeyId", ConnectKey);
                    _params.Add("SignatureMethod", "HmacSHA256");
                    _params.Add("SignatureVersion", 2);
                    _params.Add("Timestamp", CUnixTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"));
                }

                if (args.Count > 0)
                    foreach (var arg in args)
                        _params.Add(arg.Key, arg.Value.ToString());

                _request.Parameters.Clear();

                var _post_data = ToQueryString(_params);
                var _signature = GetSignatureStr(Method.GET, ApiUrl.Replace("https://", ""), endpoint, _post_data);

                _post_data += "&Signature=" + _signature;

                _request.AddHeader("Content-Type", "application/json");
                _request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36");
                _request.Resource += $"?{_post_data}";
            }

            return await Task.FromResult(_request);
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
                    var _json_result = this.DeserializeObject<JToken>(response.Content);

                    var _json_error = _json_result.SelectToken("status");
                    if (_json_error != null && _json_error.Value<string>() != "ok")
                    {
                        var _error_msg = _json_result.SelectToken("err-msg").Value<string>();
                        _result.SetFailure(
                                  _error_msg,
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