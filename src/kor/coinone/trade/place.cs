using OdinSdk.BaseLib.Coin.Trade;
using Newtonsoft.Json;

namespace CCXT.NET.Coinone.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class CPlaceOrder : OdinSdk.BaseLib.Coin.Trade.MyOrder, IMyOrder
    {
        /// <summary>
        ///
        /// </summary>
        public CPlaceOrder()
        {
            this.result = new CPlaceOrderItem();
        }

        /// <summary>
        /// 성공이면 “success”, 실패할 경우 에러 심블이 세팅된다.
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public override string message
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override bool success
        {
            get
            {
                base.success = message == "success";
                return base.success;
            }
            set => base.success = value;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "errorCode")]
        public override int statusCode
        {
            get;
            set;
        }

        /// <summary>
        /// 접수된 주문 ID
        /// </summary>
        [JsonProperty(PropertyName = "orderId")]
        public string orderId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new CPlaceOrderItem result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class CPlaceOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
    }
}