using System;

namespace CCXT.NET.Coin.Types
{
    /// <summary>
    ///
    /// </summary>
    public enum TransferType : int
    {
        /// <summary>
        ///
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///
        /// </summary>
        Submitted = 10,

        /// <summary>
        ///
        /// </summary>
        Rejected = 11,

        /// <summary>
        ///
        /// </summary>
        Accepted = 20,

        /// <summary>
        ///
        /// </summary>
        Processing = 21,

        /// <summary>
        ///
        /// </summary>
        Done = 31,

        /// <summary>
        ///
        /// </summary>
        Canceled = 41
    }

    /// <summary>
    ///
    /// </summary>
    public class TransferTypeConverter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static TransferType FromString(string s)
        {
            switch (s.Trim().ToLower())
            {
                case "submitting":
                case "submitted":
                case "almost_accepted":
                    return TransferType.Submitted;

                case "rejected":
                    return TransferType.Rejected;

                case "accepted":
                    return TransferType.Accepted;

                case "advanced":
                case "processing":
                case "pending":
                    return TransferType.Processing;

                case "completed":
                case "complete":
                case "done":
                    return TransferType.Done;

                case "canceled":
                case "cancelled":
                    return TransferType.Canceled;

                default:
                    return TransferType.Unknown;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string ToString(TransferType v)
        {
            return Enum.GetName(typeof(TransferType), v).ToLowerInvariant();
        }
    }
}