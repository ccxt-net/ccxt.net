using CCXT.NET.Coin.Trade;
using CCXT.NET.Coin.Types;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CCXT.NET.OkCoinKr.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class OMyOrders : CCXT.NET.Coin.Trade.MyOrders, IMyOrders
    {
        /// <summary>
        ///
        /// </summary>
        public OMyOrders()
        {
            this.result = new List<OMyOrderItem>();
        }

        /// <summary>
        /// true (성공)
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        private bool resultValue
        {
            set
            {
                success = value;
                message = success == true ? "success" : "failure";
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int total
        {
            get;
            set;
        }

        /// <summary>
        /// 현재 페이지
        /// </summary>
        public int current_page
        {
            get;
            set;
        }

        /// <summary>
        /// 페이지당 데이터 row수
        /// </summary>
        public int page_length
        {
            get;
            set;
        }

        /// <summary>
        /// 주문상세
        /// </summary>
        [JsonProperty(PropertyName = "orders")]
        public new List<OMyOrderItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class OMyOrderItem : CCXT.NET.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        /// 수량
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// 평균거래가격
        /// </summary>
        public decimal avg_price
        {
            get;
            set;
        }

        /// <summary>
        /// 주문시간
        /// </summary>
        [JsonProperty(PropertyName = "create_date")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// 체결된 양
        /// </summary>
        [JsonProperty(PropertyName = "deal_amount")]
        public override decimal filled
        {
            get;
            set;
        }

        /// <summary>
        /// 주문의 고유 아이디
        /// </summary>
        [JsonProperty(PropertyName = "order_id")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "orders_id")]
        public string orderIds
        {
            get;
            set;
        }

        /// <summary>
        /// 주문 당시 화폐 가격
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// -1 = 취소 됨, 0 = 미체결, 1 = 부분 체결, 2 = 전부 체결, 3 = 주문취소 처리중
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private int status
        {
            set
            {
                switch (value)
                {
                    case -1:
                        orderStatus = OrderStatus.Canceled;
                        break;

                    case 0:
                        orderStatus = OrderStatus.Open;
                        break;

                    case 1:
                        orderStatus = OrderStatus.Partially;
                        break;

                    case 2:
                        orderStatus = OrderStatus.Closed;
                        break;

                    case 3:
                        orderStatus = OrderStatus.Canceled;
                        break;
                }
            }
        }

        /// <summary>
        /// 마켓의 유일키
        /// </summary>
        [JsonProperty(PropertyName = "symbol")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// 주문 종류
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        private string sideValue
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }
    }
}