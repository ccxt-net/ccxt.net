using System;
using System.Collections.Generic;
using System.Linq;
using CCXT.NET.Poloniex.Private;

namespace CCXT.NET.Poloniex
{
    public static class Extension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="balances"></param>
        /// <param name="coin_name"></param>
        /// <returns></returns>
        public static decimal available_qty(this Dictionary<string, UserBalance> balances, string coin_name)
        {
            var _result = 0.0m;

            var _coin_balance = balances.Where(b => b.Key == coin_name).SingleOrDefault();
            if (_coin_balance.Key != null)
                _result = _coin_balance.Value.QuoteAvailable;

            return _result;
        }
    }
}