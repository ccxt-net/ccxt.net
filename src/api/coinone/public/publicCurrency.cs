using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace CCXT.NET.Coinone.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class PublicCurrency : CApiResult
    {
        /// <summary>
        /// Currency Rate.
        /// </summary>
        public decimal currency;

        /// <summary>
        /// Currency Type. Ex) USD, KRW..
        /// </summary>
        public string currencyType;
    }
}