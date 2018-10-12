using OdinSdk.BaseLib.Coin.Private;

namespace CCXT.NET.Korbit.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class KAddressItem : OdinSdk.BaseLib.Coin.Private.AddressItem, IAddressItem
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