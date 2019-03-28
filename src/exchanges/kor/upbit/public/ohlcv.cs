using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Public;
using System;

namespace CCXT.NET.Upbit.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class UOHLCVItem : OdinSdk.BaseLib.Coin.Public.OHLCVItem, IOHLCVItem
    {
        /// <summary>
        /// 마켓명
        /// </summary>
        [JsonProperty(PropertyName = "market")]
        public string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// 시가
        /// </summary>
        [JsonProperty(PropertyName = "opening_price")]
        public override decimal openPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 고가
        /// </summary>
        [JsonProperty(PropertyName = "high_price")]
        public override decimal highPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 저가
        /// </summary>
        [JsonProperty(PropertyName = "low_price")]
        public override decimal lowPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 종가
        /// </summary>
        [JsonProperty(PropertyName = "trade_price")]
        public override decimal closePrice
        {
            get;
            set;
        }

        /// <summary>
        /// 해당 캔들에서 마지막 틱이 저장된 시각
        /// </summary>
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// 누적 거래 금액
        /// </summary>
        [JsonProperty(PropertyName = "candle_acc_trade_price")]
        public override decimal amount
        {
            get;
            set;
        }

        /// <summary>
        /// 누적 거래량
        /// </summary>
        [JsonProperty(PropertyName = "candle_acc_trade_volume")]
        public override decimal volume
        {
            get;
            set;
        }

        /// <summary>
        /// 전일 종가(UTC 0시 기준)
        /// </summary>
        [JsonProperty(PropertyName = "prev_closing_price")]
        public decimal prevPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 전일 종가 대비 변화 금액
        /// </summary>
        [JsonProperty(PropertyName = "change_price")]
        public decimal changePrice
        {
            get;
            set;
        }

        /// <summary>
        /// 전일 종가 대비 변화량
        /// </summary>
        [JsonProperty(PropertyName = "change_rate")]
        public decimal changeRate
        {
            get;
            set;
        }

        /// <summary>
        /// 분 단위(유닛)
        /// </summary>
        public int unit
        {
            get;
            set;
        }

        /// <summary>
        /// 캔들 기간의 가장 첫 날
        /// </summary>
        [JsonProperty(PropertyName = "first_day_of_period")]
        public DateTime firstDay
        {
            get;
            set;
        }
    }
}