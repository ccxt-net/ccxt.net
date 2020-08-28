namespace CCXT.NET.Shared.Coin
{
    /// <summary>
    ///
    /// </summary>
    public class MarketFee
    {
        /// <summary>
        ///
        /// </summary>
        public bool tierBased
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool percentage
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal maker
        {
            get;
            set;
        }

        /// <summary>
        ///
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
    public class MarketFees
    {
        /// <summary>
        ///
        /// </summary>
        public MarketFee trading
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public MarketFee funding
        {
            get;
            set;
        }
    }
}