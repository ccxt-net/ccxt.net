using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Trade;

namespace CCXT.NET.Bitflyer.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class BPlaceOrderItem : CCXT.NET.Shared.Coin.Trade.MyOrderItem, IMyOrderItem
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