using CCXT.NET.Shared.Coin.Private;

namespace CCXT.NET.Kraken.Private
{
    /// <summary>
    ///
    /// </summary>
    public class KAddressItem : CCXT.NET.Shared.Coin.Private.AddressItem, IAddressItem
    {
        /// <summary>
        ///
        /// </summary>
        public long expiretm
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool @new
        {
            get;
            set;
        }
    }
}