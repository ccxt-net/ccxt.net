using System.Collections.Concurrent;
using System.Linq;

namespace CCXT.NET.Korbit.Private
{
    /// <summary>
    ///
    /// </summary>
    public class KBalanceItem
    {
        /// <summary>
        /// The amount of funds you can use.
        /// </summary>
        public decimal available
        {
            get;
            set;
        }

        /// <summary>
        /// The amount of funds that are being used in trade.
        /// </summary>
        public decimal trade_in_use
        {
            get;
            set;
        }

        /// <summary>
        /// The amount of funds that are being processed for withdrawal.
        /// </summary>
        public decimal withdrawal_in_use
        {
            get;
            set;
        }
    }

    /// <summary>
    /// GET https://api.korbit.co.kr/v1/user/balances
    /// </summary>
    public class KBalances
    {
        /// <summary>
        /// 
        /// </summary>
        public KBalanceItem bch
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public KBalanceItem btc
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public KBalanceItem dash
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public KBalanceItem etc
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public KBalanceItem eth
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public KBalanceItem ltc
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public KBalanceItem rep
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public KBalanceItem steem
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public KBalanceItem xmr
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public KBalanceItem xrp
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public KBalanceItem zec
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public KBalanceItem krw
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ConcurrentDictionary<string, KBalanceItem> GetBalances()
        {
            var _result = new ConcurrentDictionary<string, KBalanceItem>();

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