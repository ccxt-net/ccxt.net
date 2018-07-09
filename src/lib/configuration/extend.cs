using OdinSdk.BaseLib.Configuration;
using System;
using System.Globalization;
using CCXT.NET.Types;

namespace CCXT.NET.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class TExtend
    {
        private const int DoubleRoundingPrecisionDigits = 8;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal Normalize(this decimal value)
        {
            return Math.Round(value, DoubleRoundingPrecisionDigits, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(this ulong unixTimeStamp)
        {
            return CUnixTime.UnixEpoch.AddSeconds(unixTimeStamp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static ulong DateTimeToUnixTimeStamp(this DateTime dateTime)
        {
            return (ulong)Math.Floor(dateTime.Subtract(CUnixTime.UnixEpoch).TotalSeconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToStringNormalized(this decimal value)
        {
            return value.ToString("0." + new string('#', DoubleRoundingPrecisionDigits), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ParseDateTime(this string dateTime)
        {
            return DateTime.SpecifyKind(DateTime.ParseExact(dateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), DateTimeKind.Utc).ToLocalTime();
        }
    }
}