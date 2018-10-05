namespace CCXT.NET.Korbit.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class KUserInfoPrefs
    {
        ///// <summary>
        ///// The threshold of BTC withdrawal amount that requires MFA.Value 0 means MFA is always required
        ///// </summary>
        //public string coinOutMfaThreshold
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// The user preference whether to receive an email for successful withdrawal and deposit of KRW
        /// </summary>
        public string notifyDepositWithdrawal
        {
            get;
            set;
        }

        /// <summary>
        /// The user preference whether to receive successful fills of orders
        /// </summary>
        public string notifyTrades
        {
            get;
            set;
        }

        ///// <summary>
        ///// The user preference whether MFA is required to log in
        ///// </summary>
        //public string verifyMfaOnLogin
        //{
        //    get;
        //    set;
        //}
    }

    /// <summary>
    /// Getting User Information
    /// You can get information of a user by using the following request.
    /// </summary>
    public class KUserInfo
    {
        /// <summary>
        /// The email address registered in Korbit
        /// </summary>
        public string email
        {
            get;
            set;
        }

        /// <summary>
        /// The timestamp of user name validation.If this field is not included in the response, it means that the user did not go through the user name validation process.
        /// </summary>
        public long nameCheckedAt
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the user
        /// </summary>
        public string name
        {
            get;
            set;
        }

        /// <summary>
        /// The phone number of the user
        /// </summary>
        public string phone
        {
            get;
            set;
        }

        /// <summary>
        /// The birthday of the user
        /// </summary>
        public string birthday
        {
            get;
            set;
        }

        /// <summary>
        /// The gender of the user.m: male, f: female
        /// </summary>
        public string gender
        {
            get;
            set;
        }

        ///// <summary>
        ///// Maximum limit on the amount of coins that can be sent per day
        ///// </summary>
        //public decimal maxCoinOutPerDay
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Maximum limit on the amount of fiat currency that can be deposited per day
        ///// </summary>
        //public decimal maxFiatInPerDay
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// Maximum limit on the amount of fiat currency than can be withdrawn per day
        ///// </summary>
        //public decimal maxFiatOutPerDay
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 
        /// </summary>
        public KUserInfoPrefs prefs
        {
            get;
            set;
        }

        /// <summary>
        /// User’s membership tier
        /// </summary>
        public int userLevel
        {
            get;
            set;
        }

        ///// <summary>
        ///// The amount of coins withdrawn for today
        ///// </summary>
        //public decimal coinOutToday
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// The amount of coins withdrawn during the last 24 hours
        ///// </summary>
        //public decimal coinOutWithin24h
        //{
        //    get;
        //    set;
        //}
    }
}