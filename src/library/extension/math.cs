using System;
using System.Globalization;

namespace CCXT.NET.Extension
{
    /// <summary>
    ///
    /// </summary>
    public static partial class CExtension
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
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T CreateCopy<T>(this T source) where T : new()
        {
            var _result = default(T);

            if (source != null)
            {
                _result = new T();

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
    }
}