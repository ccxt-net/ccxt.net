using System.Collections.Generic;
using System.Linq;

namespace CCXT.NET.Korbit.Private
{
    /// <summary>
    /// 
    /// </summary>
    public class CurrencyAddress
    {
        /// <summary>
        /// The name of the bank(Valid only if currency is 'krw’).
        /// </summary>
        public string bank;

        /// <summary>
        /// The virtual account number(Valid only if currency is 'krw’).
        /// </summary>
        public string account;

        /// <summary>
        /// The name of the account owner (Valid only if currency is 'krw’).
        /// </summary>
        public string owner;

        /// <summary>
        /// The BTC address (Valid only if currency is 'btc’).
        /// </summary>
        public string address;
    }


    public class CurrencyInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string currency;

        /// <summary>
        /// Status of the registered bank account.Below are the four states that it can have.
        /// “owner_mismatch” : The bank account is valid but the name on the account is different from the profile name. 
        /// “submitted” : The bank account is being validated. “confirmed” : The bank account has been confirmed.
        /// “invalid_account” : The bank account is invalid.
        /// </summary>
        public string status;

        /// <summary>
        /// The registered name on the bank account. This field exists only when the status is “owner_mismatch”.
        /// </summary>
        public string registeredOwner;

        /// <summary>
        /// 
        /// </summary>
        public CurrencyAddress address;
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserWallet
    {
        /// <summary>
        /// This list has the BTC address or the virtual bank account where you can receive BTC or KRW respectively. 
        /// You can assign the BTC address by using v1/user/coins/address/assign and KRW bank account by using v1/user/fiats/address/assign.
        /// </summary>
        public List<CurrencyInfo> @in;

        /// <summary>
        /// This list has your bank account where you can transfer KRW.
        /// In case you do not have one, you can register one by using v1/user/fiats/address/register.
        /// </summary>
        public List<CurrencyInfo> @out;

        /// <summary>
        /// Total balance of BTC and KRW.
        /// </summary>
        public List<CurrencyValue> balance;

        /// <summary>
        /// The amount of BTC and KRW that is pending withdrawal process.The user can not spend it.
        /// </summary>
        public List<CurrencyValue> pendingOut;

        /// <summary>
        /// The amount of BTC and KRW that is held for asking and bidding orders. The user can not spend it.
        /// </summary>
        public List<CurrencyValue> pendingOrders;

        /// <summary>
        /// The available amount of BTC and KRW for the user. It is calculated by balance - pendingOut - pendingOrders.
        /// </summary>
        public List<CurrencyValue> available;

        /// <summary>
        /// Amount of krw and coin currently unavailable for being on hold in the previous buy/sell orders
        /// </summary>
        public List<CurrencyValue> tradeInUse;

        /// <summary>
        /// Amount of krw and coin currently available for buy/sell orders
        /// </summary>
        public List<CurrencyValue> tradable;

        /// <summary>
        /// 사용 가능 QTY
        /// </summary>
        public decimal available_qty(string coin_name)
        {
            var _result = 0.0m;

            var _available_coin = this.balance.Where(b => b.currency.ToLower() == coin_name.ToLower()).SingleOrDefault();
            if (_available_coin != null)
                _result = _available_coin.value;

            return _result;
        }
    }
}