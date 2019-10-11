using OdinSdk.BaseLib.Coin.Public;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CCXT.NET.Bittrex.Public
{
    /// <summary>
    ///
    /// </summary>
    public class BMarkets : OdinSdk.BaseLib.Coin.Public.Markets, IMarkets
    {
        /// <summary>
        ///
        /// </summary>
        public BMarkets()
        {
            this.result = new List<BMarketItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        public new List<BMarketItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BMarketItem : OdinSdk.BaseLib.Coin.Public.MarketItem, IMarketItem
    {
        /// <summary>
        /// uppercase string literal of a pair of currencies (ex) 'btcusd'
        /// </summary>
        [JsonProperty(PropertyName = "MarketName")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "MarketCurrency")]
        public override string baseId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "BaseCurrency")]
        public override string quoteId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "MarketCurrencyLong")]
        public override string baseLongName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "BaseCurrencyLong")]
        public override string quoteLongName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "IsActive")]
        public override bool active
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal MinTradeSize
        {
            get;
            set;
        }
    }
}