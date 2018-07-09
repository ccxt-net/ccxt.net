namespace CCXT.NET.Korbit.Public
{
    /// <summary>
    /// 각종 제약조건 ( Constants )
    /// You can get constant values such as fee rates and minimum amount of BTC to transfer, etc.
    /// </summary>
    public class PublicConstants
    {
        /// <summary>
        /// Fixed fee for KRW withdrawals. ( 1,000 KRW )
        /// </summary>
        public decimal krwWithdrawalFee;

        /// <summary>
        /// Maximum daily amount for KRW withdrawals. ( 10,000,000 KRW )
        /// </summary>
        public decimal maxKrwWithdrawal;

        /// <summary>
        /// Minimum amount for a KRW withdrawal. ( 2,000 KRW )
        /// </summary>
        public decimal minKrwWithdrawal;

        /// <summary>
        /// BTC order unit size.( 500 KRW )
        /// </summary>
        public decimal btcTickSize;

        /// <summary>
        /// Fixed fee for BTC withdrawals. ( 0.0005 BTC )
        /// </summary>
        public decimal btcWithdrawalFee;

        /// <summary>
        /// Maximum BTC amount for placing an order. ( 100 BTC )
        /// </summary>
        public decimal maxBtcOrder;

        /// <summary>
        /// Maximum price of 1 BTC for an order. ( 100,000,000 KRW )
        /// </summary>
        public decimal maxBtcPrice;

        /// <summary>
        /// Minimum BTC amount for placing an order. ( 0.01 BTC )
        /// </summary>
        public decimal minBtcOrder;

        /// <summary>
        /// Minimum price of 1 BTC for an order. ( 1,000 KRW )
        /// </summary>
        public decimal minBtcPrice;

        /// <summary>
        /// Maximum amount for BTC withdrawals. ( 5 BTC )
        /// </summary>
        public decimal maxBtcWithdrawal;

        /// <summary>
        /// Minimum amount for BTC withdrawals. ( 0.0001 BTC )
        /// </summary>
        public decimal minBtcWithdrawal;

        /// <summary>
        ///  Ethereum Classic order unit size( 10 KRW )
        /// </summary>
        public decimal etcTickSize;

        /// <summary>
        /// Maximum Etcereum Classic amount for placing an order.( 5,000ETC )
        /// </summary>
        public decimal maxEtcOrder;

        /// <summary>
        /// Maximum price of 1 ETC for an order. ( 100,000,000 KRW )
        /// </summary>
        public decimal maxEtcPrice;

        /// <summary>
        /// Minimum ETC amount for placing an order. ( 0.1 ETC )
        /// </summary>
        public decimal minEtcOrder;

        /// <summary>
        /// Minimum price of 1 ETC for an order. ( 100 KRW )
        /// </summary>
        public decimal minEtcPrice;


        /// <summary>
        ///  Ethereum order unit size( 50 KRW )
        /// </summary>
        public decimal ethTickSize;

        /// <summary>
        /// Maximum Ethereum amount for placing an order.( 20,000ETH )
        /// </summary>
        public decimal maxEthOrder;

        /// <summary>
        /// Maximum price of 1 ETH for an order. ( 100,000,000 KRW )
        /// </summary>
        public decimal maxEthPrice;

        /// <summary>
        /// Minimum ETH amount for placing an order. ( 0.5 ETH )
        /// </summary>
        public decimal minEthOrder;

        /// <summary>
        /// Minimum price of 1 ETH for an order. ( 1,000 KRW )
        /// </summary>
        public decimal minEthPrice;

        /// <summary>
        /// 2nd Tier
        /// </summary>
        public decimal minTradableLevel;
    }
}