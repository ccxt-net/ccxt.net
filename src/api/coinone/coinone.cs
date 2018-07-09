using Newtonsoft.Json;
using OdinSdk.BaseLib.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCXT.NET.Coinone
{
    /// <summary>
    /// 
    /// </summary>
    public class CoinOneClient : XApiClient
    {
        private const string __api_url = "https://api.Coinone.co.kr";
        /// <summary>
        /// 
        /// </summary>
        public CoinOneClient()
            : base(__api_url, "", "")
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public CoinOneClient(string connect_key, string secret_key)
            : base(__api_url, connect_key, secret_key)
        {
        }

        private string BytesToHex(byte[] bytes)
        {
            var _hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                _hex.AppendFormat("{0:x2}", b);

            return _hex.ToString();
        }

        private Dictionary<string, object> GetHttpHeaders(string endpoint, string payload, string access_token, string secret_key)
        {
            var _signature = "";

            var _secretKey = Encoding.UTF8.GetBytes(secret_key.ToUpper());
            using (var _hmac = new HMACSHA512(_secretKey))
            {
                _hmac.Initialize();

                var _bytes = Encoding.UTF8.GetBytes(payload);
                var _rawHmac = _hmac.ComputeHash(_bytes);

                _signature = BytesToHex(_rawHmac);
            }

            var _headers = new Dictionary<string, object>();
            {
                _headers.Add("X-Coinone-PAYLOAD", payload);
                _headers.Add("X-Coinone-SIGNATURE", _signature);
            }

            return _headers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public new async Task<T> CallApiPostAsync<T>(string endpoint, Dictionary<string, object> args = null) where T : new()
        {
            var _request = CreateJsonRequest(endpoint, Method.POST);
            {
                var _params = new Dictionary<string, object>();
                {
                    if (args != null)
                    {
                        foreach (var a in args)
                            _params.Add(a.Key, a.Value);
                    }

                    _params.Add("access_token", __connect_key);
                    _params.Add("nonce", CUnixTime.Now);
                }

                var _payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_params)));

                var _headers = GetHttpHeaders(endpoint, _payload, __connect_key, __secret_key);
                foreach (var _h in _headers)
                    _request.AddHeader(_h.Key, _h.Value.ToString());
            }

            var _client = CreateJsonClient(__api_url);
            {
                var _tcs = new TaskCompletionSource<IRestResponse>();
                var _handle = _client.ExecuteAsync(_request, response =>
                {
                    _tcs.SetResult(response);
                });

                var _response = await _tcs.Task;
                return JsonConvert.DeserializeObject<T>(_response.Content);
            }
        }
    }
}