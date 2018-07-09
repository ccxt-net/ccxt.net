namespace CCXT.NET.Korbit.Public
{
    /// <summary>
    /// 시장 현황 상세정보 ( Detailed Ticker )
    /// </summary>
    public class PublicDetailedTicker
    {
        /// <summary>
        /// Unix timestamp in milliseconds of the last filled order.
        /// </summary>
        public long timestamp;

        /// <summary>
        /// Price of the last filled order.
        /// </summary>
        public decimal last;

        /// <summary>
        /// Best bid price.
        /// </summary>
        public decimal bid;

        /// <summary>
        /// Best ask price.
        /// </summary>
        public decimal ask;

        /// <summary>
        /// Lowest price within the last 24 hours.
        /// </summary>
        public decimal low;

        /// <summary>
        /// Highest price within the last 24 hours.
        /// </summary>
        public decimal high;

        /// <summary>
        /// Transaction volume within the last 24 hours.
        /// </summary>
        public decimal volume;
    }
}