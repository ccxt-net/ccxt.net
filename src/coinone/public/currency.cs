namespace CCXT.NET.Coinone.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class CCurrency : CApiResult
    {
        /// <summary>
        /// Currency Rate.
        /// </summary>
        public decimal currency
        {
            get;
            set;
        }

        /// <summary>
        /// Currency Type. Ex) USD, KRW..
        /// </summary>
        public string currencyType
        {
            get;
            set;
        }
    }
}