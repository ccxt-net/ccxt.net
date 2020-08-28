using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Trade;
using CCXT.NET.Shared.Coin.Types;
using CCXT.NET.Shared.Configuration;
using System;
using System.Collections.Generic;

namespace CCXT.NET.ItBit.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class TMyTrades : CCXT.NET.Shared.Coin.Trade.MyTrades, IMyTrades
    {
        /// <summary>
        ///
        /// </summary>
        public TMyTrades()
        {
            this.result = new List<TMyTradeItem>();
        }

        /// <summary>
        ///
        /// </summary>
        public int totalNumberOfRecords
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public int currentPageNumber
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string latestExecutionId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public int recordsPerPage
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "tradingHistory")]
        public new List<TMyTradeItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class TMyTradeItem : CCXT.NET.Shared.Coin.Trade.MyTradeItem, IMyTradeItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "orderId")]
        public override string tradeId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "instrument")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "origin_timestamp")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        private DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "currency1")]
        public string currency1
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "currency2")]
        public string currency2
        {
            get;
            set;
        }

        /// <summary>
        /// base currency amount
        /// </summary>
        [JsonProperty(PropertyName = "currency1Amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// amount of currency 2 paid per unit of currency 1
        /// </summary>
        [JsonProperty(PropertyName = "rate")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// quote currency amount
        /// </summary>
        [JsonProperty(PropertyName = "currency2Amount")]
        public override decimal amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "commissionPaid")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "rebatesApplied")]
        public decimal rebatesApplied
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "rebateCurrency")]
        public string rebateCurrency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "commissionCurrency")]
        public string commissionCurrency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "direction")]
        private string sideValue
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }
    }
}