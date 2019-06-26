using Microsoft.Extensions.Configuration;
using CCXT.NET.Cryption;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

#pragma warning disable 1589, 1591

namespace CCXT.NET.Configuration
{
    /// <summary>
    ///
    /// </summary>
    public class CConfig
    {
        /// <summary>
        ///
        /// </summary>
        public CConfig() : this("appsettings")
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="section_name"></param>
        public CConfig(string section_name)
        {
            __section_name = section_name;
        }

        private string __section_name;

        private static CCryption __cryptor = null;

        public CCryption Cryptor
        {
            get
            {
                if (__cryptor == null)
                {
                    var _key = this.ConfigRoot["aes_key"];
                    var _iv = this.ConfigRoot["aes_iv"];

                    __cryptor = new CCryption(_key, _iv);
                }

                return __cryptor;
            }
        }

        private static bool? __is_encrypt_connection_string;

        public bool IsEncryptionConnectionString
        {
            get
            {
                if (__is_encrypt_connection_string == null)
                    __is_encrypt_connection_string = this.ConfigRoot["encrypt"] == "true";

                return __is_encrypt_connection_string.Value;
            }
        }

        public void SetConfigRoot(IConfiguration configuration)
        {
            __config_root = configuration;
        }

        private static IConfiguration __config_root = null;

        public IConfiguration ConfigRoot
        {
            get
            {
                if (__config_root == null)
                    throw new Exception("config root is null");

                return __config_root;
            }
        }

        public string GetConnectionString(string name)
        {
            return this.ConfigRoot.GetConnectionString(name);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> AllAppString()
        {
            var _result = new Dictionary<string, string>();

            var _config_section = ConfigRoot.GetSection(__section_name).GetChildren().ToList();
            foreach (ConfigurationSection _c in _config_section)
                _result.Add(_c.Key, _c.Value);

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_appkey"></param>
        /// <param name="p_default"></param>
        /// <returns></returns>
        public string GetAppString(string p_appkey, string p_default = "")
        {
            var _result = "";

            if (String.IsNullOrEmpty(p_appkey) == false)
            {
                var _config_section = ConfigRoot.GetSection(__section_name);
                if (this.IsWindows == true)
                {
                    _result = _config_section[p_appkey + ".debug"];

                    if (_result == null)
                        _result = _config_section[p_appkey];
                }
                else
                    _result = _config_section[p_appkey];
            }

            if (String.IsNullOrEmpty(_result) == true)
                _result = p_default;

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_appkey"></param>
        /// <returns></returns>
        public DateTime GetAppDateTime(string p_appkey)
        {
            var _value = GetAppString(p_appkey);
            return String.IsNullOrEmpty(_value) ? CUnixTime.UtcNow : Convert.ToDateTime(_value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_appkey"></param>
        /// <param name="p_default"></param>
        /// <returns></returns>
        public int GetAppInteger(string p_appkey, int p_default = 0)
        {
            var _value = GetAppString(p_appkey);
            return String.IsNullOrEmpty(_value) ? p_default : Convert.ToInt32(_value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_appkey"></param>
        /// <param name="p_default"></param>
        /// <returns></returns>
        public decimal GetAppDecimal(string p_appkey, decimal p_default = 0)
        {
            var _value = GetAppString(p_appkey);
            return String.IsNullOrEmpty(_value) ? p_default : Convert.ToDecimal(_value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_appkey"></param>
        /// <param name="p_default"></param>
        /// <returns></returns>
        public bool GetAppBoolean(string p_appkey, bool p_default = false)
        {
            var _value = GetAppString(p_appkey);
            return String.IsNullOrEmpty(_value) ? p_default : _value.ToLower() == "true";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_appkey"></param>
        /// <param name="p_default"></param>
        /// <returns></returns>
        public int GetHexInteger(string p_appkey, int p_default = 0)
        {
            var _value = GetAppString(p_appkey);
            return String.IsNullOrEmpty(_value) ? p_default : Convert.ToInt32(_value, 16);
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------------------------------------------------------
        private string __product_name = null;

        /// <summary>
        ///
        /// </summary>
        public string ProductName
        {
            get
            {
                if (__product_name == null)
                    __product_name = GetAppString("product.name");

                return __product_name;
            }
        }

        private string __applicationFolder = null;

        /// <summary>
        ///
        /// </summary>
        public string ApplicationDataFolder
        {
            get
            {
                if (__applicationFolder == null)
                    __applicationFolder = Path.Combine(Environment.GetEnvironmentVariable("APPDATA"), "OdinSoft");

                return __applicationFolder;
            }
        }

        private string __workingFolder = null;

        /// <summary>
        ///
        /// </summary>
        public string WorkingFolder
        {
            get
            {
                if (__workingFolder == null)
                {
                    __workingFolder = GetAppString("working.folder");

                    if (String.IsNullOrEmpty(__workingFolder) == true)
                        __workingFolder = ApplicationDataFolder;
                }

                return __workingFolder;
            }
        }

        public string CreateFolder(params string[] p_folders)
        {
            var _result = Path.Combine(p_folders);

            if (System.IO.Directory.Exists(_result) == false)
                System.IO.Directory.CreateDirectory(_result);

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_folders"></param>
        /// <returns></returns>
        public string GetWorkingFolder(params string[] p_folders)
        {
            return CreateFolder(WorkingFolder, Path.Combine(p_folders));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_folders"></param>
        /// <returns></returns>
        public string GetWorkingFolderWithDateTime(params string[] p_folders)
        {
            return GetWorkingFolder(Path.Combine(p_folders), String.Format("{0:yyyyMM}", CUnixTime.UtcNow));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_folders"></param>
        /// <returns></returns>
        public string GetDownloadFolder(params string[] p_folders)
        {
            return CreateFolder(WorkingFolder, "download", Path.Combine(p_folders));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_folders"></param>
        /// <returns></returns>
        public string GetLoggingFolder(params string[] p_folders)
        {
            return CreateFolder(WorkingFolder, "logging", Path.Combine(p_folders));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_folders"></param>
        /// <returns></returns>
        public string GetApplicationFolder(params string[] p_folders)
        {
            return CreateFolder(ApplicationDataFolder, Path.Combine(p_folders));
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string[] GetIPAddresses()
        {
            return GetIPAddresses(Dns.GetHostName());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="host_name">name of host PC</param>
        /// <returns></returns>
        public string[] GetIPAddresses(string host_name)
        {
            string[] _result;

            if (IsValidIP(host_name) == false)
            {
                var _ipadrs = Dns.GetHostEntryAsync(host_name)
                                            .GetAwaiter()
                                            .GetResult()
                                            .AddressList;

                _result = new string[_ipadrs.Length];

                var i = 0;
                foreach (var _ip in _ipadrs)
                {
                    if (_ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        _result[i++] = _ip.ToString();
                }

                Array.Resize(ref _result, i);
            }
            else
            {
                _result = new string[] { host_name };
            }

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string GetIPAddress()
        {
            return GetIPAddress(Dns.GetHostName());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="host_name">name of host PC</param>
        /// <returns></returns>
        public string GetIPAddress(string host_name)
        {
            var _result = host_name;

            if (IsValidIP(host_name) == false)
            {
                var _ipHostInfo = Dns.GetHostEntryAsync(host_name)
                                        .GetAwaiter()
                                        .GetResult();

                foreach (var _ip in _ipHostInfo.AddressList)
                {
                    if (_ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        _result = _ip.ToString();
                        break;
                    }
                }
            }

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string GetIP6Address()
        {
            return GetIP6Address(Dns.GetHostName());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="host_name">name of host PC</param>
        /// <returns></returns>
        public string GetIP6Address(string host_name)
        {
            var _result = host_name;

            if (IsValidIP(host_name) == false)
            {
                var _ipHostInfo = Dns.GetHostEntryAsync(host_name)
                                            .GetAwaiter()
                                            .GetResult();

                foreach (var _ip in _ipHostInfo.AddressList)
                {
                    if (_ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        _result = _ip.ToString().Split('%')[0];
                        break;
                    }
                }
            }

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string GetMacAddress()
        {
            var _nics = NetworkInterface.GetAllNetworkInterfaces();

            var _mac_address = "";
            foreach (NetworkInterface _adapter in _nics)
            {
                // only return MAC Address from first card
                if (String.IsNullOrEmpty(_mac_address) == true)
                {
                    //IPInterfaceProperties properties = adapter.GetIPProperties(); Line is not required
                    _mac_address = _adapter.GetPhysicalAddress().ToString();
                }
            }

            return _mac_address;
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        /// <param name="host_name">name of host PC</param>
        /// <returns></returns>
        public string GetLocalIpAddress(string host_name)
        {
            return IsLocalMachine(host_name) == true ? IPAddress : host_name;
        }

        /// <summary>
        /// method to validate an IP address
        /// using regular expressions. The pattern
        /// being used will validate an ip address
        /// with the range of 1.0.0.0 to 255.255.255.255
        /// </summary>
        /// <param name="ip_address">Address to validate</param>
        /// <returns></returns>
        public bool IsValidIP(string ip_address)
        {
            //create our match pattern
            const string _pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";

            //create our Regular Expression object
            Regex _regCheck = new Regex(_pattern);

            //boolean variable to hold the status
            var _result = false;

            //check to make sure an ip address was provided
            if (ip_address == "")
            {
                //no address provided so return false
                _result = false;
            }
            else
            {
                //address provided so use the IsMatch Method
                //of the Regular Expression object
                _result = _regCheck.IsMatch(ip_address, 0);
            }

            //return the results
            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="host_name">name of host PC</param>
        /// <returns></returns>
        public bool IsLocalMachine(string host_name)
        {
            var _result = false;

            if (String.IsNullOrEmpty(host_name) == true ||
                host_name.ToLower() == "localhost" || host_name == "127.0.0.1" ||
                host_name.ToLower() == MachineName || host_name == "::1"
                )
            {
                _result = true;
            }
            else
            {
                string[] _serverIPs = GetIPAddresses(host_name);
                if (_serverIPs.Length > 0)
                {
                    string[] _ipadrs = GetIPAddresses();
                    foreach (string _ip in _ipadrs)
                    {
                        if (_ip == _serverIPs[0])
                        {
                            _result = true;
                            break;
                        }
                    }
                }
            }

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_local_host_name"></param>
        /// <param name="p_remote_host_name"></param>
        /// <returns></returns>
        public bool IsSameMachine(string p_local_host_name, string p_remote_host_name)
        {
            var _result = false;

            if (p_local_host_name == p_remote_host_name)
            {
                _result = true;
            }
            else
            {
                string[] _localIPs = GetIPAddresses(p_local_host_name);
                if (_localIPs.Length > 0)
                {
                    string[] _remoteIPs = GetIPAddresses(p_remote_host_name);
                    foreach (string _ip in _remoteIPs)
                    {
                        if (_ip == _localIPs[0])
                        {
                            _result = true;
                            break;
                        }
                    }
                }
            }

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        public string MachineName
        {
            get
            {
                return Environment.MachineName.ToLower();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string UserDomainName
        {
            get
            {
                return ConfigRoot["USERDOMAIN"].ToLower();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string UserName
        {
            get
            {
                if (IsWindows == true)
                    return ConfigRoot["USERNAME"].ToLower();
                else
                    return ConfigRoot["USER"].ToLower();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsUnix
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsWindows
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsOSX
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            }
        }

        public bool Is64BitOperatingSystem
        {
            get
            {
                return RuntimeInformation.OSArchitecture == Architecture.X64 || RuntimeInformation.OSArchitecture == Architecture.Arm64;
            }
        }

        private string[] __ip_addresses;

        /// <summary>
        ///
        /// </summary>
        public string[] IPAddresses
        {
            get
            {
                if (__ip_addresses == null)
                    __ip_addresses = GetIPAddresses();

                return __ip_addresses;
            }
        }

        private string __ip_address;

        /// <summary>
        ///
        /// </summary>
        public string IPAddress
        {
            get
            {
                if (__ip_address == null)
                    __ip_address = GetIPAddress();

                return __ip_address;
            }
        }

        private string __ip6_address;

        /// <summary>
        ///
        /// </summary>
        public string IP6Address
        {
            get
            {
                if (__ip6_address == null)
                    __ip6_address = GetIP6Address();

                return __ip6_address;
            }
        }

        private string __mac_address;

        /// <summary>
        ///
        /// </summary>
        public string MacAddress
        {
            get
            {
                if (__mac_address == null)
                    __mac_address = GetMacAddress();

                return __mac_address;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_assembly_string"></param>
        /// <returns></returns>
        public string GetVersion(string p_assembly_string)
        {
            string[] _splitNames = p_assembly_string.Split(',');
            var _version = "";

            foreach (string f in _splitNames)
            {
                if (f.Trim().StartsWith("Version="))
                    _version = f.Trim().Replace("Version=", "");
            }

            return _version;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_assembly_string"></param>
        /// <returns></returns>
        public string GetPublicKeyToken(string p_assembly_string)
        {
            string[] _splitNames = p_assembly_string.Split(',');
            var _keyToken = "";

            foreach (string f in _splitNames)
            {
                if (f.Trim().StartsWith("PublicKeyToken="))
                    _keyToken = f.Trim().Replace("PublicKeyToken=", "").ToLower();
            }

            return _keyToken;
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        // 영어(미국), 일본어(일본), 중국어(중국), 중국어(간체), 중국어(번체), 한국어(대한민국), 태국어(태국). 필리핀어(필리핀)
        //-----------------------------------------------------------------------------------------------------------------------------
        private ArrayList __culture = new ArrayList(new string[] { "ko-kr", "en-us", "ja-jp", "zh-cn", "zh-chs", "zh-cht", "th-th", "fil-ph" });

        /// <summary>
        ///
        /// </summary>
        public string GetDefaultCulture
        {
            get
            {
                return __culture[0].ToString();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_culture"></param>
        /// <returns></returns>
        public bool ContainsCultureName(string p_culture)
        {
            return __culture.Contains(p_culture.ToLower());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_culture"></param>
        /// <returns></returns>
        public string GetCultureName(string p_culture)
        {
            var _result = p_culture;

            for (int i = 0; i < __culture.Count; i++)
            {
                if (_result.ToLower().Equals(__culture[i].ToString()) == true)
                    return _result;
            }

            switch (_result.ToLower().Substring(0, 2))
            {
                case "ko":
                    _result = __culture[0].ToString();
                    break;

                case "en":
                    _result = __culture[1].ToString();
                    break;

                case "ja":
                    _result = __culture[2].ToString();
                    break;

                case "zh":
                    _result = __culture[3].ToString();
                    break;

                default:
                    _result = __culture[1].ToString();
                    break;
            }

            return _result;
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// SQL ConnectionString에 application name을 추가 합니다.
        /// </summary>
        /// <param name="p_connection_string"></param>
        /// <param name="p_application_name"></param>
        /// <returns></returns>
        public string AppendAppNameInConnectionString(string p_connection_string, string p_application_name)
        {
            var _result = "";

            const string _cApplication = "application name";

            string[] _nodes = p_connection_string.Split(';');
            foreach (string _node in _nodes)
            {
                if (_node.Split('=')[0].ToLower() != _cApplication)
                    _result += _node + ";";
            }

            _result += String.Format("{0}={1};", _cApplication, p_application_name);

            return _result;
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool IsAdministrator()
        {
            var _result = false;

            //WindowsIdentity _identity = WindowsIdentity.GetCurrent();
            //if (_identity != null)
            //{
            //    WindowsPrincipal principal = new WindowsPrincipal(_identity);
            //    _result = principal.IsInRole(WindowsBuiltInRole.Administrator);
            //}

            return _result;
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------------------------------------------------------
    }
}