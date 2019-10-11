using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Public;
using OdinSdk.BaseLib.Coin.Types;
using OdinSdk.BaseLib.Configuration;
using System;
using System.Collections.Generic;

namespace CCXT.NET.Bithumb.Public
{
    /// <summary>
    ///
    /// </summary>
    public class BCompleteOrders : OdinSdk.BaseLib.Coin.Public.CompleteOrders, ICompleteOrders
    {
        /// <summary>
        ///
        /// </summary>
        public BCompleteOrders()
        {
            this.result = new List<BCompleteOrderItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public override int statusCode
        {
            get => base.statusCode;
            set
            {
                base.statusCode = value;

                if (statusCode == 0)
                {
                    message = "success";
                    errorCode = ErrorCode.Success;
                    success = true;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new List<BCompleteOrderItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BCompleteOrderItem : OdinSdk.BaseLib.Coin.Public.CompleteOrderItem, ICompleteOrderItem
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "cont_no")]
        public override string transactionId
        {
            get;
            set;
        }

        /// <summary>
        /// 거래 Currency 수량
        /// </summary>
        [JsonProperty(PropertyName = "units_traded")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// 1Currency당 거래금액 (BTC, ETH, DASH, LTC, ETC, XRP)
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// 총 거래금액
        /// </summary>
        [JsonProperty(PropertyName = "total")]
        public override decimal amount
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "transaction_date")]
        private string timeValue
        {
            set
            {
                var _time_value = value;
                if (_time_value.IndexOf('+') < 0 && _time_value.IndexOf('Z') < 0)
                    _time_value += "+09:00";

                timestamp = CUnixTime.ConvertToUnixTimeMilli(DateTime.Parse(_time_value).ToUniversalTime());
            }
        }

        /// <summary>
        /// 판/구매 (ask, bid)
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        private string sideValue
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }
    }
}