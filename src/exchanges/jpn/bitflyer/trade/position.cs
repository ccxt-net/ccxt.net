using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System;

namespace CCXT.NET.Bitflyer.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class BMyPositionItem : OdinSdk.BaseLib.Coin.Trade.MyPositionItem, IMyPositionItem
    {
        /// <summary>
        ///
        /// </summary>
        public string product_code
        {
            get;
            set;
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

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public override decimal price
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
        [JsonProperty(PropertyName = "commission")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "swap_point_accumulate")]
        public decimal swap
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal require_collateral
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "open_date")]
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
        public int leverage
        {
            get;
            set;
        }

        /// <summary>
        /// profit and loss
        /// </summary>
        public decimal pnl
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal sfd
        {
            get;
            set;
        }
    }
}