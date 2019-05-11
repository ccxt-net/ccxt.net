using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System;
using System.Collections.Generic;

namespace CCXT.NET.CEXIO.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class CCancelOrders : OdinSdk.BaseLib.Coin.Trade.MyOrders, IMyOrders
    {
        /// <summary>
        ///
        /// </summary>
        public CCancelOrders()
        {
            this.result = new List<CCancelOrderItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "e")]
        public string command
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "ok")]
        private string messageValue
        {
            set
            {
                message = value;
                success = message == "ok";
            }
        }

        /// <summary>
        ///
        /// </summary>
        public new List<CCancelOrderItem> result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public List<string> data
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class CCancelOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        ///// <summary>
        /////
        ///// </summary>
        //public override string orderId
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        /////
        ///// </summary>
        //public string status
        //{
        //    get;
        //    set;
        //}
    }

    /// <summary>
    ///
    /// </summary>
    public class CCancelAllOrders : OdinSdk.BaseLib.Coin.Trade.MyOrders, IMyOrders
    {
        ///// <summary>
        /////
        ///// </summary>
        //public string result
        //{
        //    get;
        //    set;
        //}
    }

    /// <summary>
    ///
    /// </summary>
    public class CMyOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        /// order id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// timestamp
        /// </summary>
        [JsonProperty(PropertyName = "time")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// Order type
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
        [JsonProperty(PropertyName = "amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// pending amount (if partially executed)
        /// </summary>
        [JsonProperty(PropertyName = "pending")]
        public decimal pending
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override string symbol
        {
            get
            {
                return symbol1 + "/" + symbol2;
            }
            set => base.symbol = value;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "symbol1")]
        private string symbol1
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "symbol2")]
        private string symbol2
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "symbol1Amount")]
        public decimal symbol1Amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "symbol2Amount")]
        public decimal symbol2Amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originLastTxTime")]
        public long lastTxTime
        {
            get;
            set;
        }

        /// <summary>
        /// Time in unix time OR in ISO format
        /// </summary>
        [JsonProperty(PropertyName = "lastTxTime")]
        private DateTime lastTxTimeValue
        {
            set
            {
                lastTxTime = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "tradingFeeUserVolumeAmount")]
        public decimal tradingFeeUserVolumeAmount
        {
            get;
            set;
        }

        // ta:{symbol2} - total amount in current currency (Maker)
        // tta:{symbol2} - total amount in current currency (Taker)
        // fa:{symbol2} - fee amount in current currency (Maker)
        // tfa:{symbol2} - fee amount in current currency (Taker)
        // a:{symbol1}:cds - credit, debit and saldo merged amount in current currency

        /// <summary>
        /// fee % value of Maker transactions
        /// </summary>
        [JsonProperty(PropertyName = "tradingFeeMaker")]
        public string tradingFeeMaker
        {
            get;
            set;
        }

        /// <summary>
        /// fee % value of Taker transactions
        /// </summary>
        [JsonProperty(PropertyName = "tradingFeeTaker")]
        public string tradingFeeTaker
        {
            get;
            set;
        }

        /// <summary>
        /// fee % value of Taker transactions
        /// </summary>
        [JsonProperty(PropertyName = "tradingFeeStrategy")]
        public string tradingFeeStrategy
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "remains")]
        private decimal remains
        {
            set
            {
                filled = quantity - value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "orderId")]
        public string clientOrderId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "pos")]
        public string pos
        {
            get;
            set;
        }

        /// <summary>
        /// Order status ('d' = done, fully executed OR 'c' = canceled, not executed OR 'cd' = cancel-done, partially executed OR 'a' = active, created)
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string statusValue
        {
            set
            {
                orderStatus = OrderStatusConverter.FromString(value);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class CMyOrderItems : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        /// order id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// timestamp
        /// </summary>
        [JsonProperty(PropertyName = "time")]
        public DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }

        /// <summary>
        /// Order type
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
        [JsonProperty(PropertyName = "amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// pending amount (if partially executed)
        /// </summary>
        [JsonProperty(PropertyName = "pending")]
        public decimal pending
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override string symbol
        {
            get
            {
                return symbol1 + "/" + symbol2;
            }
            set => base.symbol = value;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "symbol1")]
        private string symbol1
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "symbol2")]
        private string symbol2
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "symbol1Amount")]
        public decimal symbol1Amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "symbol2Amount")]
        public decimal symbol2Amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "originLastTxTime")]
        public long lastTxTime
        {
            get;
            set;
        }

        /// <summary>
        /// Time in unix time OR in ISO format
        /// </summary>
        [JsonProperty(PropertyName = "lastTxTime")]
        private DateTime lastTxTimeValue
        {
            set
            {
                lastTxTime = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "tradingFeeUserVolumeAmount")]
        public decimal tradingFeeUserVolumeAmount
        {
            get;
            set;
        }

        // ta:{symbol2} - total amount in current currency (Maker)
        // tta:{symbol2} - total amount in current currency (Taker)
        // fa:{symbol2} - fee amount in current currency (Maker)
        // tfa:{symbol2} - fee amount in current currency (Taker)
        // a:{symbol1}:cds - credit, debit and saldo merged amount in current currency

        /// <summary>
        /// fee % value of Maker transactions
        /// </summary>
        [JsonProperty(PropertyName = "tradingFeeMaker")]
        public string tradingFeeMaker
        {
            get;
            set;
        }

        /// <summary>
        /// fee % value of Taker transactions
        /// </summary>
        [JsonProperty(PropertyName = "tradingFeeTaker")]
        public string tradingFeeTaker
        {
            get;
            set;
        }

        /// <summary>
        /// fee % value of Taker transactions
        /// </summary>
        [JsonProperty(PropertyName = "tradingFeeStrategy")]
        public string tradingFeeStrategy
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "remains")]
        private decimal remains
        {
            set
            {
                filled = quantity - value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "orderId")]
        public string clientOrderId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "pos")]
        public string pos
        {
            get;
            set;
        }

        /// <summary>
        /// Order status ('d' = done, fully executed OR 'c' = canceled, not executed OR 'cd' = cancel-done, partially executed OR 'a' = active, created)
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string statusValue
        {
            set
            {
                orderStatus = OrderStatusConverter.FromString(value);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class COpenOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
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
        [JsonProperty(PropertyName = "time")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        ///
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
        [JsonProperty(PropertyName = "amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// pending amount (if partially executed)
        /// </summary>
        [JsonProperty(PropertyName = "pending")]
        public decimal pending
        {
            set
            {
                filled = quantity - value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public override string symbol
        {
            get
            {
                return symbol1 + "/" + symbol2;
            }
            set => base.symbol = value;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "symbol1")]
        private string symbol1
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "symbol2")]
        private string symbol2
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "symbol1Amount")]
        public decimal symbol1Amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "symbol2Amount")]
        public decimal symbol2Amount
        {
            get;
            set;
        }
    }
}