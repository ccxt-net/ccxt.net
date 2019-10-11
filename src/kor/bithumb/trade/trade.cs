using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Coin.Types;
using System;
using System.Collections.Generic;

namespace CCXT.NET.Bithumb.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class BMyTrades : OdinSdk.BaseLib.Coin.Trade.MyTrades, IMyTrades
    {
        /// <summary>
        ///
        /// </summary>
        public BMyTrades()
        {
            this.result = new List<BMyTradeItem>();
        }

        /// <summary>
        /// 결과 상태 코드 (정상 : 0000, 정상이외 코드는 에러 코드 참조)
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
        public new List<BMyTradeItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BMyTradeItem : OdinSdk.BaseLib.Coin.Trade.MyTradeItem, IMyTradeItem
    {
        /// <summary>
        /// 거래금액
        /// </summary>
        [JsonProperty(PropertyName = "price")]
        public override decimal amount
        {
            get;
            set;
        }

        /// <summary>
        /// 거래수수료
        /// </summary>
        [JsonProperty(PropertyName = "originFee")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// 0 : 전체, 1 : 구매완료, 2 : 판매완료, 3 : 출금중, 4 : 입금, 5 : 출금, 9 : KRW입금중
        /// </summary>
        [JsonProperty(PropertyName = "originSearch")]
        public int search
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal coin1krw
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal coin_remain
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public decimal krw_remain
        {
            get;
            set;
        }

        /// <summary>
        /// 거래 일시 Timestamp
        /// </summary>
        [JsonProperty(PropertyName = "transfer_date")]
        private long transfer_date
        {
            set
            {
                tradeId = value.ToString();
                timestamp = value / 1000;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "search")]
        private int searchValue
        {
            set
            {
                search = value;

                sideType = (search == 1) ? SideType.Bid
                         : (search == 2) ? SideType.Ask
                         : SideType.Unknown;
            }
        }

        /// <summary>
        /// 거래 Currency 수량
        /// </summary>
        [JsonProperty(PropertyName = "units")]
        private string quantityValue
        {
            set
            {
                quantity = Math.Abs(Convert.ToDecimal(value.Replace(" ", "")));
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "fee")]
        private string feeValue
        {
            set
            {
                var _fee = value.Split(' ');
                if (_fee.Length > 0)
                    fee = Convert.ToDecimal(_fee[0]);
            }
        }
    }
}