using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Trade;

namespace CCXT.NET.Korbit.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class KPlaceOrder : OdinSdk.BaseLib.Coin.Trade.MyOrder, IMyOrder
    {
        /// <summary>
        ///
        /// </summary>
        public KPlaceOrder()
        {
            this.result = new KPlaceOrderItem();
        }

        /// <summary>
        /// 성공이면 “success”, 실패할 경우 에러 심블이 세팅된다.
        /// </summary>
        [JsonProperty(PropertyName = "status")]
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
        /// 접수된 주문 ID
        /// </summary>
        [JsonProperty(PropertyName = "orderId")]
        public string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// 해당 주문에 사용된 거래 통화
        /// </summary>
        [JsonProperty(PropertyName = "currency_pair")]
        public string currency_pair
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public new KPlaceOrderItem result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class KPlaceOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
    }
}