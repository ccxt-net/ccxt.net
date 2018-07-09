using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCXT.NET.Korbit
{
    /// <summary>
    /// 
    /// </summary>
    public class KorbitClient : XApiClient
    {
        private const string __api_url = "https://api.korbit.co.kr";

        private string __user_name;
        private string __user_password;

        /// <summary>
        /// 
        /// </summary>
        public KorbitClient()
            : base(__api_url, "", "")
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connect_key"></param>
        /// <param name="secret_key"></param>
        /// <param name="user_name"></param>
        /// <param name="user_password"></param>
        public KorbitClient(string connect_key, string secret_key, string user_name, string user_password)
            : base(__api_url, connect_key, secret_key)
        {
            __user_name = user_name;
            __user_password = user_password;
        }

        private AccessToken __access_token = null;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AccessToken> AccessToken()
        {
            if (__access_token != null && String.IsNullOrEmpty(__access_token.access_token) == false)
            {
                if (__access_token.CheckExpired() == true)
                    __access_token = await GetRefreshToken(__access_token);
            }
            else if (String.IsNullOrEmpty(__connect_key) == false)
                __access_token = await GetAccessToken();

            return __access_token;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AccessToken> GetAccessToken()
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("client_id", __connect_key);
                _params.Add("client_secret", __secret_key);
                _params.Add("username", __user_name);
                _params.Add("password", __user_password);
                _params.Add("grant_type", "password");
            }

            return await this.CallApiPostAsync<AccessToken>("/v1/oauth2/access_token", _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AccessToken> GetRefreshToken(AccessToken access_token)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("client_id", __connect_key);
                _params.Add("client_secret", __secret_key);
                _params.Add("refresh_token", access_token.refresh_token);
                _params.Add("grant_type", "refresh_token");
            }

            return await this.CallApiPostAsync<AccessToken>("/v1/oauth2/access_token", _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public new async Task<T> CallApiGetAsync<T>(string endpoint, Dictionary<string, object> args = null) where T : new()
        {
            var _request = CreateJsonRequest(endpoint, Method.GET);

            if (args != null)
            {
                foreach (var a in args)
                    _request.AddParameter(a.Key, a.Value);
            }

            var _access_token = await AccessToken();
            if (_access_token != null)
                _request.AddHeader("Authorization", $"Bearer {_access_token.access_token}");

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