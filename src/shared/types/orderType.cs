using System;

namespace CCXT.NET.Shared.Coin.Types
{
    /* FIX 5.0 SP2 : OrdType <40> field
     1 = Market
     2 = Limit
     3 = Stop / Stop Loss
     4 = Stop Limit
     5 = Market On Close (No longer used)
     6 = With Or Without
     7 = Limit Or Better
     8 = Limit With Or Without
     9 = On Basis
     A = On Close (No longer used)
     B = Limit On Close (No longer used)
     C = Forex Market (No longer used)
     D = Previously Quoted
     E = Previously Indicated
     F = Forex Limit (No longer used)
     G = Forex Swap
     H = Forex Previously Quoted (No longer used)
     I = Funari (Limit day order with unexecuted portion handles as Market On Close. E.g. Japan)
     J = Market If Touched (MIT)
     K = Market With Left Over as Limit (market order with unexecuted quantity becoming limit order at last price)
     L = Previous Fund Valuation Point (Historic pricing; for CIV)
     M = Next Fund Valuation Point (Forward pricing; for CIV)
     P = Pegged
     Q = Counter-order selection
    */

    /// <summary>
    ///
    /// </summary>
    public enum OrderType : int
    {
        /// <summary>
        ///
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// limit
        /// </summary>
        Limit = 1,

        /// <summary>
        /// market
        /// </summary>
        Market = 2,

        /// <summary>
        /// position (short/long)
        /// </summary>
        Position = 3
    }

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
        public static OrderType FromString(string s)
        {
            switch (s.ToLower())
            {
                case "market":
                case "exchange market":
                case "m":
                    return OrderType.Market;

                case "limit":
                case "exchange limit":
                case "l":
                    return OrderType.Limit;

                case "position":
                case "settle-position":
                case "s":
                    return OrderType.Position;

                default:
                    return OrderType.Unknown;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string ToString(OrderType v)
        {
            return Enum.GetName(typeof(OrderType), v).ToLowerInvariant();
        }
    }
}