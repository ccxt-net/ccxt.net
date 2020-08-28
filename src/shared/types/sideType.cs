using System;

namespace CCXT.NET.Shared.Coin.Types
{
    /* FIX 5.0 SP2 : Side <54> field
     1 = Buy
     2 = Sell
     3 = Buy minus
     4 = Sell plus
     5 = Sell short
     6 = Sell short exempt
     7 = Undisclosed (valid for IOI and List Order messages only)
     8 = Cross (orders where counterparty is an exchange, valid for all messages except IOIs)
     9 = Cross short
     A = Cross short exempt
     B = "As Defined" (for use with multileg instruments)
     C = "Opposite" (for use with multileg instruments)
     D = Subscribe (e.g. CIV)
     E = Redeem (e.g. CIV)
     F = Lend (FINANCING - identifies direction of collateral)
     G = Borrow (FINANCING - identifies direction of collateral)
    */

    /// <summary>
    ///
    /// </summary>
    public enum SideType : int
    {
        /// <summary>
        ///
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// bid
        /// </summary>
        Bid = 1,

        /// <summary>
        /// ask
        /// </summary>
        Ask = 2
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
                case "short":
                case "offer":
                case "s":
                case "ask":
                case "1":
                    return SideType.Ask;

                case "buy":
                case "long":
                case "purchase":
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