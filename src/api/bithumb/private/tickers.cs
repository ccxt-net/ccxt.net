namespace CCXT.NET.Bithumb.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class TickerItem
    {
        /// <summary>
        /// 최근 24시간 내 시작 거래금액
        /// </summary>
        public decimal? opening_price
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 24시간 내 마지막 거래금액
        /// </summary>
        public decimal? closing_price
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 24시간 내 최저 거래금액
        /// </summary>
        public decimal? min_price
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 24시간 내 최고 거래금액
        /// </summary>
        public decimal? max_price
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 24시간 내 평균 거래금액
        /// </summary>
        public decimal? average_price
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 24시간 내 BTC 거래량
        /// </summary>
        public decimal? units_traded
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 1일간 BTC 거래량
        /// </summary>
        public decimal? volume_1day
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 7일간 BTC 거래량
        /// </summary>
        public decimal? volume_7day
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal? buy_price
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal? sell_price
        {
            get;
            set;
        }

        /// <summary>
        /// 현재 시간 Timestamp
        /// </summary>
        public long date
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Ticker : ApiResult<TickerItem>
    {
        /// <summary>
        /// 
        /// </summary>
        public Ticker()
        {
            this.data = new TickerItem();
        }
    }
}