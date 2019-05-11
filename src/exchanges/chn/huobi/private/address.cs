using OdinSdk.BaseLib.Coin.Private;

namespace CCXT.NET.Huobi.Private
{
    /// <summary>
    ///
    /// </summary>
    public class HAddressItem : OdinSdk.BaseLib.Coin.Private.AddressItem, IAddressItem
    {
        /// <summary>
        ///
        /// </summary>
        public override string currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override string address
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override string tag
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override bool success
        {
            get;
            set;
        }
    }
}