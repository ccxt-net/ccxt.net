using Newtonsoft.Json;
using OdinSdk.BaseLib.Configuration;
using System;
using System.Collections.Generic;

namespace CCXT.NET.Poloniex.Private
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPDepositAddress
    {
        /// <summary>
        /// 
        /// </summary>
        bool IsGenerationSuccessful
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        string Address
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PDepositAddress : IPDepositAddress
    {
        [JsonProperty("success")]
        private byte IsGenerationSuccessfulInternal
        {
            set
            {
                IsGenerationSuccessful = value == 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsGenerationSuccessful
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("response")]
        public string Address
        {
            get; private set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IPDepositWithdraw
    {
        /// <summary>
        /// 
        /// </summary>
        IList<PDepositItem> Deposits
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        IList<PWithdrawItem> Withdraws
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PDepositWithdraw : IPDepositWithdraw
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("deposits")]
        public IList<PDepositItem> Deposits
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("withdrawals")]
        public IList<PWithdrawItem> Withdraws
        {
            get; private set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IPDepositItem
    {
        /// <summary>
        /// 
        /// </summary>
        string Currency
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        string Address
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal Amount
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        DateTime Time
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        string TransactionId
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        uint Confirmations
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        string Status
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PDepositItem : IPDepositItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("currency")]
        public string Currency
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("address")]
        public string Address
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("amount")]
        public decimal Amount
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("confirmations")]
        public uint Confirmations
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("txid")]
        public string TransactionId
        {
            get; private set;
        }

        [JsonProperty("timestamp")]
        private ulong TimeInternal
        {
            set
            {
                Time = value.UnixTimeStampToDateTime();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Time
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("status")]
        public string Status
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                return Status.ToUpper() == "COMPLETE";
            }
        }
    }
}