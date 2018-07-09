using System.Collections.Generic;

namespace CCXT.NET.Coinone.Private
{
    public class BalanceData
    {
        /// <summary>
        /// Available
        /// </summary>
        public decimal avail
        {
            get;
            set;
        }

        /// <summary>
        /// Total 
        /// </summary>
        public decimal balance
        {
            get;
            set;
        }
    }

    public class WalletData
    {
        /// <summary>
        /// Total BTC. 
        /// </summary>
        public decimal balance
        {
            get;
            set;
        }

        /// <summary>
        /// Normal Wallet Label.
        /// </summary>
        public string label
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserBalance : CApiResult
    {
        /// <summary>
        /// BTC information.
        /// </summary>
        public BalanceData btc
        {
            get;
            set;
        }

        /// <summary>
        /// ETH information.
        /// </summary>
        public BalanceData eth
        {
            get;
            set;
        }

        /// <summary>
        /// ETC information.
        /// </summary>
        public BalanceData etc
        {
            get;
            set;
        }

        /// <summary>
        /// KRW information.
        /// </summary>
        public BalanceData krw
        {
            get;
            set;
        }

        /// <summary>
        /// BTC normal wallet information.
        /// </summary>
        public List<WalletData> normalWallets
        {
            get;
            set;
        }

        /// <summary>
        /// 사용 가능 QTY
        /// </summary>
        public decimal available_qty(string coin_name)
        {
            var _result = 0.0m;

            switch (coin_name.ToUpper())
            {
                case "BTC":
                    if (this.btc != null)
                        _result = this.btc.avail;
                    break;
                case "ETH":
                    if (this.eth != null)
                        _result = this.eth.avail;
                    break;
                case "ETC":
                    if (this.etc != null)
                        _result = this.etc.avail;
                    break;
                case "KRW":
                    if (this.krw != null)
                        _result = this.krw.avail;
                    break;
            }

            return _result;
        }
    }

    public class DailyBalanceData
    {
        /// <summary>
        /// Timestamp
        /// </summary>
        public long timestamp;

        /// <summary>
        /// Overall balance's value in KRW.
        /// </summary>
        public decimal value
        {
            get;
            set;
        }

        /// <summary>
        /// KRW balance. 
        /// </summary>
        public decimal krw
        {
            get;
            set;
        }

        /// <summary>
        /// BTC balance. 
        /// </summary>
        public decimal btc
        {
            get;
            set;
        }

        /// <summary>
        /// ETH balance. 
        /// </summary>
        public decimal eth
        {
            get;
            set;
        }

        /// <summary>
        /// ETC balance. 
        /// </summary>
        public decimal etc
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserDailyBalance : CApiResult
    {
        /// <summary>
        /// Daily balance's information.
        /// </summary>
        public List<DailyBalanceData> dailyBalance
        {
            get;
            set;
        }

    }
}