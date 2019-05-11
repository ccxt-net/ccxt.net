using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Trade;

namespace CCXT.NET.Bitflyer.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class BPlaceOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "child_order_acceptance_id")]
        public override string orderId
        {
            get;
            set;
        }
    }
}