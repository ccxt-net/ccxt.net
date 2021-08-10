using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Trade;
using CCXT.NET.Shared.Coin.Types;
using CCXT.NET.Shared.Configuration;
using System;

namespace CCXT.NET.BitMEX.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class BMyOrderItem : CCXT.NET.Shared.Coin.Trade.MyOrderItem, IMyOrderItem
    {
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
        [JsonProperty(PropertyName = "orderID")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string account
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string settlCurrency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "clOrdID")]
        public string clientOrderId
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
        [JsonProperty(PropertyName = "orderQty")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "leavesQty")]
        public override decimal remaining
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "timeInForce")]
        public string timeInForce
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
        public DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "side")]
        private string sideValue
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "ordStatus")]
        private string statusValue
        {
            set
            {
                orderStatus = OrderStatusConverter.FromString(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "ordType")]
        private string orderValue
        {
            set
            {
                orderType = OrderTypeConverter.FromString(value);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BCancelAllOrders
    {
        /// <summary>
        ///
        /// </summary>
        public string result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BBulkOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        public string symbol
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string side
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string execInst
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string ordType
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal orderQty
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
        /// Ctor. Accepts Typed parameters for convenience.
        /// </summary>
        public BBulkOrderItem(string symbol, SideType side, OrderType orderType, decimal orderQty, decimal price, string execInst = "")
        {
            this.symbol = symbol;
            this.side = BSideTypeConverter.ToString(side);
            this.execInst = execInst;
            this.ordType = BOrderTypeConverter.ToString(orderType);
            this.orderQty = orderQty;
            this.price = price;
        }
    }

    /// <summary>
    /// Converter between SideType and string
    /// </summary>
    public class BSideTypeConverter
    {
        /// <summary>
        /// string -> SideType
        /// </summary>
        public static SideType FromString(string side)
        {
            switch (side)
            {
                case "Sell":
                    return SideType.Ask;
                case "Buy":
                    return SideType.Bid;
                default:
                    return SideType.Unknown;
            }
        }

        /// <summary>
        /// SideType -> string
        /// </summary>
        public static string ToString(SideType side)
        {
            switch (side)
            {
                case SideType.Ask:
                    return "Sell";
                case SideType.Bid:
                    return "Buy";
                default:
                    throw new NotSupportedException("Unkown side not supported");
            }
        }
    }

    /// <summary>
    /// Converter between OrderType and string
    /// </summary>
    public class BOrderTypeConverter
    {
        /// <summary>
        /// string -> OrderType
        /// </summary>
        public static OrderType FromString(string orderType)
        {
            switch (orderType)
            {
                case "Limit":
                    return OrderType.Limit;
                case "Market":
                    return OrderType.Market;
                default:
                    return OrderType.Unknown;
            }
        }

        /// <summary>
        /// OrderType -> string
        /// </summary>
        public static string ToString(OrderType type)
        {
            switch (type)
            {
                case OrderType.Limit:
                    return "Limit";
                case OrderType.Market:
                    return "Market";
                default:
                    throw new NotSupportedException("Unkown or unsupported order type");
            }
        }
    }

    /// <summary>
    /// Items for TradeApi.UpdateOrders call
    /// </summary>
    public class BBulkUpdateOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        public string orderID
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal orderQty
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
        public BBulkUpdateOrderItem()
        {
        }
    }
}