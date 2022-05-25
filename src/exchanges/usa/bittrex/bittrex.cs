using Newtonsoft.Json.Linq;
using CCXT.NET.Shared.Coin;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace CCXT.NET.Bittrex
{
    /// <summary>
    ///
    /// </summary>
    public sealed class BittrexClient : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "Bittrex";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public BittrexClient(string division)
            : base(division)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public BittrexClient(string division, string connect_key, string secret_key)
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
                            logo = "https://user-images.githubusercontent.com/1294454/27766352-cf0b3c26-5ed5-11e7-82b7-f3826b7a97d8.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://bittrex.com/api" },
                                { "private", "https://bittrex.com/api" },
                                { "trade", "https://bittrex.com/api" },
                                { "account", "https://bittrex.com/api" },
                                { "market", "https://bittrex.com/api" },
                                { "v2", "https://bittrex.com/api/v2.0/pub" }
                            },
                            www = "https://bittrex.com",
                            doc = new List<string>
                            {
                                "https://bittrex.com/Home/Api",
                                "https://www.npmjs.org/package/node.bittrex.api",
                                "https://github.com/thebotguys/golang-bittrex-api/wiki/Bittrex-API-Reference-(Unofficial)"
                            },
                            fees = new List<string>
                            {
                                "https://bittrex.com/Fees",
                                "https://support.bittrex.com/hc/en-us/articles/115000199651-What-fees-does-Bittrex-charge-"
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
                            token = new ExchangeLimitCalled { rate = 60000 },               // 40 request per minute
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

                                maker = 0.25m / 100m,
                                taker = 0.25m / 100m
                            }
                        },
                        Options = new ExchangeOptions
                        {
                            hasAlreadyAuthenticatedSuccessfully = false
                        },
                        Timeframes = new Dictionary<string, string>
                        {
                            { "1m", "oneMin"},
                            { "5m", "fiveMin"},
                            { "30m", "thirtyMin"},
                            { "1h", "hour"},
                            { "1d", "day" }
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
                _request.AddParameter("apikey", ConnectKey);

                var _post_params = _request.Parameters.ToDictionary(p => p.Name, p => p.Value);
                
                var _post_data = ToQueryString(_post_params);
                {
                    _request.Resource += "?" + _post_data;
                    var _sign_data = ApiUrl + _request.Resource;

                    var _signature = this.ConvertHexString(Encryptor.ComputeHash(Encoding.UTF8.GetBytes(_sign_data)));
                    _request.AddHeader("apisign", _signature);
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
        public override async ValueTask<RestRequest> CreateGetRequestAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreateGetRequestAsync(endpoint, args);

            if (IsAuthentication == true)
            {
                var _nonce = GenerateOnlyNonce(16).ToString();

                _request.AddParameter("nonce", _nonce);
                _request.AddParameter("apikey", ConnectKey);

                var _post_params = _request.Parameters.ToDictionary(p => p.Name, p => p.Value);

                var _post_data = ToQueryString(_post_params);
                {
                    var _sign_data = ApiUrl + endpoint + "?" + _post_data;
                    var _signature = this.ConvertHexString(Encryptor.ComputeHash(Encoding.UTF8.GetBytes(_sign_data)));
                    {
                        _request.AddHeader("apisign", _signature);
                    }
                }
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        ///
        /// </summary>
        public new Dictionary<string, ErrorCode> ErrorMessages = new Dictionary<string, ErrorCode>
        {
            { "APISIGN_NOT_PROVIDED", ErrorCode.AuthenticationError },
            { "INVALID_SIGNATURE", ErrorCode.AuthenticationError },
            { "INVALID_CURRENCY", ErrorCode.ExchangeError },
            { "INVALID_PERMISSION", ErrorCode.AuthenticationError },
            { "INSUFFICIENT_FUNDS", ErrorCode.InsufficientFunds },
            { "QUANTITY_NOT_PROVIDED", ErrorCode.InvalidOrder },
            { "MIN_TRADE_REQUIREMENT_NOT_MET", ErrorCode.InvalidOrder },
            { "ORDER_NOT_OPEN", ErrorCode.OrderNotFound },
            { "INVALID_ORDER", ErrorCode.InvalidOrder },
            { "UUID_INVALID", ErrorCode.OrderNotFound },
            { "RATE_NOT_PROVIDED", ErrorCode.InvalidOrder },                // createLimitBuyOrder ('ETH/BTC', 1, 0)
            { "WHITELIST_VIOLATION_IP", ErrorCode.PermissionDenied }
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
                if (String.IsNullOrEmpty(response.Content) == false && response.Content[0] == '{')
                {
                    var _error_code = ErrorCode.ExchangeError;
                    var _error_msg = "";

                    var _json_result = this.DeserializeObject<JToken>(response.Content);     // { success: false, message: "message" }
                    if (_json_result != null)
                    {
                        var _success = _json_result["success"].Value<bool>();
                        if (_success == false)
                        {
                            _error_msg = _json_result["message"].Value<string>();

                            if (_error_msg == "APIKEY_INVALID")
                            {
                                _error_code = ErrorCode.DDoSProtection;
                                _error_msg = response.Content;
                            }
                            else if (_error_msg == "DUST_TRADE_DISALLOWED_MIN_VALUE_50K_SAT")
                            {
                                _error_code = ErrorCode.InvalidOrder;
                                _error_msg = "order cost should be over 50k satoshi: " + response.Content;
                            }
                            else if (_error_msg == "INVALID_ORDER")
                            {
                                // Bittrex will return an ambiguous INVALID_ORDER message
                                // upon canceling already-canceled and closed orders
                                // therefore this special case for cancelOrder
                                // let url = 'https://bittrex.com/api/v1.1/market/cancel?apikey=API_KEY&uuid=ORDER_UUID'
                                if (response.ResponseUri.AbsoluteUri.IndexOf("cancel") >= 0)
                                {
                                    var _ulrs = response.ResponseUri.AbsoluteUri.Split('?');
                                    if (_ulrs.Length > 1)
                                    {
                                        var _parts = _ulrs[1].Split('&');

                                        var _order_id = "";
                                        foreach (var _part in _parts)
                                        {
                                            var _key_value = _part.Split('=');
                                            if (_key_value[0] == "uuid")
                                            {
                                                _order_id = _key_value[1];
                                                break;
                                            }
                                        }

                                        _error_code = ErrorCode.OrderNotFound;

                                        if (String.IsNullOrEmpty(_order_id) == false)
                                            _error_msg = $"cancelOrder: {_order_id} => " + response.Content;
                                        else
                                            _error_msg = $"cancelOrder: " + response.Content;
                                    }
                                }
                            }
                            else if (ErrorMessages.ContainsKey(_error_msg) == true)
                            {
                                _error_code = ErrorMessages[_error_msg];
                                _error_msg = response.Content;
                            }
                            else if (String.IsNullOrEmpty(_error_msg) == false)
                            {
                                if (_error_msg.IndexOf("throttled. Try again") >= 0)
                                {
                                    _error_code = ErrorCode.DDoSProtection;
                                    _error_msg = response.Content;
                                }
                                else if (_error_msg.IndexOf("problem") >= 0)
                                {
                                    _error_code = ErrorCode.ExchangeNotAvailable;   // 'There was a problem processing your request.  If this problem persists, please contact...')
                                    _error_msg = response.Content;
                                }
                            }

                            _result.SetFailure(_error_msg, _error_code);
                        }
                    }
                    else
                    {
                        _error_msg = "malformed response: " + response.Content;
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