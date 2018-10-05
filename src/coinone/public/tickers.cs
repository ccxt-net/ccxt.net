namespace CCXT.NET.Coinone.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class CTickerItem
    {
        /// <summary>
        /// Highest price in 24 hours.
        /// </summary>
        public decimal high
        {
            get;
            set;
        }

        /// <summary>
        /// Lowest price in 24 hours.
        /// </summary>
        public decimal low
        {
            get;
            set;
        }

        /// <summary>
        /// Last completed price.
        /// </summary>
        public decimal last
        {
            get;
            set;
        }

        /// <summary>
        /// First price in 24 hours
        /// </summary>
        public decimal first
        {
            get;
            set;
        }

        /// <summary>
        /// BTC volume of completed orders in 24 hours.
        /// </summary>
        public decimal volume
        {
            get;
            set;
        }

        /// <summary>
        /// Currency
        /// </summary>
        public string currency
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CTicker : CApiResult
    {
        /// <summary>
        /// Highest price in 24 hours.
        /// </summary>
        public decimal high
        {
            get;
            set;
        }

        /// <summary>
        /// Lowest price in 24 hours.
        /// </summary>
        public decimal low
        {
            get;
            set;
        }

        /// <summary>
        /// Last completed price.
        /// </summary>
        public decimal last
        {
            get;
            set;
        }

        /// <summary>
        /// First price in 24 hours
        /// </summary>
        public decimal first
        {
            get;
            set;
        }

        /// <summary>
        /// BTC volume of completed orders in 24 hours.
        /// </summary>
        public decimal volume
        {
            get;
            set;
        }

        /// <summary>
        /// Currency
        /// </summary>
        public string currency
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CTickerAll : CApiResult
    {
        /// <summary>
        /// 
        /// </summary>
        public CTickerItem btc
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CTickerItem eth
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CTickerItem etc
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CTickerItem xrp
        {
            get;
            set;
        }
    }
}