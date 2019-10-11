using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using Newtonsoft.Json;

namespace CCXT.NET.Kraken.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class KMyPositionItem : OdinSdk.BaseLib.Coin.Trade.MyPositionItem, IMyPositionItem
    {
        /// <summary>
        /// Order responsible for execution of trade.
        /// </summary>
        [JsonProperty(PropertyName = "ordertxid")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// Asset pair.
        /// </summary>
        [JsonProperty(PropertyName = "pair")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// Unix timestamp of trade.
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
        /// Type of order used to open position (buy/sell).
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
        /// Order type used to open position.
        /// </summary>
        [JsonProperty(PropertyName = "ordertype")]
        private string orderValue
        {
            set
            {
                orderType = OrderTypeConverter.FromString(value);
            }
        }

        /// <summary>
        /// open/closed
        /// </summary>
        [JsonProperty(PropertyName = "posstatus")]
        private string statusValue
        {
            set
            {
                orderStatus = OrderStatusConverter.FromString(value);
            }
        }

        /// <summary>
        /// Opening cost of position (quote currency unless viqc set in <see cref="oflags"/>).
        /// </summary>
        public override decimal cost
        {
            get;
            set;
        }

        /// <summary>
        /// Opening fee of position (quote currency).
        /// </summary>
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// Position volume (base currency unless viqc set in oflags).
        /// </summary>
        [JsonProperty(PropertyName = "vol")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// Position volume closed (base currency unless viqc set in oflags).
        /// </summary>
        [JsonProperty(PropertyName = "vol_closed")]
        public override decimal filled
        {
            get;
            set;
        }

        /// <summary>
        /// Initial margin (quote currency).
        /// </summary>
        public decimal margin
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string terms
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal rollovertm
        {
            get;
            set;
        }

        /// <summary>
        /// Current value of remaining position (if docalcs requested. quote currency).
        /// </summary>
        public decimal value
        {
            get;
            set;
        }

        /// <summary>
        /// Unrealized profit/loss of remaining position (if docalcs requested.  quote currency, quote currency scale).
        /// </summary>
        public decimal net
        {
            get;
            set;
        }

        /// <summary>
        /// Comma delimited list of miscellaneous info.
        /// </summary>
        public string misc
        {
            get;
            set;
        }

        /// <summary>
        /// Comma delimited list of order flags.
        /// </summary>
        public string oflags
        {
            get;
            set;
        }

        /// <summary>
        /// Volume in quote currency.
        /// </summary>
        public decimal viqc
        {
            get;
            set;
        }
    }
}