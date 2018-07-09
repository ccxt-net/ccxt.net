namespace CCXT.NET.Bithumb.Public
{
    /// <summary>
    /// https://api.bithumb.com/public/ticker/{currency}
    /// bithumb 거래소 마지막 거래 정보
    /// * {currency} = BTC, ETH, DASH, LTC, ETC, XRP (기본값: BTC), ALL(전체)
    /// </summary>
    public class TickerItem
    {
        /// <summary>
        /// 최근 24시간 내 시작 거래금액
        /// </summary>
        public decimal opening_price
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 24시간 내 마지막 거래금액
        /// </summary>
        public decimal closing_price
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 24시간 내 최저 거래금액
        /// </summary>
        public decimal min_price
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 24시간 내 최고 거래금액
        /// </summary>
        public decimal max_price
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 24시간 내 평균 거래금액
        /// </summary>
        public decimal average_price
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 24시간 내 BTC 거래량
        /// </summary>
        public decimal units_traded
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 1일간 BTC 거래량
        /// </summary>
        public decimal volume_1day
        {
            get;
            set;
        }

        /// <summary>
        /// 최근 7일간 BTC 거래량
        /// </summary>
        public decimal volume_7day
        {
            get;
            set;
        }


        /// <summary>
        /// 거래 대기건 최고 구매가
        /// </summary>
        public decimal buy_price
        {
            get;
            set;
        }

        /// <summary>
        /// 거래 대기건 최소 판매가
        /// </summary>
        public decimal sell_price
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

    /// <summary>
    /// 
    /// </summary>
    public class TickerAll
    {
        /// <summary>
        /// 
        /// </summary>
        public TickerAll()
        {
            BTC = new TickerItem();
            ETH = new TickerItem();
            XRP = new TickerItem();
            BCH = new TickerItem();
            EOS = new TickerItem();
            LTC = new TickerItem();
            TRX = new TickerItem();
            DASH = new TickerItem();
            XMR = new TickerItem();
            VNE = new TickerItem();
            ETC = new TickerItem();
            ICX = new TickerItem();
            QTUM = new TickerItem();
            OMG = new TickerItem();
            ZEC = new TickerItem();
            BTG = new TickerItem();
        }

        /// <summary>
        /// 오미세고
        /// </summary>
        public TickerItem OMG
        {
            get;
            set;
        }

        /// <summary>
        /// 비체인
        /// </summary>
        public TickerItem VNE
        {
            get;
            set;
        }

        /// <summary>
        /// Tronix
        /// </summary>
        public TickerItem TRX
        {
            get;
            set;
        }

        /// <summary>
        /// BitCoin
        /// </summary>
        public TickerItem BTC
        {
            get;
            set;
        }

        /// <summary>
        /// Ethereum
        /// </summary>
        public TickerItem ETH
        {
            get;
            set;
        }

        /// <summary>
        /// DashCoin
        /// </summary>
        public TickerItem DASH
        {
            get;
            set;
        }

        /// <summary>
        /// LiteCoin
        /// </summary>
        public TickerItem LTC
        {
            get;
            set;
        }

        /// <summary>
        /// Ethereum Classic
        /// </summary>
        public TickerItem ETC
        {
            get;
            set;
        }

        /// <summary>
        /// Ripple
        /// </summary>
        public TickerItem XRP
        {
            get;
            set;
        }

        /// <summary>
        /// Bitcoin Cash
        /// </summary>
        public TickerItem BCH
        {
            get;
            set;
        }

        /// <summary>
        /// Monero
        /// </summary>
        public TickerItem XMR
        {
            get;
            set;
        }

        /// <summary>
        /// Z-CASH
        /// </summary>
        public TickerItem ZEC
        {
            get;
            set;
        }

        /// <summary>
        /// Quntum
        /// </summary>
        public TickerItem QTUM
        {
            get;
            set;
        }

        /// <summary>
        /// Bitcoin Gold
        /// </summary>
        public TickerItem BTG
        {
            get;
            set;
        }

        /// <summary>
        /// EOS
        /// </summary>
        public TickerItem EOS
        {
            get;
            set;
        }

        /// <summary>
        /// ICX
        /// </summary>
        public TickerItem ICX
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
    public class Tickers : ApiResult<TickerAll>
    {
        /// <summary>
        /// 
        /// </summary>
        public Tickers()
        {
            this.data = new TickerAll();
        }
    }
}