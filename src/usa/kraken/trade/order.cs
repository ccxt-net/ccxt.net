using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CCXT.NET.Kraken.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class KMyOpenOrders
    {
        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, KMyOrderItem> open
        {
            get;
            set;
        }

        /// <summary>
        ///
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
    public class KMyClosedOrders
    {
        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, KMyOrderItem> closed
        {
            get;
            set;
        }

        /// <summary>
        ///
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
    public class KMyCancelOrder
    {
        /// <summary>
        /// Number of orders cancelled.
        /// </summary>
        public int count
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

    /// <summary>
    ///
    /// </summary>
    public class KMyOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "refid")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// Volume of order (base currency unless viqc set in <see cref="oflags"/>).
        /// </summary>
        [JsonProperty(PropertyName = "vol")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// Volume executed (base currency unless viqc set in <see cref="oflags"/>).
        /// </summary>
        [JsonProperty(PropertyName = "vol_exec")]
        public override decimal filled
        {
            get;
            set;
        }

        /// <summary>
        /// Total cost (quote currency unless viqc set in <see cref="oflags"/>).
        /// </summary>
        [JsonProperty(PropertyName = "cost")]
        public override decimal cost
        {
            get;
            set;
        }

        /// <summary>
        /// Total fee (quote currency).
        /// </summary>
        [JsonProperty(PropertyName = "fee")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override OrderStatus orderStatus
        {
            get
            {
                base.orderStatus = OrderStatusConverter.FromString(status);
                if (base.orderStatus == OrderStatus.Open && filled > 0)
                    base.orderStatus = OrderStatus.Partially;

                return base.orderStatus;
            }
            set => base.orderStatus = value;
        }

        /// <summary>
        ///
        /// </summary>
        public string userref
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string reason
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal clsoetm
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal starttm
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal expiretm
        {
            get;
            set;
        }

        /// <summary>
        /// Stop price (quote currency, for trailing stops)
        /// </summary>
        public decimal stopprice
        {
            get;
            set;
        }

        /// <summary>
        /// Triggered limit price (quote currency, when limit based order type triggered).
        /// </summary>
        public decimal limitprice
        {
            get;
            set;
        }

        /// <summary>
        /// Comma delimited list of miscellaneous info.
        /// <para>stopped = triggered by stop price</para>
        /// <para>touched = triggered by touch price</para>
        /// <para>liquidated = liquidation</para>
        /// <para>partial = partial fill</para>
        /// </summary>
        public string misc
        {
            get;
            set;
        }

        /// <summary>
        /// Comma delimited list of order flags.
        /// <para>viqc = volume in quote currency</para>
        /// <para>fcib = prefer fee in base currency (default if selling)</para>
        /// <para>fciq = prefer fee in quote currency (default if buying)</para>
        /// <para>nompp = no market price protection</para>
        /// </summary>
        public string oflags
        {
            get;
            set;
        }

        /// <summary>
        /// Array of trade ids related to order (if trades info requested and data available).
        /// </summary>
        [JsonProperty(PropertyName = "Trades")]
        public string[] trades
        {
            get;
            set;
        }

        private KMyOrderDescription description = null;

        /// <summary>
        /// Order description info.
        /// </summary>
        [JsonProperty(PropertyName = "descr")]
        public KMyOrderDescription descr
        {
            get => description;
            set
            {
                description = value;

                symbol = description.symbol;
                sideType = SideTypeConverter.FromString(description.sideType);
                orderType = OrderTypeConverter.FromString(description.orderType);
            }
        }

        /// <summary>
        /// Status of order.
        /// <para>pending = order pending book entry</para>
        /// <para>open = open order</para>
        /// <para>closed = closed order</para>
        /// <para>canceled = order cancelled</para>
        /// <para>expired = order expired</para>
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string status
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "opentm")]
        private decimal opentm
        {
            set
            {
                timestamp = (long)value * 1000;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class KMyOrderDescription
    {
        /// <summary>
        /// Asset pair.
        /// </summary>
        [JsonProperty(PropertyName = "pair")]
        public string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// Type of order (buy/sell).
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string sideType
        {
            get;
            set;
        }

        /// <summary>
        /// Order type (See <see cref="TradeApi.CreateLimitOrder(string, string, decimal, decimal, SideType, Dictionary{string, object})"/>).
        /// </summary>
        public string orderType
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal price
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal price2
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string leverage
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string order
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string close
        {
            get;
            set;
        }
    }
}