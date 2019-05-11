using OdinSdk.BaseLib.Extension;
using System;

namespace OdinSdk.BaseLib.Configuration
{
    /// <summary>
    ///
    /// </summary>
    public class CUnixTime
    {
        //-----------------------------------------------------------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------------------------------------------------------
        private static DateTime DateTimeUnixEpochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 1970년 1월 1일 00:00:00 협정 세계시(UTC)
        /// </summary>
        public static DateTime UnixEpoch
        {
            get
            {
                return DateTimeUnixEpochStart;
            }
        }

        /// <summary>
        ///  이 컴퓨터의 현재 UTC(협정 세계시) 부터의 경과 시간을 초로 환산하여 정수로 나타낸 것
        /// </summary>
        public static Int64 Now
        {
            get
            {
                return ConvertToUnixTime(UtcNow);
            }
        }

        /// <summary>
        ///  이 컴퓨터의 현재 UTC(협정 세계시) 부터의 경과 시간을 밀리초로 환산하여 정수로 나타낸 것
        /// </summary>
        public static Int64 NowMilli
        {
            get
            {
                return ConvertToUnixTimeMilli(UtcNow);
            }
        }

        /// <summary>
        /// 이 컴퓨터의 현재 날짜와 시간으로 설정되고 UTC(협정 세계시)로 표시되는 System.DateTime 개체를 가져옵니다.
        /// </summary>
        public static DateTime UtcNow
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        /// <summary>
        /// 이 컴퓨터의 현재 날짜와 시간으로 설정되고 현지 시간으로 표시되는 System.DateTime 개체를 가져옵니다.
        /// </summary>
        public static DateTime LocalNow
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static DateTime MaxValue
        {
            get
            {
                return DateTime.MaxValue.ToUniversalTime();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static DateTime MinValue
        {
            get
            {
                return DateTime.MinValue.ToUniversalTime();
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        // UnixTime - unit32, 64
        //-----------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 1970년 1월 1일 00:00:00 협정 세계시(UTC) 부터의 경과 시간을 초로 환산하여 정수로 나타낸 것
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static Int64 ConvertToUnixTime(DateTime datetime)
        {
            return (Int64)((datetime.ToUniversalTime() - UnixEpoch).TotalSeconds);
        }

        /// <summary>
        /// 1970년 1월 1일 00:00:00 협정 세계시(UTC) 부터의 경과 시간을 밀리초로 환산하여 정수로 나타낸 것
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static Int64 ConvertToUnixTimeMilli(DateTime datetime)
        {
            return (Int64)((datetime.ToUniversalTime() - UnixEpoch).TotalMilliseconds);
        }

        /// <summary>
        /// string문자를 datetime으로 변환 후 UTC를 초로 환산 합니다.
        /// </summary>
        /// <param name="timeString"></param>
        /// <returns></returns>
        public static Int64 ConvertToUnixTime(string timeString)
        {
            return ConvertToUnixTime(ConvertToUtcTime(timeString));
        }

        /// <summary>
        /// string문자를 datetime으로 변환 후 UTC를 밀리초로 환산 합니다.
        /// </summary>
        /// <param name="timeString"></param>
        /// <returns></returns>
        public static Int64 ConvertToUnixTimeMilli(string timeString)
        {
            return ConvertToUnixTimeMilli(ConvertToUtcTime(timeString));
        }

        /// <summary>
        /// 현재 세계시(UTC)에 offset의 시간을 더한 값을 계산한다.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Int64 AddUnixTime(Int64 offset)
        {
            return Now + offset;
        }

        /// <summary>
        /// 현재 세계시(UTC)에 timespan의 초를 더한 값을 계산한다.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Int64 AddUnixTime(TimeSpan offset)
        {
            return AddUnixTime((Int64)offset.TotalSeconds);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static Int64 ConvertToSeconds(Int64 unixTime)
        {
            return unixTime > 9999999999 ? unixTime / 1000 : unixTime;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static Int64 ConvertToMilliSeconds(Int64 unixTime)
        {
            return unixTime <= 9999999999 ? unixTime * 1000 : unixTime;
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        // UTC Time
        //-----------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 지정된 초 수를 더한 새로운 (UTC) System.DateTime을 반환합니다.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static DateTime ConvertToUtcTime(Int64 seconds)
        {
            return UnixEpoch.AddSeconds(seconds).ToUniversalTime();
        }

        /// <summary>
        /// 지정된 밀리초 수를 더한 새로운 (UTC) System.DateTime을 반환합니다.
        /// </summary>
        /// <param name="milliSeconds"></param>
        /// <returns></returns>
        public static DateTime ConvertToUtcTimeMilli(Int64 milliSeconds)
        {
            return UnixEpoch.AddMilliseconds(milliSeconds).ToUniversalTime();
        }

        /// <summary>
        /// 지정된 초에 timespan 수를 더한 새로운 (UTC) System.DateTime을 반환합니다.
        /// </summary>
        /// <param name="unixtime"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static DateTime ConvertToUtcTime(Int64 unixtime, TimeSpan offset)
        {
            return ConvertToUtcTime(unixtime) + offset;
        }

        /// <summary>
        /// 지정된 밀리초에 timespan 수를 더한 새로운 (UTC) System.DateTime을 반환합니다.
        /// </summary>
        /// <param name="unixtime"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static DateTime ConvertToUtcTimeMilli(Int64 unixtime, TimeSpan offset)
        {
            return ConvertToUtcTimeMilli(unixtime) + offset;
        }

        /// <summary>
        /// string 문자열을 UTC로 반환합니다.(문자열은 항상 timezone 형식을 포함 하여야 합니다.)
        /// </summary>
        /// <param name="timeWithZone"></param>
        /// <example>"2015-04-20T15:49:46.427+09:00"</example>
        /// <returns></returns>
        public static DateTime ConvertToUtcTime(string timeWithZone)
        {
            return Convert.ToDateTime(timeWithZone).ToUniversalTime();
        }

        /// <summary>
        /// UTC 값에 해당 하는 세계시(UTC)를 string 형식으로 반환 합니다.
        /// </summary>
        /// <param name="unixtime"></param>
        /// <returns></returns>
        public static string ToUtcTimeString(Int64 unixtime)
        {
            return ConvertToUtcTime(unixtime).ToDateTimeZoneString();
        }

        /// <summary>
        /// UTC 값에 해당 하는 로컬 시간을 string 형식으로 반환 합니다.
        /// </summary>
        /// <param name="unixTimeMilli"></param>
        /// <returns></returns>
        public static string ToUtcTimeMilliString(Int64 unixTimeMilli)
        {
            return ConvertToLocalTimeMilli(unixTimeMilli).ToDateTimeZoneString();
        }

        /// <summary>
        /// datetime에 해당 하는 값을 string 형식으로 반환 합니다.
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string ToUtcTimeString(DateTime datetime)
        {
            return datetime.ToUniversalTime().ToDateTimeZoneString();
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        // Local Time
        //-----------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// UTC 값에 해당 하는 로컬 시간을 datetiem 형식으로 반환 합니다.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static DateTime ConvertToLocalTime(Int64 seconds)
        {
            return ConvertToUtcTime(seconds).ToLocalTime();
        }

        /// <summary>
        /// UTC 값에 해당 하는 로컬 시간을 datetiem 형식으로 반환 합니다.
        /// </summary>
        /// <param name="milliSeconds"></param>
        /// <returns></returns>
        public static DateTime ConvertToLocalTimeMilli(Int64 milliSeconds)
        {
            return ConvertToUtcTimeMilli(milliSeconds).ToLocalTime();
        }

        /// <summary>
        /// string 문자열을 Local 시간으로 반환합니다.(문자열은 항상 timezone 형식을 포함 하여야 합니다.)
        /// </summary>
        /// <param name="timeWithZone"></param>
        /// <example>"2015-04-20T15:49:46.427+09:00"</example>
        /// <returns></returns>
        public static DateTime ConvertToLocalTime(string timeWithZone)
        {
            return ConvertToUtcTime(timeWithZone).ToLocalTime();
        }

        /// <summary>
        /// UTC 값에 해당 하는 로컬 시간을 string 형식으로 반환 합니다.
        /// </summary>
        /// <param name="unixtime"></param>
        /// <returns></returns>
        public static string ToLocalTimeString(Int64 unixtime)
        {
            return ConvertToLocalTime(unixtime).ToDateTimeString();
        }

        /// <summary>
        /// UTC 값에 해당 하는 로컬 시간을 string 형식으로 반환 합니다.
        /// </summary>
        /// <param name="unixTimeMilli"></param>
        /// <returns></returns>
        public static string ToLocalTimeMilliString(Int64 unixTimeMilli)
        {
            return ConvertToLocalTimeMilli(unixTimeMilli).ToDateTimeString();
        }

        /// <summary>
        /// datetime에 해당 하는 값을 string 형식으로 반환 합니다.
        /// </summary>
        /// <param name="localtime"></param>
        /// <returns></returns>
        public static string ToLocalTimeString(DateTime localtime)
        {
            return localtime.ToLocalTime().ToDateTimeString();
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        // parse
        //-----------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 문자열이 datetime으로 변환 가능 한지 여부를 확인 합니다.
        /// </summary>
        /// <param name="timeString">날짜와 시간이 포함된 변환할 문자열입니다.</param>
        /// <returns>s 매개 변수가 변환되면 true이고, 그렇지 않으면 false입니다.</returns>
        public static bool IsDateTimeFormat(string timeString)
        {
            return TryParse(timeString);
        }

        /// <summary>
        /// 날짜와 시간에 대한 지정된 문자열 표현을 해당 System.DateTime 요소로 변환하고, 변환에 성공했는지 여부를 나타내는 값을 반환합니다.
        /// </summary>
        /// <param name="s">날짜와 시간이 포함된 변환할 문자열입니다.</param>
        /// <returns>s 매개 변수가 변환되면 true이고, 그렇지 않으면 false입니다.</returns>
        public static bool TryParse(string s)
        {
            DateTime _tdate;
            return DateTime.TryParse(s, out _tdate);
        }

        /// <summary>
        /// 날짜 및 시간에 대한 지정된 문자열 표현을 해당 System.DateTime으로 변환합니다.
        /// </summary>
        /// <param name="s">날짜와 시간이 포함된 변환할 문자열입니다.</param>
        /// <returns>s에 포함된 날짜 및 시간에 해당하는 개체입니다.</returns>
        public static DateTime Parse(string s)
        {
            return DateTime.Parse(s);
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Get the first day of the month for any full date submitted
        /// </summary>
        /// <param name="p_dayOfTarget"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfMonth(DateTime p_dayOfTarget)
        {
            // set return value to the first day of the month
            // for any date passed in to the method

            // create a datetime variable set to the passed in date
            DateTime _firstDay = p_dayOfTarget;

            // remove all of the days in the month
            // except the first day and set the
            // variable to hold that date
            _firstDay = _firstDay.AddDays(-(_firstDay.Day - 1));

            // return the first day of the month
            return _firstDay;
        }

        /// <summary>
        /// Get the first day of the month for a month passed by it's integer value
        /// </summary>
        /// <param name="p_monthOfTarget"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfMonth(int p_monthOfTarget)
        {
            // set return value to the last day of the month
            // for any date passed in to the method

            // create a datetime variable set to the passed in date
            var _firstDay = new DateTime(UtcNow.Year, p_monthOfTarget, 1).ToUniversalTime();

            // remove all of the days in the month
            // except the first day and set the
            // variable to hold that date
            _firstDay = _firstDay.AddDays(-(_firstDay.Day - 1));

            // return the first day of the month
            return _firstDay;
        }

        /// <summary>
        /// Get the last day of the month for any full date
        /// </summary>
        /// <param name="p_dayOfTarget"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(DateTime p_dayOfTarget)
        {
            // set return value to the last day of the month
            // for any date passed in to the method

            // create a datetime variable set to the passed in date
            DateTime _lastDay = p_dayOfTarget;

            // overshoot the date by a month
            _lastDay = _lastDay.AddMonths(1);

            // remove all of the days in the next month
            // to get bumped down to the last day of the
            // previous month
            _lastDay = _lastDay.AddDays(-(_lastDay.Day));

            // return the last day of the month
            return _lastDay;
        }

        /// <summary>
        /// Get the last day of a month expressed by it's integer value
        /// </summary>
        /// <param name="p_monthOfTarget"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(int p_monthOfTarget)
        {
            // set return value to the last day of the month
            // for any date passed in to the method

            // create a datetime variable set to the passed in date
            var _lastDay = new DateTime(UtcNow.Year, p_monthOfTarget, 1).ToUniversalTime();

            // overshoot the date by a month
            _lastDay = _lastDay.AddMonths(1);

            // remove all of the days in the next month
            // to get bumped down to the last day of the
            // previous month
            _lastDay = _lastDay.AddDays(-(_lastDay.Day));

            // return the last day of the month
            return _lastDay;
        }

        //-----------------------------------------------------------------------------------------------------------------------------
        // pure coding
        //-----------------------------------------------------------------------------------------------------------------------------
        private static readonly int[] DaysToMonth365 = new int[]
        {
            0,
            31,
            59,
            90,
            120,
            151,
            181,
            212,
            243,
            273,
            304,
            334,
            365
        };

        private static readonly int[] DaysToMonth366 = new int[]
        {
            0,
            31,
            60,
            91,
            121,
            152,
            182,
            213,
            244,
            274,
            305,
            335,
            366
        };

        /// <summary>
        ///
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsLeapYear(int year)
        {
            if (year < 1 || year > 9999)
                throw new ArgumentOutOfRangeException("year", "ArgumentOutOfRange_Year");

            return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int DaysInYear(int year)
        {
            if (year < 1 || year > 9999)
                throw new ArgumentOutOfRangeException("year", "ArgumentOutOfRange_Year");

            var _days = year * 365 + (int)year / 4 - (int)year / 100 + (int)year / 400;
            return _days;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int DaysInMonth(int year, int month)
        {
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException("month", "ArgumentOutOfRange_Month");

            int[] array = IsLeapYear(year) ? DaysToMonth366 : DaysToMonth365;
            return array[month - 1];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="days"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static UInt64 GetUnixTimeSecond(int days, int hours, int minutes, int seconds)
        {
            return (UInt64)days * 3600L * 24L + (UInt64)hours * 3600L + (UInt64)minutes * 60L + (UInt64)seconds;
        }

        /*-----------------------------------------------------------------------------------------------------------------------------

            var _now_time = DateTime.UtcNow;

            var _unix_epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var _unix_time = (UInt64)((_now_time - _unix_epoch).TotalSeconds);

            var _days = DaysInYear(_now_time.Year - 1) - DaysInYear(1969)
                        + DaysInMonth(_now_time.Year, _now_time.Month)
                        + _now_time.Day - 1;

            var _hours = _now_time.Hour;
            var _minutes = _now_time.Minute;
            var _seconds = _now_time.Second;

            var _calc_time = GetUnixTimeSecond(_days, _hours, _minutes, _seconds);
            if (_unix_time != _calc_time)
                Debug.Write(String.Format("different: {0}, {1}", _unix_time, _calc_time));

        -----------------------------------------------------------------------------------------------------------------------------*/
    }
}