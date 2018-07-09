using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCXT.NET.Korbit.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public class TradeApi
    {
        private string __connect_key;
        private string __secret_key;
        private string __user_name;
        private string __user_password;

        /// <summary>
        /// 
        /// </summary>
        public TradeApi(string connect_key, string secret_key, string user_name, string user_password)
        {
            __connect_key = connect_key;
            __secret_key = secret_key;
            __user_name = user_name;
            __user_password = user_password;
        }

        private KorbitClient __trade_client = null;

        private KorbitClient TradeClient
        {
            get
            {
                if (__trade_client == null)
                    __trade_client = new KorbitClient(__connect_key, __secret_key, __user_name, __user_password);
                return __trade_client;
            }
        }

        /// <summary>
        /// 매수 주문
        /// </summary>
        /// <param name="currency_pair">비트코인 거래 기준으로 필드값을 가져온다.</param>
        /// <param name="coin_amount">매수하고자 하는 코인의 수량</param>
        /// <param name="price">비트코인의 가격(원화). 500원 단위로만 가능하다. 지정가 주문(type=limit)인 경우에만 유효하다.</param>
        /// <param name="fiat_amount">코인을 구매하는데 사용하고자 하는 금액(원화). 시장가 주문(type=market)인 경우에만 유효하며, 이 파라미터를 사용할 경우 price 파라미터와 coin_amount 파라미터는 사용할 수 없다.</param>
        /// <returns></returns>
        public async Task<TradePlace> PlaceLimitBuy(string currency_pair, decimal coin_amount, decimal price, decimal fiat_amount)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency_pair", currency_pair.ToLower());
                _params.Add("type", "limit");
                _params.Add("price", price);
                _params.Add("coin_amount", coin_amount);
                _params.Add("fiat_amount", fiat_amount);
            }

            return await TradeClient.CallApiPostAsync<TradePlace>("/v1/user/orders/buy", _params);
        }

        /// <summary>
        /// 매수 주문
        /// </summary>
        /// <param name="currency_pair">비트코인 거래 기준으로 필드값을 가져온다.</param>
        /// <param name="coin_amount">매수하고자 하는 코인의 수량</param>
        /// <param name="price">비트코인의 가격(원화). 500원 단위로만 가능하다. 지정가 주문(type=limit)인 경우에만 유효하다.</param>
        /// <param name="fiat_amount">코인을 구매하는데 사용하고자 하는 금액(원화). 시장가 주문(type=market)인 경우에만 유효하며, 이 파라미터를 사용할 경우 price 파라미터와 coin_amount 파라미터는 사용할 수 없다.</param>
        /// <returns></returns>
        public async Task<TradePlace> PlaceMarketBuy(string currency_pair, decimal coin_amount, decimal price, decimal fiat_amount)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency_pair", currency_pair.ToLower());
                _params.Add("type", "market");
                _params.Add("price", price);
                _params.Add("coin_amount", coin_amount);
                _params.Add("fiat_amount", fiat_amount);
            }

            return await TradeClient.CallApiPostAsync<TradePlace>("/v1/user/orders/buy", _params);
        }

        /// <summary>
        /// 매도 주문
        /// </summary>
        /// <param name="currency_pair">비트코인 거래 기준으로 필드값을 가져온다.</param>
        /// <param name="coin_amount">매도하고자 하는 코인의 수량. 지정가 주문인 경우에는 해당 수량을 price 파라미터에 지정한 가격으로 판매하는 주문을 생성한다. 시장가 주문(type=market)인 경우에는 해당 수량을 시장가에 판매하는 주문을 생성하며, 이 경우 price 파라미터는 사용할 수 없다.</param>
        /// <param name="price">비트코인의 가격(원화). 500원 단위로만 가능하다. 지정가 주문(type=limit)인 경우에만 유효하다.</param>
        /// <returns></returns>
        public async Task<TradePlace> PlaceLimitSell(string currency_pair, decimal coin_amount, decimal price)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency_pair", currency_pair.ToLower());
                _params.Add("type", "limit");
                _params.Add("price", price);
                _params.Add("coin_amount", coin_amount);
            }

            return await TradeClient.CallApiPostAsync<TradePlace>("/v1/user/orders/sell", _params);
        }

        /// <summary>
        /// 매도 주문
        /// </summary>
        /// <param name="currency_pair">비트코인 거래 기준으로 필드값을 가져온다.</param>
        /// <param name="price">비트코인의 가격(원화). 500원 단위로만 가능하다. 지정가 주문(type=limit)인 경우에만 유효하다.</param>
        /// <param name="coin_amount">매도하고자 하는 코인의 수량. 지정가 주문인 경우에는 해당 수량을 price 파라미터에 지정한 가격으로 판매하는 주문을 생성한다. 시장가 주문(type=market)인 경우에는 해당 수량을 시장가에 판매하는 주문을 생성하며, 이 경우 price 파라미터는 사용할 수 없다.</param>
        /// <returns></returns>
        public async Task<TradePlace> PlaceMarketSell(string currency_pair, decimal coin_amount, decimal price)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency_pair", currency_pair.ToLower());
                _params.Add("type", "market");
                _params.Add("price", price);
                _params.Add("coin_amount", coin_amount);
            }

            return await TradeClient.CallApiPostAsync<TradePlace>("/v1/user/orders/sell", _params);
        }

        /// <summary>
        /// 주문 취소
        /// </summary>
        /// <param name="currency_pair">비트코인 거래 기준으로 필드값을 가져온다.</param>
        /// <param name="order_id">취소할 주문의 ID</param>
        /// <returns></returns>
        public async Task<List<TradeCancel>> CancelOrder(string currency_pair, string order_id)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency_pair", currency_pair.ToLower());
                _params.Add("id", order_id);
            }

            return await TradeClient.CallApiPostAsync<List<TradeCancel>>("/v1/user/orders/cancel", _params);
        }

        /// <summary>
        /// 미 체결 주문내역
        /// </summary>
        /// <param name="currency_pair">비트코인 거래 기준으로 필드값을 가져온다.</param>
        /// <param name="offset">전체 데이터 중 offset(0부터 시작) 번 째 데이터부터 limit개를 가져오도록 지정 가능하다. offset의 기본값은 0이며, limit의 기본값은 10이다.</param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<TradeOpenOrders> OpenOrders(string currency_pair, int offset = 0, int limit = 10)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency_pair", currency_pair.ToLower());
                _params.Add("offset", offset);
                _params.Add("limit", limit);
            }

            return await TradeClient.CallApiGetAsync<TradeOpenOrders>("/v1/user/orders/open", _params);
        }

        /// <summary>
        /// 거래소 주문 조회 
        /// </summary>
        /// <param name="currency_pair">비트코인 거래 기준으로 필드값을 가져온다.</param>
        /// <param name="status">'unfilled’, 'partially_filled’, 'filled’ 값 중의 하나로, 주문 상태에 따라 조회할 수 있다. 여러 상태를 조합하여 status=unfilled status=partially_filled 와 같은 복합적인 콜을 만들어 낼 수 있다.</param>
        /// <param name="id">주문의 ID로 조회할 수 있다. 여러 id를 조합하여 id=90308 id=90374 와 같은 복합적인 콜을 만들어 낼 수 있다.</param>
        /// <param name="offset">전체 데이터 중 offset(기본값은 0)번째부터 데이터를 가져오도록 지정할 수 있다. 기본값은 0이다.</param>
        /// <param name="limit">전체 데이터 중 limit(기본값은 10)개만 가져오도록 지정할 수 있다. 기본값은 40이다.</param>
        /// <returns></returns>
        public async Task<TradeOpenOrders> CompleteOrders(string currency_pair, string status, string id, int offset = 0, int limit = 10)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency_pair", currency_pair.ToLower());
                _params.Add("status", status);
                _params.Add("id", id);
                _params.Add("offset", offset);
                _params.Add("limit", limit);
            }

            return await TradeClient.CallApiGetAsync<TradeOpenOrders>("/v1/user/orders", _params);
        }
    }
}