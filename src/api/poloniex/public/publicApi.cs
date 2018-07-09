using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CCXT.NET.Configuration;

namespace CCXT.NET.Poloniex.Public
{
    /// <summary>
    /// https://poloniex.com/
    /// </summary>
    public class PublicApi
    {
        private string __end_point;
        
        /// <summary>
        /// 
        /// </summary>
        public PublicApi(string end_point = "public")
        {
            __end_point = end_point;
        }

        private PoloniexClient __public_client = null;

        private PoloniexClient PublicClient
        {
            get
            {
                if (__public_client == null)
                    __public_client = new PoloniexClient();
                return __public_client;
            }
        }

        /// <summary>
        /// poloniex 거래소 마지막 거래 정보
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, PublicTicker>> GetTicker()
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnTicker");
            }

            return await PublicClient.CallApiGetAsync<Dictionary<string, PublicTicker>>(__end_point, _params);
        }

        /// <summary>
        /// poloniex 거래소 판/구매 등록 대기 또는 거래 중 내역 정보
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public async Task<PublicOrderBook> GetOrderBook(CurrencyPair currency_pair, uint depth)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnOrderBook");
                _params.Add("currencyPair", currency_pair);
                _params.Add("depth", depth);
            }

            return await PublicClient.CallApiGetAsync<PublicOrderBook>(__end_point, _params);
        }

        /// <summary>
        /// poloniex 거래소 거래 체결 완료 내역
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <returns></returns>
        public async Task<List<PublicTrade>> GetTradeHistory(CurrencyPair currency_pair)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnTradeHistory");
                _params.Add("currencyPair", currency_pair);
            }

            return await PublicClient.CallApiGetAsync<List<PublicTrade>>(__end_point, _params);
        }

        /// <summary>
        /// poloniex 거래소 거래 체결 완료 내역
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <returns></returns>
        public async Task<List<PublicTrade>> GetTradeHistory(CurrencyPair currency_pair, DateTime start_time, DateTime end_time)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnTradeHistory");
                _params.Add("currencyPair", currency_pair);
                _params.Add("start", start_time.DateTimeToUnixTimeStamp());
                _params.Add("end", end_time.DateTimeToUnixTimeStamp());
            }

            return await PublicClient.CallApiGetAsync<List<PublicTrade>>(__end_point, _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <param name="period"></param>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <returns></returns>
        public async Task<List<PublicChart>> GetChartData(CurrencyPair currency_pair, ChartPeriod period, DateTime start_time, DateTime end_time)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnChartData");
                _params.Add("currencyPair", currency_pair);
                _params.Add("start", start_time.DateTimeToUnixTimeStamp());
                _params.Add("end", end_time.DateTimeToUnixTimeStamp());
                _params.Add("period", (int)period);
            }

            return await PublicClient.CallApiGetAsync<List<PublicChart>>(__end_point, _params);
        }
    }
}