using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

#pragma warning disable 1591

namespace OdinSdk.BaseLib.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class CConfigProvider : ConfigurationProvider, IConfigurationSource
    {
        /// <summary>
        /// 
        /// </summary>
        public CConfigProvider()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Load()
        {
            Data = UnencryptMyConfiguration();
        }

        private IDictionary<string, string> UnencryptMyConfiguration()
        {
            // do whatever you need to do here, for example load the file and unencrypt key by key
            //Like:
            var configValues = new Dictionary<string, string>
           {
                {"encrypt", "false"},
                {"aes_key", "cfyNZnGDw1JZel6xlWbD6wRyrr1fB+RJevhzWUzTzB4="},
                {"aes_iv", "R6UF7qWp9VeRPciPOk6R2g=="}
           };
            return configValues;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new CConfigProvider();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class CustomConfigProviderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddEncryptedProvider(this IConfigurationBuilder builder)
        {
            return builder.Add(new CConfigProvider());
        }
    }
}