using OdinSdk.BaseLib.Coin.Types;
using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCXT.NET.Poloniex.Trade
{
    /// <summary>
    /// https://poloniex.com/
    /// </summary>
    public class TradeApi
    {
        private readonly string __connect_key;
        private readonly string __secret_key;
        private string __end_point;

        /// <summary>
        /// 
        /// </summary>
        public TradeApi(string connect_key, string secret_key, string end_point = "tradingApi")
        {
            __connect_key = connect_key;
            __secret_key = secret_key;

            __end_point = end_point;
        }

        private PoloniexClient __trade_client = null;

        private PoloniexClient TradeClient
        {
            get
            {
                if (__trade_client == null)
                    __trade_client = new PoloniexClient(__connect_key, __secret_key);
                return __trade_client;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <returns></returns>
        public async Task<List<PTradeOrderItem>> OpenOrders(CurrencyPair currency_pair)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnOpenOrders");
                _params.Add("currencyPair", currency_pair);
            }

            return await TradeClient.CallApiPostAsync<List<PTradeOrderItem>>(__end_point, _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<PTradeOrderItem>> AllOpenOrders()
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnOpenOrders");
                _params.Add("currencyPair", "all");
            }

            return await TradeClient.CallApiPostAsync<List<PTradeOrderItem>>(__end_point, _params);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <returns></returns>
        public async Task<List<PTradeOrderItem>> GetTrades(CurrencyPair currency_pair, DateTime start_time, DateTime end_time)
        {
            var _result = new List<PTradeOrderItem>();

            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "returnTradeHistory");
                _params.Add("currencyPair", currency_pair);
                _params.Add("start", start_time.DateTimeToUnixTimeStamp());
                _params.Add("end", end_time.DateTimeToUnixTimeStamp());
            }

            var _json_value = await TradeClient.CallApiPost1Async(__end_point, _params);

            var _json_result = TradeClient.GetResponseMessage(_json_value.Response);
            if (_json_result.success == true)
                _result = TradeClient.DeserializeObject<List<PTradeOrderItem>>(_json_value.Content);

            return _result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_pair"></param>
        /// <param name="type"></param>
        /// <param name="pricePerCoin"></param>
        /// <param name="amountQuote"></param>
        /// <returns></returns>
        public async Task<ulong> PlaceOrder(CurrencyPair currency_pair, SideType type, decimal pricePerCoin, decimal amountQuote)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", type == SideType.Ask ? "sell" : "buy");
                _params.Add("currencyPair", currency_pair);
                _params.Add("rate", pricePerCoin.ToStringNormalized());
                _params.Add("amount", amountQuote.ToStringNormalized());
            }

            var _data = await TradeClient.CallApiPostAsync<JObject>(__end_point, _params);
            return _data.Value<ulong>("orderNumber");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteOrder(CurrencyPair currency_pair, ulong order_id)
        {
            var _params = new Dictionary<string, object>();
            {
                _params.Add("command", "cancelOrder");
                _params.Add("currencyPair", currency_pair);
                _params.Add("orderNumber", order_id);
            }

            var _data = await TradeClient.CallApiPostAsync<JObject>(__end_point, _params);
            return _data.Value<byte>("success") == 1;
        }
    }
}