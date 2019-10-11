using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCXT.NET.ItBit
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ItbitClient : OdinSdk.BaseLib.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "ItBit";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public ItbitClient(string division)
            : base(division)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public ItbitClient(string division, string connect_key, string secret_key)
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
                            logo = "https://user-images.githubusercontent.com/1294454/27822159-66153620-60ad-11e7-89e7-005f6d7f3de0.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://api.itbit.com/v1" },
                                { "private", "https://api.itbit.com/v1" },
                                { "trade", "https://api.itbit.com" },
                                { "beta", "https://beta-api.itbit.com" },
                                { "v2", "https://www.itbit.com/api/v2" }
                            },
                            www = "https://www.itbit.com",
                            doc = new List<string>
                            {
                                "https://api.itbit.com/docs",
                                "https://www.itbit.com/api"
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
                            token = new ExchangeLimitCalled { rate = 60000 },               // 30 request per minute
                            @public = new ExchangeLimitCalled { rate = 2000 },
                            @private = new ExchangeLimitCalled { rate = 2000 },
                            trade = new ExchangeLimitCalled { rate = 2000 },
                            total = new ExchangeLimitCalled { rate = 2000 }
                        },
                        Fees = new MarketFees
                        {
                            trading = new MarketFee
                            {
                                tierBased = false,      // true for tier-based/progressive
                                percentage = false,     // fixed commission

                                maker = 0.0m,
                                taker = 0.0m
                            }
                        },
                    };
                }

                return base.ExchangeInfo;
            }
        }

        private SHA256Managed __sha256 = null;

        /// <summary>
        ///
        /// </summary>
        public SHA256Managed Sha256Managed
        {
            get
            {
                if (__sha256 == null)
                    __sha256 = new SHA256Managed();

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
        public override async Task<IRestRequest> CreateGetRequest(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreateGetRequest(endpoint, args);

            if (IsAuthentication == true)
            {
                var _post_data = ToQueryString(_request.Parameters);
                {
                    var _query = _post_data.Length > 0 ? "?" + _post_data : "";

                    var _method = _request.Method.ToString();
                    var _url = ApiUrl + endpoint + _query;
                    var _body = "";
                    var _nonce = GenerateOnlyNonce(16).ToString();
                    var _timestamp = GenerateNonceString(13);

                    var _json_sign = this.SerializeObject(new[]
                    {
                        _method,
                        _url,
                        _body,
                        _nonce,
                        _timestamp
                    }, Formatting.None);

                    var _sign_nonce = Sha256Managed.ComputeHash(Encoding.UTF8.GetBytes(_nonce + _json_sign));
                    var _sign_data = Encoding.UTF8.GetBytes(_url).Concat(_sign_nonce).ToArray();

                    var _signature = Convert.ToBase64String(Encryptor.ComputeHash(_sign_data));
                    {
                        _request.AddHeader("Authorization", $"{ConnectKey}:{_signature}");
                        _request.AddHeader("X-Auth-Timestamp", _timestamp);
                        _request.AddHeader("X-Auth-Nonce", _nonce);
                    }
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
        public override async Task<IRestRequest> CreatePostRequest(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreatePostRequest(endpoint, args);

            if (IsAuthentication == true)
            {
                var _params = new Dictionary<string, object>();
                {
                    foreach (var _param in _request.Parameters)
                        _params.Add(_param.Name, _param.Value);

                    _request.Parameters.Clear();
                }

                var _post_data = this.SerializeObject(_params, Formatting.None);
                {
                    var _method = _request.Method.ToString();
                    var _url = ApiUrl + endpoint;
                    var _body = _post_data;
                    var _nonce = GenerateOnlyNonce(16).ToString();
                    var _timestamp = GenerateNonceString(13);

                    var _json_sign = this.SerializeObject(new[]
                    {
                        _method,
                        _url,
                        _body,
                        _nonce,
                        _timestamp
                    }, Formatting.None);

                    var _sign_nonce = Sha256Managed.ComputeHash(Encoding.UTF8.GetBytes(_nonce + _json_sign));
                    var _sign_data = Encoding.UTF8.GetBytes(_url).Concat(_sign_nonce).ToArray();

                    var _signature = Convert.ToBase64String(Encryptor.ComputeHash(_sign_data));
                    {
                        _request.AddHeader("Authorization", $"{ConnectKey}:{_signature}");
                        _request.AddHeader("X-Auth-Timestamp", _timestamp);
                        _request.AddHeader("X-Auth-Nonce", _nonce);
                    }

                    _request.AddParameter(new Parameter
                    {
                        ContentType = "",
                        Name = "application/json",
                        Type = ParameterType.RequestBody,
                        Value = _post_data
                    });
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
        public override async Task<IRestRequest> CreateDeleteRequest(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreateDeleteRequest(endpoint, args);

            if (IsAuthentication == true)
            {
                var _params = new Dictionary<string, object>();
                {
                    foreach (var _param in _request.Parameters)
                        _params.Add(_param.Name, _param.Value);

                    _request.Parameters.Clear();
                }

                var _delete_data = _params.Count > 0 ? this.SerializeObject(_params, Formatting.None) : "";
                {
                    var _method = _request.Method.ToString();
                    var _url = ApiUrl + endpoint;
                    var _body = _delete_data;
                    var _nonce = GenerateOnlyNonce(16).ToString();
                    var _timestamp = GenerateNonceString(13);

                    var _json_sign = this.SerializeObject(new[]
                    {
                        _method,
                        _url,
                        _body,
                        _nonce,
                        _timestamp
                    }, Formatting.None);

                    var _sign_nonce = Sha256Managed.ComputeHash(Encoding.UTF8.GetBytes(_nonce + _json_sign));
                    var _sign_data = Encoding.UTF8.GetBytes(_url).Concat(_sign_nonce).ToArray();

                    var _signature = Convert.ToBase64String(Encryptor.ComputeHash(_sign_data));
                    {
                        _request.AddHeader("Authorization", $"{ConnectKey}:{_signature}");
                        _request.AddHeader("X-Auth-Timestamp", _timestamp);
                        _request.AddHeader("X-Auth-Nonce", _nonce);
                    }

                    _request.AddParameter(new Parameter
                    {
                        ContentType = "",
                        Name = "application/json",
                        Type = ParameterType.RequestBody,
                        Value = _delete_data
                    });
                }
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
                if (response.IsSuccessful == false)
                {
                    var _json_result = this.DeserializeObject<JToken>(response.Content);

                    var _description = _json_result.SelectToken("description");
                    var _statusValue = _json_result.SelectToken("code");

                    if (_statusValue != null && _description != null)
                    {
                        var _json_message = _description.Value<string>();
                        var _json_code = _statusValue.Value<int>();

                        _result.SetFailure(
                                _json_message,
                                ErrorCode.ResponseDataError,
                                _json_code
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