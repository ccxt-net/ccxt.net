using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;

namespace CCXT.NET.Anxpro.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public class APlaceOrder : OdinSdk.BaseLib.Coin.Trade.MyOrder, IMyOrder
    {
        /// <summary>
        /// 
        /// </summary>
        public APlaceOrder()
        {
            this.result = new APlaceOrderItem();
        }

        /// <summary>
        /// success or error
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        private string resultValue
        {
            set
            {
                message = value;
                success = message == "success";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "originResult")]
        public new APlaceOrderItem result
        {
            get;
            set;
        }

        /// <summary>
        /// order id in a UUID format
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        private string data
        {
            set
            {
                result.orderId = value;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ACancelOrder : OdinSdk.BaseLib.Coin.Trade.MyOrder, IMyOrder
    {
        /// <summary>
        /// 
        /// </summary>
        public ACancelOrder()
        {
            this.result = new APlaceOrderItem();
        }

        /// <summary>
        /// success or error
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        private string resultValue
        {
            set
            {
                message = value;
                success = message == "success";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new APlaceOrderItem result
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class APlaceOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "oid")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        /// This field returns empty at this moment
        /// </summary>
        public string qid
        {
            get;
            set;
        }
    }
}