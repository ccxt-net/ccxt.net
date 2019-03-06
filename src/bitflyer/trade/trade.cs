using Newtonsoft.Json;
using OdinSdk.BaseLib.Configuration;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using System;

namespace CCXT.NET.Bitflyer.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public class BMyTradeItem : OdinSdk.BaseLib.Coin.Trade.MyTradeItem, IMyTradeItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "child_order_id")]
        public override string tradeId
        {
            get;
            set;
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
        [JsonProperty(PropertyName = "price")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "size")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "exec_date")]
        private DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "buy_child_order_acceptance_id")]
        public string buy_child_order_acceptance_id
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "sell_child_order_acceptance_id")]
        public string sell_child_order_acceptance_id
        {
            get;
            set;
        }
    }
}