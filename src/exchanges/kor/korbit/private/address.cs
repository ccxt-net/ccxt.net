using CCXT.NET.Coin.Private;

namespace CCXT.NET.Korbit.Private
{
    /// <summary>
    ///
    /// </summary>
    public class KAddressItem : CCXT.NET.Coin.Private.AddressItem, IAddressItem
    {
        /// <summary>
        ///
        /// </summary>
        private string status
        {
            set
            {
                this.success = value == "success";
            }
        }
    }
}