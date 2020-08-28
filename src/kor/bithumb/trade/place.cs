using Newtonsoft.Json;
using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Coin.Trade;
using System.Collections.Generic;

namespace CCXT.NET.Bithumb.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class BPlaceOrder : CCXT.NET.Shared.Coin.Trade.MyOrder, IMyOrder
    {
        /// <summary>
        /// 결과 상태 코드 (정상 : 0000, 정상이외 코드는 에러 코드 참조)
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public override int statusCode
        {
            get => base.statusCode;
            set
            {
                base.statusCode = value;

                if (statusCode == 0)
                {
                    message = "success";
                    errorCode = ErrorCode.Success;
                    success = true;
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BPlaceOrders : CCXT.NET.Shared.Coin.Trade.MyOrders, IMyOrders
    {
        /// <summary>
        ///
        /// </summary>
        public BPlaceOrders()
        {
            this.result = new List<BPlaceOrderItem>();
        }

        /// <summary>
        /// 주문번호
        /// </summary>
        [JsonProperty(PropertyName = "order_id")]
        public string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// 결과 상태 코드 (정상 : 0000, 정상이외 코드는 에러 코드 참조)
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public override int statusCode
        {
            get => base.statusCode;
            set
            {
                base.statusCode = value;

                if (statusCode == 0)
                {
                    message = "success";
                    errorCode = ErrorCode.Success;
                    success = true;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public new List<BPlaceOrderItem> result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public List<BPlaceOrderItem> data
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BPlaceOrderItem : CCXT.NET.Shared.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        /// 체결 수량
        /// </summary>
        [JsonProperty(PropertyName = "units")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// 1Currency당 체결가
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// KRW 체결가
        /// </summary>
        [JsonProperty(PropertyName = "total")]
        public override decimal amount
        {
            get;
            set;
        }

        /// <summary>
        /// 수수료
        /// </summary>
        [JsonProperty(PropertyName = "fee")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// 체결 번호
        /// </summary>
        [JsonProperty(PropertyName = "cont_id")]
        public string contractId
        {
            get;
            set;
        }
    }
}