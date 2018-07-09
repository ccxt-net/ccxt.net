using System;

namespace CCXT.NET.Types
{
    /// <summary>
    /// 
    /// </summary>
    public enum SideType
    {
        /// <summary>
        /// 
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// bid
        /// </summary>
        Ask,

        /// <summary>
        /// ask
        /// </summary>
        Bid
    }

    /// <summary>
    /// 
    /// </summary>
    public class SideTypeConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static SideType FromString(string s)
        {
            switch (s.ToLower())
            {
                case "sell":
                case "s":
                case "ask":
                case "1":
                    return SideType.Ask;

                case "buy":
                case "b":
                case "bid":
                case "0":
                    return SideType.Bid;

                default:
                    return SideType.Unknown;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string ToString(SideType v)
        {
            return Enum.GetName(typeof(SideType), v).ToLowerInvariant();
        }
    }
}