using System.Collections.Generic;

namespace CCXT.NET.Korbit.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public class KOpenOrderItem
    {
        /// <summary>
        /// 주문 유입 시각
        /// </summary>
        public long timestamp
        {
            get; set;
        }

        /// <summary>
        /// 주문 일련번호
        /// </summary>
        public ulong id
        {
            get; set;
        }

        /// <summary>
        /// 주문 종류. “bid"는 매수주문, "ask"은 매도주문
        /// </summary>
        public string type
        {
            get; set;
        }

        /// <summary>
        /// 주문가격
        /// </summary>
        public CurrencyValue price
        {
            get; set;
        }

        /// <summary>
        /// 주문한 BTC 수량.
        /// </summary>
        public CurrencyValue total
        {
            get; set;
        }

        /// <summary>
        /// 주문한 BTC 수량 중 아직 체결되지 않은 수량
        /// </summary>
        public CurrencyValue open
        {
            get; set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class KOpenOrders : List<KOpenOrderItem>
    {
    }
}