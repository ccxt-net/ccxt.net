using CCXT.NET.Coin.Public;
using Newtonsoft.Json;

namespace CCXT.NET.BitMEX.Public
{
    /// <summary>
    ///
    /// </summary>
    public class BOrderBookItem : CCXT.NET.Coin.Public.OrderBookItem, IOrderBookItem
    {
        /// <summary>
        ///
        /// </summary>
        public string symbol
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public long id
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string side
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
        public override decimal price
        {
            get;
            set;
        }
    }
}