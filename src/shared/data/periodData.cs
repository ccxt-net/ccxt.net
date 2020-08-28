using System.Collections.Generic;

namespace CCXT.NET.Shared.Coin.Data
{
    /// <summary>
    ///
    /// </summary>
    public class PeriodData
    {
        /// <summary>
        ///
        /// </summary>
        public string dealer
        {
            get;
            set;
        }

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
        public string coin
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public long time_value
        {
            get;
            set;
        }

        /// <summary>
        /// Today's opening price.
        /// </summary>
        public decimal open_price
        {
            get;
            set;
        }

        /// <summary>
        /// Yesterday's closed price.
        /// </summary>
        public decimal close_price
        {
            get;
            set;
        }

        /// <summary>
        /// Highest price in last 10Minute.
        /// </summary>
        public decimal high_price
        {
            get;
            set;
        }

        /// <summary>
        /// Lowest price in last 10Minute.
        /// </summary>
        public decimal low_price
        {
            get;
            set;
        }

        /// <summary>
        /// Total BTC sell volume in last 10Minute.
        /// </summary>
        public decimal sell_volume
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal sell_amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal sell_count
        {
            get;
            set;
        }

        /// <summary>
        /// Total BTC buy volume in last 10Minute.
        /// </summary>
        public decimal buy_volume
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal buy_amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal buy_count
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class Periods : List<PeriodData>
    {
    }
}