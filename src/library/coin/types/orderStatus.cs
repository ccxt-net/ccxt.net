using System;

namespace CCXT.NET.Coin.Types
{
    /* FIX 5.0 SP2 : OrdStatus <39> field
     0 = New
     1 = Partially filled
     2 = Filled
     3 = Done for day
     4 = Canceled
     6 = Pending Cancel (i.e. result of Order Cancel Request)
     7 = Stopped
     8 = Rejected
     9 = Suspended
     A = Pending New
     B = Calculated
     C = Expired
     D = Accepted for Bidding
     E = Pending Replace (i.e. result of Order Cancel/Replace Request)
     5 = Replaced (No longer used)
    */

    /// <summary>
    ///
    /// </summary>
    public enum OrderStatus : int
    {
        /// <summary>
        /// unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// new, unfilled
        /// </summary>
        Open = 11,

        /// <summary>
        /// partially_filled
        /// </summary>
        Partially = 12,

        /// <summary>
        /// filled
        /// </summary>
        Closed = 21,

        /// <summary>
        /// canceled, rejected, expired
        /// </summary>
        Canceled = 31
    }

    /// <summary>
    ///
    /// </summary>
    public class OrderStatusConverter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        public static bool IsAlive(OrderStatus orderStatus)
        {
            return orderStatus == OrderStatus.Open || orderStatus == OrderStatus.Partially;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static OrderStatus FromString(string s)
        {
            switch (s.ToLower())
            {
                case "in queue":
                case "new":
                case "unfilled":
                case "open":
                case "live":
                case "active":
                case "placed":
                case "wait":
                case "o":
                    return OrderStatus.Open;

                case "partially_filled":
                case "partiallyfilled":
                case "partially filled":
                case "pending":
                case "submitted":
                case "updated":
                case "p":
                    return OrderStatus.Partially;

                case "finished":
                case "filled":
                case "closed":
                case "completed":
                case "done":
                case "c":
                    return OrderStatus.Closed;

                case "stopped":
                case "rejected":
                case "replaced":
                case "expired":
                case "cancel":
                case "canceled":
                case "cancelled":
                    return OrderStatus.Canceled;

                default:
                    return OrderStatus.Unknown;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string ToString(OrderStatus v)
        {
            return Enum.GetName(typeof(OrderStatus), v).ToLowerInvariant();
        }
    }
}