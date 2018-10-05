namespace CCXT.NET.Korbit.Public
{
    /// <summary>
    /// 각종 제약조건 ( Constants )
    /// You can get constant values such as fee rates and minimum amount of BTC to transfer, etc.
    /// </summary>
    public class KConstants
    {
        /// <summary>
        /// Fixed fee for KRW withdrawals. ( 1,000 KRW )
        /// </summary>
        public decimal krwWithdrawalFee
        {
            get; set;
        }

        /// <summary>
        /// Maximum daily amount for KRW withdrawals. ( 10,000,000 KRW )
        /// </summary>
        public decimal maxKrwWithdrawal
        {
            get; set;
        }

        /// <summary>
        /// Minimum amount for a KRW withdrawal. ( 2,000 KRW )
        /// </summary>
        public decimal minKrwWithdrawal
        {
            get; set;
        }

        /// <summary>
        /// BTC order unit size.( 500 KRW )
        /// </summary>
        public decimal btcTickSize
        {
            get; set;
        }

        /// <summary>
        /// Fixed fee for BTC withdrawals. ( 0.0005 BTC )
        /// </summary>
        public decimal btcWithdrawalFee
        {
            get; set;
        }

        /// <summary>
        /// Maximum BTC amount for placing an order. ( 100 BTC )
        /// </summary>
        public decimal maxBtcOrder
        {
            get; set;
        }

        /// <summary>
        /// Maximum price of 1 BTC for an order. ( 100,000,000 KRW )
        /// </summary>
        public decimal maxBtcPrice
        {
            get; set;
        }

        /// <summary>
        /// Minimum BTC amount for placing an order. ( 0.01 BTC )
        /// </summary>
        public decimal minBtcOrder
        {
            get; set;
        }

        /// <summary>
        /// Minimum price of 1 BTC for an order. ( 1,000 KRW )
        /// </summary>
        public decimal minBtcPrice
        {
            get; set;
        }

        /// <summary>
        /// Maximum amount for BTC withdrawals. ( 5 BTC )
        /// </summary>
        public decimal maxBtcWithdrawal
        {
            get; set;
        }

        /// <summary>
        /// Minimum amount for BTC withdrawals. ( 0.0001 BTC )
        /// </summary>
        public decimal minBtcWithdrawal
        {
            get; set;
        }

        /// <summary>
        ///  Ethereum Classic order unit size( 10 KRW )
        /// </summary>
        public decimal etcTickSize
        {
            get; set;
        }

        /// <summary>
        /// Maximum Etcereum Classic amount for placing an order.( 5,000ETC )
        /// </summary>
        public decimal maxEtcOrder
        {
            get; set;
        }

        /// <summary>
        /// Maximum price of 1 ETC for an order. ( 100,000,000 KRW )
        /// </summary>
        public decimal maxEtcPrice
        {
            get; set;
        }

        /// <summary>
        /// Minimum ETC amount for placing an order. ( 0.1 ETC )
        /// </summary>
        public decimal minEtcOrder
        {
            get; set;
        }

        /// <summary>
        /// Minimum price of 1 ETC for an order. ( 100 KRW )
        /// </summary>
        public decimal minEtcPrice
        {
            get; set;
        }


        /// <summary>
        ///  Ethereum order unit size( 50 KRW )
        /// </summary>
        public decimal ethTickSize
        {
            get; set;
        }

        /// <summary>
        /// Maximum Ethereum amount for placing an order.( 20,000ETH )
        /// </summary>
        public decimal maxEthOrder
        {
            get; set;
        }

        /// <summary>
        /// Maximum price of 1 ETH for an order. ( 100,000,000 KRW )
        /// </summary>
        public decimal maxEthPrice
        {
            get; set;
        }

        /// <summary>
        /// Minimum ETH amount for placing an order. ( 0.5 ETH )
        /// </summary>
        public decimal minEthOrder
        {
            get; set;
        }

        /// <summary>
        /// Minimum price of 1 ETH for an order. ( 1,000 KRW )
        /// </summary>
        public decimal minEthPrice
        {
            get; set;
        }

        /// <summary>
        /// 2nd Tier
        /// </summary>
        public decimal minTradableLevel
        {
            get; set;
        }
    }
}