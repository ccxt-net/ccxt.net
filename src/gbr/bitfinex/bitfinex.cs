using OdinSdk.BaseLib.Coin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCXT.NET.Bitfinex
{
    /// <summary>
    ///
    /// </summary>
    public sealed class BitfinexClient : OdinSdk.BaseLib.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "Bitfinex";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public BitfinexClient(string division)
            : base(division)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public BitfinexClient(string division, string connect_key, string secret_key)
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
                            "VG"
                        },
                        Urls = new ExchangeUrls
                        {
                            logo = "https://user-images.githubusercontent.com/1294454/27766244-e328a50c-5ed2-11e7-947b-041416579bb3.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://api.bitfinex.com" },
                                { "private", "https://api.bitfinex.com" },
                                { "trade", "https://api.bitfinex.com" }
                            },
                            www = "https://www.bitfinex.com",
                            doc = new List<string>
                            {
                                "https://bitfinex.readme.io/v1/docs",
                                "https://github.com/bitfinexcom/bitfinex-api-node",
                                "https://docs.bitfinex.com/docs"
                            },
                            fees = new List<string>
                            {
                                "https://www.bitfinex.com/fees"
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

                                maker = 0.1m / 100m,
                                taker = 0.2m / 100m
                            }
                        },
                        AmountMultiplier = new Dictionary<string, decimal>
                        {
                        },
                        CurrencyIds = new Dictionary<string, string>
                        {
                            { "BCC", "CST_BCC" },
                            { "BCU", "CST_BCU" },
                            { "DAT", "DATA" },
                            { "DSH", "DASH" },               // Bitfinex names Dash as DSH, instead of DASH
                            { "IOT", "IOTA" },
                            { "MNA", "MANA" },
                            { "QSH", "QASH" },
                            { "QTM", "QTUM" },
                            { "SNG", "SNGLS" },
                            { "SPK", "SPANK" },
                            { "YYW", "YOYOW" }
                        },
                        Timeframes = new Dictionary<string, string>
                        {
                            { "1m", "1m"},
                            { "5m", "5m"},
                            { "15m", "15m"},
                            { "30m", "30m"},
                            { "1h", "1h"},
                            { "3h", "3h"},
                            { "6h", "6h"},
                            { "12h", "12h"},
                            { "1d", "1D"},
                            { "1w", "7D"},
                            { "2w", "14D"},
                            { "1M", "1M" }
                        },
                        CurrencyNicks = new Dictionary<string, string>
                        {
                            { "AGI", "agi" },
                            { "AID", "aid" },
                            { "AIO", "aio" },
                            { "ANT", "ant" },
                            { "AVT", "aventus" }, // #1811
                            { "BAT", "bat" },
                            { "BCH", "bcash" }, // undocumented
                            { "BCI", "bci" },
                            { "BFT", "bft" },
                            { "BTC", "bitcoin" },
                            { "BTG", "bgold" },
                            { "CFI", "cfi" },
                            { "DAI", "dai" },
                            { "DASH", "dash" },
                            { "DATA", "datacoin" },
                            { "DTH", "dth" },
                            { "EDO", "eidoo" }, // #1811
                            { "ELF", "elf" },
                            { "EOS", "eos" },
                            { "ETC", "ethereumc" },
                            { "ETH", "ethereum" },
                            { "ETP", "metaverse" },
                            { "FUN", "fun" },
                            { "GNT", "golem" },
                            { "IOST", "ios" },
                            { "IOTA", "iota" },
                            { "LRC", "lrc" },
                            { "LTC", "litecoin" },
                            { "MANA", "mna" },
                            { "MIT", "mit" },
                            { "MTN", "mtn" },
                            { "NEO", "neo" },
                            { "ODE", "ode" },
                            { "OMG", "omisego" },
                            { "OMNI", "mastercoin" },
                            { "QASH", "qash" },
                            { "QTUM", "qtum" }, // #1811
                            { "RCN", "rcn" },
                            { "RDN", "rdn" },
                            { "REP", "rep" },
                            { "REQ", "req" },
                            { "RLC", "rlc" },
                            { "SAN", "santiment" },
                            { "SNGLS", "sng" },
                            { "SNT", "status" },
                            { "SPANK", "spk" },
                            { "STJ", "stj" },
                            { "TNB", "tnb" },
                            { "TRX", "trx" },
                            { "USD", "wire" },
                            { "USDT", "tetheruso" }, // undocumented
                            { "WAX", "wax" },
                            { "XLM", "xlm" },
                            { "XMR", "monero" },
                            { "XRP", "ripple" },
                            { "XVG", "xvg" },
                            { "YOYOW", "yoyow" },
                            { "ZEC", "zcash" },
                            { "ZRX", "zrx" }
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

                var _nonce = GenerateOnlyNonce(13).ToString();

                _params.Add("nonce", _nonce);
                _params.Add("request", endpoint);

                var _payload = this.SerializeObject(_params, Formatting.None);
                {
                    var _sign_data = Convert.ToBase64String(Encoding.UTF8.GetBytes(_payload));
                    var _signature = this.ConvertHexString(Encryptor.ComputeHash(Encoding.UTF8.GetBytes(_sign_data)));

                    _request.AddHeader("X-BFX-PAYLOAD", _sign_data);
                    _request.AddHeader("X-BFX-SIGNATURE", _signature.ToLower());
                    _request.AddHeader("X-BFX-APIKEY", ConnectKey);
                }
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        ///
        /// </summary>
        public new Dictionary<string, ErrorCode> ErrorMessages = new Dictionary<string, ErrorCode>
        {
            { "temporarily_unavailable", ErrorCode.ExchangeNotAvailable },                                  // Sorry, the service is temporarily unavailable. See https://www.bitfinex.com/ for more info.
            { "Order could not be cancelled.", ErrorCode.OrderNotFound },                                   // non-existent order
            { "No such order found.", ErrorCode.OrderNotFound },                                            // ?
            { "Order price must be positive.", ErrorCode.InvalidOrder },                                    // on price <= 0
            { "Could not find a key matching the given X-BFX-APIKEY.", ErrorCode.AuthenticationError },
            { "This API key does not have permission for this action", ErrorCode.AuthenticationError },     // authenticated but not authorized
            { "Key price should be a decimal number, e.g. '123.456'", ErrorCode.InvalidOrder },             // on isNaN (price)
            { "Key amount should be a decimal number, e.g. '123.456'", ErrorCode.InvalidOrder },            // on isNaN (amount)
            { "ERR_RATE_LIMIT", ErrorCode.DDoSProtection },
            { "Nonce is too small.", ErrorCode.InvalidNonce },
            { "No summary found.", ErrorCode.ExchangeError },                                               // fetchTradingFees (summary) endpoint can give this vague error message
            { "Invalid order: not enough exchange balance for ", ErrorCode.InsufficientFunds },             // when buying cost is greater than the available quote currency
            { "Invalid order, ErrorCode.minimum size for ", ErrorCode.InvalidOrder },                       // when amount below limits.amount.min
            { "Invalid order", ErrorCode.InvalidOrder },                                                    // ?
            { "RateLimt", ErrorCode.RateLimit },                                                            // ?
            { "The available balance is only", ErrorCode.InsufficientFunds }                                // {"status":"error","message":"Cannot withdraw 1.0027 ETH from your exchange wallet. The available balance is only 0.0 ETH. If you have limit orders, open positions, unused or active margin funding, this will decrease your available balance. To increase it, you can cancel limit orders or reduce/close your positions.","withdrawal_id":0,"fees":"0.0027"}
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
                if (String.IsNullOrEmpty(response.Content) == false && response.Content != "[]")
                {
                    if (response.IsSuccessful == false && response.Content[0] == '{')
                    {
                        var _json_result = this.DeserializeObject<JToken>(response.Content);

                        var _json_status = _json_result.SelectToken("status");
                        if (_json_status == null || (_json_status != null && _json_status.Value<string>() != "success"))
                        {
                            var _message = "";

                            var _json_error = _json_result.SelectToken("error");

                            var _json_message = _json_result.SelectToken("message");
                            if (_json_message != null)
                            {
                                _message = _json_message.Value<string>();
                            }
                            else if (_json_error != null)
                            {
                                _message = _json_error.Value<string>();
                            }
                            else
                            {
                                _result.SetFailure(
                                        response.Content,
                                        ErrorCode.ResponseDataError
                                    );
                            }

                            if (String.IsNullOrEmpty(_message) == false)
                            {
                                var _error_code = ErrorMessages
                                                        .Where(e => _message.IndexOf(e.Key) >= 0)
                                                        .OrderByDescending(e => e.Key.Length)
                                                        .FirstOrDefault();

                                if (_error_code.Key != null)
                                    _result.errorCode = _error_code.Value;
                                else
                                    _result.errorCode = ErrorCode.ResponseDataError;

                                _result.SetFailure(
                                        _message,
                                        _result.errorCode
                                    );
                            }
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