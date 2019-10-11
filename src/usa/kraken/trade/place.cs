using OdinSdk.BaseLib.Coin.Trade;
using Newtonsoft.Json;

namespace CCXT.NET.Kraken.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class KMyPlaceOrder
    {
        /// <summary>
        /// Order description info.
        /// </summary>
        [JsonProperty(PropertyName = "descr")]
        public KMyPlaceDescription description
        {
            get;
            set;
        }

        /// <summary>
        /// Array of transaction ids for order (if order was added successfully).
        /// </summary>
        [JsonProperty(PropertyName = "txid")]
        public string[] transactionIds
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class KMyPlaceDescription
    {
        /// <summary>
        /// Order description.
        /// </summary>
        public string order
        {
            get;
            set;
        }

        /// <summary>
        /// Conditional close order description (if conditional close set).
        /// </summary>
        public string close
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class KMyPlaceOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        /// Number of orders cancelled.
        /// </summary>
        public override int count
        {
            get;
            set;
        }

        /// <summary>
        /// If set, order(s) is/are pending cancellation.
        /// </summary>
        public bool? pending
        {
            get;
            set;
        }
    }
}