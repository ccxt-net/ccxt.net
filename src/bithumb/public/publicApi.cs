using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCXT.NET.Bithumb.Public
{
    /// <summary>
    /// https://api.bithumb.com/
    /// </summary>
    public class PublicApi
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DealerName = "Bithumb";

        private readonly string __connect_key;
        private readonly string __secret_key;

        /// <summary>
        /// 
        /// </summary>
        public PublicApi()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public PublicApi(string connect_key, string secret_key)
        {
            __connect_key = connect_key;
            __secret_key = secret_key;
        }

        private BithumbClient __public_client = null;

        private BithumbClient publicClient
        {
            get
            {
                if (__public_client == null)
                    __public_client = new BithumbClient(__connect_key, __secret_key);
                return __public_client;
            }
        }

        /// <summary>
        /// bithumb 거래소 마지막 거래 정보
        /// </summary>
        /// <param name="currency">BTC, ETH, DASH, LTC, ETC, XRP (기본값: BTC), ALL(전체)</param>
        /// <returns></returns>
        public async Task<Ticker> FetchTicker(string currency)
        {
            return await publicClient.CallApiGetAsync<Ticker>($"/public/ticker/{currency.ToUpper()}");
        }

        /// <summary>
        /// bithumb 거래소 마지막 거래 정보
        /// </summary>
        /// <returns></returns>
        public async Task<BTickers> FetchTickers()
        {
            return await publicClient.CallApiGetAsync<BTickers>($"/public/ticker/ALL");
        }

        /// <summary>
        /// bithumb 거래소 판/구매 등록 대기 또는 거래 중 내역 정보
        /// </summary>
        /// <param name="currency">BTC, ETH, DASH, LTC, ETC, XRP (기본값: BTC), ALL(전체)</param>
        /// <param name="group_orders">Value : 0 또는 1 (Default : 1)</param>
        /// <param name="count">Value : 1 ~ 50 (Default : 20)</param>
        /// <returns></returns>
        public async Task<OrderBooks> FetchOrderBooks(string currency = "ALL", int group_orders = 1, int count = 20)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("group_orders", group_orders);
                _params.Add("count", count);
            }

            return await publicClient.CallApiGetAsync<OrderBooks>($"/public/orderbook/{currency.ToUpper()}", _params);
        }

        /// <summary>
        /// bithumb 거래소 거래 체결 완료 내역
        /// </summary>
        /// <param name="currency">BTC, ETH, DASH, LTC, ETC, XRP (기본값: BTC)</param>
        /// <param name="cont_no">체결번호</param>
        /// <param name="count">Value : 1 ~ 100 (Default : 20)</param>
        /// <returns></returns>
        public async Task<BcompleteOrders> FetchTrades(string currency, int cont_no = 0, int count = 20)
        {
            var _params = new Dictionary<string, object>();
            {
                if (cont_no > 0)
                    _params.Add("cont_no", cont_no);

                _params.Add("count", count);
            }

            return await publicClient.CallApiGetAsync<BcompleteOrders>($"/public/transaction_history/{currency.ToUpper()}", _params);
        }
    }
}