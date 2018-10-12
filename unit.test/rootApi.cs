using OdinSdk.BaseLib.Coin;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Xunit.Abstractions;

namespace XUnit
{
    public class RootApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputHelper"></param>
        public RootApi(ITestOutputHelper outputHelper)
        {
            XApiClient.TestXUnitMode = XUnitMode.UseJsonFile;
            this.tConsole = outputHelper;
        }

        /// <summary>
        /// 
        /// </summary>
        public ITestOutputHelper tConsole
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string tRootFolder
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void WriteLine(string message)
        {
            Debug.WriteLine(message);
            this.tConsole.WriteLine(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="api_client"></param>
        /// <param name="json_object"></param>
        public void WriteJson(XApiClient api_client, object json_object)
        {
            var _message = api_client.SerializeObject(json_object);
            this.WriteLine(_message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="api_client"></param>
        /// <param name="args"></param>
        /// <param name="caller_name"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetJsonContent(XApiClient api_client, string caller_name, Dictionary<string, object> args = null)
        {
            return GetJsonContent(api_client, tRootFolder, caller_name, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="api_client"></param>
        /// <param name="args"></param>
        /// <param name="caller_name"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetJsonContent(XApiClient api_client, string root_folder, string caller_name, Dictionary<string, object> args = null)
        {
            if (args == null)
                args = new Dictionary<string, object>();
#if DEBUG
            if (XApiClient.TestXUnitMode != XUnitMode.UseExchangeServer)
            {
                var _file_name = Path.Combine(root_folder, api_client.DealerName, $"{caller_name}.json");
                if (File.Exists(_file_name) == true)
                {
                    var _json_string = File.ReadAllText(_file_name);
                    this.WriteLine(_file_name);

                    this.WriteLine(_json_string);

                    if (args.ContainsKey("jsonContent") == true)
                        args.Remove("jsonContent");
                    args.Add("jsonContent", _json_string);
                }
            }
#endif
            return args;
        }
    }
}