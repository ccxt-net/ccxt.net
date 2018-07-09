using System.Collections.Generic;

namespace CCXT.NET.Coinone.Private
{
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