using System.Collections.Generic;

namespace CCXT.NET.Coinone.Private
{
    /// <summary>
    /// Virtual account's information.
    /// </summary>
    public class VirtualAccountInfo
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
    public class MobileInfo
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
    public class BankInfo
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
    public class EmailInfo
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

    public class TraderType
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

    public class FeeRate
    {
        /// <summary>
        /// btc User's fee.
        /// </summary>
        public TraderType btc
        {
            get;
            set;
        }

        /// <summary>
        /// eth User's fee.
        /// </summary>
        public TraderType eth
        {
            get;
            set;
        }

        /// <summary>
        /// etc User's fee.
        /// </summary>
        public TraderType etc
        {
            get;
            set;
        }
    }

    public class UserInfoData
    {
        /// <summary>
        /// Virtual account's information.
        /// </summary>
        public VirtualAccountInfo virtualAccountInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Mobile authentication's information.
        /// </summary>
        public MobileInfo mobileInfo
        {
            get;
            set;
        }

        /// <summary>
        /// User's bank information.
        /// </summary>
        public BankInfo bankInfo
        {
            get;
            set;
        }

        /// <summary>
        /// User's email information.
        /// </summary>
        public EmailInfo emailInfo
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
        public FeeRate feeRate
        {
            get;
            set;
        }
    }

    public class UserInfo : CApiResult
    {
        /// <summary>
        /// 
        /// </summary>
        public UserInfoData userInfo
        {
            get;
            set;
        }
    }
}