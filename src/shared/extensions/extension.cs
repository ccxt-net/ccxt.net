using CCXT.NET.Shared.Configuration;
using RestSharp;
using System;
using System.Globalization;
using System.Linq;

namespace CCXT.NET.Shared.Extension
{
    /// <summary>
    ///
    /// </summary>
    public static partial class CExtension
    {
        public static void RemoveParameters(this RestRequest request)
        {
            var _params = request.Parameters.ToArray();
            foreach (var _param in _params)
                request.RemoveParameter(_param);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T CreateCopy<T>(this T source) where T : new()
        {
            var _result = new T();

            if (source != null)
            {
                //_result = new T();

                foreach (var _s in source.GetType().GetProperties())
                {
                    foreach (var _t in _result.GetType().GetProperties())
                    {
                        if (_t.Name != _s.Name)
                            continue;

                        (_t.GetSetMethod()).Invoke(_result,
                                new object[]
                                {
                                    _s.GetGetMethod().Invoke(source, null)
                                });
                    }
                };
            }

            return _result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="string_date"></param>
        /// <returns></returns>
        public static bool StringDateValidationCheck(this string string_date)
        {
            var _result = false;

            if (String.IsNullOrEmpty(string_date) == false)
            {
                var _year = 0;
                var _month = 0;
                var _day = 0;

                // "20201225"
                if (string_date.Length == 8)
                {
                    _year = Convert.ToInt32(string_date.Substring(0, 4));
                    _month = Convert.ToInt32(string_date.Substring(4, 2));
                    _day = Convert.ToInt32(string_date.Substring(6, 2));
                }
                else if (string_date.Length == 10)
                // "2020/12/25", "2020-12-25"
                {
                    _year = Convert.ToInt32(string_date.Substring(0, 4));
                    _month = Convert.ToInt32(string_date.Substring(5, 2));
                    _day = Convert.ToInt32(string_date.Substring(8, 2));
                }

                if (_year <= DateTime.MaxValue.Year && _year >= DateTime.MinValue.Year)
                {
                    if (_month >= 1 && _month <= 12)
                    {
                        if (_day >= 1 && _day <= DateTime.DaysInMonth(_year, _month))
                            _result = true;
                    }
                }
            }

            return _result;
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
            return (ulong)Math.Floor(dateTime.ToUniversalTime().Subtract(CUnixTime.UnixEpoch).TotalSeconds);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ParseDateTime(this string dateTime)
        {
            return DateTime.SpecifyKind(DateTime.ParseExact(dateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), DateTimeKind.Utc);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static DateTime ChangeTime(this DateTime dateTime, decimal hours, decimal minutes, decimal seconds, decimal milliseconds)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                (int)hours,
                (int)minutes,
                (int)seconds,
                (int)milliseconds,
                dateTime.Kind);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static string ToLogDateTimeString()
        {
            return CUnixTime.UtcNow.ToLongDateTimeString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string ToLongDateTimeString(this DateTime datetime)
        {
            return String.Format("{0:yyyy-MM-dd-HH:mm:ss}", datetime);
        }

        /// <summary>
        /// 연-월-일(yyyy-MM-dd) 형식으로 변환 합니다.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToDateString(this DateTime d)
        {
            return d.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 연월일(yyyyMMdd) 형식으로 변환 합니다.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToDateString2(this DateTime d)
        {
            return d.ToString("yyyyMMdd");
        }

        /// <summary>
        /// 연-월-일 시:분:초(yyyy-MM-dd HH:mm:ss) 형식으로 변환 합니다.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToDateTimeString(this DateTime d)
        {
            return d.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 연-월-일T시:분:초.Zone(yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK) 형식으로 변환 합니다.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToDateTimeZoneString(this DateTime d)
        {
            return d.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK");
        }

        /// <summary>
        /// 연-월-일 시:분(yyyy-MM-dd HH:mm) 형식으로 변환 합니다.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToDateHHmmString(this DateTime d)
        {
            return d.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// 시:분:초(HH:mm:ss) 형식으로 변환 합니다.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToTimeHHmmssString(this DateTime d)
        {
            return d.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 시분초(HHmmss) 형식으로 변환 합니다.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToTimeHHmmssString2(this DateTime d)
        {
            return d.ToString("HHmmss");
        }

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
        /// <param name="value"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal Normalize(this decimal value, int decimals)
        {
            return Math.Round(value, decimals, MidpointRounding.AwayFromZero);
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
        /// <param name="figure"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static decimal RoundDown(this decimal figure, int precision)
        {
            return Decimal.Floor(figure * (decimal)Math.Pow(10, precision)) / (decimal)Math.Pow(10, precision);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="figure"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static decimal RoundUp(this decimal figure, int precision)
        {
            return Decimal.Ceiling(figure * (decimal)Math.Pow(10, precision)) / (decimal)Math.Pow(10, precision);
        }
    }
}