using System.Collections.Generic;

namespace CCXT.NET.BitMEX.Public
{
    /// <summary>
    ///
    /// </summary>
    public class BUdfHistory
    {
        /// <summary>
        ///
        /// </summary>
        public string s
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public List<long> t
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public List<decimal> c
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public List<decimal> o
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public List<decimal> h
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public List<decimal> l
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public List<decimal> v
        {
            get;
            set;
        }
    }
}