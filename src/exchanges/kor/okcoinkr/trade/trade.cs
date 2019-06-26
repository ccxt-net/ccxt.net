using Newtonsoft.Json;
using CCXT.NET.Coin.Trade;
using CCXT.NET.Coin.Types;

namespace CCXT.NET.OkCoinKr.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class OMyTradeItem : CCXT.NET.Coin.Trade.MyTradeItem, IMyTradeItem
    {
        /// <summary>
        /// 거래ID
        /// </summary>
        [JsonProperty(PropertyName = "tid")]
        public override string tradeId
        {
            get;
            set;
        }

        /// <summary>
        /// 거래가격
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// 거래 수량
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// 거래시간 (밀리초)
        /// </summary>
        [JsonProperty(PropertyName = "date_ms")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// 거래일자
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public long date
        {
            get;
            set;
        }

        /// <summary>
        /// 거래 유형 (buy/sell)
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