using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CCXT.NET.Shared.Coin.Public;
using System.Collections.Generic;

namespace CCXT.NET.Kraken.Public
{
    /// <summary>
    ///
    /// </summary>
    public class KMarketItem : CCXT.NET.Shared.Coin.Public.MarketItem, IMarketItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "base")]
        public override string baseLongName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "quote")]
        public override string quoteLongName
        {
            get;
            set;
        }

        /// <summary>
        /// order amount should be evenly divisible by lot unit size of
        /// </summary>
        [JsonProperty(PropertyName = "originLot")]
        public override decimal lot
        {
            get;
            set;
        }

        /// <summary>
        /// alter name only kraken
        /// </summary>
        [JsonProperty(PropertyName = "altname")]
        public string altname
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string aclass_base
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string aclass_quote
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "lot")]
        public string lotUnit
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool dark_pool
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public int pair_decimals
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public int lot_decimals
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public int lot_multiplier
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public JArray leverage_buy
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public JArray leverage_sell
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public List<JArray> fees
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public List<JArray> fees_maker
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string fee_volume_currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal margin_call
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal margin_stop
        {
            get;
            set;
        }
    }
}