using System.Collections.Generic;
using OdinSdk.BaseLib.Coin;

namespace CCXT.NET.Bithumb.Private
{
    /// <summary>
    /// 회원 거래 내역
    /// </summary>
    public class BTransferItem
    {
        /// <summary>
        /// 검색 구분 (0 : 전체, 1 : 구매완료, 2 : 판매완료, 3 : 출금중, 4 : 입금, 5 : 출금, 9 : KRW입금중)
        /// </summary>
        public int search
        {
            get;
            set;
        }

        /// <summary>
        /// 거래 일시 Timestamp
        /// </summary>
        public long transfer_date
        {
            get;
            set;
        }

        /// <summary>
        /// 거래 Currency 수량 (BTC, ETH, DASH, LTC, ETC, XRP)
        /// </summary>
        public string units
        {
            get;
            set;
        }

        /// <summary>
        /// 거래금액
        /// </summary>
        public decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// 1Currency당 거래금액 (btc, eth, dash, ltc, etc, xrp, bch, xmr, zec, qtum, btg, eos, icx)
        /// </summary>
        public decimal btc1krw
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal eth1krw
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal dash1krw
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal ltc1krw
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal etc1krw
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal xrp1krw
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal bch1krw
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal xmr1krw
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal zec1krw
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal qtum1krw
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal btg1krw
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal eos1krw
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal icx1krw
        {
            get;
            set;
        }

        /// <summary>
        /// 거래수수료
        /// </summary>
        public string fee
        {
            get;
            set;
        }

        /// <summary>
        /// 거래 후 Currency 잔액 (btc, eth, dash, ltc, etc, xrp, bch, xmr, zec, qtum, btg, eos, icx)
        /// </summary>
        public decimal btc_remain
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal eth_remain
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal dash_remain
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal ltc_remain
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal etc_remain
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal xrp_remain
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal bch_remain
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal xmr_remain
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal zec_remain
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal qtum_remain
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal btg_remain
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal eos_remain
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal icx_remain
        {
            get;
            set;
        }

        /// <summary>
        /// 거래 후 KRW 잔액
        /// </summary>
        public decimal krw_remain
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 회원 거래 내역
    /// </summary>
    public class BTransfers : ApiResult<List<BTransferItem>>
    {
        /// <summary>
        /// 
        /// </summary>
        public BTransfers()
        {
            this.result = new List<BTransferItem>();
        }
    }
}