using OdinSdk.BaseLib.Coin.Private;

namespace CCXT.NET.Kraken.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class KAddressItem : OdinSdk.BaseLib.Coin.Private.AddressItem, IAddressItem
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