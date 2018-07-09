using System.Collections.Generic;

namespace CCXT.NET.Korbit.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public class TradeOpenOrder
    {
        /// <summary>
        /// 주문 유입 시각
        /// </summary>
        public long timestamp;

        /// <summary>
        /// 주문 일련번호
        /// </summary>
        public ulong id;

        /// <summary>
        /// 주문 종류. “bid"는 매수주문, "ask"은 매도주문
        /// </summary>
        public string type;

        /// <summary>
        /// 주문가격
        /// </summary>
        public CurrencyValue price;

        /// <summary>
        /// 주문한 BTC 수량.
        /// </summary>
        public CurrencyValue total;

        /// <summary>
        /// 주문한 BTC 수량 중 아직 체결되지 않은 수량
        /// </summary>
        public CurrencyValue open;
    }

    public class TradeOpenOrders : List<TradeOpenOrder>
    {
    }
}