using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CCXT.NET.Korbit.Private
{
    /// <summary>
    ///
    /// </summary>
    public class UserBalanceData
    {
        /// <summary>
        /// The amount of funds you can use.
        /// </summary>
        public decimal available;

        /// <summary>
        /// The amount of funds that are being used in trade.
        /// </summary>
        public decimal trade_in_use;

        /// <summary>
        /// The amount of funds that are being processed for withdrawal.
        /// </summary>
        public decimal withdrawal_in_use;
    }

    /// <summary>
    /// GET https://api.korbit.co.kr/v1/user/balances
    /// </summary>
    public class UserBalances
    {
        /// <summary>
        /// 
        /// </summary>
        public UserBalanceData bch;

        /// <summary>
        /// 
        /// </summary>
        public UserBalanceData btc;

        /// <summary>
        /// 
        /// </summary>
        public UserBalanceData dash;

        /// <summary>
        /// 
        /// </summary>
        public UserBalanceData etc;

        /// <summary>
        /// 
        /// </summary>
        public UserBalanceData eth;

        /// <summary>
        /// 
        /// </summary>
        public UserBalanceData ltc;

        /// <summary>
        /// 
        /// </summary>
        public UserBalanceData rep;

        /// <summary>
        /// 
        /// </summary>
        public UserBalanceData steem;

        /// <summary>
        /// 
        /// </summary>
        public UserBalanceData xmr;

        /// <summary>
        /// 
        /// </summary>
        public UserBalanceData xrp;

        /// <summary>
        /// 
        /// </summary>
        public UserBalanceData zec;

        /// <summary>
        /// 
        /// </summary>
        public UserBalanceData krw;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ConcurrentDictionary<string, UserBalanceData> GetBalances()
        {
            var _result = new ConcurrentDictionary<string, UserBalanceData>();

            _result["bch"] = this.bch;
            _result["btc"] = this.btc;
            _result["dash"] = this.dash;

            _result["etc"] = this.etc;
            _result["eth"] = this.eth;
            _result["ltc"] = this.ltc;

            _result["rep"] = this.rep;
            _result["steem"] = this.steem;
            _result["xmr"] = this.xmr;

            _result["xrp"] = this.xrp;
            _result["zec"] = this.zec;
            _result["krw"] = this.krw;

            return _result;
        }

        /// <summary>
        /// 사용 가능 QTY
        /// </summary>
        public decimal available_qty(string coin_name)
        {
            var _result = 0.0m;

            var _available_coin = this.GetBalances().Where(b => b.Key.ToLower() == coin_name.ToLower()).SingleOrDefault().Value;
            if (_available_coin != null)
                _result = _available_coin.available;

            return _result;
        }
    }
}