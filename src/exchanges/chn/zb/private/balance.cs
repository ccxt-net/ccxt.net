using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin.Private;

namespace CCXT.NET.Zb.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class ZBalances : OdinSdk.BaseLib.Coin.Private.Balances, IBalances
    {
        /// <summary>
        /// 
        /// </summary>
        public ZBalances()
        {
            this.result = new List<ZBalanceItem>();
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "originResult")]
        public new List<ZBalanceItem> result
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        private JObject data
        {
            set
            {
                result = value["coins"].ToObject<List<ZBalanceItem>>();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ZBalanceItem : OdinSdk.BaseLib.Coin.Private.BalanceItem, IBalanceItem
    {
        /// <summary>
        /// English name of the coin
        /// </summary>
        [JsonProperty(PropertyName = "enName")]
        public string enName
        {
            get;
            set;
        }

        /// <summary>
        /// Frozen assets
        /// </summary>
        [JsonProperty(PropertyName = "freez")]
        public override decimal used
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "fundstype")]
        public int fundstype
        {
            get;
            set;
        }

        /// <summary>
        /// Reserved decimal
        /// </summary>
        [JsonProperty(PropertyName = "unitDecimal")]
        public int unitDecimal
        {
            get;
            set;
        }

        /// <summary>
        /// Chinese name of the coin
        /// </summary>
        [JsonProperty(PropertyName = "cnName")]
        public string cnName
        {
            get;
            set;
        }

        /// <summary>
        /// Is it rechargeable
        /// </summary>
        [JsonProperty(PropertyName = "isCanRecharge")]
        public bool isCanRecharge
        {
            get;
            set;
        }

        /// <summary>
        /// Currency symbol
        /// </summary>
        [JsonProperty(PropertyName = "unitTag")]
        public override string currency
        {
            get;
            set;
        }

        /// <summary>
        /// Is it withdraw cash
        /// </summary>
        [JsonProperty(PropertyName = "isCanWithdraw")]
        public bool isCanWithdraw
        {
            get;
            set;
        }

        /// <summary>
        /// Available assets
        /// </summary>
        [JsonProperty(PropertyName = "available")]
        public override decimal free
        {
            get;
            set;
        }

        /// <summary>
        /// Is it financial management
        /// </summary>
        [JsonProperty(PropertyName = "canLoan")]
        public bool canLoan
        {
            get;
            set;
        }

        /// <summary>
        /// CoinType
        /// </summary>
        [JsonProperty(PropertyName = "key")]
        public string key
        {
            get;
            set;
        }
    }
}