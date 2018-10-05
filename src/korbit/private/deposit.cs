namespace CCXT.NET.Korbit.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class KDepositAddress
    {
        /// <summary>
        /// The address of your wallet.
        /// </summary>
        public string address
        {
            get;
            set;
        }

        /// <summary>
        /// Destination tag used in XRP transactions. Only shows for XRP account.
        /// </summary>
        public string destination_tag
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the bank. Shows only for KRW.
        /// </summary>
        public string bank_name
        {
            get;
            set;
        }

        /// <summary>
        /// The account number of the bank. Shows only for KRW.
        /// </summary>
        public string account_number
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the owner of the registered bank. Shows only for KRW.
        /// </summary>
        public string account_name
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class KDepositCoins
    {
        /// <summary>
        /// 
        /// </summary>
        public KDepositAddress btc
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public KDepositAddress etc
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public KDepositAddress eth
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public KDepositAddress xrp
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public KDepositAddress krw
        {
            get;
            set;
        }
    }

    /// <summary>
    /// GET https://api.korbit.co.kr/v1/user/accounts
    /// </summary>
    public class KDeposit
    {
        /// <summary>
        /// 
        /// </summary>
        public KDepositCoins deposit
        {
            get;
            set;
        }
    }
}