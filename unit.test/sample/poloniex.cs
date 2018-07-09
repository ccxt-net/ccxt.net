using CCXT.NET.Poloniex;
using CCXT.NET.Poloniex.Private;
using CCXT.NET.Poloniex.Public;
using CCXT.NET.Poloniex.Trade;
using CCXT.NET.Types;
using OdinSdk.BaseLib.Configuration;
using System;
using System.Linq;

namespace CCXT.NET.Sample
{
    public partial class Poloniex
    {
        /// <summary>
        /// 1. Public API
        /// </summary>
        public static async void XPublicApi(int debug_step = 2)
        {
            var _public_api = new PublicApi();

            if (debug_step == 1)
            {
                var _ticker = await _public_api.GetTicker();
                if (_ticker.Count > 0)
                    Console.WriteLine(_ticker.Count);
            }

            if (debug_step == 2)
            {
                var _orderbook = await _public_api.GetOrderBook(CurrencyPair.Parse("BTC_DASH"), 20);
                if (_orderbook.SellOrders.Count > 0)
                    Console.WriteLine(_orderbook.SellOrders.Count);
            }

            if (debug_step == 3)
            {
                var _trades = await _public_api.GetTradeHistory(CurrencyPair.Parse("BTC_XRP"));
                if (_trades.Count > 0)
                    Console.WriteLine(_trades.Count);
            }

            if (debug_step == 4)
            {
                var _end_time = DateTime.Now;
                var _start_time = _end_time.AddHours(-10);

                var _trades = await _public_api.GetTradeHistory(CurrencyPair.Parse("BTC_XRP"), _start_time, _end_time);
                if (_trades.Count > 0)
                    Console.WriteLine(_trades.Count);
            }

            if (debug_step == 5)
            {
                var _end_time = DateTime.Now;
                var _start_time = _end_time.AddDays(-10);

                var _trades = await _public_api.GetChartData(CurrencyPair.Parse("BTC_XRP"), ChartPeriod.Minutes30, _start_time, _end_time);
                if (_trades.Count > 0)
                    Console.WriteLine(_trades.Count);
            }
        }

        /// <summary>
        /// 2. User API
        /// </summary>
        public static async void XUserApi(int skip_step = 3)
        {
            var _private_api = new PrivateApi("", "");

            if (skip_step == 1)
            {
                var _balances = await _private_api.GetBalances();
                if (_balances.Count > 0)
                    Console.WriteLine(_balances.Where(b => b.Key == "XRP").SingleOrDefault().Value.QuoteAvailable);
            }

            if (skip_step == 2)
            {
                var _addresses = await _private_api.GetDepositAddresses();
                if (_addresses.Count > 0)
                    Console.WriteLine(_addresses.Count);
            }

            if (skip_step == 3)
            {
                var _end_time = DateTime.MaxValue;
                var _start_time = CUnixTime.UnixEpoch;

                var _deposit_withdrawal = await _private_api.GetDepositsAndWithdrawals(_start_time, _end_time);
                if (_deposit_withdrawal.Deposits.Count > 0 || _deposit_withdrawal.Withdrawals.Count > 0)
                    Console.WriteLine(_deposit_withdrawal.Deposits.Count);
            }

            if (skip_step == 4)
            {
                var _address = await _private_api.GenerateNewDepositAddress("LTC");
                if (_address.IsGenerationSuccessful == true)
                    Console.WriteLine(_address.Address);
            }

            if (skip_step == 5)
            {
                var _withdraw = await _private_api.Withdrawal("XRP", 10.15m, "", "");
                if (_withdraw.IsGenerationSuccessful == true)
                    Console.WriteLine(_withdraw.Address);
            }
        }

        /// <summary>
        /// 3. Trade API
        /// </summary>
        public static async void XTradeApi(int skip_step = 1)
        {
            var _trade_api = new TradeApi("", "");

            if (skip_step == 1)
            {
                var _open_orders = await _trade_api.OpenOrders(CurrencyPair.Parse("BTC_XRP"));
                if (_open_orders.Count > 0)
                    Console.WriteLine(_open_orders.Count);
            }

            if (skip_step == 2)
            {
                var _all_open_orders = await _trade_api.AllOpenOrders();
                if (_all_open_orders.Count > 0)
                    Console.WriteLine(_all_open_orders.Count);
            }

            if (skip_step == 3)
            {
                var _end_time = DateTime.Now;
                var _start_time = _end_time.AddDays(-10);

                var _trades = await _trade_api.GetTrades(CurrencyPair.Parse("BTC_XRP"), _start_time, _end_time);
                if (_trades.Count > 0)
                    Console.WriteLine(_trades.Count);
            }

            if (skip_step == 4)
            {
                var _post_order = await _trade_api.PlaceOrder(CurrencyPair.Parse("BTC_XRP"), SideType.Ask, 0.00011000m, 1000.0m);
                Console.WriteLine(_post_order);
            }

            if (skip_step == 5)
            {
                var _delete_order = await _trade_api.DeleteOrder(CurrencyPair.Parse("BTC_XRP"), 60123953979);
                Console.WriteLine(_delete_order);
            }
        }

        public static void Start(int debug_step = 1)
        {
            // 1. Public API
            if (debug_step == 1)
                XPublicApi();

            // 2. Private API
            if (debug_step == 2)
                XUserApi();

            // 3. Trade API
            if (debug_step == 3)
                XTradeApi();
        }
    }
}