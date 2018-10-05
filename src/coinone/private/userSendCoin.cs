using System.Collections.Generic;

namespace CCXT.NET.Coinone.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class UserSendCoin : CApiResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string txid
        {
            get;
            set;
        }
    }
}