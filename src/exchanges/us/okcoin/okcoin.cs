using CCXT.NET.Shared.Coin;
using CCXT.NET.Shared.Extension;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCXT.NET.OKCoin
{
    /// <summary>
    /// Okcoin exchange
    /// </summary>
    public sealed class OkcoinClient : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        /// Okcoin exchange
        /// </summary>
        public override string DealerName { get; set; } = "Okcoin";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public OkcoinClient(string division)
            : base(division)
        {
        }

        /// <summary>
        /// Constructor with API keys
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public OkcoinClient(string division, string connect_key, string secret_key)
            : base(division, connect_key, secret_key, authentication: true)
        {
        }

        /// <summary>
        /// information of exchange for trading
        /// </summary>
        public override ExchangeInfo ExchangeInfo
        {
            get
            {
                if (base.ExchangeInfo == null)
                {
                    base.ExchangeInfo = new ExchangeInfo(this.DealerName)
                    {
                        Countries = new List<string>
                        {
                            "US"
                        },
                        Urls = new ExchangeUrls
                        {
                            logo = "",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://api.okcoin.com" },
                                { "private", "https://api.okcoin.com" },
                                { "trade", "https://api.okcoin.com" }
                            },
                            www = "https://www.okcoin.com",
                            doc = new List<string>
                            {
                                "https://docs.okcoin.com"
                            }
                        },
                        RequiredCredentials = new RequiredCredentials
                        {
                            apikey = true,
                            secret = true,
                            uid = false,
                            login = false,
                            password = false,
                            twofa = false
                        },
                        LimitRate = new ExchangeLimitRate
                        {
                            useTotal = false,
                            token = new ExchangeLimitCalled { rate = 60000 },
                            @public = new ExchangeLimitCalled { rate = 1000 },
                            @private = new ExchangeLimitCalled { rate = 1000 },
                            trade = new ExchangeLimitCalled { rate = 1000 },
                            total = new ExchangeLimitCalled { rate = 1000 }
                        },
                        Fees = new MarketFees
                        {
                            trading = new MarketFee
                            {
                                tierBased = false,
                                percentage = true,
                                maker = 0.1m / 100,
                                taker = 0.1m / 100
                            }
                        },
                        Timeframes = new Dictionary<string, string>
                        {
                            { "1m", "1min" },
                            { "5m", "5min" },
                            { "15m", "15min" },
                            { "30m", "30min" },
                            { "1h", "hour" },
                            { "1d", "day" },
                            { "1w", "week" }
                        }
                    };
                }

                return base.ExchangeInfo;
            }
        }
    }
}

