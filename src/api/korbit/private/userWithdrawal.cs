using System.Collections.Generic;
using System.Linq;

namespace CCXT.NET.Korbit.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class UserWithdrawal
    {
        /// <summary>
        /// The unique ID of the BTC withdrawal request. You can use this ID to cancel a BTC withdrawal request or get the status of it.
        /// </summary>
        public string transferId;

        /// <summary>
        /// 'success’ if the BTC withdrawal was successful, an error symbol otherwise.
        /// </summary>
        public string status;
    }

    /// <summary>
    /// Query Status of BTC Deposit and Transfer
    /// </summary>
    public class UserCoinsStatusData
    {
        /// <summary>
        /// Unix timestamp in milliseconds by the time the BTC deposit or withdrawal request was created.
        /// </summary>
        public long timestamp;

        /// <summary>
        /// The unique ID of BTC deposit or withdrawal request.
        /// </summary>
        public ulong id;

        /// <summary>
        /// “coin-in” if it is a BTC deposit request, “coin-out” if it is a BTC withdrawal request.
        /// </summary>
        public string type;

        /// <summary>
        /// The amount of BTC to deposit or withdraw. 
        /// The value field in it has the amount of deposit or withdrawal. 
        /// The currency field in it is always 'btc’ for now.
        /// </summary>
        public decimal amount;

        /// <summary>
        /// The user’s BTC address assigned in Korbit system to receive BTC. 
        /// This field comes only if type is 'coin-in’.
        /// </summary>
        public string @in;

        /// <summary>
        /// The user’s BTC address to where BTC was sent. 
        /// This field comes only if type is 'coin-out’.
        /// </summary>
        public string @out;

        /// <summary>
        /// Unix timestamp in milliseconds by the time the BTC deposit or withdrawal was completed. 
        /// If it is pending, this field is not included in the response.
        /// </summary>
        public long completedAt;
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserCoinsStatus : List<UserCoinsStatusData>
    {

    }
}