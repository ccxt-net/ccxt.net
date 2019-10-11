using OdinSdk.BaseLib.Coin.Private;
using OdinSdk.BaseLib.Coin.Types;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CCXT.NET.Anxpro.Private
{
    /// <summary>
    ///
    /// </summary>
    public class ATransfer : OdinSdk.BaseLib.Coin.Private.Transfer, ITransfer
    {
        /// <summary>
        ///
        /// </summary>
        public ATransfer()
        {
            this.result = new ATransferItem();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        private string resultValue
        {
            set
            {
                message = value;
                success = message == "success";
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new ATransferItem result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ATransfers : OdinSdk.BaseLib.Coin.Private.Transfers, ITransfers
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "result")]
        private string resultValue
        {
            set
            {
                message = value;
                success = message == "success";
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public ATransferData data
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ATransferData
    {
        /// <summary>
        /// No of records in this wallet
        /// </summary>
        public int records
        {
            get;
            set;
        }

        /// <summary>
        /// Current page number
        /// </summary>
        public int current_page
        {
            get;
            set;
        }

        /// <summary>
        /// Maximum number of page available
        /// </summary>
        public int max_page
        {
            get;
            set;
        }

        /// <summary>
        /// Maximum number of result per page, currently set at 50
        /// </summary>
        public int max_result
        {
            get;
            set;
        }

        /// <summary>
        /// A list of wallet history object
        /// </summary>
        public List<ATransferItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ATransferItem : OdinSdk.BaseLib.Coin.Private.TransferItem, ITransferItem
    {
        /// <summary>
        /// Index no of this wallet history object
        /// </summary>
        public int Index
        {
            get;
            set;
        }

        /// <summary>
        /// Unixtimestamp of this wallet history being created
        /// </summary>
        [JsonProperty(PropertyName = "Date")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// Wallet History Type
        /// </summary>
        /// <para>withdraw = Money being withdrawn (eg to bank account)</para>
        /// <para>deposit = Incoming deposit of currency</para>
        /// <para>in = Traded (crypto) currency gained after a bid order</para>
        /// <para>out = Traded (crypto) currency removed after an ask order</para>
        /// <para>earned = Auxiliary (settlement) currency added after an ask order</para>
        /// <para>spent = Auxiliary (settlement) currency removed after a bid order</para>
        /// <para>fee = Fee deducted from balance for the above transaction type</para>
        [JsonProperty(PropertyName = "Type")]
        private string transactionValue
        {
            set
            {
                transactionType = TransactionTypeConverter.FromString(value);
            }
        }

        /// <summary>
        /// Value of this transaction
        /// </summary>
        public ATransferValue Value
        {
            get;
            set;
        }

        /// <summary>
        /// Balance of the wallet after this transaction complete
        /// </summary>
        public ATransferValue Balance
        {
            get;
            set;
        }

        /// <summary>
        /// A description of this wallet history
        /// </summary>
        public string Info
        {
            get;
            set;
        }

        /// <summary>
        /// The corresponding Trade
        /// </summary>
        public ATransferTrade Trade
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Currency Object refer to a json block of the form
    /// </summary>
    public class ATransferValue
    {
        /// <summary>
        /// The currency code
        /// </summary>
        public string currency
        {
            get;
            set;
        }

        /// <summary>
        /// The value in display format, might contains grouping separator(,) and ends with a currency code
        /// </summary>
        public string display
        {
            get;
            set;
        }

        /// <summary>
        /// The value in display short format, rounding to 2 decimal places
        /// </summary>
        public string display_short
        {
            get;
            set;
        }

        /// <summary>
        /// The value itself, does not contain any formatting text
        /// </summary>
        public decimal value
        {
            get;
            set;
        }

        /// <summary>
        /// The value itself, multiplied by its corresponding multiplier (10^5, 10^8, etc), does not contain any formatting text
        /// </summary>
        /// <para>Cash/Fiat Amount                             = 10^2 => ex) HKD 1234.56 -> 123456</para>
        /// <para>Crypto/Coins Amount                          = 10^8 => ex) BTC 1234.56789 -> 123456789000</para>
        /// <para>Currency pair rate - BTC to FIAT, LTC to BTC = 10^5 => ex) BTCHKD 4,100.31234 -> 410031234</para>
        /// <para>Currency pair rate - all other crypto        = 10^8 => ex) DOGEBTC 0.012345 -> 1234500</para>
        public decimal value_int
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class ATransferTrade
    {
        /// <summary>
        /// Order id of the corresponding trade
        /// </summary>
        public string oid
        {
            get;
            set;
        }

        /// <summary>
        /// (Optional) Not implemented
        /// </summary>
        public string app
        {
            get;
            set;
        }

        /// <summary>
        /// Trade id in UUID format
        /// </summary>
        public string tid
        {
            get;
            set;
        }

        /// <summary>
        /// Amount of the traded currency (always)
        /// </summary>
        public ATransferValue Amount
        {
            get;
            set;
        }

        /// <summary>
        /// Type of the trade, either "market" or "limit"
        /// </summary>
        public string Properties
        {
            get;
            set;
        }
    }
}