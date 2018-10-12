using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Public;

namespace CCXT.NET.BitMEX.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class BMarketItem : OdinSdk.BaseLib.Coin.Public.MarketItem, IMarketItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string state
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "symbol")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string rootSymbol
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string underlying
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string positionCurrency
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string quoteCurrency
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string settlCurrency
        {
            get;
            set;
        }        

        /// <summary>
        /// 
        /// </summary>
        public decimal maxOrderQty
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal maxPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal lastPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal lotSize
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal tickSize
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal multiplier
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal initMargin
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal maintMargin
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal riskLimit
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal riskStep
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal settlementFee
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal insuranceFee
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "takerFee")]
        public override decimal takerFee
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "makerFee")]
        public override decimal makerFee
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool future
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool prediction
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string type
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool swap
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int maxLeverage
        {
            get;
            set;
        }
    }
}