using CCXT.NET.Coin;
using CCXT.NET.Coin.Public;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CCXT.NET.Bithumb.Public
{
    /// <summary>
    ///
    /// </summary>
    public class BTicker : CCXT.NET.Coin.Public.Ticker, ITicker
    {
        /// <summary>
        ///
        /// </summary>
        public BTicker()
        {
            this.result = new BTickerItem();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public override int statusCode
        {
            get => base.statusCode;
            set
            {
                base.statusCode = value;

                if (statusCode == 0)
                {
                    message = "success";
                    errorCode = ErrorCode.Success;
                    success = true;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new BTickerItem result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BTickers : CCXT.NET.Coin.Public.Tickers, ITickers
    {
        /// <summary>
        ///
        /// </summary>
        public BTickers()
        {
            this.result = new Dictionary<string, JToken>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public override int statusCode
        {
            get => base.statusCode;
            set
            {
                base.statusCode = value;

                if (statusCode == 0)
                {
                    message = "success";
                    errorCode = ErrorCode.Success;
                    success = true;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new Dictionary<string, JToken> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BTickerItem : CCXT.NET.Coin.Public.TickerItem, ITickerItem
    {
        /// <summary>
        /// 64-bit Unix Timestamp in milliseconds since Epoch 1 Jan 1970
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// highest price for last 24H
        /// </summary>
        [JsonProperty(PropertyName = "max_price")]
        public override decimal highPrice
        {
            get;
            set;
        }

        /// <summary>
        /// lowest price for last 24H
        /// </summary>
        [JsonProperty(PropertyName = "min_price")]
        public override decimal lowPrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best bid (buy) price
        /// </summary>
        [JsonProperty(PropertyName = "buy_price")]
        public override decimal bidPrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best ask (sell) price
        /// </summary>
        [JsonProperty(PropertyName = "sell_price")]
        public override decimal askPrice
        {
            get;
            set;
        }

        /// <summary>
        /// volume weighted average price
        /// </summary>
        [JsonProperty(PropertyName = "average_price")]
        public override decimal vwap
        {
            get;
            set;
        }

        /// <summary>
        /// opening price before 24H
        /// </summary>
        [JsonProperty(PropertyName = "opening_price")]
        public override decimal openPrice
        {
            get;
            set;
        }

        /// <summary>
        /// price of last trade (closing price for current period)
        /// </summary>
        [JsonProperty(PropertyName = "closing_price")]
        public override decimal closePrice
        {
            get;
            set;
        }

        /// <summary>
        /// volume of base currency traded for last 24 hours
        /// </summary>
        [JsonProperty(PropertyName = "volume_1day")]
        public override decimal baseVolume
        {
            get;
            set;
        }

        /// <summary>
        /// volume of quote currency traded for last 24 hours
        /// </summary>
        [JsonProperty(PropertyName = "quoteVolume")]
        public override decimal quoteVolume
        {
            get
            {
                base.quoteVolume = (this.baseVolume != 0) ? this.vwap / this.baseVolume : 0m;
                return base.quoteVolume;
            }
            set => base.quoteVolume = value;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "units_traded")]
        public decimal units_traded
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 7일간 BTC 거래량
        /// </summary>
        [JsonProperty(PropertyName = "volume_7day")]
        public decimal volume_7day
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "24H_fluctate")]
        public decimal day_fluctate
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "24H_fluctate_rate")]
        public decimal day_fluctate_rate
        {
            get;
            set;
        }
    }
}