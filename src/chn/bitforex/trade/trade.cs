using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Trade;
using CCXT.NET.Shared.Coin.Types;
using System.Collections.Generic;

namespace CCXT.NET.Bitforex.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class BMyTrades : CCXT.NET.Shared.Coin.Trade.MyTrades, IMyTrades
    {
        /// <summary>
        ///
        /// </summary>
        public BMyTrades()
        {
            this.result = new List<BMyTradeItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new List<BMyTradeItem> result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "success")]
        public override bool success
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BMyTradeItem : CCXT.NET.Shared.Coin.Trade.MyTradeItem, IMyTradeItem
    {
        /// <summary>
        /// Transaction pairs such as coin-usd-btc, coin-usd-eth, etc.
        /// </summary>
        [JsonProperty(PropertyName = "symbol")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// Average transaction price
        /// </summary>
        [JsonProperty(PropertyName = "avgPrice")]
        public decimal avgPrice
        {
            get;
            set;
        }

        /// <summary>
        /// Order creation time
        /// </summary>
        [JsonProperty(PropertyName = "createTime")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// last update time
        /// </summary>
        [JsonProperty(PropertyName = "lastTime")]
        public long lastTime
        {
            get;
            set;
        }

        /// <summary>
        /// The number of transactions
        /// </summary>
        [JsonProperty(PropertyName = "dealAmount")]
        public decimal dealAmount
        {
            get;
            set;
        }

        /// <summary>
        /// Order quantity
        /// </summary>
        [JsonProperty(PropertyName = "orderAmount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// Order ID
        /// </summary>
        [JsonProperty(PropertyName = "orderId")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// Lower unit price
        /// </summary>
        [JsonProperty(PropertyName = "orderPrice")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// Fees
        /// </summary>
        [JsonProperty(PropertyName = "tradeFee")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// Trading methods: 1 buy, 2 sell
        /// </summary>
        [JsonProperty(PropertyName = "tradeType")]
        public override SideType sideType
        {
            get;
            set;
        }
    }
}