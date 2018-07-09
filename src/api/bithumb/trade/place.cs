using System.Collections.Generic;

namespace CCXT.NET.Bithumb.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public class PlaceItem
    {
        /// <summary>
        /// 체결 번호
        /// </summary>
        public long cont_id
        {
            get;
            set;
        }

        /// <summary>
        /// 체결 수량
        /// </summary>
        public decimal units
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
        /// KRW 체결가
        /// </summary>
        public decimal total
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
    }

    /// <summary>
    /// bithumb 회원 판/구매 거래 주문 등록 및 체결
    /// </summary>
    public class Place : ApiResult<List<PlaceItem>>
    {
        /// <summary>
        /// 주문 ID
        /// </summary>
        public string order_id
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 시장가 구매
    /// </summary>
    public class PlaceMarketItem
    {
        /// <summary>
        /// 체결 번호
        /// </summary>
        public long cont_id
        {
            get;
            set;
        }

        /// <summary>
        /// 총 구매/판매 수량(수수료 포함)
        /// </summary>
        public decimal units
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
        /// 구매/판매 KRW
        /// </summary>
        public decimal total
        {
            get;
            set;
        }

        /// <summary>
        /// 구매/판매 수수료
        /// </summary>
        public decimal fee
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 시장가 구매
    /// </summary>
    public class PlaceMarket : ApiResult<List<PlaceMarketItem>>
    {
        /// <summary>
        /// 주문 번호
        /// </summary>
        public string order_id
        {
            get;
            set;
        }
    }
}