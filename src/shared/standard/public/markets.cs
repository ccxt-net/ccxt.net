using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CCXT.NET.Shared.Coin.Public
{
    /// <summary>
    ///
    /// </summary>
    public class MarketPrecision
    {
        /// <summary>
        /// amount (수량)
        /// </summary>
        public decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// price (단가)
        /// </summary>
        public decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// cost (금액)
        /// </summary>
        public decimal amount
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class MarketMinMax
    {
        /// <summary>
        /// should be great min
        /// </summary>
        public decimal min
        {
            get;
            set;
        }

        /// <summary>
        /// should be less max
        /// </summary>
        public decimal max
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class MarketLimits
    {
        /// <summary>
        /// order amount (수량)
        /// </summary>
        public MarketMinMax quantity
        {
            get;
            set;
        }

        /// <summary>
        /// min/max limits for the price of the order (단가)
        /// </summary>
        public MarketMinMax price
        {
            get;
            set;
        }

        /// <summary>
        /// limits for order cost = price * amount (금액)
        /// </summary>
        public MarketMinMax amount
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IMarketItem
    {
        /// <summary>
        /// string literal for referencing within an exchange (ex) 'BTC/USD'
        /// </summary>
        string marketId
        {
            get;
            set;
        }

        /// <summary>
        /// uppercase string literal of a pair of currencies (ex) 'btcusd'
        /// </summary>
        string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// uppercase string, base currency, 3 or more letters (ex) 'BTC'
        /// </summary>
        string baseName
        {
            get;
            set;
        }

        /// <summary>
        /// uppercase string, quote currency, 3 or more letters (ex) 'USD'
        /// </summary>
        string quoteName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string baseId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string quoteId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string baseLongName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string quoteLongName
        {
            get;
            set;
        }

        /// <summary>
        /// order amount should be evenly divisible by lot unit size of
        /// </summary>
        decimal lot
        {
            get;
            set;
        }

        /// <summary>
        /// boolean, market status
        /// </summary>
        bool active
        {
            get;
            set;
        }

        /// <summary>
        /// number of decimal digits "after the dot"
        /// </summary>
        MarketPrecision precision
        {
            get;
            set;
        }

        /// <summary>
        /// value limits when placing orders on this market
        /// </summary>
        MarketLimits limits
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class MarketItem : IMarketItem
    {
        /// <summary>
        ///
        /// </summary>
        public MarketItem()
        {
            depositEnabled = true;
            withdrawEnabled = true;
        }

        /// <summary>
        /// string literal for referencing within an exchange (ex) 'BTC/USD'
        /// </summary>
        public virtual string marketId
        {
            get;
            set;
        }

        /// <summary>
        /// uppercase string literal of a pair of currencies (ex) 'btcusd'
        /// </summary>
        public virtual string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// uppercase string, base currency, 3 or more letters (ex) 'BTC'
        /// </summary>
        public virtual string baseName
        {
            get;
            set;
        }

        /// <summary>
        /// uppercase string, quote currency, 3 or more letters (ex) 'USD'
        /// </summary>
        public virtual string quoteName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual string baseId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual string quoteId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual string baseLongName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual string quoteLongName
        {
            get;
            set;
        }

        /// <summary>
        /// order amount should be evenly divisible by lot unit size of
        /// </summary>
        public virtual decimal lot
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual decimal makerFee
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual decimal takerFee
        {
            get;
            set;
        }

        /// <summary>
        /// boolean, market status
        /// </summary>
        public virtual bool active
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool depositEnabled
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool withdrawEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// number of decimal digits "after the dot"
        /// </summary>
        public virtual MarketPrecision precision
        {
            get;
            set;
        }

        /// <summary>
        /// value limits when placing orders on this market
        /// </summary>
        public virtual MarketLimits limits
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IMarket : IApiResult<IMarketItem>
    {
    }

    /// <summary>
    ///
    /// </summary>
    public class Market : ApiResult<IMarketItem>, IMarket
    {
        /// <summary>
        ///
        /// </summary>
        public Market(string marketId)
        {
            this.result = new MarketItem
            {
                marketId = marketId
            };
        }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IMarkets : IApiResult<Dictionary<string, IMarketItem>>
    {
#if RAWJSON

        /// <summary>
        ///
        /// </summary>
        string rawJson
        {
            get;
            set;
        }

#endif
    }

    /// <summary>
    ///
    /// </summary>
    public class Markets : ApiResult<Dictionary<string, IMarketItem>>, IMarkets
    {
        /// <summary>
        ///
        /// </summary>
        public Markets(bool success = false)
            : base(success)
        {
            this.result = new Dictionary<string, IMarketItem>();
        }

#if RAWJSON

        /// <summary>
        ///
        /// </summary>
        [JsonIgnore]
        public virtual string rawJson
        {
            get;
            set;
        }

#endif

        private Dictionary<string, string> __coin_names = null;

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, string> CoinNames
        {
            get
            {
                if (__coin_names == null)
                {
                    __coin_names = this.result
                        .Select(m => new
                        {
                            id = m.Value.baseId,
                            name = m.Value.baseName
                        })
                        .GroupBy(m => new
                        {
                            m.id,
                            m.name
                        })
                        .OrderBy(m => m.Key.id)
                        .ToDictionary(m => m.Key.id ?? "", m => m.Key.name ?? "");
                }

                return __coin_names;
            }
        }

        private Dictionary<string, string> __currency_names = null;

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, string> CurrencyNames
        {
            get
            {
                if (__currency_names == null)
                {
                    __currency_names = this.result
                        .Select(m => new
                        {
                            id = m.Value.baseId,
                            name = m.Value.baseName
                        })
                        .Concat
                        (
                            this.result
                                .Select(m => new
                                {
                                    id = m.Value.quoteId,
                                    name = m.Value.quoteName
                                })
                        )
                        .GroupBy(m => new
                        {
                            m.id,
                            m.name
                        })
                        .OrderBy(m => m.Key.id)
                        .ToDictionary(m => m.Key.id ?? "", m => m.Key.name ?? "");
                }

                return __currency_names;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coin_id">base id which is used on exchange's server</param>
        /// <returns></returns>
        public string GetCoinName(string coin_id)
        {
            return this.CoinNames.ContainsKey(coin_id)
                        ? this.CurrencyNames.Where(c => c.Key == coin_id).FirstOrDefault().Value
                        : coin_id;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coin_name">base coin name</param>
        /// <returns></returns>
        public string GetCoinId(string coin_name)
        {
            return this.CoinNames.ContainsValue(coin_name)
                        ? this.CurrencyNames.Where(c => c.Value == coin_name).FirstOrDefault().Key
                        : coin_name;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currency_id">base or quote id which is used on exchange's server</param>
        /// <returns></returns>
        public string GetCurrencyName(string currency_id)
        {
            return this.CurrencyNames.ContainsKey(currency_id)
                        ? this.CurrencyNames.Where(c => c.Key == currency_id).FirstOrDefault().Value
                        : currency_id;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <returns></returns>
        public string GetCurrencyId(string currency_name)
        {
            return this.CurrencyNames.ContainsValue(currency_name)
                        ? this.CurrencyNames.Where(c => c.Value == currency_name).FirstOrDefault().Key
                        : currency_name;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="market_id"></param>
        /// <returns></returns>
        public MarketItem GetMarketByMarketId(string market_id)
        {
            var _result = this.result
                              .Where(m => m.Value.marketId == market_id)
                              .SingleOrDefault();

            return _result.Value as MarketItem;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public MarketItem GetMarketBySymbol(string symbol)
        {
            var _result = this.result
                              .Where(m => m.Value.symbol == symbol)
                              .SingleOrDefault();

            return _result.Value as MarketItem;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_id"></param>
        /// <returns></returns>
        public MarketItem GetMarketByBaseId(string base_id)
        {
            var _result = this.result
                              .Where(m => m.Value.baseId == base_id)
                              .FirstOrDefault();

            return _result.Value as MarketItem;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <returns></returns>
        public MarketItem GetMarketByBaseName(string base_name)
        {
            var _result = this.result
                              .Where(m => m.Value.baseId == base_name)
                              .FirstOrDefault();

            return _result.Value as MarketItem;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="quote_id"></param>
        /// <returns></returns>
        public MarketItem GetMarketByQuoteId(string quote_id)
        {
            var _result = this.result
                              .Where(m => m.Value.quoteId == quote_id)
                              .FirstOrDefault();

            return _result.Value as MarketItem;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="quote_name"></param>
        /// <returns></returns>
        public MarketItem GetMarketByQuoteName(string quote_name)
        {
            var _result = this.result
                              .Where(m => m.Value.quoteName == quote_name)
                              .FirstOrDefault();

            return _result.Value as MarketItem;
        }
    }
}