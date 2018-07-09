using CCXT.NET.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCXT.NET.Korbit.Public
{
    /// <summary>
    /// Check the current status of the market by listing open and filled orders.
    /// </summary>
    public class PublicApi
    {
        public const string DealerName = "Korbit";

        private KorbitClient __api_client = null;

        private KorbitClient APiClient
        {
            get
            {
                if (__api_client == null)
                    __api_client = new KorbitClient();
                return __api_client;
            }
        }

        /// <summary>
        /// https://bitbucket.org/korbit/public-api/wiki/browse/
        /// </summary>
        /// <returns></returns>
        public async Task<Version> Version()
        {
            return await APiClient.CallApiGetAsync<Version>("/v1/version");
        }

        /// <summary>
        /// 시장 현황 (Ticker)
        /// </summary>
        /// <param name="currency_pair">The type of trading currency of which information you want to query for. 
        /// Bitcoin trading is default. As our BETA service, you can also specify “etc_krw” for Ethereum Classic 
        /// trading and “eth_krw” for Ethereum trading.</param>
        /// <returns></returns>
        public async Task<PublicTicker> Ticker(string currency_pair = "btc_krw")
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency_pair", currency_pair.ToLower());
            }

            return await APiClient.CallApiGetAsync<PublicTicker>("/v1/ticker", _params);
        }

        /// <summary>
        /// 시장 현황 상세정보 (DeatiledTicker)
        /// </summary>
        /// <param name="currency_pair">The type of trading currency of which information you want to query for. 
        /// Bitcoin trading is default. As our BETA service, you can also specify “etc_krw” for 
        /// Ethereum Classic trading and “eth_krw” for Ethereum trading.</param>
        /// <returns></returns>
        public async Task<PublicDetailedTicker> DetailedTicker(string currency_pair = "btc_krw")
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency_pair", currency_pair.ToLower());
            }

            return await APiClient.CallApiGetAsync<PublicDetailedTicker>("/v1/ticker/detailed", _params);
        }

        /// <summary>
        /// 매수/매도 호가 ( Orderbook )
        /// </summary>
        /// <param name="currency_pair">The type of trading currency of which information you want to query for. 
        /// Bitcoin trading is default. As our BETA service, you can also specify “etc_krw” for 
        /// Ethereum Classic trading and “eth_krw” for Ethereum trading.</param>
        /// <param name="category">List ask orders only if category=“ask”, bid orders only if category=“bid”, all orders if category=“all”.</param>
        /// <returns></returns>
        public async Task<PublicOrderBook> OrderBook(string currency_pair = "btc_krw", OrderCategory category = OrderCategory.all)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency_pair", currency_pair.ToLower());
                _params.Add("category", category);
            }

            return await APiClient.CallApiGetAsync<PublicOrderBook>("/v1/orderbook", _params);
        }

        /// <summary>
        /// List of Filled Orders
        /// </summary>
        /// <param name="currency_pair">The type of trading currency of which information you want to query for. 
        /// Bitcoin trading is default. As our BETA service, you can also specify “etc_krw” for 
        /// Ethereum Classic trading and “eth_krw” for Ethereum trading.</param>
        /// <param name="time">The time period you want to query. 
        /// If this parameter is specified as minute, it queries data within the last minute, 
        /// hour means the last hour, day means the last 24 hours.</param>
        /// <returns></returns>
        public async Task<PublicCompleteOrders> CompleteOrders(string currency_pair = "btc_krw", TimeSymbol time = TimeSymbol.hour)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency_pair", currency_pair.ToLower());
                _params.Add("time", time);
            }

            return await APiClient.CallApiGetAsync<PublicCompleteOrders>("/v1/transactions", _params);
        }

        /// <summary>
        /// You can get constant values such as fee rates and minimum amount of BTC to transfer, etc.
        /// </summary>
        /// <returns>Constants</returns>
        public async Task<PublicConstants> Constants()
        {
            return await APiClient.CallApiGetAsync<PublicConstants>("/v1/constants");
        }
    }
}