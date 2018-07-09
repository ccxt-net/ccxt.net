using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCXT.NET.Coinone.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class PublicApi
    {
        private CoinOneClient __public_client = null;

        private CoinOneClient PublicClient
        {
            get
            {
                if (__public_client == null)
                    __public_client = new CoinOneClient();
                return __public_client;
            }
        }

        public async Task<PublicCurrency> Currency(string currencyType = "KRW")
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currencyType", currencyType);
            }

            return await PublicClient.CallApiGetAsync<PublicCurrency>("/currency", _params);
        }

        /// <summary>
        /// Public - Orderbook
        /// </summary>
        /// <param name="currency">Default value: btc, Allowed values: btc, eth, etc, xrp</param>
        /// <returns></returns>
        public async Task<PublicOrderBook> OrderBook(string currency)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency", currency);
            }

            return await PublicClient.CallApiGetAsync<PublicOrderBook>("/orderbook", _params);
        }

        /// <summary>
        /// Public - Ticker
        /// </summary>
        /// <param name="currency">Default value: btc, Allowed values: btc, eth, etc, xrp</param>
        /// <returns></returns>
        public async Task<PublicTicker> Ticker(string currency)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency", currency);
            }

            return await PublicClient.CallApiGetAsync<PublicTicker>("/ticker", _params);
        }

        /// <summary>
        /// Public - TickerAll
        /// </summary>
        /// <returns></returns>
        public async Task<PublicTickerAll> TickerAll()
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency", "all");
            }

            return await PublicClient.CallApiGetAsync<PublicTickerAll>("/ticker", _params);
        }

        /// <summary>
        /// Public - Recent Complete Orders
        /// </summary>
        /// <param name="currency">Default value: btc, Allowed values: btc, eth, etc, xrp</param>
        /// <param name="period">Default value: hour, Allowed values: hour, day</param>
        /// <returns></returns>
        public async Task<PublicTrades> Trades(string currency, string period)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("currency", currency);
                _params.Add("period", period);
            }

            return await PublicClient.CallApiGetAsync<PublicTrades>("/trades", _params);
        }
    }
}