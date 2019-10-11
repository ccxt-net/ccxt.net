using OdinSdk.BaseLib.Coin.Private;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CCXT.NET.GateIO.Private
{
    /// <summary>
    ///
    /// </summary>
    public class GBalances
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public bool success
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "available")]
        public JObject available
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "locked")]
        public JObject locked
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class GBalanceItem : OdinSdk.BaseLib.Coin.Private.BalanceItem, IBalanceItem
    {
        /// <summary>
        ///
        /// </summary>
        public override string currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override decimal free
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override decimal used
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public override decimal total
        {
            get;
            set;
        }
    }
}