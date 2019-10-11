using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin.Public;
using OdinSdk.BaseLib.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace CCXT.NET.Gemini.Public
{
    /// <summary>
    ///
    /// </summary>
    public class GTickerItem : OdinSdk.BaseLib.Coin.Public.TickerItem, ITickerItem
    {
        /// <summary>
        /// string symbol of the market ('BTCUSD', 'ETHBTC', ...)
        /// </summary>
        [JsonProperty(PropertyName = "symbol")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// current best bid (buy) price
        /// </summary>
        [JsonProperty(PropertyName = "bid")]
        public override decimal bidPrice
        {
            get;
            set;
        }

        /// <summary>
        /// current best ask (sell) price
        /// </summary>
        [JsonProperty(PropertyName = "ask")]
        public override decimal askPrice
        {
            get;
            set;
        }

        /// <summary>
        /// volume weighted average price
        /// </summary>
        [JsonProperty(PropertyName = "vwap")]
        public override decimal vwap
        {
            get
            {
                base.vwap = (this.baseVolume != 0) ? this.quoteVolume / this.baseVolume : 0m;
                return base.vwap;
            }
            set => base.vwap = value;
        }

        /// <summary>
        /// price of last trade (closing price for current period)
        /// </summary>
        [JsonProperty(PropertyName = "last")]
        public override decimal closePrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "volume")]
        public JObject volumeData
        {
            set
            {
                var _volume = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(value.ToString());

                baseVolume = _volume.ElementAt(0).Value.Value<decimal>();
                quoteVolume = _volume.ElementAt(1).Value.Value<decimal>();
                timestamp = CUnixTime.ConvertToMilliSeconds(_volume.ElementAt(2).Value.Value<long>());
            }
        }
    }
}