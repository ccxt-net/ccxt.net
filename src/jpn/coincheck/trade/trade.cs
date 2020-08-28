using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CCXT.NET.Shared.Coin.Trade;
using CCXT.NET.Shared.Coin.Types;
using CCXT.NET.Shared.Configuration;
using System;
using System.Collections.Generic;

namespace CCXT.NET.CoinCheck.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class CMyTrades : CCXT.NET.Shared.Coin.Trade.MyTrades, IMyTrades
    {
        /// <summary>
        ///
        /// </summary>
        public CMyTrades()
        {
            this.result = new List<CMyTradeItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "transactions")]
        public new List<CMyTradeItem> result
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "success")]
        public override bool success
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class CMyTradeItem : CCXT.NET.Shared.Coin.Trade.MyTradeItem, IMyTradeItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string tradeId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "order_id")]
        public override string orderId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        private DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "funds")]
        private JObject funds
        {
            get;
            set;
            //{
            //    //var pair = symbol.Split('_'); // btc_jpy만 제공함.
            //    //price = value[pair[1]].Value<decimal>();
            //    //quantity = value[pair[0]].Value<decimal>();
            //    price = value["btc"].Value<decimal>();
            //    quantity = value["jpy"].Value<decimal>();
            //}
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "pair")]
        private string pair
        {
            set
            {
                symbol = value;
                //price = Math.Abs(funds[value.Split('_')[1]].Value<decimal>());
                quantity = Math.Abs(funds[value.Split('_')[0]].Value<decimal>());
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "rate")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "fee_currency")]
        public string fee_currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "fee")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public MakerType makerType
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "liquidity")]
        private string liquidity
        {
            set
            {
                makerType = MakerTypeConverter.FromString(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "side")]
        private string side
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }
    }
}