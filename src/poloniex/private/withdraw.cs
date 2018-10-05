using Newtonsoft.Json;
using OdinSdk.BaseLib.Configuration;
using System;

namespace CCXT.NET.Poloniex.Private
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPWithdrawItem
    {
        /// <summary>
        /// 
        /// </summary>
        ulong Id
        {
            get;
        }

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
        string IpAddress
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
    public class PWithdrawItem : IPWithdrawItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("withdrawalNumber")]
        public ulong Id
        {
            get; private set;
        }

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
        [JsonProperty("fee")]
        public decimal Fee
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
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
                var _completed = false;

                var _states = Status.Split(':');
                if (_states.Length > 0)
                    _completed = _states[0].ToUpper() == "COMPLETE";

                return _completed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TransactionId
        {
            get
            {
                var _result = "";

                if (IsCompleted == true)
                {
                    var _states = Status.Split(' ');
                    _result = _states.Length > 1 ? _states[1].ToLower() : "";
                }

                return _result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("ipAddress")]
        public string IpAddress
        {
            get; private set;
        }
    }
}