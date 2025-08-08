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

namespace CCXT.NET.EU.yobit
{
    /// <summary>
    /// Yobit exchange
    /// </summary>
    public sealed class YobitClient : CCXT.NET.Shared.Coin.XApiClient, IXApiClient
    {
        /// <summary>
        /// Yobit exchange
        /// </summary>
        public override string DealerName { get; set; } = "Yobit";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        public YobitClient(string division)
            : base(division)
        {
        }

        /// <summary>
        /// Constructor with API keys
        /// </summary>
        /// <param name="division">exchange's division for communication</param>
        /// <param name="connect_key">exchange's api key for connect</param>
        /// <param name="secret_key">exchange's secret key for signature</param>
        public YobitClient(string division, string connect_key, string secret_key)
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
                            "EU"
                        },
                        Urls = new ExchangeUrls
                        {
                            logo = "",
                            api = new Dictionary<string, string>
                            {
                                { "public", "https://api.yobit.com" },
                                { "private", "https://api.yobit.com" },
                                { "trade", "https://api.yobit.com" }
                            },
                            www = "https://www.yobit.com",
                            doc = new List<string>
                            {
                                "https://docs.yobit.com"
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

