using System;

namespace CCXT.NET.Shared.Converter
{
    /// <summary>
    ///
    /// </summary>
	public class Numerical
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static decimal TruncateDecimal(decimal value, int precision)
        {
            var _step = (decimal)Math.Pow(10, precision);
            return Math.Floor(_step * value) / _step;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int PrecisionFromString(string value)
        {
            var _spilt = value.Split('.');
            return (_spilt.Length > 1) ? _spilt[1].Length : 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static bool CompareDecimal(decimal lvalue, decimal rvalue, bool round = true)
        {
            var _result = false;

            var _l_values = lvalue.ToString().Split('.');
            var _r_values = rvalue.ToString().Split('.');

            if (_l_values[0] == _r_values[0])
            {
                var _l_decimal = Convert.ToDecimal(lvalue) - Convert.ToDecimal(_l_values[0]);
                var _r_decimal = Convert.ToDecimal(rvalue) - Convert.ToDecimal(_r_values[0]);

                var _l_value = _l_values.Length > 1 ? _l_values[1] : "0";
                var _r_value = _r_values.Length > 1 ? _r_values[1] : "0";

                var _length = _l_value.Length > _r_value.Length ? _r_value.Length : _l_value.Length;
                if (round)
                {
                    _l_decimal = Decimal.Round(_l_decimal, _length);
                    _r_decimal = Decimal.Round(_r_decimal, _length);
                }
                else
                {
                    _l_decimal = Convert.ToDecimal(_l_value.Substring(0, _length));
                    _r_decimal = Convert.ToDecimal(_r_value.Substring(0, _length));
                }

                _result = _l_decimal == _r_decimal;
            }

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxPrecision"></param>
        /// <returns></returns>
        public static string TruncateToString(decimal? value, int maxPrecision = 0)
        {
            var _result = (value ?? 0m).ToString();

            var _parts = _result.Split('.');
            if (maxPrecision > 0)
            {
                if (_parts.Length > 1)
                {
                    if (_parts[1].Length > maxPrecision)
                        _result = $"{_parts[0]}.{_parts[1].Substring(0, maxPrecision)}";
                }
            }
            else
            {
                _result = _parts[0];
            }

            return _result;
        }
    }
}