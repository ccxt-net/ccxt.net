using System.Collections.Generic;

namespace CCXT.NET.Korbit.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public class KCompleteOrderItem
    {
        /// <summary>
        /// 주문의 ID 식별번호.
        /// </summary>
        public ulong id
        {
            get; set;
        }

        /// <summary>
        /// 주문이 이루어진 화폐 단위.
        /// </summary>
        public string currency_pair
        {
            get; set;
        }

        /// <summary>
        /// 주문의 매매 종류. 매수 주문일 시에는 'bid’, 매도 주문일 시에는 'ask'가 나오게 된다.
        /// </summary>
        public string side
        {
            get; set;
        }

        /// <summary>
        /// 현재까지 체결된 주문에 대한 가격의 가중평균치.
        /// </summary>
        public decimal avg_price
        {
            get; set;
        }

        /// <summary>
        /// 주문 시에 설정한 지정가. 시장가 주문일 경우에는 기본값인 0으로 나오게 된다.
        /// </summary>
        public decimal price
        {
            get; set;
        }

        /// <summary>
        /// 주문 시에 설정한 지정가. 시장가 주문일 경우에는 기본값인 0으로 나오게 된다.
        /// </summary>
        public decimal order_amount
        {
            get; set;
        }

        /// <summary>
        /// 현재까지 체결된 코인의 수량. filledAmount와 orderAmount가 같을 때 주문이 체결 완료된다.
        /// </summary>
        public decimal filled_amount
        {
            get; set;
        }

        /// <summary>
        /// 거래를 주문한 시각. Unix timestamp(milliseconds)로 제공된다.
        /// </summary>
        public long created_at
        {
            get; set;
        }

        /// <summary>
        /// 거래가 부분 체결된 최종 시각. Unix timestamp(milliseconds)로 제공된다. 
        /// 부분적으로도 전혀 체결되지 않은 주문(unfilled)에서는 이 필드는 표시되지 않는다.
        /// </summary>
        public long last_filled_at
        {
            get; set;
        }

        /// <summary>
        /// 현재 주문의 상태. 상태에 따라 'unfilled’, 'partially_filled’, 'filled’ 값으로 나오게 된다.
        /// </summary>
        public string status
        {
            get; set;
        }

        /// <summary>
        /// 거래 수수료. 매수 주문일 시에는 해당 매수 코인으로 수수료가 적용되며, 
        /// 매도 주문일 시에는 원화(KRW)로 수수료가 적용된다. 
        /// 부분적으로도 전혀 체결되지 않은 주문(unfilled)에서는 이 필드는 표시되지 않는다.
        /// </summary>
        public decimal fee
        {
            get; set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class KCompleteOrders : List<KCompleteOrderItem>
    {
    }
}