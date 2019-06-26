using Newtonsoft.Json;
using CCXT.NET.Coin;
using CCXT.NET.Coin.Trade;
using CCXT.NET.Coin.Types;
using System.Collections.Generic;

namespace CCXT.NET.Bithumb.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class BMyOpenOrders : CCXT.NET.Coin.Trade.MyOrders, IMyOrders
    {
        /// <summary>
        ///
        /// </summary>
        public BMyOpenOrders()
        {
            this.result = new List<BMyOpenOrderItem>();
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

                if (base.statusCode == 0)
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
        [JsonProperty(PropertyName = "data")]
        public new List<BMyOpenOrderItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BMyOpenOrderItem : CCXT.NET.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "order_id")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// 주문 화폐단위
        /// </summary>
        [JsonProperty(PropertyName = "order_currency")]
        public string order_currency
        {
            get;
            set;
        }

        /// <summary>
        /// 주문일시 Timestamp
        /// </summary>
        [JsonProperty(PropertyName = "order_date")]
        private long timeValue
        {
            set
            {
                timestamp = value / 1000;
            }
        }

        /// <summary>
        /// 결제 화폐단위
        /// </summary>
        [JsonProperty(PropertyName = "payment_currency")]
        public string payment_currency
        {
            get;
            set;
        }

        /// <summary>
        /// 주문요청 구분 (bid : 구매, ask : 판매)
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        private string sideValue
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }

        /// <summary>
        /// 주문상태(placed : 거래 진행 중)
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string statusValue
        {
            set
            {
                orderStatus = OrderStatusConverter.FromString(value);
            }
        }

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
        /// 주문 체결 잔액
        /// </summary>
        [JsonProperty(PropertyName = "units_remaining")]
        public override decimal remaining
        {
            get;
            set;
        }

        /// <summary>
        /// 1Currency당 거래금액
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// 거래 수수료
        /// </summary>
        [JsonProperty(PropertyName = "fee")]
        private decimal feeValue
        {
            set
            {
                fee = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originFee")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public long? date_completed
        {
            get;
            set;
        }

        /// <summary>
        /// 거래체결 완료 총 거래금액, 완료상태가 아닌 경우 NULL
        /// </summary>
        [JsonProperty(PropertyName = "total")]
        public decimal total
        {
            get;
            set;
        }
    }
}