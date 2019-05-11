using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Trade;

namespace CCXT.NET.Bittrex.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class BPlaceOrder : OdinSdk.BaseLib.Coin.Trade.MyOrder, IMyOrder
    {
        /// <summary>
        ///
        /// </summary>
        public BPlaceOrder()
        {
            this.result = new BPlaceOrderItem();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public new BPlaceOrderItem result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BPlaceOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "uuid")]
        public override string orderId
        {
            get;
            set;
        }
    }
}