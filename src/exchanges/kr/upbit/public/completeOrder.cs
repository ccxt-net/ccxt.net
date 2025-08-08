using Newtonsoft.Json;
using CCXT.NET.Shared.Coin.Public;
using CCXT.NET.Shared.Coin.Types;

namespace CCXT.NET.Upbit.Public
{
    /// <summary>
    ///
    /// </summary>
    public class UCompleteOrderItem : CCXT.NET.Shared.Coin.Public.CompleteOrderItem, ICompleteOrderItem
    {
        /// <summary>
        /// 마켓 구분 코드
        /// </summary>
        [JsonProperty(PropertyName = "market")]
        public string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// 체결 타임스탬프
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "trade_volume")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "trade_price")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "prev_closing_price")]
        public decimal prevPrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "change_price")]
        public decimal changePrice
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "chane_price")]
        private decimal chane_price
        {
            set
            {
                changePrice = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "ask_bid")]
        private string sideValue
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }
    }
}