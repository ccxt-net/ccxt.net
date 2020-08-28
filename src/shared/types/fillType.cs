using System;

namespace CCXT.NET.Shared.Coin.Types
{
    /// <summary>
    ///
    /// </summary>
    public enum FillType : int
    {
        /// <summary>
        ///
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// FILL
        /// </summary>
        Fill = 1,

        /// <summary>
        /// Partial
        /// </summary>
        Partial_Fill = 2
    }

    /// <summary>
    ///
    /// </summary>
    public class FillTypeConverter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static FillType FromString(string s)
        {
            switch (s.ToLower())
            {
                case "complete_fill":
                case "fill":
                case "f":
                    return FillType.Fill;

                case "partial_fill":
                case "partial":
                case "p":
                    return FillType.Partial_Fill;

                default:
                    return FillType.Unknown;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string ToString(FillType v)
        {
            return Enum.GetName(typeof(FillType), v).ToLowerInvariant();
        }
    }
}