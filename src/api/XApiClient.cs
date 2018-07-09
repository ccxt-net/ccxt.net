using Newtonsoft.Json;
using OdinSdk.BaseLib.Serialize;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCXT.NET
{
    /// <summary>
    /// 
    /// </summary>
    public class XApiClient : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        protected const string __content_type = "application/json";

        /// <summary>
        /// 
        /// </summary>
        protected const string __user_agent = "ccxt-net/1.0.2018.07";

        private string __api_url = "";

        /// <summary>
        /// 
        /// </summary>
        protected string __connect_key;

        /// <summary>
        /// 
        /// </summary>
        protected string __secret_key;

        /// <summary>
        /// 
        /// </summary>
        public XApiClient(string api_url, string connect_key, string secret_key)
        {
            __api_url = api_url;
            __connect_key = connect_key;
            __secret_key = secret_key;
        }

        private static char[] __to_digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] EncodeHex(byte[] data)
        {
            int l = data.Length;
            byte[] _result = new byte[l << 1];

            // two characters form the hex value.
            for (int i = 0, j = 0; i < l; i++)
            {
                _result[j++] = (byte)__to_digits[(0xF0 & data[i]) >> 4];
                _result[j++] = (byte)__to_digits[0x0F & data[i]];
            }

            return _result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rgData"></param>
        /// <returns></returns>
        public string EncodeURIComponent(Dictionary<string, object> rgData)
        {
            string _result = String.Join("&", rgData.Select((x) => String.Format("{0}={1}", x.Key, x.Value)));

            _result = System.Net.WebUtility.UrlEncode(_result)
                        .Replace("+", "%20").Replace("%21", "!")
                        .Replace("%27", "'").Replace("%28", "(")
                        .Replace("%29", ")").Replace("%26", "&")
                        .Replace("%3D", "=").Replace("%7E", "~");

            return _result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseurl"></param>
        /// <returns></returns>
        public IRestClient CreateJsonClient(string baseurl)
        {
            var _client = new RestClient(baseurl);
            {
                _client.RemoveHandler(__content_type);
                _client.AddHandler(__content_type, new RestSharpJsonNetDeserializer());
                _client.Timeout = 5 * 1000;
                _client.ReadWriteTimeout = 32 * 1000;
                _client.UserAgent = __user_agent;
            }

            return _client;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public IRestRequest CreateJsonRequest(string resource, Method method = Method.GET)
        {
            var _request = new RestRequest(resource, method)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = new RestSharpJsonNetSerializer()
            };

            return _request;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual async Task<T> CallApiPostAsync<T>(string endpoint, Dictionary<string, object> args = null) where T : new()
        {
            var _response = await CallApiPostAsync(endpoint, args);
            return JsonConvert.DeserializeObject<T>(_response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual async Task<string> CallApiPostAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = CreateJsonRequest(endpoint, Method.POST);
            {
                var _params = new Dictionary<string, object>();
                {
                    _params.Add("endpoint", endpoint);
                    if (args != null)
                    {
                        foreach (var a in args)
                            _params.Add(a.Key, a.Value);
                    }
                }

                foreach (var a in _params)
                    _request.AddParameter(a.Key, a.Value);
            }

            var _client = CreateJsonClient(__api_url);
            {
                var _tcs = new TaskCompletionSource<IRestResponse>();
                var _handle = _client.ExecuteAsync(_request, response =>
                {
                    _tcs.SetResult(response);
                });

                var _response = await _tcs.Task;
                return _response.Content;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual async Task<T> CallApiGetAsync<T>(string endpoint, Dictionary<string, object> args = null) where T : new()
        {
            var _response = await CallApiGetAsync(endpoint, args);
            return JsonConvert.DeserializeObject<T>(_response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual async Task<string> CallApiGetAsync(string endpoint, Dictionary<string, object> args = null)
        {
            var _request = CreateJsonRequest(endpoint, Method.GET);

            if (args != null)
            {
                foreach (var a in args)
                    _request.AddParameter(a.Key, a.Value);
            }

            var _client = CreateJsonClient(__api_url);
            {
                var _tcs = new TaskCompletionSource<IRestResponse>();
                var _handle = _client.ExecuteAsync(_request, response =>
                {
                    _tcs.SetResult(response);
                });

                var _response = await _tcs.Task;
                return _response.Content;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
        }
    }
}