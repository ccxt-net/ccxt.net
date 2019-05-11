using System.Diagnostics;

namespace OdinSdk.BaseLib.Coin.Types
{
    /// <summary>
    ///
    /// </summary>
    public class SymbolType
    {
        /// <summary>
        ///
        /// </summary>
        public string id
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string symbol
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
        public string fiat
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static SymbolType ConvertToType(string symbol)
        {
            var _coins = symbol.Split('/');
            Debug.Assert(_coins.Length == 2);

            return new SymbolType
            {
                id = _coins[0],
                symbol = symbol,
                coin = _coins[0],
                fiat = _coins[1]
            };
        }
    }
}