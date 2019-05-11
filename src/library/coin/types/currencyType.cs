using System;

namespace OdinSdk.BaseLib.Coin.Types
{
    /// <summary>
    ///
    /// </summary>
    public enum CurrencyType
    {
        /// <summary>
        ///
        /// </summary>
        unknown,

        /// <summary>
        ///
        /// </summary>
        cny,

        /// <summary>
        ///
        /// </summary>
        krw,

        /// <summary>
        ///
        /// </summary>
        usd,

        /// <summary>
        ///
        /// </summary>
        usdt,

        /// <summary>
        ///
        /// </summary>
        eur,

        /// <summary>
        ///
        /// </summary>
        hkd,

        /// <summary>
        ///
        /// </summary>
        jpy,

        /// <summary>
        ///
        /// </summary>
        btc
    }

    /// <summary>
    ///
    /// </summary>
    public class CurrencyTypeConverter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static CurrencyType FromString(string s)
        {
            var _result = CurrencyType.unknown;

            if (Enum.IsDefined(typeof(CurrencyType), s) == true)
                _result = (CurrencyType)Enum.Parse(typeof(CurrencyType), s);

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string ToString(CurrencyType v)
        {
            return Enum.GetName(typeof(CurrencyType), v).ToLowerInvariant();
        }
    }
}