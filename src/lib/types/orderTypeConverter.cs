using System;

namespace CCXT.NET.Types
{
    /// <summary>
    /// 
    /// </summary>
    public class OrderTypeConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static SideType FromString(string s)
        {
            switch (s)
            {
                case "sell":
                    return SideType.Ask;
 
                case "buy":
                    return SideType.Bid;

                default:
                    throw new ArgumentException();
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