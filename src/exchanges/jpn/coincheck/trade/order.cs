using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System;
using System.Collections.Generic;

namespace CCXT.NET.CoinCheck.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class CMyOrders : OdinSdk.BaseLib.Coin.Trade.MyOrders, IMyOrders
    {
        /// <summary>
        ///
        /// </summary>
        public CMyOrders()
        {
            this.result = new List<CMyOrderItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "orders")]
        public new List<CMyOrderItem> result
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
    public class CMyOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        /// Order ID（It's the same ID in New order.）
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// order type（"sell" or "buy"）
        /// </summary>
        [JsonProperty(PropertyName = "order_type")]
        private string sideValue
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }

        /// <summary>
        /// Order rate ( Market order if null)
        /// </summary>
        [JsonProperty(PropertyName = "rate")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// Deal pair
        /// </summary>
        [JsonProperty(PropertyName = "pair")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// Unsettle order amount
        /// </summary>
        [JsonProperty(PropertyName = "pending_amount")]
        private decimal pending_amount
        {
            set
            {
                if (value != 0)
                {
                    quantity = value;
                    orderType = OrderType.Limit;
                }
            }
        }

        /// <summary>
        /// Unsettled order amount (only for spot market buy order)
        /// </summary>
        [JsonProperty(PropertyName = "pending_market_buy_amount")]
        private decimal pending_market_buy_amount
        {
            set
            {
                if (value != 0)
                {
                    quantity = value;
                    orderType = OrderType.Market;
                }
            }
        }

        /// <summary>
        /// Stop Loss Order's Rate
        /// </summary>
        [JsonProperty(PropertyName = "stop_loss_rate")]
        public decimal stop_loss_rate
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        private DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }
    }
}