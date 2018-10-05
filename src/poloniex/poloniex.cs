using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCXT.NET.Poloniex
{
    /// <summary>
    /// 
    /// </summary>
    public class PoloniexClient : XApiClient
    {
        private const string __api_url = "https://poloniex.com";

        /// <summary>
        /// 
        /// </summary>
        public PoloniexClient()
            : base(__api_url, "", "")
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connect_key"></param>
        /// <param name="secret_key"></param>
        public PoloniexClient(string connect_key, string secret_key)
            : base(__api_url, connect_key, secret_key)
        {
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

        private BigInteger CurrentHttpPostNonce
        {
            get;
            set;
        }

        private string GetCurrentHttpPostNonce()
        {
            var _ne_nonce = new BigInteger(
                                    Math.Round(
                                        DateTime.UtcNow.Subtract(
                                            CUnixTime.UnixEpoch
                                        )
                                        .TotalMilliseconds * 1000,
                                        MidpointRounding.AwayFromZero
                                    )
                                );

            if (_ne_nonce > CurrentHttpPostNonce)
            {
                CurrentHttpPostNonce = _ne_nonce;
            }
            else
            {
                CurrentHttpPostNonce += 1;
            }

            return CurrentHttpPostNonce.ToString(CultureInfo.InvariantCulture);
        }

        private string HttpPostString(List<Parameter> dictionary)
        {
            var _result = "";

            foreach (var _entry in dictionary)
            {
                var _value = _entry.Value as string;
                if (_value == null)
                    _result += "&" + _entry.Name + "=" + _entry.Value;
                else
                    _result += "&" + _entry.Name + "=" + _value.Replace(' ', '+');
            }

            return _result.Substring(1);
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
                    _params.Add("nonce", GetCurrentHttpPostNonce());

                    if (args != null)
                    {
                        foreach (var a in args)
                            _params.Add(a.Key, a.Value);
                    }
                }

                foreach (var _p in _params)
                    _request.AddParameter(_p.Key, _p.Value);

                var _post_data = HttpPostString(_request.Parameters);
                var _post_bytes = Encoding.UTF8.GetBytes(_post_data);
                var _post_hash = Encryptor.ComputeHash(_post_bytes);

                var _signature = ConvertHexString(_post_hash);
                {
                    _request.AddHeader("Key", ConnectKey);
                    _request.AddHeader("Sign", _signature);
                }
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
                    var _json_result = this.DeserializeObject<JToken>(response.Content);

                    var _json_error = _json_result.SelectToken("error");
                    if (_json_error == null)
                    {
                        var _json_success = _json_result.SelectToken("success");
                        if (_json_success != null && _json_success.Value<long>() != 1)
                        {
                            var _message = _result.message;

                            var _json_message = _json_result.SelectToken("response");
                            if (_json_message != null)
                                _message = _json_message.Value<string>();

                            _result.SetFailure(_message, ErrorCode.ResponseDataError);
                        }
                    }
                    else
                    {
                        _result.SetFailure(_json_error.Value<string>(), ErrorCode.ResponseDataError);
                    }
                }
                else
                {
                    _result.SetFailure(errorCode: ErrorCode.NotFoundData);
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