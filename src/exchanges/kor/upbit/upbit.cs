using CCXT.NET.Shared.Coin;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCXT.NET.Upbit
{
    /// <summary>
    /// UPBIT cryptocurrency exchange client implementation
    /// Official API Documentation: https://docs.upbit.com/kr/reference
    /// Authentication: JWT Bearer Token with HMAC SHA256 signature
    /// API Version: v1.5.7 (표준화 완성 버전)
    /// </summary>
    public sealed class UpbitClient : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        /// Exchange name identifier
        /// </summary>
        public override string DealerName { get; set; } = "Upbit";

        /// <summary>
        /// Initialize Upbit client for public API access only
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public UpbitClient(string division)
            : base(division)
        {
        }

        /// <summary>
        /// Initialize Upbit client with authentication for private API access
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public UpbitClient(string division, string connect_key, string secret_key)
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
                            logo = "https://user-images.githubusercontent.com/1294454/49245610-eeaabe00-f423-11e8-9cba-4b0aed794799.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://api.upbit.com/v1" },
                                { "private", "https://api.upbit.com/v1" },
                                { "trade", "https://api.upbit.com/v1" },
                                { "public.old", "https://crix-api-endpoint.upbit.com/v1/crix" },
                                { "private.old", "https://ccx.upbit.com/api/v1" },
                                { "trade.old", "https://ccx.upbit.com/api/v1" },
                                { "dunamu", "https://quotation-api-cdn.dunamu.com/v1/forex" },
                                { "ccx", "https://ccx.upbit.com/api/v1" },
                                { "cs3", "https://s3.ap-northeast-2.amazonaws.com" }
                            },
                            www = "https://upbit.com",
                            doc = new List<string>
                            {
                                "https://docs.upbit.com/kr/reference",
                                "https://docs.upbit.com/"
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
                            token = new ExchangeLimitCalled { rate = 60000 },           // 40 requests per minute
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

                                maker = 0.05m / 100m,
                                taker = 0.05m / 100m
                            }
                        },
                        Timeframes = new Dictionary<string, string>
                        {
                            { "1m", "minutes" },
                            { "3m", "minutes" },
                            { "5m", "minutes" },
                            { "10m", "minutes" },
                            { "15m", "minutes" },
                            { "30m", "minutes" },
                            { "60m", "minutes" },
                            { "240m", "minutes" },
                            { "1d", "days" },
                            { "1w", "weeks" },
                            { "1M", "months" },
                        }
                    };
                }

                return base.ExchangeInfo;
            }
        }

        /// <summary>
        /// Standard error code mappings for UPBIT API responses
        /// Reference: https://docs.upbit.com/kr/reference  
        /// Based on official UPBIT API v1.5.7 documentation
        /// </summary>
        public new Dictionary<int, string> ErrorMessages = new Dictionary<int, string>
        {
            { 0, "Success" },
            
            // HTTP Status Codes
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
            
            // UPBIT Specific Error Codes from official documentation
            { 40001, "Invalid market format" },
            { 40002, "Market not found" },
            { 40003, "Invalid order type" },
            { 40004, "Invalid order side" },
            { 40005, "Insufficient funds" },
            { 40006, "Order not found" },
            { 40007, "Invalid order amount" },
            { 40008, "Invalid order price" },
            { 40009, "Market trading suspended" },
            { 40010, "Order already cancelled" },
            { 40011, "Order cannot be cancelled" },
            { 40012, "Minimum order amount not met" },
            { 40013, "Maximum order amount exceeded" },
            { 40014, "Invalid withdraw address" },
            { 40015, "Withdraw amount too small" },
            { 40016, "Withdraw amount too large" },
            { 40017, "Daily withdraw limit exceeded" },
            { 40018, "Authentication required" },
            { 40019, "Two-factor authentication required" },
            { 40020, "Account verification required" }
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

        private JwtHeader __jwt_header = null;

        /// <summary>
        /// JWT header for UPBIT API authentication
        /// Uses HMAC SHA256 signature algorithm
        /// </summary>
        public JwtHeader JwtHeader
        {
            get
            {
                if (__jwt_header == null)
                {
                    var _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
                    var _credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

                    __jwt_header = new JwtHeader(_credentials);
                }

                return __jwt_header;
            }
        }

        private JwtSecurityTokenHandler __jwt_handler = null;

        /// <summary>
        /// JWT token handler for creating Bearer tokens
        /// </summary>
        public JwtSecurityTokenHandler JwtHandler
        {
            get
            {
                if (__jwt_handler == null)
                    __jwt_handler = new JwtSecurityTokenHandler();

                return __jwt_handler;
            }
        }

        /// <summary>
        /// Create authenticated POST request with JWT Bearer token
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns>Configured RestSharp request</returns>
        public override async ValueTask<RestRequest> CreatePostRequestAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreatePostRequestAsync(endpoint, args);

            if (IsAuthentication)
            {
                var _nonce = GenerateOnlyNonce(13);
                var _post_params = _request.Parameters.ToDictionary(p => p.Name, p => p.Value);
                var _post_data = ToQueryString(_post_params);

                var _payload = new JwtPayload
                {
                    { "access_key", ConnectKey },
                    { "nonce", _nonce },
                    { "query", _post_data }
                };

                var _secToken = new JwtSecurityToken(this.JwtHeader, _payload);
                var _signature = JwtHandler.WriteToken(_secToken);

                _request.AddHeader("Authorization", $"Bearer {_signature}");
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        /// Create authenticated GET request with JWT Bearer token
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns>Configured RestSharp request</returns>
        public override async ValueTask<RestRequest> CreateGetRequestAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreateGetRequestAsync(endpoint, args);

            if (IsAuthentication)
            {
                var _nonce = GenerateOnlyNonce(13);
                var _post_params = _request.Parameters.ToDictionary(p => p.Name, p => p.Value);
                var _post_data = ToQueryString(_post_params);

                var _payload = new JwtPayload
                {
                    { "access_key", ConnectKey },
                    { "nonce", _nonce },
                    { "query", _post_data }
                };

                var _secToken = new JwtSecurityToken(this.JwtHeader, _payload);
                var _signature = JwtHandler.WriteToken(_secToken);

                _request.AddHeader("Authorization", $"Bearer {_signature}");
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        /// Create authenticated DELETE request with JWT Bearer token
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns>Configured RestSharp request</returns>
        public override async ValueTask<RestRequest> CreateDeleteRequestAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await base.CreateDeleteRequestAsync(endpoint, args);

            if (IsAuthentication)
            {
                var _nonce = GenerateOnlyNonce(13);
                var _post_params = _request.Parameters.ToDictionary(p => p.Name, p => p.Value);
                var _post_data = ToQueryString(_post_params);

                var _payload = new JwtPayload
                {
                    { "access_key", ConnectKey },
                    { "nonce", _nonce },
                    { "query", _post_data }
                };

                var _secToken = new JwtSecurityToken(this.JwtHeader, _payload);
                var _signature = JwtHandler.WriteToken(_secToken);

                _request.AddHeader("Authorization", $"Bearer {_signature}");
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        /// Standardized response message processing for UPBIT API
        /// Handles both HTTP errors and UPBIT-specific error responses
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
                    if (!String.IsNullOrEmpty(response.Content) && response.Content[0] == '{')
                    {
                        var _json_result = this.DeserializeObject<JToken>(response.Content);

                        // Standard UPBIT error format: {"error":{"name":"V1::Exceptions::OrderNotFound","message":"주문을 찾지 못했습니다."}}
                        var _json_error = _json_result.SelectToken("error");
                        if (_json_error != null)
                        {
                            var _error_code = ErrorCode.ExchangeError;
                            var _error_msg = _json_error["message"]?.Value<string>() ?? "Unknown error";

                            // Parse error name to determine error type
                            var _json_name = _json_error["name"];
                            if (_json_name != null)
                            {
                                var _error_name = _json_name.Value<string>();
                                var _names = _error_name.Split(new string[] { "::" }, StringSplitOptions.None);
                                
                                if (_names.Length > 2)
                                {
                                    // Map UPBIT error names to standard error codes
                                    _error_code = _names[2] switch
                                    {
                                    "OrderNotFound" => ErrorCode.OrderNotFound,
                                    "InsufficientFunds" => ErrorCode.InsufficientFunds,
                                    "InvalidOrder" => ErrorCode.InvalidOrder,
                                    "InvalidAmount" => ErrorCode.InvalidAmount,
                                    "InvalidPrice" => ErrorCode.InvalidOrder,
                                    "MarketNotFound" => ErrorCode.ExchangeError,
                                    "AuthenticationError" => ErrorCode.AuthenticationError,
                                    "PermissionDenied" => ErrorCode.PermissionDenied,
                                    "TooManyRequests" => ErrorCode.RateLimit,
                                    _ => ErrorCode.ExchangeError
                                    };
                                }
                            }

                            _result.SetFailure(_error_msg, _error_code);
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