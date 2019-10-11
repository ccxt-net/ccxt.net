using OdinSdk.BaseLib.Coin.Trade;
using Newtonsoft.Json;

namespace CCXT.NET.OKEx.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class OPlaceOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        /// order ID of the newly placed order
        /// </summary>
        [JsonProperty(PropertyName = "order_id")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// true means order placed successfully
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public bool result
        {
            get;
            set;
        }
    }
}