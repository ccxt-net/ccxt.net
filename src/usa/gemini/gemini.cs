using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCXT.NET.Gemini
{
    /// <summary>
    ///
    /// </summary>
    public sealed class GeminiClient : OdinSdk.BaseLib.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "Gemini";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public GeminiClient(string division)
            : base(division)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public GeminiClient(string division, string connect_key, string secret_key)
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
                            logo = "https://user-images.githubusercontent.com/1294454/27816857-ce7be644-6096-11e7-82d6-3c257263229c.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://api.gemini.com" },
                                { "private", "https://api.gemini.com" },
                                { "trade", "https://api.gemini.com" },
                                { "test", "https://api.sandbox.gemini.com" }
                            },
                            www = "https://gemini.com",
                            doc = new List<string>
                            {
                                "https://docs.gemini.com/rest-api",
                                "https://docs.sandbox.gemini.com"
                            },
                            fees = new List<string>
                            {
                                "https://gemini.com/fee-schedule/",
                                "https://gemini.com/transfer-fees/"
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

                                maker = 0.25m / 100m,
                                taker = 0.25m / 100m
                            }
                        }
                    };
                }

                return base.ExchangeInfo;
            }
        }

        private HMACSHA384 __encryptor = null;

        /// <summary>
        ///
        /// </summary>
        public HMACSHA384 Encryptor
        {
            get
            {
                if (__encryptor == null)
                    __encryptor = new HMACSHA384(Encoding.UTF8.GetBytes(SecretKey));

                return __encryptor;
            }
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

                var _nonce = GenerateOnlyNonce(16).ToString();

                _params.Add("request", endpoint);
                _params.Add("nonce", _nonce);

                var _post_data = this.SerializeObject(_params, Formatting.None);
                {
                    var _sign_data = Convert.ToBase64String(Encoding.UTF8.GetBytes(_post_data));
                    var _signature = this.ConvertHexString(Encryptor.ComputeHash(Encoding.UTF8.GetBytes(_sign_data)));

                    _request.AddHeader("X-GEMINI-APIKEY", ConnectKey);
                    _request.AddHeader("X-GEMINI-PAYLOAD", _sign_data);
                    _request.AddHeader("X-GEMINI-SIGNATURE", _signature);
                }
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        ///
        /// </summary>
        public new Dictionary<string, string> ErrorMessages = new Dictionary<string, string>
        {
            { "AuctionNotOpen", "Failed to place an auction-only order because there is no current auction open for this symbol" },
            { "ClientOrderIdTooLong", "The Client Order ID must be under 100 characters" },
            { "ClientOrderIdMustBeString", "The Client Order ID must be a string" },
            { "ConflictingOptions", "New orders using a combination of order execution options are not supported" },
            { "EndpointMismatch", "The request was submitted to an endpoint different than the one in the payload" },
            { "EndpointNotFound", "No endpoint was specified" },
            { "IneligibleTiming", "Failed to place an auction order for the current auction on this symbol because the timing is not eligible: new orders may only be placed before the auction begins." },
            { "InsufficientFunds", "The order was rejected because of insufficient funds" },
            { "InvalidJson", "The JSON provided is invalid" },
            { "InvalidNonce", "The nonce was not greater than the previously used nonce, or was not present" },
            { "InvalidOrderType", "An unknown order type was provided" },
            { "InvalidPrice", "For new orders, the price was invalid" },
            { "InvalidQuantity", "A negative or otherwise invalid quantity was specified" },
            { "InvalidSide", "For new orders, and invalid side was specified" },
            { "InvalidSignature", "The signature did not match the expected signature" },
            { "InvalidSymbol", "An invalid symbol was specified" },
            { "InvalidTimestampInPayload", "The JSON payload contained a timestamp parameter with an unsupported value." },
            { "Maintenance", "The system is down for maintenance" },
            { "MarketNotOpen", "The order was rejected because the market is not accepting new orders" },
            { "MissingApikeyHeader", "The X-GEMINI-APIKEY header was missing" },
            { "MissingOrderField", "A required order_id field was not specified" },
            { "MissingRole", "The API key used to access this endpoint does not have the required role assigned to it" },
            { "MissingPayloadHeader", "The X-GEMINI-PAYLOAD header was missing" },
            { "MissingSignatureHeader", "The X-GEMINI-SIGNATURE header was missing" },
            { "NoSSL", "You must use HTTPS to access the API" },
            { "OptionsMustBeArray", "The options parameter must be an array." },
            { "OrderNotFound", "The order specified was not found" },
            { "RateLimit", "Requests were made too frequently. See Rate limit below." },
            { "System", "We are experiencing technical issues" },
            { "UnsupportedOption", "This order execution option is not supported." }
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
                if (String.IsNullOrEmpty(response.Content) == false && (response.Content[0] == '{' || response.Content[0] == '['))
                {
                    var _json_result = this.DeserializeObject<JToken>(response.Content);

                    var _json_reason = _json_result.SelectToken("reason");
                    if (_json_reason != null)
                    {
                        var _json_message = _json_result.SelectToken("message");
                        if (_json_message != null)
                        {
                            var _error_code = ErrorCode.ExchangeError;
                            var _error_msg = _json_message.Value<string>();

                            var _reason = _json_reason.Value<string>();
                            if (ErrorMessages.ContainsKey(_reason) == true)
                            {
                                _error_msg = ErrorMessages[_reason];

                                var _error_enum = Enum.Parse(typeof(ErrorCode), _reason);
                                if (_error_enum != null)
                                    _error_code = (ErrorCode)_error_enum;
                            }

                            _result.SetFailure(_error_msg, _error_code);
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