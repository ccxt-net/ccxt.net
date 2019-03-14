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
    public class CMyPositions : OdinSdk.BaseLib.Coin.Trade.MyPositions, IMyPositions
    {
        /// <summary>
        /// 
        /// </summary>
        public CMyPositions()
        {
            this.result = new List<CMyPositionItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new List<CMyPositionItem> result
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
    public class CMyPositionItem : OdinSdk.BaseLib.Coin.Trade.MyPositionItem, IMyPositionItem
    {
        /// <summary>
        /// ID
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string positionId
        {
            get;
            set;
        }

        /// <summary>
        /// Currency Pair
        /// </summary>
        [JsonProperty(PropertyName = "pair")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// Position status ( "open", "closed" )
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string status
        {
            set
            {
                orderStatus = OrderStatusConverter.FromString(value);
            }
        }

        /// <summary>
        /// Position's created_at
        /// </summary>
        public override long timestamp
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

        /// <summary>
        /// Position's closed_at
        /// </summary>
        public long closedTimestamp
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "closed_at")]
        private DateTime closedTimeValue
        {
            set
            {
                closedTimestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }

        /// <summary>
        /// Average open rate
        /// </summary>
        [JsonProperty(PropertyName = "open_rate")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// Average close rate
        /// </summary>
        [JsonProperty(PropertyName = "closed_rate")]
        public decimal closed_rate
        {
            get;
            set;
        }

        /// <summary>
        /// Current amount
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// All contracted amount
        /// </summary>
        [JsonProperty(PropertyName = "all_amount")]
        public override decimal filled
        {
            get;
            set;
        }

        /// <summary>
        /// Side ( "buy", "sell" )
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
        /// Profit and Loss
        /// </summary>
        [JsonProperty(PropertyName = "pl")]
        public decimal pl
        {
            get;
            set;
        }

        /// <summary>
        /// About new order
        /// </summary>
        [JsonProperty(PropertyName = "new_order")]
        public CMyOrderItem new_order
        {
            get;
            set;
        }

        /// <summary>
        /// About closed orders
        /// </summary>
        [JsonProperty(PropertyName = "close_orders")]
        public List<CMyOrderItem> close_orders
        {
            get;
            set;
        }
    }
}