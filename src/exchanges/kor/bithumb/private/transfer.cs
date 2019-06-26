using Newtonsoft.Json;
using CCXT.NET.Coin;
using CCXT.NET.Coin.Private;
using CCXT.NET.Coin.Types;
using System;
using System.Collections.Generic;

namespace CCXT.NET.Bithumb.Private
{
    /// <summary>
    ///
    /// </summary>
    public class BTransfers : CCXT.NET.Coin.Private.Transfers, ITransfers
    {
        /// <summary>
        ///
        /// </summary>
        public BTransfers()
        {
            this.result = new List<BTransferItem>();
        }

        /// <summary>
        /// 결과 상태 코드 (정상 : 0000, 정상이외 코드는 에러 코드 참조)
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public override int statusCode
        {
            get => base.statusCode;
            set
            {
                base.statusCode = value;

                if (statusCode == 0)
                {
                    message = "success";
                    errorCode = ErrorCode.Success;
                    success = true;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new List<BTransferItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BTransfer : CCXT.NET.Coin.Private.Transfer, ITransfer
    {
        /// <summary>
        /// 결과 상태 코드 (정상 : 0000, 정상이외 코드는 에러 코드 참조)
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public override int statusCode
        {
            get => base.statusCode;
            set
            {
                base.statusCode = value;

                if (statusCode == 0)
                {
                    message = "success";
                    errorCode = ErrorCode.Success;
                    success = true;
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BTransferItem : CCXT.NET.Coin.Private.TransferItem, ITransferItem
    {
        /// <summary>
        /// 거래수수료
        /// </summary>
        [JsonProperty(PropertyName = "originFee")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// 0 : 전체, 1 : 구매완료, 2 : 판매완료, 3 : 출금중, 4 : 입금, 5 : 출금, 9 : KRW입금중
        /// </summary>
        [JsonProperty(PropertyName = "originSearch")]
        public int search
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "search")]
        private int searchValue
        {
            set
            {
                search = value;

                transactionType = (search == 3) ? TransactionType.Withdrawing
                                : (search == 4) ? TransactionType.Withdraw
                                : (search == 5) ? TransactionType.Deposit
                                : (search == 9) ? TransactionType.Depositing
                                : TransactionType.Unknown;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "transfer_date")]
        private long transfer_date
        {
            set
            {
                timestamp = value / 1000;
            }
        }

        /// <summary>
        /// 거래 Currency 수량
        /// </summary>
        [JsonProperty(PropertyName = "units")]
        private string amountValue
        {
            set
            {
                amount = Math.Abs(Convert.ToDecimal(value.Replace(" ", "")));
            }
        }

        /// <summary>
        /// 거래수수료
        /// </summary>
        [JsonProperty(PropertyName = "fee")]
        private string feeValue
        {
            set
            {
                var _fee = value.Split(' ');
                if (_fee.Length > 0)
                    fee = Convert.ToDecimal(_fee[0]);
            }
        }
    }
}