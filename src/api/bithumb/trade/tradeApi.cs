using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCXT.NET.Bithumb.Trade
{
    /// <summary>
    /// https://api.bithumb.com/
    /// </summary>
    public class TradeApi
    {
        private string __connect_key;
        private string __secret_key;

        /// <summary>
        /// 
        /// </summary>
        public TradeApi(string connect_key, string secret_key)
        {
            __connect_key = connect_key;
            __secret_key = secret_key;
        }

        private BithumbClient __trade_client = null;

        private BithumbClient tradeClient
        {
            get
            {
                if (__trade_client == null)
                    __trade_client = new BithumbClient(__connect_key, __secret_key);
                return __trade_client;
            }
        }

        /// <summary>
        /// bithumb 회원 판/구매 거래 주문 등록 및 체결 (미수 주문등록 및 체결은 현 API에서 지원 안 함)
        /// </summary>
        /// <param name="order_currency">BTC, ETH, DASH, LTC, ETC, XRP (기본값: BTC)</param>
        /// <param name="units">주문 수량, 1회 최소 수량 (BTC: 0.0001 | ETH: 0.001 | DASH: 0.001 | LTC: 0.01 | ETC: 0.01 | XRP: 1) - 1회 최대 수량 (BTC: 300 | ETH: 2,500 | DASH: 4,000 | LTC: 15,000 | ETC: 30,000 | XRP: 2,500,000)</param>
        /// <param name="price">1Currency당 거래금액 (BTC, ETH, DASH, LTC, ETC, XRP)</param>
        /// <param name="payment_currency">KRW (기본값)</param>
        /// <param name="misu">신용거래(Y : 사용, N : 일반) – 추후 제공</param>
        /// <returns></returns>
        public async Task<Place> PlaceLimitBuy(string order_currency, decimal units, decimal price, string payment_currency = "KRW", string misu = "N")
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("units", units);
                _params.Add("price", price);
                _params.Add("type", "bid");
                _params.Add("order_currency", order_currency.ToUpper());
                _params.Add("payment_currency", payment_currency.ToUpper());
                _params.Add("misu", misu);
            }

            return await tradeClient.CallApiPostAsync<Place>("/trade/place", _params);
        }

        /// <summary>
        /// bithumb 회원 판매 거래 주문 등록 및 체결 (미수 주문등록 및 체결은 현 API에서 지원 안 함)
        /// </summary>
        /// <param name="order_currency">BTC, ETH, DASH, LTC, ETC, XRP (기본값: BTC)</param>
        /// <param name="units">주문 수량, 1회 최소 수량 (BTC: 0.0001 | ETH: 0.001 | DASH: 0.001 | LTC: 0.01 | ETC: 0.01 | XRP: 1) - 1회 최대 수량 (BTC: 300 | ETH: 2,500 | DASH: 4,000 | LTC: 15,000 | ETC: 30,000 | XRP: 2,500,000)</param>
        /// <param name="price">1Currency당 거래금액 (BTC, ETH, DASH, LTC, ETC, XRP)</param>
        /// <param name="payment_currency">KRW (기본값)</param>
        /// <param name="misu">신용거래(Y : 사용, N : 일반) – 추후 제공</param>
        /// <returns></returns>
        public async Task<Place> PlaceLimitSell(string order_currency, decimal units, decimal price, string payment_currency = "KRW", string misu = "N")
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("units", units);
                _params.Add("price", price);
                _params.Add("type", "ask");
                _params.Add("order_currency", order_currency.ToUpper());
                _params.Add("payment_currency", payment_currency.ToUpper());
                _params.Add("misu", misu);
            }

            return await tradeClient.CallApiPostAsync<Place>("/trade/place", _params);
        }

        /// <summary>
        /// 시장가 구매
        /// </summary>
        /// <param name="currency">BTC, ETH, DASH, LTC, ETC, XRP (기본값: BTC)</param>
        /// <param name="units">주문 수량, 1회 최소 수량 (BTC: 0.0001 | ETH: 0.001 | DASH: 0.001 | LTC: 0.01 | ETC: 0.01 | XRP: 1) - 1회 거래 한도 : 1억원</param>
        /// <returns></returns>
        public async Task<PlaceMarket> PlaceMarketBuy(string currency, decimal units)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency", currency.ToUpper());
                _params.Add("units", units);
            }

            return await tradeClient.CallApiPostAsync<PlaceMarket>("/trade/market_buy", _params);
        }

        /// <summary>
        /// 시장가 판매
        /// </summary>
        /// <param name="currency">BTC, ETH, DASH, LTC, ETC, XRP (기본값: BTC)</param>
        /// <param name="units">주문 수량, 1회 최소 수량 (BTC: 0.0001 | ETH: 0.001 | DASH: 0.001 | LTC: 0.01 | ETC: 0.01 | XRP: 1) - 1회 거래 한도 : 1억원</param>
        /// <returns></returns>
        public async Task<PlaceMarket> PlaceMarketSell(string currency, decimal units)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency", currency.ToUpper());
                _params.Add("units", units);
            }

            return await tradeClient.CallApiPostAsync<PlaceMarket>("/trade/market_sell", _params);
        }

        /// <summary>
        /// bithumb 회원 판/구매 거래 취소
        /// </summary>
        /// <param name="currency">BTC, ETH, DASH, LTC, ETC, XRP (기본값: BTC)</param>
        /// <param name="order_id">판/구매 주문 등록된 주문번호</param>
        /// <param name="type">거래유형 (bid : 구매, ask : 판매)</param>
        /// <returns></returns>
        public async Task<Cancel> CancelOrder(string currency, string order_id, string type)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency", currency.ToUpper());
                _params.Add("order_id", order_id);
                _params.Add("type", type);
            }

            return await tradeClient.CallApiPostAsync<Cancel>("/trade/cancel", _params);
        }

        /// <summary>
        /// bithumb 회원 btc 출금(회원등급에 따른 BTC, ETH, DASH, LTC, ETC, XRP 출금)
        /// </summary>
        /// <param name="currency">BTC, ETH, DASH, LTC, ETC, XRP (기본값: BTC)</param>
        /// <param name="units">Currency 출금 하고자 하는 수량, 1회 최소 수량 (BTC: 0.001 | ETH: 0.01 | DASH: 0.01 | LTC: 0.01 | ETC: 0.01 | XRP: 21) - 1회 최대 수량 : 회원등급수량</param>
        /// <param name="address">Currency 출금 주소 (BTC, ETH, DASH, LTC, ETC, XRP)</param>
        /// <param name="destination">Currency 출금 Destination Tag (XRP 출금시)</param>
        /// <returns></returns>
        public async Task<Withdraw> BtcWithdrawal(string currency, decimal units, string address, string destination = null)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency", currency.ToUpper());
                _params.Add("units", units);
                _params.Add("address", address);
                if (destination != null)
                    _params.Add("destination", destination);
            }

            return await tradeClient.CallApiPostAsync<Withdraw>("/trade/btc_withdrawal", _params);
        }

        /// <summary>
        /// bithumb 회원 krw 출금 신청
        /// </summary>
        /// <param name="bank">은행코드_은행명</param>
        /// <param name="account">출금계좌번호</param>
        /// <param name="price">출금 금액</param>
        /// <returns></returns>
        public async Task<Withdraw> KrwWithdrawal(string bank, string account, decimal price)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("bank", bank);
                _params.Add("account", account);
                _params.Add("price", price);
            }

            return await tradeClient.CallApiPostAsync<Withdraw>("/trade/krw_withdrawal", _params);
        }

        /// <summary>
        /// bithumb 회원 krw 입금 가상계좌 정보 요청
        /// </summary>
        /// <returns></returns>
        public async Task<KrwDeposit> KrwDeposit()
        {
            return await tradeClient.CallApiPostAsync<KrwDeposit>("/trade/krw_deposit");
        }
    }
}