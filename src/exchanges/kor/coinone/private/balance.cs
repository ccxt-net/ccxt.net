using CCXT.NET.Coin.Private;
using Newtonsoft.Json;

namespace CCXT.NET.Coinone.Private
{
    /// <summary>
    ///
    /// </summary>
    public class CBalanceItem : CCXT.NET.Coin.Private.BalanceItem, IBalanceItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "avail")]
        public override decimal free
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "balance")]
        public override decimal total
        {
            get;
            set;
        }
    }
}