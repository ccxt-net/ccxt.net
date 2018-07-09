namespace CCXT.NET.Coinone.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class PublicTickerData 
    {
        /// <summary>
        /// Highest price in 24 hours.
        /// </summary>
        public decimal high;

        /// <summary>
        /// Lowest price in 24 hours.
        /// </summary>
        public decimal low;

        /// <summary>
        /// Last completed price.
        /// </summary>
        public decimal last;

        /// <summary>
        /// First price in 24 hours
        /// </summary>
        public decimal first;

        /// <summary>
        /// BTC volume of completed orders in 24 hours.
        /// </summary>
        public decimal volume;

        /// <summary>
        /// Currency
        /// </summary>
        public string currency;
    }

    /// <summary>
    /// 
    /// </summary>
    public class PublicTicker : CApiResult
    {
        /// <summary>
        /// Highest price in 24 hours.
        /// </summary>
        public decimal high;

        /// <summary>
        /// Lowest price in 24 hours.
        /// </summary>
        public decimal low;

        /// <summary>
        /// Last completed price.
        /// </summary>
        public decimal last;

        /// <summary>
        /// First price in 24 hours
        /// </summary>
        public decimal first;

        /// <summary>
        /// BTC volume of completed orders in 24 hours.
        /// </summary>
        public decimal volume;

        /// <summary>
        /// Currency
        /// </summary>
        public string currency;
    }

    /// <summary>
    /// 
    /// </summary>
    public class PublicTickerAll : CApiResult
    {
        /// <summary>
        /// 
        /// </summary>
        public PublicTickerData btc
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public PublicTickerData eth
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public PublicTickerData etc
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public PublicTickerData xrp
        {
            get;
            set;
        }
    }
}