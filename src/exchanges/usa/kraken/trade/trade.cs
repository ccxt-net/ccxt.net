using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using System.Collections.Generic;

namespace CCXT.NET.Kraken.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class KMyTrades
    {
        /// <summary>
        /// array of trade info with txid as the key
        /// </summary>
        public Dictionary<string, KMyTradeItem> trades
        {
            get;
            set;
        }

        /// <summary>
        /// amount of available trades info matching criteria
        /// </summary>
        public int count
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class KMyTradeItem : OdinSdk.BaseLib.Coin.Trade.MyTradeItem, IMyTradeItem
    {
        /// <summary>
        /// prder transaction id
        /// </summary>
        [JsonProperty(PropertyName = "ordertxid")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// position transactio id
        /// </summary>
        [JsonProperty(PropertyName = "postxid")]
        public string positionId
        {
            get;
            set;
        }

        /// <summary>
        /// asset pair
        /// </summary>
        [JsonProperty(PropertyName = "pair")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originOrderType")]
        public override OrderType orderType
        {
            get;
            set;
        }

        /// <summary>
        /// volume (base currency)
        /// </summary>
        [JsonProperty(PropertyName = "vol")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// initial margin (quote currency)
        /// </summary>
        public decimal? margin
        {
            get;
            set;
        }

        /// <summary>
        /// total cost of order (quote currency)
        /// </summary>
        public decimal? cost
        {
            get;
            set;
        }

        /// <summary>
        /// comma delimited list of miscellaneous info
        /// closing = trade closes all or part of a position
        /// </summary>
        public string misc
        {
            get;
            set;
        }

        /// <summary>
        /// unix timestamp of trade
        /// </summary>
        [JsonProperty(PropertyName = "time")]
        private decimal timeValue
        {
            set
            {
                timestamp = (long)value * 1000;
            }
        }

        /// <summary>
        /// type of order (buy/sell)
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        private string sideValue
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }

        /// <summary>
        /// order type
        /// </summary>
        [JsonProperty(PropertyName = "orderType")]
        private string orderValue
        {
            set
            {
                orderType = OrderTypeConverter.FromString(value);
            }
        }

        #region If the trade opened a position, the follow fields are also present in the trade info

        /// <summary>
        /// Position status (open/closed)
        /// </summary>
        [JsonProperty(PropertyName = "posstatus")]
        private string statusValue
        {
            set
            {
                positionStatus = OrderStatusConverter.FromString(value);
            }
        }

        /// <summary>
        /// Position status (open/closed)
        /// </summary>
        public OrderStatus positionStatus
        {
            get;
            set;
        }

        /// <summary>
        /// average price of closed portion of position (quote currency)
        /// </summary>
        [JsonProperty(PropertyName = "cprice")]
        public decimal? closedPrice
        {
            get;
            set;
        }

        /// <summary>
        /// total cost of closed portion of position (quote currency)
        /// </summary>
        [JsonProperty(PropertyName = "ccost")]
        public decimal? closedCost
        {
            get;
            set;
        }

        /// <summary>
        /// total fee of closed portion of position (quote currency)
        /// </summary>
        [JsonProperty(PropertyName = "cfee")]
        public decimal? closedFee
        {
            get;
            set;
        }

        /// <summary>
        /// Total volume of closed portion of position (quote currency)
        /// </summary>
        [JsonProperty(PropertyName = "cvol")]
        public decimal? closedVolume
        {
            get;
            set;
        }

        /// <summary>
        /// total margin freed in closed portion of position (quote currency)
        /// </summary>
        [JsonProperty(PropertyName = "cmargin")]
        public decimal? closedMargin
        {
            get;
            set;
        }

        /// <summary>
        /// net profit/loss of closed portion of position (quote currency, quote currency scale)
        /// </summary>
        public decimal? net
        {
            get;
            set;
        }

        /// <summary>
        /// List of closing trades for position (if available).
        /// </summary>
        public string[] trades
        {
            get;
            set;
        }

        #endregion If the trade opened a position, the follow fields are also present in the trade info
    }
}