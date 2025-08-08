using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Coin.Public;
using System.Collections.Generic;

namespace CCXT.NET.Bithumb.Public
{
    /// <summary>
    ///
    /// </summary>
    public class BOrderBooks : CCXT.NET.Shared.Coin.Public.OrderBooks, IOrderBooks
    {
        /// <summary>
        ///
        /// </summary>
        public BOrderBooks()
        {
            this.result = new BOrderBook();
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
        public new BOrderBook result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BOrderBook : CCXT.NET.Shared.Coin.Public.OrderBook, IOrderBook
    {
        /// <summary>
        /// 결제 화폐단위
        /// </summary>
        public string payment_currency
        {
            get;
            set;
        }

        /// <summary>
        /// 주문 화폐단위
        /// </summary>
        public string order_currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "bids")]
        private List<JToken> bidsValue
        {
            set
            {
                this.bids = new List<OrderBookItem>();

                foreach (var _bid in value)
                {
                    var _b = new OrderBookItem
                    {
                        price = _bid["price"].Value<decimal>(),
                        quantity = _bid["quantity"].Value<decimal>(),
                        count = 1
                    };

                    _b.amount = _b.quantity * _b.price;
                    this.bids.Add(_b);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "asks")]
        private List<JToken> asksValue
        {
            set
            {
                this.asks = new List<OrderBookItem>();

                foreach (var _ask in value)
                {
                    var _a = new OrderBookItem
                    {
                        price = _ask["price"].Value<decimal>(),
                        quantity = _ask["quantity"].Value<decimal>(),
                        count = 1
                    };

                    _a.amount = _a.quantity * _a.price;
                    this.asks.Add(_a);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [JsonProperty(PropertyName = "originBids")]
        public override List<OrderBookItem> bids
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [JsonProperty(PropertyName = "originAsks")]
        public override List<OrderBookItem> asks
        {
            get;
            set;
        }
    }
}