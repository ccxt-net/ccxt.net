using CCXT.NET.Configuration;
using CCXT.NET.Serialize;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CCXT.NET.Coin
{
    /// <summary>
    ///
    /// </summary>
    public interface IXApiClient
    {
        /// <summary>
        ///
        /// </summary>
        string DealerName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        ExchangeInfo ExchangeInfo
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        bool IsAuthentication
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        string ApiUrl
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        string ConnectKey
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        string SecretKey
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        string UserName
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        string UserPassword
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        Dictionary<object, object> ErrorMessages
        {
            get;
            set;
        }

        #region method common lib

        /// <summary>
        ///
        /// </summary>
        /// <param name="error_code"></param>
        /// <returns></returns>
        string GetErrorMessage(int error_code);

        /// <summary>
        ///
        /// </summary>
        /// <param name="response">response value arrive from exchange's server</param>
        /// <returns></returns>
        BoolResult GetResponseMessage(IRestResponse response = null);

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="max_retry"></param>
        /// <param name="delay_milliseconds"></param>
        /// <returns></returns>
        Task<IRestResponse> RestExecuteAsync(IRestRequest request, int max_retry, int delay_milliseconds);

        #endregion method common lib

        #region method post

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<IRestRequest> CreatePostRequest(string endpoint, Dictionary<string, object> args = null);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<T> CallApiPostAsync<T>(string endpoint, Dictionary<string, object> args = null) where T : new();

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<string> CallApiPostAsync(string endpoint, Dictionary<string, object> args = null);

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<(string Content, IRestResponse Response)> CallApiPost1Async(string endpoint, Dictionary<string, object> args = null);

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<IRestResponse> CallApiPost2Async(string endpoint, Dictionary<string, object> args = null);

        #endregion method post

        #region method get

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<IRestRequest> CreateGetRequest(string endpoint, Dictionary<string, object> args = null);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<T> CallApiGetAsync<T>(string endpoint, Dictionary<string, object> args = null) where T : new();

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<string> CallApiGetAsync(string endpoint, Dictionary<string, object> args = null);

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<(string Content, IRestResponse Response)> CallApiGet1Async(string endpoint, Dictionary<string, object> args = null);

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<IRestResponse> CallApiGet2Async(string endpoint, Dictionary<string, object> args = null);

        #endregion method get

        #region method delete

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<IRestRequest> CreateDeleteRequest(string endpoint, Dictionary<string, object> args = null);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<T> CallApiDeleteAsync<T>(string endpoint, Dictionary<string, object> args = null) where T : new();

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<string> CallApiDeleteAsync(string endpoint, Dictionary<string, object> args = null);

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<(string Content, IRestResponse Response)> CallApiDelete1Async(string endpoint, Dictionary<string, object> args = null);

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<IRestResponse> CallApiDelete2Async(string endpoint, Dictionary<string, object> args = null);

        #endregion method delete
    }

    /// <summary>
    ///
    /// </summary>
    public class XApiClient : IXApiClient, IDisposable
    {
        /// <summary>
        ///
        /// </summary>
        public static XUnitMode TestXUnitMode = XUnitMode.UseExchangeServer;

        /// <summary>
        ///
        /// </summary>
        public static ConcurrentDictionary<string, long> marketLastNonce = new ConcurrentDictionary<string, long>();

        /// <summary>
        ///
        /// </summary>
        public const string ContentType = "application/json";

        /// <summary>
        ///
        /// </summary>
        public const string UserAgent = "odinsoft-ccxt.net/1.0.2018.07";

        /// <summary>
        ///
        /// </summary>
        public virtual string DealerName
        {
            get;
            set;
        }

        /// <summary>
        /// information of exchange for trading
        /// </summary>
        public virtual ExchangeInfo ExchangeInfo
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsAuthentication { get; set; } = false;

        /// <summary>
        ///
        /// </summary>
        public string ApiUrl { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        public string ConnectKey { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        public string SecretKey { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        public string UserPassword { get; set; } = "";

        /// <summary>
        ///
        /// </summary>
        /// <param name="division">exchange's api url for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        /// <param name="user_name">exchange's id or uuid for login</param>
        /// <param name="user_password">exchange's password for login</param>
        /// <param name="authentication"></param>
        public XApiClient(string division, string connect_key = "", string secret_key = "", string user_name = "", string user_password = "", bool authentication = false)
        {
            ApiUrl = this.ExchangeInfo.GetApiUrl(division);

            ConnectKey = connect_key;
            SecretKey = secret_key;

            UserName = user_name;
            UserPassword = user_password;

            IsAuthentication = authentication;
        }

        #region method common

        /// <summary>
        ///
        /// </summary>
        /// <param name="digit_count"></param>
        /// <returns></returns>
        public BigInteger GenerateNonceValue(int digit_count)
        {
            var _milli_seconds = CUnixTime.UtcNow.Subtract(CUnixTime.UnixEpoch).TotalMilliseconds;

            var _nonce = new BigInteger(Math.Round(_milli_seconds * 10000.0, MidpointRounding.AwayFromZero));
            {
                var _nonce_size = _nonce.ToString().Length;

                digit_count = digit_count <= 0 ? 16 : digit_count;
                if (_nonce_size > digit_count)
                    _nonce = BigInteger.Divide(_nonce, BigInteger.Pow(10, _nonce_size - digit_count));
                else if (_nonce_size < digit_count)
                    _nonce = _nonce * BigInteger.Pow(10, digit_count - _nonce_size);
            }

            return _nonce;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="digit_count"></param>
        /// <param name="right_length"></param>
        /// <returns></returns>
        public string GenerateNonceString(int digit_count, int right_length = 0)
        {
            var _nonce_string = GenerateNonceValue(digit_count).ToString(CultureInfo.InvariantCulture);

            var _start_index = right_length > 0 ? _nonce_string.Length - right_length : 0;
            return _nonce_string.Substring(_start_index);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="digit_count"></param>
        /// <returns></returns>
        public long GenerateOnlyNonce(int digit_count)
        {
            var _nonce = (long)GenerateNonceValue(digit_count);

            if (marketLastNonce.ContainsKey(this.DealerName) == true)
            {
                var _last_nonce = marketLastNonce[this.DealerName];
#if DEBUG
                if (_last_nonce.ToString().Length != _nonce.ToString().Length)
                    throw new Exception("nonce length mismatch error");
#endif
                if (_last_nonce >= _nonce)
                    _nonce = _last_nonce++;
            }

            marketLastNonce[this.DealerName] = _nonce;

            return _nonce;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <param name="json"></param>
        /// <returns></returns>
        public string ToQueryString(List<Parameter> args, bool json = false)
        {
            var _args = new Dictionary<string, object>();

            foreach (var _entry in args)
                _args.Add(_entry.Name, _entry.Value);

            return ToQueryString(_args, json);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <param name="json"></param>
        /// <returns></returns>
        public string ToQueryString(Dictionary<string, object> args, bool json = false)
        {
            var _result = "";

            if (json == true)
            {
                var _params = new List<string>();
                foreach (var _entry in args)
                    _params.Add(String.Format("'{0}':'{1}'", _entry.Key, _entry.Value));

                _result = "{" + String.Join(",", _params) + "}";
            }
            else
            {
                _result = String.Join("&", args.Select(a => $"{a.Key}={HttpUtility.UrlEncode((a.Value ?? "").ToString())}"));
            }

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public string ToQueryString2(Dictionary<string, object> args)
        {
            return String.Join("&", args.Select(a => $"{a.Key}={Uri.EscapeDataString((a.Value ?? "").ToString())}"));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public string ConvertHexString(byte[] buffer)
        {
            return BitConverter.ToString(buffer).Replace("-", "");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public Dictionary<string, object> MergeParamsAndArgs(Dictionary<string, object> args)
        {
            return MergeParamsAndArgs(new Dictionary<string, object>(), args);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="params"></param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public Dictionary<string, object> MergeParamsAndArgs(Dictionary<string, object> @params, Dictionary<string, object> args)
        {
            if (args != null)
            {
                foreach (var _arg in args)
                {
                    if (@params.ContainsKey(_arg.Key) == true)
                        @params.Remove(_arg.Key);

                    @params.Add(_arg.Key, _arg.Value);
                }
            }

            return @params;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="baseurl"></param>
        /// <returns></returns>
        public IRestClient CreateJsonClient(string baseurl)
        {
            var _client = new RestClient(baseurl)
            {
                Timeout = 5 * 1000,
                ReadWriteTimeout = 32 * 1000,
                UserAgent = UserAgent,
                Encoding = Encoding.GetEncoding(65001)
            };

            _client.RemoveHandler(ContentType);
            //_client.AddHandler(ContentType, new RestSharpJsonNetDeserializer());
            _client.AddHandler(ContentType, () => new RestSharpJsonNetDeserializer());

            return _client;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <param name="method"></param>
        /// <returns></returns>
        public IRestRequest CreateJsonRequest(string resource, Dictionary<string, object> args = null, Method method = Method.GET)
        {
            var _request = new RestRequest(resource, method)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new RestSharpJsonNetSerializer()
            };

            if (args != null)
            {
                foreach (var _arg in args)
                {
#if DEBUG
                    if (TestXUnitMode == XUnitMode.UseExchangeServer && _arg.Key == "jsonContent")
                        continue;
#endif
                    if (_arg.Value.GetType() == typeof(CArgument))
                    {
                        var _margs = _arg.Value as CArgument;
                        if (_margs.isArray == true)
                        {
                            foreach (var _marg in (_margs.value as Array))
                                _request.AddParameter(_arg.Key, _marg);
                        }
                        else if (_margs.isJson == true)
                        {
                            _request.AddParameter(_arg.Key, JsonConvert.SerializeObject(_margs.value));
                        }
                        else
                        {
                            _request.AddParameter(_arg.Key, _margs.value);
                        }
                    }
                    else
                    {
                        _request.AddParameter(_arg.Key, _arg.Value);
                    }
                }
            }

            return _request;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Dictionary<object, object> ErrorMessages
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="error_code"></param>
        /// <returns></returns>
        public virtual string GetErrorMessage(int error_code)
        {
            return ErrorMessages.ContainsKey(error_code) == true
                                  ? ErrorMessages[error_code].ToString()
                                  : "failure";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="response">response value arrive from exchange's server</param>
        /// <returns></returns>
        public virtual BoolResult GetResponseMessage(IRestResponse response = null)
        {
            var _result = new BoolResult();

            if (response != null)
            {
                if (response.IsSuccessful == true)
                {
                    var _json_result = this.DeserializeObject<JToken>(response.Content);
                    if (_json_result.SelectToken("message") != null)
                    {
                        _result.SetFailure(
                                _json_result["message"].Value<string>(),
                                ErrorCode.ExchangeError
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="max_retry"></param>
        /// <param name="delay_milliseconds"></param>
        /// <returns></returns>
        public virtual async Task<IRestResponse> RestExecuteAsync(IRestRequest request, int max_retry = 3, int delay_milliseconds = 1000)
        {
            var _result = (IRestResponse)null;

            var _client = CreateJsonClient(ApiUrl);

            for (var _retry_count = 0; _retry_count < max_retry; _retry_count++)
            {
                var _tcs = new TaskCompletionSource<IRestResponse>();
                {
                    var _handle = _client.ExecuteAsync(request, response =>
                    {
                        _tcs.SetResult(response);
                    });

                    _result = await _tcs.Task;
                }

                if (_result.ResponseStatus != ResponseStatus.TimedOut && _result.StatusCode != HttpStatusCode.RequestTimeout)
                    break;

#if DEBUG
                Debug.WriteLine($"request timeout: {_retry_count}/{max_retry}, {request.Resource}");
#endif

                await Task.Delay(delay_milliseconds);
            }

            return _result;
        }

        #endregion method common

        #region method post

        private static RestSerializerSettings __rest_settings = null;

        private RestSerializerSettings RestSerializerSettings
        {
            get
            {
                if (__rest_settings == null)
                    __rest_settings = new RestSerializerSettings();
                return __rest_settings;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json_string"></param>
        /// <returns></returns>
        public T DeserializeObject<T>(string json_string) //where T : new()
        {
            return JsonConvert.DeserializeObject<T>(json_string, this.RestSerializerSettings);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="json_object"></param>
        /// <param name="formatting"></param>
        /// <returns></returns>
        public string SerializeObject(object json_object, Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(json_object, formatting, this.RestSerializerSettings);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<IRestRequest> CreatePostRequest(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = CreateJsonRequest(endpoint, args, Method.POST);
            return await Task.FromResult(_request);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<T> CallApiPostAsync<T>(string endpoint, Dictionary<string, object> args = null) where T : new()
        {
            var _response = await CallApiPostAsync(endpoint, args);
            return this.DeserializeObject<T>(_response);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<string> CallApiPostAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _response = await CallApiPost2Async(endpoint, args);
            return _response.Content;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<(string Content, IRestResponse Response)> CallApiPost1Async(string endpoint, Dictionary<string, object> args = null)
        {
            var _response = await CallApiPost2Async(endpoint, args);
            return (_response.Content, _response);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<IRestResponse> CallApiPost2Async(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await CreatePostRequest(endpoint, args);
#if DEBUG
            if (TestXUnitMode == XUnitMode.ModeUnknown)
                throw new Exception("POST: xUnitMode is unknown error when debug mode");

            if (TestXUnitMode != XUnitMode.UseExchangeServer)
            {
                var _result = new RestResponse()
                {
                    Request = _request,
                    Content = args.ContainsKey("jsonContent") ? args["jsonContent"].ToString() : "",
                    ResponseStatus = ResponseStatus.Completed,
                    StatusCode = HttpStatusCode.OK
                };

                return await Task.FromResult(_result);
            }
#endif
            return await RestExecuteAsync(_request);
        }

        #endregion method post

        #region method get

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<IRestRequest> CreateGetRequest(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = CreateJsonRequest(endpoint, args, Method.GET);
            return await Task.FromResult(_request);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<T> CallApiGetAsync<T>(string endpoint, Dictionary<string, object> args = null) where T : new()
        {
            var _response = await CallApiGet2Async(endpoint, args);
            return this.DeserializeObject<T>(_response.Content);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<string> CallApiGetAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _response = await CallApiGet2Async(endpoint, args);
            return _response.Content;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<(string Content, IRestResponse Response)> CallApiGet1Async(string endpoint, Dictionary<string, object> args = null)
        {
            var _response = await CallApiGet2Async(endpoint, args);
            return (_response.Content, _response);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<IRestResponse> CallApiGet2Async(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await CreateGetRequest(endpoint, args);
#if DEBUG
            if (TestXUnitMode == XUnitMode.ModeUnknown)
                throw new Exception("GET: xUnitMode is unknown error when debug mode");

            if (TestXUnitMode != XUnitMode.UseExchangeServer)
            {
                var _result = new RestResponse()
                {
                    Request = _request,
                    Content = args.ContainsKey("jsonContent") ? args["jsonContent"].ToString() : "",
                    ResponseStatus = ResponseStatus.Completed,
                    StatusCode = HttpStatusCode.OK
                };

                return await Task.FromResult(_result);
            }
#endif
            return await RestExecuteAsync(_request);
        }

        #endregion method get

        #region method delete

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<IRestRequest> CreateDeleteRequest(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = CreateJsonRequest(endpoint, args, Method.DELETE);
            return await Task.FromResult(_request);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<T> CallApiDeleteAsync<T>(string endpoint, Dictionary<string, object> args = null) where T : new()
        {
            var _response = await CallApiDelete2Async(endpoint, args);
            return this.DeserializeObject<T>(_response.Content);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<string> CallApiDeleteAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _response = await CallApiDelete2Async(endpoint, args);
            return _response.Content;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<(string Content, IRestResponse Response)> CallApiDelete1Async(string endpoint, Dictionary<string, object> args = null)
        {
            var _response = await CallApiDelete2Async(endpoint, args);
            return (_response.Content, _response);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<IRestResponse> CallApiDelete2Async(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await CreateDeleteRequest(endpoint, args);
#if DEBUG
            if (TestXUnitMode == XUnitMode.ModeUnknown)
                throw new Exception("DELETE: xUnitMode is unknown error when debug mode");

            if (TestXUnitMode != XUnitMode.UseExchangeServer)
            {
                var _result = new RestResponse()
                {
                    Request = _request,
                    Content = args.ContainsKey("jsonContent") ? args["jsonContent"].ToString() : "",
                    ResponseStatus = ResponseStatus.Completed,
                    StatusCode = HttpStatusCode.OK
                };

                return await Task.FromResult(_result);
            }
#endif
            return await RestExecuteAsync(_request);
        }

        #endregion method delete

        #region method put

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<IRestRequest> CreatePutRequest(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = CreateJsonRequest(endpoint, args, Method.PUT);
            return await Task.FromResult(_request);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<T> CallApiPutAsync<T>(string endpoint, Dictionary<string, object> args = null) where T : new()
        {
            var _response = await CallApiPut2Async(endpoint, args);
            return this.DeserializeObject<T>(_response.Content);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<string> CallApiPutAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _response = await CallApiPut2Async(endpoint, args);
            return _response.Content;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<(string Content, IRestResponse Response)> CallApiPut1Async(string endpoint, Dictionary<string, object> args = null)
        {
            var _response = await CallApiPut2Async(endpoint, args);
            return (_response.Content, _response);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="endpoint">api link address of a function</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<IRestResponse> CallApiPut2Async(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = await CreatePutRequest(endpoint, args);
#if DEBUG
            if (TestXUnitMode == XUnitMode.ModeUnknown)
                throw new Exception("PUT: xUnitMode is unknown error when debug mode");

            if (TestXUnitMode != XUnitMode.UseExchangeServer)
            {
                var _result = new RestResponse()
                {
                    Request = _request,
                    Content = args.ContainsKey("jsonContent") ? args["jsonContent"].ToString() : "",
                    ResponseStatus = ResponseStatus.Completed,
                    StatusCode = HttpStatusCode.OK
                };

                return await Task.FromResult(_result);
            }
#endif
            return await RestExecuteAsync(_request);
        }

        #endregion method put

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
        }
    }
}