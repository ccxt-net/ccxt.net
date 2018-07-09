using System;
using System.Collections.Generic;
using System.Linq;

namespace CCXT.NET.Poloniex
{
    /// <summary>
    /// 
    /// </summary>
    public class CurrencyPair
    {
        private const char SeparatorCharacter = '_';

        /// <summary>
        /// 
        /// </summary>
        public string CurrencyName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string CoinName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_name"></param>
        /// <param name="coin_name"></param>
        public CurrencyPair(string currency_name, string coin_name)
        {
            CurrencyName = currency_name;
            CoinName = coin_name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <returns></returns>
        public static CurrencyPair Parse(string currency_pair)
        {
            var _split = currency_pair.Split(SeparatorCharacter);
            return new CurrencyPair(_split[0], _split[1]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return CurrencyName + SeparatorCharacter + CoinName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(CurrencyPair a, CurrencyPair b)
        {
            if (ReferenceEquals(a, b)) return true;
            if ((object)a == null ^ (object)b == null) return false;

            return a.CurrencyName == b.CurrencyName && a.CoinName == b.CoinName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(CurrencyPair a, CurrencyPair b)
        {
            return !(a == b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var b = obj as CurrencyPair;
            return (object)b != null && Equals(b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equals(CurrencyPair b)
        {
            return b.CurrencyName == CurrencyName && b.CoinName == CoinName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}