using CCXT.NET.Configuration;

namespace CCXT.NET.Poloniex.Public
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPublicOrder
    {
        /// <summary>
        /// 
        /// </summary>
        decimal PricePerCoin
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal AmountQuote
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal AmountBase
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PublicOrder : IPublicOrder
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal PricePerCoin
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal AmountQuote
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal AmountBase
        {
            get
            {
                return (AmountQuote * PricePerCoin).Normalize();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal PublicOrder(decimal pricePerCoin, decimal amountQuote)
        {
            PricePerCoin = pricePerCoin;
            AmountQuote = amountQuote;
        }

        /// <summary>
        /// 
        /// </summary>
        internal PublicOrder()
        {

        }
    }
}