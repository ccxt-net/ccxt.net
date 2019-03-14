using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using System.Collections.Generic;

namespace CCXT.NET.Anxpro.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public class AMyOrders : OdinSdk.BaseLib.Coin.Trade.MyOrders, IMyOrders
    {
        /// <summary>
        /// 
        /// </summary>
        public AMyOrders()
        {
            this.result = new List<AMyOrderItem>();
        }

        /// <summary>
        /// 
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
        public new List<AMyOrderItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AMyOrderItem : OdinSdk.BaseLib.Coin.Trade.MyOrderItem, IMyOrderItem
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
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "item")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        public string currency
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "originAmount")]
        public override decimal amount
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "originPrice")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public AMyOrderValue amountValue
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "effective_amount")]
        public AMyOrderValue effectiveValue
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public AMyOrderValue priceValue
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "priority")]
        public long priority
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public JArray actions
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
    /// Currency Object refer to a json block of the form
    /// </summary>
    public class AMyOrderValue
    {
        /// <summary>
        /// The currency code
        /// </summary>
        public string currency
        {
            get;
            set;
        }

        /// <summary>
        /// The value in display format, might contains grouping separator(,) and ends with a currency code
        /// </summary>
        public string display
        {
            get;
            set;
        }

        /// <summary>
        /// The value in display short format, rounding to 2 decimal places
        /// </summary>
        public string display_short
        {
            get;
            set;
        }

        /// <summary>
        /// The value itself, does not contain any formatting text
        /// </summary>
        public decimal value
        {
            get;
            set;
        }

        /// <summary>
        /// The value itself, multiplied by its corresponding multiplier (10^5, 10^8, etc), does not contain any formatting text
        /// </summary>
        /// <para>Cash/Fiat Amount                             = 10^2 => ex) HKD 1234.56 -> 123456</para>
        /// <para>Crypto/Coins Amount                          = 10^8 => ex) BTC 1234.56789 -> 123456789000</para>
        /// <para>Currency pair rate - BTC to FIAT, LTC to BTC = 10^5 => ex) BTCHKD 4,100.31234 -> 410031234</para>
        /// <para>Currency pair rate - all other crypto        = 10^8 => ex) DOGEBTC 0.012345 -> 1234500</para>        
        public decimal value_int
        {
            get;
            set;
        }
    }
}