using Newtonsoft.Json;
using CCXT.NET.Coin.Trade;
using CCXT.NET.Coin.Types;

namespace CCXT.NET.Korbit.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class KMyTradeItem : CCXT.NET.Coin.Trade.MyTradeItem, IMyTradeItem
    {
        /// <summary>
        /// 주문의 ID 식별번호.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string tradeId
        {
            get;
            set;
        }

        /// <summary>
        /// 거래를 주문한 시각. Unix timestamp(milliseconds)로 제공된다.
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// 주문 시에 지정한 코인의 수량.
        /// </summary>
        [JsonProperty(PropertyName = "order_amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// 주문 시에 설정한 지정가. 시장가 주문일 경우에는 기본값인 0으로 나오게 된다.
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// 원화(KRW) 기준 주문 금액. 시장가 매도 주문의 경우 이 필드는 표시되지 않는다.
        /// </summary>
        [JsonProperty(PropertyName = "order_total")]
        public override decimal amount
        {
            get;
            set;
        }

        /// <summary>
        /// 거래 수수료. 매수 주문일 시에는 해당 매수 코인으로 수수료가 적용되며, 매도 주문일 시에는 원화(KRW)로 수수료가 적용된다.
        /// 부분적으로도 전혀 체결되지 않은 주문(unfilled)에서는 이 필드는 표시되지 않는다.
        /// </summary>
        [JsonProperty(PropertyName = "fee")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// 주문이 이루어진 화폐 단위.
        /// </summary>
        public string currency_pair
        {
            get;
            set;
        }

        /// <summary>
        /// 현재까지 체결된 주문에 대한 가격의 가중평균치.
        /// </summary>
        public decimal avg_price
        {
            get;
            set;
        }

        /// <summary>
        /// 거래가 부분 체결된 최종 시각. Unix timestamp(milliseconds)로 제공된다.
        /// 부분적으로도 전혀 체결되지 않은 주문(unfilled)에서는 이 필드는 표시되지 않는다.
        /// </summary>
        public long last_filled_at
        {
            get;
            set;
        }

        /// <summary>
        /// 주문의 매매 종류. 매수 주문일 시에는 'bid’, 매도 주문일 시에는 'ask'가 나오게 된다.
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