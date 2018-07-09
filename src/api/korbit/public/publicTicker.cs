namespace CCXT.NET.Korbit.Public
{
    /// <summary>
    /// 시장 현황 (Ticker)
    /// </summary>
    public class PublicTicker
    {
        /// <summary>
        /// Unix timestamp in milliseconds of the last filled order.
        /// </summary>
        public long timestamp;

        /// <summary>
        /// Price of the last filled order.
        /// </summary>
        public decimal last;
    }
}