namespace CCXT.NET.Shared.Coin.Data
{
    /// <summary>
    ///
    /// </summary>
    public class XWatcherSite
    {
        /// <summary>
        ///
        /// </summary>
        public string site_name
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string X_status
        {
            get;
            set;
        }

        /// <summary>
        /// currency
        /// </summary>
        public decimal X_price
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string Y_status
        {
            get;
            set;
        }

        /// <summary>
        /// coin
        /// </summary>
        public decimal Y_price
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal has_currency
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class XWatcherMessage
    {
        /// <summary>
        ///
        /// </summary>
        public XWatcherSite A
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public XWatcherSite B
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string quote_name
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string base_name
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal stock_qty
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal target_rate
        {
            get;
            set;
        }
    }
}