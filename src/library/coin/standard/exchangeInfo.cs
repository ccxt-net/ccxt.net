using CCXT.NET.Coin;
using CCXT.NET.Coin.Public;
using CCXT.NET.Coin.Types;
using CCXT.NET.Configuration;
using CellWars.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#pragma warning disable EF1001

namespace CCXT.NET
{
    /// <summary>
    ///
    /// </summary>
    public class ExchangeInfo
    {
        /// <summary>
        ///
        /// </summary>
        public ExchangeInfo(string dealer_name)
        {
            this.DealerName = dealer_name;
            this.NonceStyle = NonceStyle.UnixMillisecondsString;
        }

        /// <summary>
        ///
        /// </summary>
        private AsyncLock __info_async_lock = new AsyncLock();

        /// <summary>
        ///
        /// </summary>
        public string DealerName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public List<string> Countries
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public ExchangeUrls Urls
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public NonceStyle NonceStyle
        {
            get;
            set;
        }

        private Dictionary<string, string> __currency_ids;

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, string> CurrencyIds
        {
            get
            {
                if (__currency_ids == null)
                {
                    __currency_ids = new Dictionary<string, string>
                    {
                        { "XBT", "BTC" },
                        { "BCC", "BCH" },
                        { "DRK", "DASH" }
                    };
                }

                return __currency_ids;
            }
            set
            {
                this.MergeCurrencyCode(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, string> CurrencyNicks
        {
            get;
            set;
        }

        private Dictionary<string, string> __timeframe;

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, string> Timeframes
        {
            get
            {
                if (__timeframe == null)
                {
                    __timeframe = new Dictionary<string, string>
                    {
                        { "1m", ""},
                        { "3m", ""},
                        { "5m", ""},
                        { "15m", ""},
                        { "30m", ""},
                        { "1h", ""},
                        { "2h", ""},
                        { "4h", ""},
                        { "6h", ""},
                        { "8h", ""},
                        { "12h", ""},
                        { "1d", ""},
                        { "3d", ""},
                        { "1w", ""},
                        { "1M", "" }
                    };
                }

                return __timeframe;
            }
            set
            {
                this.MergeTimeframe(value);

                __timeframe = __timeframe
                                    .Where(t => t.Value != "")
                                    .ToDictionary(t => t.Key, t => t.Value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, decimal> AmountMultiplier
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public RequiredCredentials RequiredCredentials
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public ExchangeLimitRate LimitRate
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public MarketFees Fees
        {
            get;
            set;
        }

        /// <summary>
        /// symbols, market ids and exchanger's information
        /// </summary>
        public Markets Markets
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public ExchangeOptions Options
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public void ApiCallLock(ExchangeLimitCalled limitCalled)
        {
            using (__info_async_lock.Lock())
            {
                var _now_milli = CUnixTime.NowMilli;

                var _wait_milli = limitCalled.called + limitCalled.rate - _now_milli;
                if (_wait_milli > 0)
                {
                    Task.Delay((int)_wait_milli).Wait();      //Thread.Sleep(_wait_milli);
                    _now_milli += _wait_milli;
                }

                limitCalled.called = _now_milli;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void ApiCallWait(TradeType tradeType = TradeType.Total)
        {
            if (LimitRate.useTotal == false)
            {
                if (tradeType == TradeType.Public)
                    this.ApiCallLock(LimitRate.@public);
                else if (tradeType == TradeType.Private)
                    this.ApiCallLock(LimitRate.@private);
                else if (tradeType == TradeType.Trade)
                    this.ApiCallLock(LimitRate.trade);
                else if (tradeType == TradeType.Token)
                    this.ApiCallLock(LimitRate.token);
                else
                    this.ApiCallLock(LimitRate.total);
            }
            else
                this.ApiCallLock(LimitRate.total);

            LimitRate.lastType = tradeType;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <returns></returns>
        public long GetTimestamp(string timeframe)
        {
            var _result = (long)0;

            var _num_alpha_filter = new Regex("(?<Numeric>[0-9]*)(?<Alpha>[a-zA-Z]*)");
            {
                var _match_time = _num_alpha_filter.Match(timeframe);

                var _nummeric = _match_time.Groups["Numeric"].Value;
                var _alpha = _match_time.Groups["Alpha"].Value;

                _result = (_alpha == "m") ? 60
                                : (_alpha == "h") ? 60 * 60
                                : (_alpha == "d") ? 60 * 60 * 24
                                : (_alpha == "w") ? 60 * 60 * 24 * 7
                                : (_alpha == "M") ? 60 * 60 * 24 * 30
                                : (_alpha == "y") ? 60 * 60 * 24 * 365
                                : 0;

                _result *= Convert.ToInt64(_nummeric);
            }

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currency_ids"></param>
        /// <returns></returns>
        public Dictionary<string, string> MergeCurrencyCode(Dictionary<string, string> currency_ids)
        {
            foreach (var _currency_id in currency_ids)
            {
                var _key = _currency_id.Key.ToUpper();
                var _value = _currency_id.Value.ToUpper();

                if (this.CurrencyIds.ContainsKey(_key) == true)
                    this.CurrencyIds[_key] = _value;
                else
                    this.CurrencyIds.Add(_key, _value);
            }

            return this.CurrencyIds;
        }

        /// <summary>
        /// read only
        /// </summary>
        /// <param name="currency_id">base or quote id which is used on exchange's server</param>
        /// <returns></returns>
        public string GetCommonCurrencyName(string currency_id)
        {
            var _result = currency_id.ToUpper();

            foreach (var _id in this.CurrencyIds)
            {
                if (_id.Key == _result)
                {
                    _result = _id.Value;
                    break;
                }
            }

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currency_name">base or quote name which is used on exchange's server</param>
        /// <returns></returns>
        public string GetCommonCurrencyId(string currency_name)
        {
            var _result = currency_name.ToUpper();

            foreach (var _id in this.CurrencyIds)
            {
                if (_id.Value == _result)
                {
                    _result = _id.Key;
                    break;
                }
            }

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="timeframes"></param>
        /// <returns></returns>
        public Dictionary<string, string> MergeTimeframe(Dictionary<string, string> timeframes)
        {
            foreach (var _timeframe in timeframes)
            {
                if (this.Timeframes.ContainsKey(_timeframe.Key) == true)
                    this.Timeframes[_timeframe.Key] = _timeframe.Value;
                else
                    this.Timeframes.Add(_timeframe.Key, _timeframe.Value);
            }

            return this.Timeframes;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <returns></returns>
        public string GetTimeframe(string timeframe)
        {
            return (Timeframes.ContainsKey(timeframe) == true)
                                ? ((String.IsNullOrEmpty(Timeframes[timeframe]) == false) ? Timeframes[timeframe] : timeframe)
                                : timeframe;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="division"></param>
        /// <returns></returns>
        public string GetApiUrl(string division)
        {
            var _result = this.Urls.api.FirstOrDefault().Value;

            if (this.Urls.api.ContainsKey(division) == true)
                _result = this.Urls.api[division];

            return _result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public decimal GetAmountMultiplier(string currency_name, decimal value)
        {
            var _result = value;

            if (this.AmountMultiplier.ContainsKey(currency_name) == true)
                _result = this.AmountMultiplier[currency_name];

            return _result;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ExchangeOptions
    {
        /// <summary>
        ///
        /// </summary>
        public string[] fiats
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, bool> futures
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool hasAlreadyAuthenticatedSuccessfully
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ExchangeLimitCalled
    {
        /// <summary>
        ///
        /// </summary>
        public long rate
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public long called
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ExchangeLimitRate
    {
        /// <summary>
        /// If true then calculate the total number of calls.
        /// </summary>
        public bool useTotal
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public TradeType lastType
        {
            get;
            set;
        }

        /// <summary>
        /// token refresh interval by milliseconds
        /// </summary>
        public ExchangeLimitCalled token
        {
            get;
            set;
        }

        /// <summary>
        /// public api call interval by milliseconds
        /// </summary>
        public ExchangeLimitCalled @public
        {
            get;
            set;
        }

        /// <summary>
        /// private api call interval by milliseconds
        /// </summary>
        public ExchangeLimitCalled @private
        {
            get;
            set;
        }

        /// <summary>
        /// private api call interval by milliseconds
        /// </summary>
        public ExchangeLimitCalled trade
        {
            get;
            set;
        }

        /// <summary>
        /// All other calls combined, are limited
        /// </summary>
        public ExchangeLimitCalled total
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ExchangeUrls
    {
        /// <summary>
        ///
        /// </summary>
        public string logo
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string www
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public List<string> fees
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool testnet
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, string> api
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public List<string> doc
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class RequiredCredentials
    {
        /// <summary>
        ///
        /// </summary>
        public bool apikey
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool secret
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool uid
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool login
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool password
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool twofa
        {
            get;
            set;
        }
    }
}