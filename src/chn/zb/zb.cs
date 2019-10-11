using OdinSdk.BaseLib.Coin;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCXT.NET.Zb
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ZbClient : OdinSdk.BaseLib.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        public override string DealerName { get; set; } = "Zb";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public ZbClient(string division)
            : base(division)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public ZbClient(string division, string connect_key, string secret_key)
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
                            logo = "https://user-images.githubusercontent.com/1294454/32859187-cd5214f0-ca5e-11e7-967d-96568e2e2bd1.jpg",
                            api = new Dictionary<string, string>
                            {
                                { "public", "http://api.zb.com/data" },
                                { "private", "https://trade.zb.com/api" },
                                { "trade", "https://trade.zb.com/api" }
                            },
                            www = "https://www.zb.com",
                            doc = new List<string>
                            {
                                "https://www.zb.com/i/developer"
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
                            { "3m","3min" },
                            { "5m","5min" },
                            { "15m","15min" },
                            { "30m","30min" },
                            { "1h","1hour" },
                            { "2h","2hour" },
                            { "4h","4hour" },
                            { "6h","6hour" },
                            { "12h","12hour" },
                            { "1d","1day" },
                            { "3d","3day" },
                            { "1w","1week" }
                        }
                    };
                }

                return base.ExchangeInfo;
            }
        }

        private string hmacSign(string aValue, string aKey)
        {
            byte[] k_ipad = new byte[64];
            byte[] k_opad = new byte[64];

            byte[] keyb;
            byte[] value;

            try
            {
                var _encoding = Encoding.GetEncoding(encodingCharset);

                keyb = _encoding.GetBytes(aKey);
                value = _encoding.GetBytes(aValue);
            }
            catch (Exception)
            {
                keyb = null;
                value = null;
            }

            for (int i = keyb.Length; i < 64; i++)
            {
                k_ipad[i] = 54;
                k_opad[i] = 92;
            }

            for (int i = 0; i < keyb.Length; i++)
            {
                k_ipad[i] = (byte)(keyb[i] ^ 0x36);
                k_opad[i] = (byte)(keyb[i] ^ 0x5c);
            }

            var _sMd5_1 = MakeMD5(k_ipad.Concat(value).ToArray());
            var _dg = MakeMD5(k_opad.Concat(_sMd5_1).ToArray());

            return toHex(_dg);
        }

        private string toHex(byte[] input)
        {
            if (input == null)
                return null;

            var _output = new StringBuilder(input.Length * 2);
            for (var i = 0; i < input.Length; i++)
            {
                var _current = input[i] & 0xff;

                if (_current < 16)
                    _output.Append('0');

                _output.Append(_current.ToString("x"));
            }

            return _output.ToString();
        }

        private string getHmac(string[] args, string key)
        {
            if (args == null || args.Length == 0)
                return null;

            var _sb = new StringBuilder();
            for (int i = 0; i < args.Length; i++)
                _sb.Append(args[i]);

            return hmacSign(_sb.ToString(), key);
        }

        private byte[] MakeMD5(byte[] original)
        {
            var hashmd5 = new MD5CryptoServiceProvider();
            return hashmd5.ComputeHash(original);
        }

        private const string encodingCharset = "UTF-8";

        private string digest(String aValue)
        {
            byte[] value;

            try
            {
                aValue = aValue.Trim();

                var _encoding = Encoding.GetEncoding(encodingCharset);
                value = _encoding.GetBytes(aValue);

                var ha = (HashAlgorithm)CryptoConfig.CreateFromName("SHA");
                value = ha.ComputeHash(value);
            }
            catch (Exception)
            {
                throw;
            }

            return toHex(value);
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
                var _params = new Dictionary<string, object>();
                {
                    _params.Add("accesskey", ConnectKey);

                    foreach (var _param in _request.Parameters)
                        _params.Add(_param.Name, _param.Value);

                    _request.Parameters.Clear();
                }

                var _nonce = GenerateOnlyNonce(13).ToString();

                var _secretkey = digest(SecretKey);

                var _post_data = ToQueryString(_params);
                {
                    var _sign_data = hmacSign(_post_data, _secretkey);

                    _post_data += $"&sign={_sign_data}&reqTime={_nonce}";
                }

                _request.Resource = endpoint + $"?{_post_data}";
            }

            return await Task.FromResult(_request);
        }

        /// <summary>
        ///
        /// </summary>
        public new Dictionary<int, string> ErrorMessages = new Dictionary<int, string>
        {
            { 1000, "Success" },
            { 1001, "Error Tips" },
            { 1002, "Internal Error" },
            { 1003, "Validate No Pass" },
            { 1004, "Transaction Password Locked" },
            { 1005, "Transaction Password Error" },
            { 1006, "Real-name verification is pending approval or not approval" },
            { 1009, "This interface is in maintaining" },
            { 1010, "Temporarily not open" },
            { 1012, "Permission denied." },
            { 1013, "Cannot trade, if you have questions please contact online customer service" },
            { 2002, "Insufficient BTC Balance" },
            { 2003, "Insufficient LTC Balance" },
            { 2005, "Insufficient ETH Balance" },
            { 2006, "Insufficient ETC Balance" },
            { 2007, "Insufficient BTS Balance" },
            { 2009, "Insufficient account balance" },
            { 3001, "Not Found Order" },
            { 3002, "Invalid Money" },
            { 3003, "Invalid Amount" },
            { 3004, "No Such User" },
            { 3005, "Invalid Parameters" },
            { 3006, "Invalid IP or Differ From the Bound IP" },
            { 3007, "Invalid Request Time" },
            { 3008, "Not Found Transaction Record" },
            { 4001, "API Interface is locked or not enabled" },
            { 4002, "Request Too Frequently" }
        };

        /// <summary>
        ///
        /// </summary>
        /// <param name="error_code"></param>
        /// <returns></returns>
        public override string GetErrorMessage(int error_code)
        {
            return ErrorMessages.ContainsKey(error_code) == true
                                  ? ErrorMessages[error_code]
                                  : "failure";
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

                    var _json_error = _json_result.SelectToken("code");
                    if (_json_error != null)
                    {
                        var _error_code = _json_error.Value<int>();
                        if (_error_code != 1000)
                        {
                            _result.SetFailure(
                                      GetErrorMessage(_error_code),
                                      ErrorCode.ResponseDataError,
                                      _error_code
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