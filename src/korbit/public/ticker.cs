namespace CCXT.NET.Korbit.Public
{
    /// <summary>
    /// 시장 현황 상세정보 ( Detailed Ticker )
    /// </summary>
    public class KDetailedTicker
    {
        /// <summary>
        /// Unix timestamp in milliseconds of the last filled order.
        /// </summary>
        public long timestamp
        {
            get; set;
        }

        /// <summary>
        /// Price of the last filled order.
        /// </summary>
        public decimal last
        {
            get; set;
        }

        /// <summary>
        /// Best bid price.
        /// </summary>
        public decimal bid
        {
            get; set;
        }

        /// <summary>
        /// Best ask price.
        /// </summary>
        public decimal ask
        {
            get; set;
        }

        /// <summary>
        /// Lowest price within the last 24 hours.
        /// </summary>
        public decimal low
        {
            get; set;
        }

        /// <summary>
        /// Highest price within the last 24 hours.
        /// </summary>
        public decimal high
        {
            get; set;
        }

        /// <summary>
        /// Transaction volume within the last 24 hours.
        /// </summary>
        public decimal volume
        {
            get; set;
        }
    }

    /// <summary>
    /// 시장 현황 (Ticker)
    /// </summary>
    public class KTicker
    {
        /// <summary>
        /// Unix timestamp in milliseconds of the last filled order.
        /// </summary>
        public long timestamp
        {
            get; set;
        }

        /// <summary>
        /// Price of the last filled order.
        /// </summary>
        public decimal last
        {
            get; set;
        }
    }
}