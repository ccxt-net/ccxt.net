using System.Collections.Generic;
using OdinSdk.BaseLib.Coin;

namespace CCXT.NET.Bithumb.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class BOrderDetail
    {   
        /// <summary>
        /// 체결 시간 Timestamp
        /// </summary>
        public long transaction_date
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
        /// BTC, ETH, DASH, LTC, ETC, XRP
        /// </summary>
        public string order_currency
        {
            get;
            set;
        }

        /// <summary>
        /// 결제 화폐단위(KRW)
        /// </summary>
        public string payment_currency
        {
            get;
            set;
        }

        /// <summary>
        /// 체결 수량
        /// </summary>
        public decimal units_traded
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
        /// 수수료
        /// </summary>
        public decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// 체결가
        /// </summary>
        public decimal total
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 판/구매 거래 주문 등록 또는 진행 중인 거래
    /// </summary>
    public class BOrderDetails : ApiResult<List<BOrderDetail>>
    {
        /// <summary>
        /// 
        /// </summary>
        public BOrderDetails()
        {
            this.result = new List<BOrderDetail>();
        }
    }
}