using System.Collections.Generic;
using Newtonsoft.Json;

namespace CCXT.NET.Bithumb.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class OpenOrder
    {
        /// <summary>
        /// 
        /// </summary>
        public string order_id
        {
            get;
            set;
        }

        /// <summary>
        /// 주문 화페단위
        /// </summary>
        public string order_currency
        {
            get;
            set;
        }

        /// <summary>
        /// 주문일시 Timestamp
        /// </summary>
        public long order_date
        {
            get;
            set;
        }

        /// <summary>
        /// 결제 화폐단위
        /// </summary>
        public string payment_currency
        {
            get;
            set;
        }

        /// <summary>
        /// 주문요청 구분 (bid : 구매, ask : 판매)
        /// </summary>
        public string type
        {
            get;
            set;
        }

        /// <summary>
        /// 주문상태(placed : 거래 진행 중)
        /// </summary>
        public string status
        {
            get;
            set;
        }

        /// <summary>
        /// 거래요청 Currency (BTC, ETH, DASH, LTC, ETC, XRP)
        /// </summary>
        public decimal units
        {
            get;
            set;
        }

        /// <summary>
        /// 주문 체결 잔액
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal units_remaining
        {
            get;
            set;
        }

        /// <summary>
        /// 1Currency당 체결가 (BTC, ETH, DASH, LTC, ETC, XRP)
        /// </summary>
        public decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// 신용거래 여부 (Y : 신용거래, N : 일반거래)
        /// </summary>
        public string misu_yn
        {
            get;
            set;
        }

        /// <summary>
        /// 거래 수수료
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// 거래체결 완료 총 거래금액, 완료상태가 아닌 경우 NULL
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal total
        {
            get;
            set;
        }

        /// <summary>
        /// 거래체결 완료일시 Timestamp, 완료상태가 아닌 경우 NULL
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long date_completed
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 판/구매 거래 주문 등록 또는 진행 중인 거래
    /// </summary>
    public class OpenOrders : ApiResult<List<OpenOrder>>
    {
        /// <summary>
        /// 
        /// </summary>
        public OpenOrders()
        {
            this.data = new List<OpenOrder>();
        }
    }
}