using System;

namespace OdinSdk.BaseLib.Coin.Types
{
    /// <summary>
    ///
    /// </summary>
    public enum TransactionType : int
    {
        /// <summary>
        ///
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///
        /// </summary>
        Withdraw = 10,

        /// <summary>
        ///
        /// </summary>
        Withdrawing = 11,

        /// <summary>
        ///
        /// </summary>
        Deposit = 20,

        /// <summary>
        ///
        /// </summary>
        Depositing = 21
    }

    /// <summary>
    ///
    /// </summary>
    public class TransactionTypeConverter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static TransactionType FromString(string s)
        {
            switch (s.ToLower())
            {
                case "send":
                case "s":
                case "withdraw":
                case "withdrawal":
                case "w":
                    return TransactionType.Withdraw;

                case "sending":
                case "withdrawing":
                    return TransactionType.Withdrawing;

                case "receive":
                case "r":
                case "deposit":
                case "d":
                    return TransactionType.Deposit;

                case "receiving":
                case "depositing":
                    return TransactionType.Depositing;

                default:
                    return TransactionType.Unknown;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string ToString(TransactionType v)
        {
            return Enum.GetName(typeof(TransactionType), v).ToLowerInvariant();
        }
    }
}