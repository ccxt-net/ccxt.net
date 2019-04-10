using OdinSdk.BaseLib.Coin.Public;
using OdinSdk.BaseLib.Coin.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OdinSdk.BaseLib.Coin.Private
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPrivateApi
    {
        /// <summary>
        /// 
        /// </summary>
        XApiClient privateClient
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        PublicApi publicApi
        {
            get;
            set;
        }

        /// <summary>
        /// Create a new deposit address 
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<Address> CreateAddress(string currency_name, Dictionary<string, object> args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<Address> FetchAddress(string currency_name, Dictionary<string, object> args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<Addresses> FetchAddresses(Dictionary<string, object> args);

        /// <summary>
        ///  send currency to defined address
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="address">coin address for send</param>
        /// <param name="tag">Secondary address identifier for coins like XRP,XMR etc.</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<Transfer> CoinWithdraw(string currency_name, string address, string tag, decimal quantity, Dictionary<string, object> args);

        /// <summary>
        /// cancel withdraw(send) with transferId
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="transferId">The unique id of the withdrawal request specified</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<Transfer> CancelCoinWithdraw(string currency_name, string transferId, Dictionary<string, object> args);

        /// <summary>
        /// send fiat
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="bank_name">name of bank</param>
        /// <param name="account"></param>
        /// <param name="amount">amount of fiat</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<Transfer> FiatWithdraw(string currency_name, string bank_name, string account, decimal amount, Dictionary<string, object> args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="transferId">The unique id of the withdrawal request specified</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<Transfer> FetchTransfer(string currency_name, string transferId, Dictionary<string, object> args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limits">You can set the maximum number of transactions you want to get with this parameter</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<Transfers> FetchTransfers(string currency_name, string timeframe, long since, int limits, Dictionary<string, object> args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limits">You can set the maximum number of transactions you want to get with this parameter</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<Transfers> FetchAllTransfers(string timeframe, long since, int limits, Dictionary<string, object> args);

        /// <summary>
        /// Get information about a wallet in your account.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<Wallet> FetchWallet(string base_name, string quote_name, Dictionary<string, object> args = null);

        /// <summary>
        /// Get information about all wallets in your account.
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<Wallets> FetchWallets(string user_id, Dictionary<string, object> args = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<Balance> FetchBalance(string base_name, string quote_name, Dictionary<string, object> args);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        Task<Balances> FetchBalances(Dictionary<string, object> args);
    }

    /// <summary>
    /// 
    /// </summary>
    public class PrivateApi : IPrivateApi
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual XApiClient privateClient
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual PublicApi publicApi
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<Address> CreateAddress(string currency_name, Dictionary<string, object> args = null)
        {
            var _result = new Address();

            var _currency_id = await publicApi.LoadCurrencyId(currency_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new AddressItem();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<Address> FetchAddress(string currency_name, Dictionary<string, object> args = null)
        {
            var _result = new Address();

            var _currency_id = await publicApi.LoadCurrencyId(currency_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new AddressItem();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<Addresses> FetchAddresses(Dictionary<string, object> args = null)
        {
            var _result = new Addresses();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new List<IAddressItem>();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_markets);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="address">coin address for send</param>
        /// <param name="tag">Secondary address identifier for coins like XRP,XMR etc.</param>
        /// <param name="quantity">amount of coin</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<Transfer> CoinWithdraw(string currency_name, string address, string tag, decimal quantity, Dictionary<string, object> args = null)
        {
            var _result = new Transfer();

            var _currency_id = await publicApi.LoadCurrencyId(currency_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new TransferItem();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="transferId">The unique id of the withdrawal request specified</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<Transfer> CancelCoinWithdraw(string currency_name, string transferId, Dictionary<string, object> args = null)
        {
            var _result = new Transfer();

            var _currency_id = await publicApi.LoadCurrencyId(currency_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new TransferItem();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        /// send fiat
        /// </summary>
        /// <param name="currency_name">quote coin name</param>
        /// <param name="bank_name">name of bank</param>
        /// <param name="account"></param>
        /// <param name="amount">amount of fiat</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<Transfer> FiatWithdraw(string currency_name, string bank_name, string account, decimal amount, Dictionary<string, object> args = null)
        {
            var _result = new Transfer();

            var _currency_id = await publicApi.LoadCurrencyId(currency_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new TransferItem();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="transferId">The unique id of the withdrawal request specified</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<Transfer> FetchTransfer(string currency_name, string transferId, Dictionary<string, object> args)
        {
            var _result = new Transfer();

            var _currency_id = await publicApi.LoadCurrencyId(currency_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new TransferItem();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        /// query status of deposits and transfers 
        /// </summary>
        /// <param name="currency_name">base coin or quote coin name</param>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limits">You can set the maximum number of transactions you want to get with this parameter</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<Transfers> FetchTransfers(string currency_name, string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new Transfers();

            var _currency_id = await publicApi.LoadCurrencyId(currency_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                var _timestamp = privateClient.ExchangeInfo.GetTimestamp(timeframe);
                var _timeframe = privateClient.ExchangeInfo.GetTimeframe(timeframe);

                _result.result = new List<ITransferItem>();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeframe">time frame interval (optional): default "1d"</param>
        /// <param name="since">return committed data since given time (milli-seconds) (optional): default 0</param>
        /// <param name="limits">You can set the maximum number of transactions you want to get with this parameter</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<Transfers> FetchAllTransfers(string timeframe = "1d", long since = 0, int limits = 20, Dictionary<string, object> args = null)
        {
            var _result = new Transfers();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                var _timestamp = privateClient.ExchangeInfo.GetTimestamp(timeframe);
                var _timeframe = privateClient.ExchangeInfo.GetTimeframe(timeframe);

                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new List<ITransferItem>();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_markets);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        /// Get information about a wallet in your account.
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<Wallet> FetchWallet(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new Wallet();

            var _currency_id = await publicApi.LoadCurrencyId(base_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new WalletItem();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<Wallets> FetchWallets(string user_id, Dictionary<string, object> args = null)
        {
            var _result = new Wallets();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new List<IWalletItem>();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_markets);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="base_name">The type of trading base-currency of which information you want to query for.</param>
        /// <param name="quote_name">The type of trading quote-currency of which information you want to query for.</param>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<Balance> FetchBalance(string base_name, string quote_name, Dictionary<string, object> args = null)
        {
            var _result = new Balance();

            var _currency_id = await publicApi.LoadCurrencyId(base_name);
            if (_currency_id.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new BalanceItem();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);

                _result.supported = false;
            }
            else
            {
                _result.SetResult(_currency_id);
            }

            return await Task.FromResult(_result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">Add additional attributes for each exchange</param>
        /// <returns></returns>
        public virtual async Task<Balances> FetchBalances(Dictionary<string, object> args = null)
        {
            var _result = new Balances();

            var _markets = await publicApi.LoadMarkets();
            if (_markets.success == true)
            {
                privateClient.ExchangeInfo.ApiCallWait(TradeType.Private);

                _result.result = new List<IBalanceItem>();

                _result.SetFailure("not supported yet", ErrorCode.NotSupported);
                _result.supported = false;
            }
            else
            {
                _result.SetResult(_markets);
            }

            return await Task.FromResult(_result);
        }
    }
}