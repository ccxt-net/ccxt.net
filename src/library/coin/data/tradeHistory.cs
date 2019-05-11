using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OdinSdk.BaseLib.Coin.Types;

namespace OdinSdk.BaseLib.Coin.Data
{
    /// <summary>
    ///
    /// </summary>
    public class TradeHistory
    {
        /// <summary>
        ///
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public DealerType dealer
        {
            get;
            set;
        }

        /// <summary>
        /// Indicate 'buy' or 'sell' trade.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public SideType type
        {
            get;
            set;
        }

        /// <summary>
        /// Trade id.
        /// </summary>
        public long tid
        {
            get;
            set;
        }

        /// <summary>
        /// Unix time in seconds since 1 January 1970.
        /// </summary>
        public long timestamp
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrencyType currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public CoinType coin
        {
            get;
            set;
        }

        /// <summary>
        /// Price for 1 BTC.
        /// </summary>
        public decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// Amount of BTC traded.
        /// </summary>
        public decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal amount
        {
            get;
            set;
        }
    }
}