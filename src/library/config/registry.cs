using Microsoft.Win32;
using System;
using System.IO;

namespace OdinSdk.BaseLib.Configuration
{
    /// <summary>
    ///
    /// </summary>
    public class CRegistry
    {
        private static CConfig __cconfig = new CConfig();

        //-------------------------------------------------------------------------------------------------------------------------
        //
        //-------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        public const string c_regRootKey = @"Software\OdinSoft\";

        /// <summary>
        ///
        /// </summary>
        public enum RegistryFolderType
        {
            /// <summary>
            ///
            /// </summary>
            client,

            /// <summary>
            ///
            /// </summary>
            server,

            /// <summary>
            ///
            /// </summary>
            manager,

            /// <summary>
            ///
            /// </summary>
            shared
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_folder_type"></param>
        /// <param name="p_registry_hive"></param>
        public CRegistry(RegistryFolderType p_folder_type = RegistryFolderType.shared, RegistryHive p_registry_hive = RegistryHive.LocalMachine)
            : this("ODINSOFT", "SDK", "V5.2.2017.01", p_folder_type, p_registry_hive)
        {
        }

        /// <summary>
        /// 사용 시 정의를 제일 위 상단에 정의 바랍니다.
        /// 파라메터는(client, server, manager, shared)등을 사용 할 수 있습니다.
        /// </summary>
        /// <param name="p_solution_name"></param>
        /// <param name="p_module_name"></param>
        /// <param name="p_version"></param>
        /// <param name="p_folder_type"></param>
        /// <param name="p_registry_hive"></param>
        public CRegistry(
                string p_solution_name, string p_module_name, string p_version,
                RegistryFolderType p_folder_type = RegistryFolderType.shared,
                RegistryHive p_registry_hive = RegistryHive.LocalMachine
            )
        {
            this.Version = p_version;
            this.SolutionName = p_solution_name;
            this.ModuleName = p_module_name;
            this.FolderType = p_folder_type;

            this.SdkRegistryRoot = Path.Combine(this.SdkResistryBase, p_module_name, p_folder_type.ToString());
            this.SdkRegisryHive = p_registry_hive;
        }

        //-------------------------------------------------------------------------------------------------------------------------
        //
        //-------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        public static bool UserInteractive
        {
            get
            {
                return __cconfig.IsWindows;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string SdkRegistryRoot
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string SdkResistryBase
        {
            get
            {
                return Path.Combine(c_regRootKey, this.Version, this.SolutionName);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public RegistryHive SdkRegisryHive
        {
            get;
            set;
        }

        //-------------------------------------------------------------------------------------------------------------------------
        //
        //-------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_registry_root"></param>
        /// <returns></returns>
        public RegistryKey GetRegistryKey(string p_registry_root = "")
        {
            var _regloc = RegistryKey.OpenBaseKey
                    (
                        SdkRegisryHive,
                        __cconfig.Is64BitOperatingSystem == true ? RegistryView.Registry64 : RegistryView.Registry32
                    );

            if (String.IsNullOrEmpty(p_registry_root))
                p_registry_root = SdkRegistryRoot;

            var _result = _regloc.OpenSubKey(p_registry_root, true);
            if (_result == null)
                _result = _regloc.CreateSubKey(p_registry_root, true);

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        public void DeleteRegistry(string p_registry_root = "")
        {
            var _regkey = GetRegistryKey(p_registry_root);
            if (_regkey != null)
                _regkey.DeleteSubKeyTree("");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_regkey"></param>
        /// <param name="p_defult"></param>
        /// <param name="p_registry_root"></param>
        /// <returns></returns>
        public string GetRegValue(string p_regkey, string p_defult = "", string p_registry_root = "")
        {
            var _result = "";

            var _regkey = GetRegistryKey(p_registry_root);
            if (_regkey != null)
            {
                var _value = _regkey.GetValue(p_regkey, null);
                if (_value == null)
                {
                    _result = p_defult;
                    SetRegValue(p_regkey, p_defult, p_registry_root);
                }
                else
                    _result = _value.ToString();

                _regkey.Flush();
            }

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_regkey"></param>
        /// <param name="p_value"></param>
        /// <param name="p_registry_root"></param>
        public void SetRegValue(string p_regkey, object p_value, string p_registry_root = "")
        {
            var _regkey = GetRegistryKey(p_registry_root);
            if (_regkey != null)
            {
                _regkey.SetValue(p_regkey, p_value);
                _regkey.Flush();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_appkey"></param>
        /// <param name="p_default"></param>
        /// <param name="p_registry_root"></param>
        /// <returns></returns>
        public string GetAppString(string p_appkey, string p_default = "", string p_registry_root = "")
        {
            var _result = GetRegValue(p_appkey, p_default, p_registry_root);

            if (String.IsNullOrEmpty(_result) == true)
            {
                _result = __cconfig.GetAppString(p_appkey);

                if (String.IsNullOrEmpty(_result) == false)
                    SetRegValue(p_appkey, _result, p_registry_root);
                else
                    _result = p_default;
            }

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_appkey"></param>
        /// <param name="p_default"></param>
        /// <param name="p_registry_root"></param>
        /// <returns></returns>
        public int GetAppInteger(string p_appkey, int p_default = 0, string p_registry_root = "")
        {
            var _value = GetAppString(p_appkey: p_appkey, p_registry_root: p_registry_root);
            return String.IsNullOrEmpty(_value) ? p_default : Convert.ToInt32(_value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_appkey"></param>
        /// <param name="p_default"></param>
        /// <param name="p_registry_root"></param>
        /// <returns></returns>
        public bool GetAppBoolean(string p_appkey, bool p_default = false, string p_registry_root = "")
        {
            var _value = GetAppString(p_appkey: p_appkey, p_registry_root: p_registry_root);
            return String.IsNullOrEmpty(_value) ? p_default : _value.ToLower() == "true";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p_appkey"></param>
        /// <param name="p_default"></param>
        /// <param name="p_registry_root"></param>
        /// <returns></returns>
        public int GetHexInteger(string p_appkey, int p_default = 0, string p_registry_root = "")
        {
            var _value = GetAppString(p_appkey: p_appkey, p_registry_root: p_registry_root);
            return String.IsNullOrEmpty(_value) ? p_default : Convert.ToInt32(_value, 16);
        }

        //-------------------------------------------------------------------------------------------------------------------------
        // For Shared
        //-------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        public string SolutionName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string ModuleName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string Version
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public RegistryFolderType FolderType
        {
            get;
            set;
        }

        //-------------------------------------------------------------------------------------------------------------------------
        //
        //-------------------------------------------------------------------------------------------------------------------------

        private readonly string RegistryNDP = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

        /// <summary>
        ///
        /// </summary>
        public int Get46or461FromRegistry()
        {
            var _result = 0;

            var _regloc = RegistryKey.OpenBaseKey
                    (
                        RegistryHive.LocalMachine,
                        __cconfig.Is64BitOperatingSystem == true ? RegistryView.Registry64 : RegistryView.Registry32
                    );

            var _ndpKey = _regloc.OpenSubKey(RegistryNDP);
            if (_ndpKey != null)
                _result = Convert.ToInt32(_ndpKey.GetValue("Release") ?? "0");

            return _result;
        }

        ///// <summary>
        /////
        ///// </summary>
        ///// <returns></returns>
        //public string GetFrameworkVersion()
        //{
        //    // Retrieve the version of the framework executing this program
        //    return String.Format(@"v{0}.{1}.{2}", Environment.Version.Major, Environment.Version.Minor, Environment.Version.Build);
        //}

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string GetFrameworkRoot()
        {
            // This is the location of the .Net Framework Registry Key
            var _framwork_reg_path = @"Software\Microsoft\.NetFramework";

            var _regloc = RegistryKey.OpenBaseKey
            (
                RegistryHive.LocalMachine,
                __cconfig.Is64BitOperatingSystem == true ? RegistryView.Registry64 : RegistryView.Registry32
            );

            // Get a non-writable key from the registry
            var _net_framework = _regloc.OpenSubKey(_framwork_reg_path, false);

            // Retrieve the install root path for the framework
            var _install_root = _net_framework.GetValue("InstallRoot") ?? "";

            return _install_root.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool CheckNetFrameworkInstalled(int releaseKey = 393295)
        {
            var _result = false;

            // Opens the registry key for the .NET Framework entry.
            var _framwork_reg_path = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            var _regloc = RegistryKey.OpenBaseKey
            (
                RegistryHive.LocalMachine,
                __cconfig.Is64BitOperatingSystem == true ? RegistryView.Registry64 : RegistryView.Registry32
            );

            /*
               if (releaseKey >= 393295) {
                    return "4.6 or later";
                }
                if ((releaseKey >= 379893)) {
		            return "4.5.2 or later";
                }
	            if ((releaseKey >= 378675)) {
		            return "4.5.1 or later";
	            }
	            if ((releaseKey >= 378389)) {
		            return "4.5 or later";
	            }

                return "No 4.5 or later version detected";
            */

            using (var _ndpkey = _regloc.OpenSubKey(_framwork_reg_path, false))
            {
                var _release_key = _ndpkey.GetValue("Release") ?? "0";
                if (Convert.ToInt32(_release_key) >= releaseKey)
                    _result = true;
            }

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="releaseKey"></param>
        /// <returns></returns>
        public string GetDotNetFrameworkName(int releaseKey = 393295)
        {
            var _result = "";

            if (releaseKey >= 393295)
                _result = "4.6 or later";
            else if ((releaseKey >= 379893))
                _result = "4.5.2 or later";
            else if ((releaseKey >= 378675))
                _result = "4.5.1 or later";
            else //if ((releaseKey >= 378389))
                _result = "4.5 or later";

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public bool IsProgramInstalled(string displayName)
        {
            var _result = false;

            var _uninstall_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

            var _regloc = RegistryKey.OpenBaseKey
            (
                RegistryHive.LocalMachine,
                RegistryView.Registry32
            );

            using (var _regkey = _regloc.OpenSubKey(_uninstall_key))
            {
                foreach (string _name in _regkey.GetSubKeyNames())
                {
                    using (var _key = _regkey.OpenSubKey(_name))
                    {
                        var _value = _key.GetValue("DisplayName");
                        if (_value == null)
                            continue;

                        if (String.Compare(_value.ToString(), 0, displayName, 0, displayName.Length) == 0)
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
        /// <param name="is64bit"></param>
        /// <returns></returns>
        public bool IsRedistributableInstalled(bool is64bit)
        {
            var _result = false;

            var _uninstall_key = @"SOFTWARE\Microsoft\VisualStudio\14.0\VC\Runtimes\";
            if (is64bit == true)
                _uninstall_key += @"x64";
            else
                _uninstall_key += @"x86";

            var _regloc = RegistryKey.OpenBaseKey
            (
                RegistryHive.LocalMachine,
                __cconfig.Is64BitOperatingSystem == true ? RegistryView.Registry64 : RegistryView.Registry32
            );

            if (_regloc != null)
            {
                var _regkey = _regloc.OpenSubKey(_uninstall_key, false);
                if (_regkey != null)
                {
                    var _value = _regkey.GetValue("Installed") ?? "0";
                    if (_value.ToString() == "1")
                        _result = true;
                }
            }

            return _result;
        }

        //-------------------------------------------------------------------------------------------------------------------------
        // For Client
        //-------------------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------------------
        // For Server
        //-------------------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------------------
        //
        //-------------------------------------------------------------------------------------------------------------------------
    }
}