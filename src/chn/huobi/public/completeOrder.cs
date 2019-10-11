using OdinSdk.BaseLib.Coin.Public;
using OdinSdk.BaseLib.Coin.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CCXT.NET.Huobi.Public
{
    /// <summary>
    ///
    /// </summary>
    public class HCompleteOrders : OdinSdk.BaseLib.Coin.Public.CompleteOrders, ICompleteOrders
    {
        /// <summary>
        ///
        /// </summary>
        public HCompleteOrders()
        {
            this.result = new List<HCompleteOrderItem>();
        }

        /// <summary>
        ///
        /// </summary>
        public new List<HCompleteOrderItem> result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string status
        {
            set
            {
                success = value == "ok";
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public List<JObject> data
        {
            set
            {
                foreach (var _v in value)
                {
                    var _completeOrderItems = _v["data"].ToObject<List<HCompleteOrderItem>>();

                    foreach (var _c in _completeOrderItems)
                    {
                        this.result.Add(_c);
                    }
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class HCompleteOrderItem : OdinSdk.BaseLib.Coin.Public.CompleteOrderItem, ICompleteOrderItem
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
        [JsonProperty(PropertyName = "ts")]
        public override long timestamp
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
        [JsonProperty(PropertyName = "amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "direction")]
        private string direction
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }
    }
}