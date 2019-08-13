using Microsoft.Extensions.Configuration;
using CCXT.NET.Coin;
using System;
using System.IO;

namespace XUnit
{
    public class TestConfig
    {
        private static IConfigurationRoot __app_settings = null;

        /// <summary>
        ///
        /// </summary>
        public static IConfigurationRoot AppSettings
        {
            get
            {
                if (__app_settings == null)
                {
                    __app_settings = new ConfigurationBuilder()
                                  .SetBasePath(Directory.GetCurrentDirectory())
                                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                  .Build();
                }

                return __app_settings;
            }
        }

        private static IConfigurationSection __connect_sections = null;

        public static IConfigurationSection ConnectSections
        {
            get
            {
                if (__connect_sections == null)
                    __connect_sections = AppSettings.GetSection("connects");

                return __connect_sections;
            }
        }

        private static IConfigurationSection __app_sections = null;

        public static IConfigurationSection AppSections
        {
            get
            {
                if (__app_sections == null)
                    __app_sections = AppSettings.GetSection("appsettings");

                return __app_sections;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static bool SupportedCheck
        {
            get
            {
                return Convert.ToBoolean(AppSections["supported_check"]);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="exchange_name"></param>
        /// <returns></returns>
        public static ConnectionKey GetConnectionKey(string exchange_name)
        {
            var _exchange_name = exchange_name.ToLower();

            return new ConnectionKey
            {
                key_name = ConnectSections[$"{_exchange_name}.key.name"],
                connect_key = ConnectSections[$"{_exchange_name}.connect.key"],
                secret_key = ConnectSections[$"{_exchange_name}.secret.key"],
                user_name = ConnectSections[$"{_exchange_name}.user.name"],
                password = ConnectSections[$"{_exchange_name}.user.password"],
                user_mail = ConnectSections[$"{_exchange_name}.user.mail"],
                user_id = ConnectSections[$"{_exchange_name}.user.id"],
                wallet_id = ConnectSections[$"{_exchange_name}.wallet.id"]
            };
        }
    }
}