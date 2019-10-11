using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using System.Collections.Generic;

namespace CCXT.NET.Huobi.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class HMyTrades : OdinSdk.BaseLib.Coin.Trade.MyTrades, IMyTrades
    {
        /// <summary>
        ///
        /// </summary>
        public HMyTrades()
        {
            this.result = new List<HMyTradeItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new List<HMyTradeItem> result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string status
        {
            set
            {
                success = value == "ok" ? true : false;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class HMyTradeItem : OdinSdk.BaseLib.Coin.Trade.MyTradeItem, IMyTradeItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "symbol")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "account-id")]
        public string accountId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "created-at")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        private string type
        {
            set
            {
                var _type = value.Split('-');
                sideType = SideTypeConverter.FromString(_type[0]);
                orderType = OrderTypeConverter.FromString(_type[1]);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "filled-amount")]
        public decimal filled
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "filled-cash-amount")]
        public decimal filledCashAmount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "filled-fees")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "finished-at")]
        public long finishedAt
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "user-id")]
        public string userId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "source")]
        public string source
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public string state
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "canceled-at")]
        public long canceledAt
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "exchange")]
        public string exchange
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "batch")]
        public string batch
        {
            get;
            set;
        }
    }
}