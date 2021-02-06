using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCXT.NET.Binance
{
    /// <summary>
    ///
    /// </summary>
    public sealed class BinanceClient : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "Binance";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public BinanceClient(string division)
            : base(division)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public BinanceClient(string division, string connect_key, string secret_key)
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
                            "HK"
                        },
                        Urls = new ExchangeUrls
                        {
                            logo = "https://user-images.githubusercontent.com/1294454/29604020-d5483cdc-87ee-11e7-94c7-d1a8d9169293.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://api.binance.com/api/v3" },
                                { "private", "https://api.binance.com/api/v3" },
                                { "trade", "https://api.binance.com/api/v3" },
                                { "web", "https://www.binance.com" },
                                { "wapi", "https://api.binance.com/wapi/v3" },
                                { "v3", "https://api.binance.com/api/v3" },
                                { "v1", "https://api.binance.com/api/v1" }
                            },
                            www = "https://www.binance.com",
                            doc = new List<string>
                            {
                                "https://github.com/binance-exchange/binance-official-api-docs/blob/master/rest-api.md",
                                "https://github.com/binance-exchange/binance-official-api-docs/blob/master/wapi-api.md"
                            },
                            fees = new List<string>
                            {
                                "https://www.binance.com/fees.html"
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
                            token = new ExchangeLimitCalled { rate = 60000 },                // 120 request per minute
                            @public = new ExchangeLimitCalled { rate = 500 },
                            @private = new ExchangeLimitCalled { rate = 500 },
                            trade = new ExchangeLimitCalled { rate = 500 },
                            total = new ExchangeLimitCalled { rate = 500 }
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
                        CurrencyIds = new Dictionary<string, string>
                        {
                            { "YOYO", "YOYOW" },
                            { "BCC", "BCH" },
                            { "NANO", "XRB" }
                        },
                        Options = new ExchangeOptions
                        {
                            hasAlreadyAuthenticatedSuccessfully = false
                        },
                        Timeframes = new Dictionary<string, string>
                        {
                            { "1m", "1m"},
                            { "3m", "3m"},
                            { "5m", "5m"},
                            { "15m", "15m"},
                            { "30m", "30m"},
                            { "1h", "1h"},
                            { "2h", "2h"},
                            { "4h", "4h"},
                            { "6h", "6h"},
                            { "8h", "8h"},
                            { "12h", "12h"},
                            { "1d", "1d"},
                            { "3d", "3d"},
                            { "1w", "1w"},
                            { "1M", "1M" }
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
        public override async ValueTask<IRestRequest> CreatePostRequestAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreatePostRequestAsync(endpoint, args);

            if (IsAuthentication == true)
            {
                var _params = new Dictionary<string, object>();
                {
                    foreach (var _param in _request.Parameters)
                        _params.Add(_param.Name, _param.Value);

                    _request.Parameters.Clear();
                }

                _params.Add("recvWindow", 60 * 1000);
                _params.Add("timestamp", CUnixTime.NowMilli);

                var _post_data = ToQueryString(_params);
                {
                    var _signature = this.ConvertHexString(Encryptor.ComputeHash(Encoding.UTF8.GetBytes(_post_data)));
                    _post_data += $"&signature={_signature}";

                    _request.Resource += $"?{_post_data}";
                }

                _request.AddHeader("X-MBX-APIKEY", ConnectKey);
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public override async ValueTask<IRestRequest> CreateGetRequestAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreateGetRequestAsync(endpoint, args);

            if (IsAuthentication == true)
            {
                _request.AddParameter("recvWindow", 60 * 1000);
                _request.AddParameter("timestamp", CUnixTime.NowMilli);

                var _post_params = _request.Parameters.ToDictionary(p => p.Name, p => p.Value);

                var _post_data = ToQueryString(_post_params);
                {
                    var _signature = this.ConvertHexString(Encryptor.ComputeHash(Encoding.UTF8.GetBytes(_post_data)));
                    _request.AddParameter("signature", _signature);

                    _request.AddHeader("X-MBX-APIKEY", ConnectKey);
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
        public override async ValueTask<IRestRequest> CreateDeleteRequestAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreateDeleteRequestAsync(endpoint, args);

            if (IsAuthentication == true)
            {
                var _params = new Dictionary<string, object>();
                {
                    foreach (var _param in _request.Parameters)
                        _params.Add(_param.Name, _param.Value);

                    _request.Parameters.Clear();
                }

                _params.Add("recvWindow", 60 * 1000);
                _params.Add("timestamp", CUnixTime.NowMilli);

                var _post_data = ToQueryString(_params);
                {
                    var _signature = this.ConvertHexString(Encryptor.ComputeHash(Encoding.UTF8.GetBytes(_post_data)));
                    _post_data += $"&signature={_signature}";

                    _request.Resource += $"?{_post_data}";
                }

                _request.AddHeader("X-MBX-APIKEY", ConnectKey);
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        ///
        /// </summary>
        public new Dictionary<int, ErrorCode> ErrorMessages = new Dictionary<int, ErrorCode>
        {
                { -1000, ErrorCode.ExchangeNotAvailable }, 	    // {"code":-1000,"msg":"An unknown error occured while processing the request."}
                { -1013, ErrorCode.InvalidOrder }, 		        // createOrder -> invalid quantity/invalid price/MIN_NOTIONAL
                { -1021, ErrorCode.InvalidNonce }, 		        // your time is ahead of server
                { -1100, ErrorCode.InvalidOrder }, 		        // createOrder(symbol, 1, asdf) -> Illegal characters found in parameter price
                { -2010, ErrorCode.InsufficientFunds }, 	    // createOrder -> Account has insufficient balance for requested action.
                { -2011, ErrorCode.OrderNotFound }, 	        // cancelOrder(1, BTC/USDT) -> UNKNOWN_ORDER
                { -2013, ErrorCode.OrderNotFound }, 	        // fetchOrder (1, BTC/USDT) -> Order does not exist
                { -2014, ErrorCode.AuthenticationError }, 	    // { "code":-2014, "msg", "API-key format invalid." }
                { -2015, ErrorCode.AuthenticationError } 	    // "Invalid API-key, IP, or permissions for action."
        };

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
                var _error_code = ErrorCode.Success;
                var _error_msg = "";

                if ((int)response.StatusCode == 418 || (int)response.StatusCode == 429)
                {
                    _error_code = ErrorCode.DDoSProtection;
                    _error_msg = response.Content;
                }
                else if ((int)response.StatusCode >= 400)
                {
                    if (response.Content.IndexOf("Price * QTY is zero or less") >= 0)
                    {
                        _error_code = ErrorCode.InvalidOrder;
                        _error_msg = "order cost = amount * price is zero or less " + response.Content;
                    }
                    else if (response.Content.IndexOf("LOT_SIZE") >= 0)
                    {
                        _error_code = ErrorCode.InvalidOrder;
                        _error_msg = "order amount should be evenly divisible by lot size, use this.amountToLots (symbol, amount) " + response.Content;
                    }
                    else if (response.Content.IndexOf("PRICE_FILTER") >= 0)
                    {
                        _error_code = ErrorCode.InvalidOrder;
                        _error_msg = "order price exceeds allowed price precision or invalid, use this.priceToPrecision (symbol, amount) " + response.Content;
                    }
                }

                if (_error_code == ErrorCode.Success)
                {
                    if (String.IsNullOrEmpty(response.Content) == false && response.Content[0] == '{')
                    {
                        var _json_result = this.DeserializeObject<JToken>(response.Content);

                        // check success value for wapi endpoints
                        // response in format {'msg': 'The coin does not exist.', 'success': true/false}

                        var _json_success = _json_result.SelectToken("success");
                        if (_json_success != null)
                        {
                            var _success = _json_success.Value<bool>();
                            if (_success == false)
                            {
                                var _json_message = _json_result.SelectToken("msg");
                                if (_json_message != null)
                                {
                                    var _msg = _json_message.Value<string>();
                                    _json_result = this.DeserializeObject<JToken>(_msg);
                                }
                            }
                        }

                        var _json_error = _json_result.SelectToken("code");
                        if (_json_error != null)
                        {
                            _result.statusCode = _json_error.Value<int>();

                            if (ErrorMessages.ContainsKey(_result.statusCode) == true)
                            {
                                // a workaround for {"code":-2015,"msg":"Invalid API-key, IP, or permissions for action."}
                                // despite that their message is very confusing, it is raised by Binance
                                // on a temporary ban (the API key is valid, but disabled for a while)

                                if (_result.statusCode == -2015 && this.ExchangeInfo.Options.hasAlreadyAuthenticatedSuccessfully == true)
                                {
                                    _error_code = ErrorCode.DDoSProtection;
                                    _error_msg = "temporary banned: " + response.Content;
                                }
                                else
                                {
                                    var _json_message = _json_result.SelectToken("msg");
                                    if (_json_message != null)
                                    {
                                        var _msg = _json_message.Value<string>();

                                        if (_msg == "Order would trigger immediately.")
                                        {
                                            _error_code = ErrorCode.InvalidOrder;
                                            _error_msg = response.Content;
                                        }
                                        else if (_msg == "Account has insufficient balance for requested action.")
                                        {
                                            _error_code = ErrorCode.InsufficientFunds;
                                            _error_msg = response.Content;
                                        }
                                        else if (_msg == "Rest API trading is not enabled.")
                                        {
                                            _error_code = ErrorCode.ExchangeNotAvailable;
                                            _error_msg = response.Content;
                                        }
                                    }

                                    if (_error_code == ErrorCode.Success)
                                    {
                                        _error_code = ErrorMessages[_result.statusCode];
                                        _error_msg = response.Content;
                                    }
                                }
                            }
                            else
                            {
                                _error_code = ErrorCode.ExchangeError;
                                _error_msg = "unknown error code: " + response.Content;
                            }
                        }
                        else if (_result.success == false)
                        {
                            _error_code = ErrorCode.ExchangeError;
                            _error_msg = "success value false: " + response.Content;
                        }
                    }
                }

                if (_error_code != ErrorCode.Success)
                    _result.SetFailure(_error_msg, _error_code);

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