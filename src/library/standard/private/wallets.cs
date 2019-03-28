using System.Collections.Generic;

namespace OdinSdk.BaseLib.Coin.Private
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWalletItem
    {
        /// <summary>
        /// 
        /// </summary>
        string userId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        string walletId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        string walletName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        BalanceItem balance
        {
            get;
            set;
        }

#if DEBUG
        /// <summary>
        /// 
        /// </summary>
        string rawJson
        {
            get;
            set;
        }
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    public class WalletItem : IWalletItem
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual string userId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string walletId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string walletName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual BalanceItem balance
        {
            get;
            set;
        }

#if DEBUG
        /// <summary>
        /// 
        /// </summary>
        public virtual string rawJson
        {
            get;
            set;
        }
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IWallet : IApiResult<IWalletItem>
    {
#if DEBUG
        /// <summary>
        /// 
        /// </summary>
        string rawJson
        {
            get;
            set;
        }
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    public class Wallet : ApiResult<IWalletItem>, IWallet
    {
        /// <summary>
        /// 
        /// </summary>
        public Wallet()
        {
            this.result = new WalletItem();
        }

#if DEBUG
        /// <summary>
        /// 
        /// </summary>
        public virtual string rawJson
        {
            get;
            set;
        }
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IWallets : IApiResult<List<IWalletItem>>
    {
#if DEBUG
        /// <summary>
        /// 
        /// </summary>
        string rawJson
        {
            get;
            set;
        }
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    public class Wallets : ApiResult<List<IWalletItem>>, IWallets
    {
        /// <summary>
        /// 
        /// </summary>
        public Wallets()
        {
            this.result = new List<IWalletItem>();
        }

#if DEBUG
        /// <summary>
        /// 
        /// </summary>
        public virtual string rawJson
        {
            get;
            set;
        }
#endif
    }
}