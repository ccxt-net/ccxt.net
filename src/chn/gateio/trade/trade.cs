using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System;
using System.Collections.Generic;

namespace CCXT.NET.GateIO.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public class GMyTrades : OdinSdk.BaseLib.Coin.Trade.MyTrades, IMyTrades
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public override bool success
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "trades")]
        public new List<GMyTradeItem> result
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public override string message
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GMyTradeItem : OdinSdk.BaseLib.Coin.Trade.MyTradeItem, IMyTradeItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string tradeId
        {
            get;
            set;
        }

        /// <summary>
        /// orderid
        /// </summary>
        [JsonProperty(PropertyName = "orderid")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// 
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
        [JsonProperty(PropertyName = "rate")]
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
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "time_unix")]
        private long timeValue
        {
            set
            {
                timestamp = value * 1000;
            }
        }
    }
}