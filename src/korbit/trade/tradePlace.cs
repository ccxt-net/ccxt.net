namespace CCXT.NET.Korbit.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public class TradePlace
    {
        /// <summary>
        /// 접수된 주문 ID
        /// </summary>
        public string orderId;

        /// <summary>
        /// 성공이면 “success”, 실패할 경우 에러 심블이 세팅된다.
        /// </summary>
        public string status;

        /// <summary>
        /// 해당 주문에 사용된 거래 통화
        /// </summary>
        public string currency_pair;
    }
}