namespace CCXT.NET.Coinone.Trade
{
    /// <summary>
    /// 
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

        private CoinOneClient __trade_client = null;

        private CoinOneClient TradeClient
        {
            get
            {
                if (__trade_client == null)
                    __trade_client = new CoinOneClient(__connect_key, __secret_key);
                return __trade_client;
            }
        }

    }
}