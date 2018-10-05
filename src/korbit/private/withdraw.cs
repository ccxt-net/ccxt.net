using System.Collections.Generic;

namespace CCXT.NET.Korbit.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class KWithdraw
    {
        /// <summary>
        /// The unique ID of the BTC withdrawal request. You can use this ID to cancel a BTC withdrawal request or get the status of it.
        /// </summary>
        public string transferId
        {
            get;
            set;
        }

        /// <summary>
        /// 'success’ if the BTC withdrawal was successful, an error symbol otherwise.
        /// </summary>
        public string status
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Query Status of BTC Deposit and Transfer
    /// </summary>
    public class KCoinStatusItem
    {
        /// <summary>
        /// Unix timestamp in milliseconds by the time the BTC deposit or withdrawal request was created.
        /// </summary>
        public long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// The unique ID of BTC deposit or withdrawal request.
        /// </summary>
        public ulong id
        {
            get;
            set;
        }

        /// <summary>
        /// “coin-in” if it is a BTC deposit request, “coin-out” if it is a BTC withdrawal request.
        /// </summary>
        public string type
        {
            get;
            set;
        }

        /// <summary>
        /// The amount of BTC to deposit or withdraw. 
        /// The value field in it has the amount of deposit or withdrawal. 
        /// The currency field in it is always 'btc’ for now.
        /// </summary>
        public decimal amount
        {
            get;
            set;
        }

        /// <summary>
        /// The user’s BTC address assigned in Korbit system to receive BTC. 
        /// This field comes only if type is 'coin-in’.
        /// </summary>
        public string @in
        {
            get;
            set;
        }

        /// <summary>
        /// The user’s BTC address to where BTC was sent. 
        /// This field comes only if type is 'coin-out’.
        /// </summary>
        public string @out
        {
            get;
            set;
        }

        /// <summary>
        /// Unix timestamp in milliseconds by the time the BTC deposit or withdrawal was completed. 
        /// If it is pending, this field is not included in the response.
        /// </summary>
        public long completedAt
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class KCoinStatus : List<KCoinStatusItem>
    {
    }
}