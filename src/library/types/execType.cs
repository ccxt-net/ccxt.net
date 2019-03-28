using System;

namespace OdinSdk.BaseLib.Coin.Types
{
    /* FIX 5.0 SP2 : ExecType <150> field
     0 = New
     3 = Done for day
     4 = Canceled
     5 = Replaced
     6 = Pending Cancel (e.g. result of Order Cancel Request)
     7 = Stopped
     8 = Rejected
     9 = Suspended
     A = Pending New
     B = Calculated
     C = Expired
     D = Restated (Execution Report sent unsolicited by sellside, with ExecRestatementReason (378) set)
     E = Pending Replace (e.g. result of Order Cancel/Replace Request)
     F = Trade (partial fill or fill)
     G = Trade Correct
     H = Trade Cancel
     I = Order Status
     J = Trade in a Clearing Hold
     K = Trade has been released to Clearing
     L = Triggered or Activated by System
     */

    /// <summary>
    /// 
    /// </summary>
    public enum ExecType : int
    {
        /// <summary>
        /// 
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 
        /// </summary>
        Trade = 1,

        /// <summary>
        /// 
        /// </summary>
        Replaced = 2,

        /// <summary>
        /// 
        /// </summary>
        New = 3,

        /// <summary>
        /// 
        /// </summary>
        Canceled = 4,

        /// <summary>
        /// 
        /// </summary>
        Rejected = 8
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExecTypeConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ExecType FromString(string s)
        {
            switch (s.ToLower())
            {
                case "trade":
                    return ExecType.Trade;

                case "replaced":
                    return ExecType.Replaced;

                case "new":
                    return ExecType.New;

                case "canceled":
                    return ExecType.Canceled;

                case "rejected":
                    return ExecType.Rejected;

                default:
                    return ExecType.Unknown;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static ExecType FromChar(char c)
        {
            switch (c)
            {
                case 'F':
                    return ExecType.Trade;

                case '5':
                    return ExecType.Replaced;

                case '0':
                    return ExecType.New;

                case '4':
                    return ExecType.Canceled;

                case '8':
                    return ExecType.Rejected;

                default:
                    return ExecType.Unknown;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string ToString(ExecType v)
        {
            return Enum.GetName(typeof(ExecType), v).ToLowerInvariant();
        }
    }
}