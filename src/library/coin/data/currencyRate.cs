using System.Collections.Generic;

namespace CCXT.NET.Coin.Data
{
    /// <summary>
    ///
    /// </summary>
    public class CurrencyRateData
    {
        /// <summary>
        ///
        /// </summary>
        public string currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string sign
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal rate
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class CurrencyRates : List<CurrencyRateData>
    {
    }
}