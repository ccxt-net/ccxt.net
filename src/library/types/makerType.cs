using System;

namespace OdinSdk.BaseLib.Coin.Types
{
    /// <summary>
    /// 
    /// </summary>
    public enum MakerType : int
    {
        /// <summary>
        /// 
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// market
        /// </summary>
        Maker = 1,

        /// <summary>
        /// limit
        /// </summary>
        Taker = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public class MakerTypeConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static MakerType FromString(string s)
        {
            switch (s.ToLower())
            {
                case "maker":
                case "m":
                    return MakerType.Maker;

                case "taker":
                case "t":
                    return MakerType.Taker;

                default:
                    return MakerType.Unknown;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string ToString(MakerType v)
        {
            return Enum.GetName(typeof(MakerType), v).ToLowerInvariant();
        }
    }
}