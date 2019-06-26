using Newtonsoft.Json;
using CCXT.NET.Coin.Public;
using System.Collections.Generic;

namespace CCXT.NET.Upbit.Public
{
    /// <summary>
    ///
    /// </summary>
    public class UOrderBook : CCXT.NET.Coin.Public.OrderBook, IOrderBook
    {
        /// <summary>
        /// 마켓 코드
        /// </summary>
        [JsonProperty(PropertyName = "market")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// 호가 생성 시각
        /// </summary>
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// 호가 매도 총 잔량
        /// </summary>
        [JsonProperty(PropertyName = "total_ask_size")]
        public decimal totalAskQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// 호가 매수 총 잔량
        /// </summary>
        [JsonProperty(PropertyName = "total_bid_size")]
        public decimal totalBidQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// 호가
        /// </summary>
        [JsonProperty(PropertyName = "orderbook_units")]
        private List<UOrderBookItem> orderbooks
        {
            set
            {
                this.asks = new List<IOrderBookItem>();
                this.bids = new List<IOrderBookItem>();

                foreach (var _o in value)
                {
                    this.asks.Add(new OrderBookItem
                    {
                        quantity = _o.ask_size,
                        price = _o.ask_price,
                        amount = _o.ask_size * _o.ask_price,
                        count = 1
                    });

                    this.bids.Add(new OrderBookItem
                    {
                        quantity = _o.bid_size,
                        price = _o.bid_price,
                        amount = _o.bid_size * _o.bid_price,
                        count = 1
                    });
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class UOrderBookItem
    {
        /// <summary>
        /// 매도 호가
        /// </summary>
        public decimal ask_price
        {
            get;
            set;
        }

        /// <summary>
        /// 매수 호가
        /// </summary>
        public decimal bid_price
        {
            get;
            set;
        }

        /// <summary>
        /// 매도 수량
        /// </summary>
        public decimal ask_size
        {
            get;
            set;
        }

        /// <summary>
        /// 매수 수량
        /// </summary>
        public decimal bid_size
        {
            get;
            set;
        }
    }
}