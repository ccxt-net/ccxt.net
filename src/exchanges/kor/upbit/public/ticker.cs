using CCXT.NET.Coin.Public;
using Newtonsoft.Json;
using System;

namespace CCXT.NET.Upbit.Public
{
    /// <summary>
    ///
    /// </summary>
    public class UTickerItem : CCXT.NET.Coin.Public.TickerItem, ITickerItem
    {
        /// <summary>
        /// 마켓명
        /// </summary>
        [JsonProperty(PropertyName = "market")]
        public override string symbol
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
        ///
        /// </summary>
        [JsonProperty(PropertyName = "trade_volume")]
        public decimal closeVolume
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "prev_closing_price")]
        public override decimal prevPrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "change")]
        public string riseAndFall
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "change_price")]
        public override decimal changePrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "change_rate")]
        public decimal changeRate
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
        [JsonProperty(PropertyName = "acc_trade_price")]
        public decimal totalAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 누적 거래량
        /// </summary>
        [JsonProperty(PropertyName = "acc_trade_volume")]
        public decimal totalQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// 누적 거래 금액 24h
        /// </summary>
        [JsonProperty(PropertyName = "acc_trade_price_24h")]
        public decimal totalAmount24h
        {
            get;
            set;
        }

        /// <summary>
        /// 누적 거래량 24h
        /// </summary>
        [JsonProperty(PropertyName = "acc_trade_volume_24h")]
        public decimal totalQuantity24h
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "highest_52_week_price")]
        public decimal highPrice52Weeks
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "highest_52_week_date")]
        public DateTime highDate52Weeks
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "lowest_52_week_price")]
        public decimal lowPrice52Weeks
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "lowest_52_week_date")]
        public DateTime lowDate52Weeks
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
    }
}