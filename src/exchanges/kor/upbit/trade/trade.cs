using CCXT.NET.Coin.Trade;
using CCXT.NET.Coin.Types;
using CCXT.NET.Configuration;
using Newtonsoft.Json;
using System;

namespace CCXT.NET.Upbit.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class UMyTradeItem : CCXT.NET.Coin.Trade.MyTradeItem, IMyTradeItem
    {
        /// <summary>
        /// 체결의 고유 아이디
        /// </summary>
        [JsonProperty(PropertyName = "uuid")]
        public override string tradeId
        {
            get;
            set;
        }

        /// <summary>
        /// 체결 가격
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// 체결 가격의 평균가
        /// </summary>
        public decimal avg_price
        {
            get;
            set;
        }

        /// <summary>
        /// 마켓의 유일키
        /// </summary>
        [JsonProperty(PropertyName = "market")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// 체결 양
        /// </summary>
        [JsonProperty(PropertyName = "volume")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// 체결 후 남은 주문 양
        /// </summary>
        public decimal remaining_volume
        {
            get;
            set;
        }

        /// <summary>
        /// 수수료로 예약된 비용
        /// </summary>
        [JsonProperty(PropertyName = "reserved_fee")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// 남은 수수료
        /// </summary>
        public decimal remaining_fee
        {
            get;
            set;
        }

        /// <summary>
        /// 사용된 수수료
        /// </summary>
        public decimal paid_fee
        {
            get;
            set;
        }

        /// <summary>
        /// 거래에 사용중인 비용
        /// </summary>
        public decimal locked
        {
            get;
            set;
        }

        /// <summary>
        /// 체결된 양
        /// </summary>
        [JsonProperty(PropertyName = "executed_volume")]
        public decimal filled
        {
            get;
            set;
        }

        /// <summary>
        /// 해당 주문에 걸린 체결 수
        /// </summary>
        [JsonProperty(PropertyName = "trade_count")]
        public int count
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public OrderStatus orderStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 주문 상태
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        private string statusValue
        {
            set
            {
                orderStatus = OrderStatusConverter.FromString(value);
            }
        }

        /// <summary>
        /// 주문 생성 시간
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        private DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }

        /// <summary>
        /// 주문 방식
        /// </summary>
        [JsonProperty(PropertyName = "ord_type")]
        private string orderValue
        {
            set
            {
                orderType = OrderTypeConverter.FromString(value);
            }
        }

        /// <summary>
        /// 체결 종류
        /// </summary>
        [JsonProperty(PropertyName = "side")]
        private string sideValue
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }
    }
}