namespace CCXT.NET.Coinone.Private
{
    /// <summary>
    /// Virtual account's information.
    /// </summary>
    public class CAccountInfo
    {
        /// <summary>
        /// Virtual account's depositor.
        /// </summary>
        public string depositor
        {
            get;
            set;
        }

        /// <summary>
        /// Virtual account's number. 
        /// </summary>
        public string accountNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Virtual account's bank name.
        /// </summary>
        public string bankName
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Mobile authentication's information.
    /// </summary>
    public class CMobileInfo
    {
        /// <summary>
        /// Mobile phone's user name.
        /// </summary>
        public string userName
        {
            get;
            set;
        }

        /// <summary>
        /// Phone number.
        /// </summary>
        public string phoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Mobile phone's corporation code.
        /// </summary>
        public string phoneCorp
        {
            get;
            set;
        }

        /// <summary>
        /// true' If a user is authenticated.
        /// </summary>
        public bool isAuthenticated
        {
            get;
            set;
        }
    }

    /// <summary>
    /// User's bank information.
    /// </summary>
    public class CBankInfo
    {
        /// <summary>
        /// Account's depositor.
        /// </summary>
        public string depositor
        {
            get;
            set;
        }

        /// <summary>
        /// Account's bankCode.
        /// </summary>
        public string bankCode
        {
            get;
            set;
        }

        /// <summary>
        /// Account's number.
        /// </summary>
        public string accountNumber
        {
            get;
            set;
        }

        /// <summary>
        /// true' If a user is authenticated.
        /// </summary>
        public bool isAuthenticated
        {
            get;
            set;
        }
    }

    /// <summary>
    /// User's email information.
    /// </summary>
    public class CEmailInfo
    {
        /// <summary>
        /// User's email address.
        /// </summary>
        public string email
        {
            get;
            set;
        }

        /// <summary>
        /// true' If a user is authenticated.
        /// </summary>
        public bool isAuthenticated
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CTraderType
    {
        /// <summary>
        /// Percent of maker fee.
        /// </summary>
        public decimal maker
        {
            get;
            set;
        }

        /// <summary>
        /// Percent of taker fee.
        /// </summary>
        public decimal taker
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CFeeRate
    {
        /// <summary>
        /// btc User's fee.
        /// </summary>
        public CTraderType btc
        {
            get;
            set;
        }

        /// <summary>
        /// eth User's fee.
        /// </summary>
        public CTraderType eth
        {
            get;
            set;
        }

        /// <summary>
        /// etc User's fee.
        /// </summary>
        public CTraderType etc
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CUserInfoItem
    {
        /// <summary>
        /// Virtual account's information.
        /// </summary>
        public CAccountInfo virtualAccountInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Mobile authentication's information.
        /// </summary>
        public CMobileInfo mobileInfo
        {
            get;
            set;
        }

        /// <summary>
        /// User's bank information.
        /// </summary>
        public CBankInfo bankInfo
        {
            get;
            set;
        }

        /// <summary>
        /// User's email information.
        /// </summary>
        public CEmailInfo emailInfo
        {
            get;
            set;
        }

        /// <summary>
        /// User's security level. Summation of authentications.
        /// </summary>
        public int securityLevel
        {
            get;
            set;
        }

        /// <summary>
        /// User's fee.
        /// </summary>
        public CFeeRate feeRate
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CUserInfo : CApiResult
    {
        /// <summary>
        /// 
        /// </summary>
        public CUserInfoItem userInfo
        {
            get;
            set;
        }
    }
}