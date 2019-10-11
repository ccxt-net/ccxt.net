using OdinSdk.BaseLib.Coin.Public;
using OdinSdk.BaseLib.Coin.Types;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CCXT.NET.Quoinex.Public
{
    /// <summary>
    ///
    /// </summary>
    public class QCompleteOrders : OdinSdk.BaseLib.Coin.Public.CompleteOrders, ICompleteOrders
    {
        /// <summary>
        ///
        /// </summary>
        public QCompleteOrders()
        {
            this.result = new List<QCompleteOrderItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "current_page")]
        public int current_page
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "total_pages")]
        public int total_pages
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "models")]
        public new List<QCompleteOrderItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    /// recent trade data
    /// </summary>
    public class QCompleteOrderItem : OdinSdk.BaseLib.Coin.Public.CompleteOrderItem, ICompleteOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string transactionId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        private long timeValue
        {
            set
            {
                timestamp = value * 1000;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "taker_side")]
        private string sideValue
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }
    }
}