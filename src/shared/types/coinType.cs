using System;

namespace CCXT.NET.Shared.Coin.Types
{
    /// <summary>
    ///
    /// </summary>
    public enum CoinType
    {
        /// <summary>
        ///
        /// </summary>
        unknown,

        /// <summary>
        ///
        /// </summary>
        ada,

        /// <summary>
        ///
        /// </summary>
        eos,

        /// <summary>
        ///
        /// </summary>
        trx,

        /// <summary>
        ///
        /// </summary>
        btc,

        /// <summary>
        ///
        /// </summary>
        ltc,

        /// <summary>
        ///
        /// </summary>
        eth,

        /// <summary>
        ///
        /// </summary>
        etc,

        /// <summary>
        ///
        /// </summary>
        xrp,

        /// <summary>
        ///
        /// </summary>
        bch,

        /// <summary>
        ///
        /// </summary>
        bgc,

        /// <summary>
        ///
        /// </summary>
        nvc,

        /// <summary>
        ///
        /// </summary>
        xpm,

        /// <summary>
        ///
        /// </summary>
        nmc,

        /// <summary>
        ///
        /// </summary>
        ppc,

        /// <summary>
        ///
        /// </summary>
        dash,

        /// <summary>
        ///
        /// </summary>
        doge
    }

    /// <summary>
    ///
    /// </summary>
    public class CoinTypeConverter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static CoinType FromString(string s)
        {
            var _result = CoinType.unknown;

            if (Enum.IsDefined(typeof(CoinType), s) == true)
                _result = (CoinType)Enum.Parse(typeof(CoinType), s);

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string ToString(CoinType v)
        {
            return Enum.GetName(typeof(CoinType), v).ToLowerInvariant();
        }
    }
}