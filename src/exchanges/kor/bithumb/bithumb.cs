using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Extension;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCXT.NET.Bithumb
{
    /// <summary>
    /// BITHUMB cryptocurrency exchange client implementation
    /// Official API Documentation: https://apidocs.bithumb.com/reference
    /// Authentication: HMAC SHA512 signature with API key
    /// API Version: v1.0 (표준화 완성 버전)
    /// </summary>
    public sealed class BithumbClient : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        /// Exchange name identifier
        /// </summary>
        public override string DealerName { get; set; } = "Bithumb";

        /// <summary>
        /// Initialize Bithumb client for public API access only
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public BithumbClient(string division)
            : base(division)
        {
        }

        /// <summary>
        /// Initialize Bithumb client with authentication for private API access
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public BithumbClient(string division, string connect_key, string secret_key)
            : base(division, connect_key, secret_key, authentication: true)
        {
        }

        /// <summary>
        /// Exchange configuration and information
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
                            logo = "https://user-images.githubusercontent.com/1294454/30597177-ea800172-9d5e-11e7-804c-b9d4fa9b56b0.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://api.bithumb.com/public" },
                                { "private", "https://api.bithumb.com" },
                                { "trade", "https://api.bithumb.com" },
                                { "web", "https://www.bithumb.com" }
                            },
                            www = "https://www.bithumb.com",
                            doc = new List<string>
                            {
                                "https://www.bithumb.com/u1/US127"
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
                            token = new ExchangeLimitCalled { rate = 60000 },            // 120 request per minute
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

                                maker = 0.15m / 100m,
                                taker = 0.15m / 100m
                            }
                        },
                        Timeframes = new Dictionary<string, string>
                        {
                            { "1m", "01M"},
                            { "3m", "03M"},
                            { "5m", "05M"},
                            { "10m", "10M"},
                            { "30m", "30M"},
                            { "1h", "01H"},
                            { "6h", "06H"},
                            { "12h", "12H"},
                            { "1d", "24H"}
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

            if (IsAuthentication)
            {
                var _params = new Dictionary<string, object>();
                {
                    foreach (var _param in _request.Parameters)
                        _params.Add(_param.Name, _param.Value);

                    _request.RemoveParameters();
                }

                _params.Add("endpoint", endpoint);

                var _post_data = ToQueryString2(_params);
                {
                    var _nonce = GenerateOnlyNonce(13).ToString();

                    var _sign_data = $"{endpoint}\0{_post_data}\0{_nonce}";
                    var _sign_hash = Encryptor.ComputeHash(Encoding.UTF8.GetBytes(_sign_data));

                    var _signature = Convert.ToBase64String(Encoding.UTF8.GetBytes(ConvertHexString(_sign_hash).ToLower()));
                    {
                        _request.AddHeader("Api-Key", ConnectKey);
                        _request.AddHeader("Api-Sign", _signature);
                        _request.AddHeader("Api-Nonce", _nonce);
                    }

                    _request.AddParameter("application/x-www-form-urlencoded", _post_data, ParameterType.RequestBody);
                }
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        /// Standard error code mappings for BITHUMB API responses
        /// Reference: https://apidocs.bithumb.com/reference
        /// Based on official BITHUMB API v1.0 documentation
        /// </summary>
        public new Dictionary<int, string> ErrorMessages = new Dictionary<int, string>
        {
            { 0, "Success" },
            
            // HTTP Status Codes (표준)
            { 200, "OK - Request successful" },
            { 400, "Bad Request - Invalid parameters" },
            { 401, "Unauthorized - Invalid API key or signature" },
            { 403, "Forbidden - Access denied or insufficient permissions" },
            { 404, "Not Found - Endpoint or resource not found" },
            { 422, "Unprocessable Entity - Validation error" },
            { 429, "Too Many Requests - Rate limit exceeded" },
            { 500, "Internal Server Error" },
            { 502, "Bad Gateway" },
            { 503, "Service Unavailable" },
            { 504, "Gateway Timeout" },
            
            // BITHUMB Specific Error Codes from official documentation
            { 5100, "Bad request" },
            { 5200, "Not member" },
            { 5300, "Invalid apikey" },
            { 5302, "Method not allowed" },
            { 5400, "Database fail" },
            { 5500, "Invalid parameter" },
            { 5600, "Custom notice (output error messages in context)" },
            { 5900, "Unknown error" },
            
            // 추가 BITHUMB 에러 코드
            { 5001, "Order not found" },
            { 5002, "Insufficient funds" },
            { 5003, "Invalid order type" },
            { 5004, "Invalid order amount" },
            { 5005, "Market not found" },
            { 5006, "Order already cancelled" },
            { 5007, "Minimum order amount not met" },
            { 5008, "Maximum order amount exceeded" },
            { 5009, "Market trading suspended" },
            { 5010, "Invalid withdraw address" }
        };

        /// <summary>
        /// Get standardized error message for error code
        /// </summary>
        /// <param name="error_code">Error code from API response</param>
        /// <returns>Human-readable error message</returns>
        public override string GetErrorMessage(int error_code)
        {
            return ErrorMessages.ContainsKey(error_code)
                ? ErrorMessages[error_code]
                : $"Unknown error code: {error_code}";
        }

        /// <summary>
        /// Standardized response message processing for BITHUMB API
        /// Handles both HTTP errors and BITHUMB-specific error responses
        /// </summary>
        /// <param name="response">response value arrive from exchange's server</param>
        /// <returns>Standardized result with success/failure status</returns>
        public override BoolResult GetResponseMessage(RestResponse response = null)
        {
            var _result = new BoolResult();

            if (response != null)
            {
                // Handle successful HTTP responses with potential API errors
                if (response.IsSuccessful)
                {
                    if (!String.IsNullOrEmpty(response.Content) && (response.Content[0] == '{' || response.Content[0] == '['))
                    {
                        var _json_result = this.DeserializeObject<JToken>(response.Content);

                        // Standard BITHUMB error format: {"status":"5300","message":"Invalid apikey"}
                        var _json_status = _json_result.SelectToken("status");
                        if (_json_status != null)
                        {
                            var _status_code = _json_status.Value<int>();
                            if (_status_code != 0)
                            {
                                var _error_msg = GetErrorMessage(_status_code);

                                var _json_message = _json_result.SelectToken("message");
                                if (_json_message != null)
                                    _error_msg = _json_message.Value<string>();

                                // Map BITHUMB error codes to standard error codes
                                var _error_code = _status_code switch
                                {
                                    5001 => ErrorCode.OrderNotFound,
                                    5002 => ErrorCode.InsufficientFunds,
                                    5003 => ErrorCode.InvalidOrder,
                                    5004 => ErrorCode.InvalidAmount,
                                    5005 => ErrorCode.ExchangeError,
                                    5006 => ErrorCode.OrderNotFound,
                                    5100 => ErrorCode.ExchangeError,
                                    5200 => ErrorCode.AuthenticationError,
                                    5300 => ErrorCode.AuthenticationError,
                                    5302 => ErrorCode.ExchangeError,
                                    5400 => ErrorCode.ExchangeNotAvailable,
                                    5500 => ErrorCode.ExchangeError,
                                    5600 => ErrorCode.ExchangeError,
                                    5900 => ErrorCode.ExchangeError,
                                    _ => ErrorCode.ExchangeError
                                };

                                _result.SetFailure(_error_msg, _error_code, _status_code);
                            }
                        }
                    }
                }
                else
                {
                    // Handle HTTP-level errors
                    var _http_error_code = (int)response.StatusCode;
                    var _error_message = GetErrorMessage(_http_error_code);
                    
                    // Map HTTP status codes to standard error codes
                    var _error_code = _http_error_code switch
                    {
                        400 => ErrorCode.ExchangeError,
                        401 => ErrorCode.AuthenticationError,
                        403 => ErrorCode.PermissionDenied,
                        404 => ErrorCode.ExchangeError,
                        429 => ErrorCode.RateLimit,
                        500 => ErrorCode.ExchangeNotAvailable,
                        502 => ErrorCode.ExchangeNotAvailable,
                        503 => ErrorCode.ExchangeNotAvailable,
                        504 => ErrorCode.ExchangeNotAvailable,
                        _ => ErrorCode.ExchangeError
                    };

                    _result.SetFailure(
                        _error_message,
                        _error_code,
                        _http_error_code,
                        false
                    );
                }

                // Final check for REST-level errors
                if (_result.success && !response.IsSuccessful)
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