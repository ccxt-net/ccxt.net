using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System;

namespace CCXT.NET.Poloniex.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPTradeOrderItem
    {
        /// <summary>
        /// 
        /// </summary>
        string gloablTradeId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        string tradeId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        ulong orderId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        SideType sideType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal pricePerCoin
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal amountQuote
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal amountBase
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        string category
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        DateTime date
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PTradeOrderItem : IPTradeOrderItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string gloablTradeId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string tradeId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("orderNumber")]
        public ulong orderId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("type")]
        private string typeInternal
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("originType")]
        public SideType sideType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("rate")]
        public decimal pricePerCoin
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("amount")]
        public decimal amountQuote
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("total")]
        public decimal amountBase
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string category
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime date
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ITrade : IPTradeOrderItem
    {
        /// <summary>
        /// 
        /// </summary>
        DateTime Time
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Trade : PTradeOrderItem, ITrade
    {
        [JsonProperty("date")]
        private string TimeInternal
        {
            set
            {
                Time = value.ParseDateTime();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Time
        {
            get; private set;
        }
    }
}