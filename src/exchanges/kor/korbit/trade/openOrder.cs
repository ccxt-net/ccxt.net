using Newtonsoft.Json;
using CCXT.NET.Coin.Trade;
using CCXT.NET.Coin.Types;

namespace CCXT.NET.Korbit.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class KCurrencyValue
    {
        /// <summary>
        ///
        /// </summary>
        public string currency;

        /// <summary>
        ///
        /// </summary>
        public decimal value;
    }

    /// <summary>
    ///
    /// </summary>
    public class KMyOpenOrderItem : CCXT.NET.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        /// 주문 일련번호
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// 주문 유입 시각
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originPrice")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// 주문가격. price.value로 주문 가격이 들어온다.
        /// 이후 원화 이외의 통화로 거래하도록 허용할 경우에 대비하여 currency 구분을 두도록 하였으나,
        /// 지금은 항상 krw로 세팅된다.
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public KCurrencyValue priceValue
        {
            get;
            set;
        }

        /// <summary>
        /// 주문한 BTC 수량. 이 필드 아래에 currency와 value 필드가 온다.
        /// currency는 항상 ‘btc'로 들어오며, value에는 주문한 BTC 수량이 들어온다.
        /// </summary>
        [JsonProperty(PropertyName = "total")]
        public KCurrencyValue totalValue
        {
            get;
            set;
        }

        /// <summary>
        /// 주문한 BTC 수량 중 아직 체결되지 않은 수량. 이 필드 아래에 currency와 value 필드가 온다.
        /// currency는 항상 ‘btc'로 들어오며, value에는 아직 체결되지 않은 BTC 수량.
        /// </summary>
        [JsonProperty(PropertyName = "open")]
        public KCurrencyValue openValue
        {
            get;
            set;
        }

        /// <summary>
        /// 주문 종류. “bid"는 매수주문, "ask"은 매도주문
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