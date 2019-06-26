using Newtonsoft.Json;
using CCXT.NET.Coin.Trade;

namespace CCXT.NET.Bittrex.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class BPlaceOrder : CCXT.NET.Coin.Trade.MyOrder, IMyOrder
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
    public class BPlaceOrderItem : CCXT.NET.Coin.Trade.MyOrderItem, IMyOrderItem
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