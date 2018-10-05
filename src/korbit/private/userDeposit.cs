namespace CCXT.NET.Korbit.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDepositAddress
    {
        /// <summary>
        /// The address of your wallet.
        /// </summary>
        public string address;

        /// <summary>
        /// Destination tag used in XRP transactions. Only shows for XRP account.
        /// </summary>
        public string destination_tag;

        /// <summary>
        /// The name of the bank. Shows only for KRW.
        /// </summary>
        public string bank_name;

        /// <summary>
        /// The account number of the bank. Shows only for KRW.
        /// </summary>
        public string account_number;

        /// <summary>
        /// The name of the owner of the registered bank. Shows only for KRW.
        /// </summary>
        public string account_name;

    }

    /// <summary>
    /// 
    /// </summary>
    public class UserDepositCoins
    {
        /// <summary>
        /// 
        /// </summary>
        public UserDepositAddress btc;

        /// <summary>
        /// 
        /// </summary>
        public UserDepositAddress etc;

        /// <summary>
        /// 
        /// </summary>
        public UserDepositAddress eth;

        /// <summary>
        /// 
        /// </summary>
        public UserDepositAddress xrp;

        /// <summary>
        /// 
        /// </summary>
        public UserDepositAddress krw;
    }

    /// <summary>
    /// GET https://api.korbit.co.kr/v1/user/accounts
    /// </summary>
    public class UserDeposit
    {
        /// <summary>
        /// 
        /// </summary>
        public UserDepositCoins deposit;
    }
}