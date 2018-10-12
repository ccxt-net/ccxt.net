using OdinSdk.BaseLib.Coin;
using OdinSdk.BaseLib.Converter;
using System;
using System.Collections.Generic;
using Xunit;

namespace XUnit
{
    public partial class PrivateApi
    {
        [Fact]
        public async void Korbit()
        {
            var _api_key = TestConfig.GetConnectionKey("Korbit");
            var _args = new Dictionary<string, object>();
            var _timeframe = "1d";
            var _since = 0; //1514764800000;
            var _limit = 100;

            if (String.IsNullOrEmpty(_api_key.secret_key) == false || XApiClient.TestXUnitMode == XUnitMode.UseJsonFile)
            {
                var _private_api = new CCXT.NET.Korbit.Private.PrivateApi(_api_key.connect_key, _api_key.secret_key, _api_key.user_name, _api_key.password);

#if DEBUG
                if (XApiClient.TestXUnitMode != XUnitMode.UseExchangeServer)
                    await _private_api.publicApi.LoadMarkets(false, GetJsonContent(_private_api.privateClient, tRootFolder.Replace(@"\private", @"\public"), "fetchMarkets", _args));
#endif

                var _new_address = await _private_api.CreateAddress("XRP", GetJsonContent(_private_api.privateClient, "createAddress", _args));
                if ((_new_address.supported == true || TestConfig.SupportedCheck == true) && _new_address.message.IndexOf("assign address not allowed") < 0)
                {
                    this.WriteJson(_private_api.privateClient, _new_address);

                    Assert.NotNull(_new_address);
                    Assert.True(_new_address.success, $"create an address return error: {_new_address.message}");
                    Assert.True(_new_address.message == "success");
                    Assert.False(String.IsNullOrEmpty(_new_address.result.currency));
                    Assert.False(String.IsNullOrEmpty(_new_address.result.address));
                }

                var _address = await _private_api.FetchAddress("XRP", GetJsonContent(_private_api.privateClient, "fetchAddress", _args));
                if (_address.supported == true || TestConfig.SupportedCheck == true)
                {
                    this.WriteJson(_private_api.privateClient, _address);

                    Assert.NotNull(_address);
                    Assert.True(_address.success, $"fetch an address return error: {_address.message}");
                    Assert.True(_address.message == "success");

                    Assert.False(String.IsNullOrEmpty(_address.result.currency));
                    Assert.False(String.IsNullOrEmpty(_address.result.address));
                }

                var _addresses = await _private_api.FetchAddresses(GetJsonContent(_private_api.privateClient, "fetchAddresses", _args));
                if (_addresses.supported == true || TestConfig.SupportedCheck == true)
                {
                    this.WriteJson(_private_api.privateClient, _addresses);

                    Assert.NotNull(_addresses);
                    Assert.True(_addresses.success, $"fetch addresses return error: {_addresses.message}");
                    Assert.True(_addresses.message == "success");
                    Assert.True(_addresses.result.Count >= 0);

                    foreach (var _d in _addresses.result)
                    {
                        Assert.False(String.IsNullOrEmpty(_d.currency));
                        Assert.False(String.IsNullOrEmpty(_d.address));
                    }
                }
#if DEBUG
                if (XApiClient.TestXUnitMode != XUnitMode.UseExchangeServer)
                {
                    var _withdraw = await _private_api.CoinWithdraw("XRP", "rN9qNpgnBaZwqCg8CvUZRPqCcPPY7wfWep", "3162679978", 10.0m, GetJsonContent(_private_api.privateClient, "coinWithdraw", _args));
                    if ((_withdraw.supported == true || TestConfig.SupportedCheck == true) && _withdraw.statusCode != 400)
                    {
                        this.WriteJson(_private_api.privateClient, _withdraw);

                        Assert.NotNull(_withdraw);
                        Assert.True(_withdraw.success, $"withdraw return error: {_withdraw.message}");
                        Assert.True(_withdraw.message == "success");
                        Assert.False(String.IsNullOrEmpty(_withdraw.result.transferId));
                    }

                    if (String.IsNullOrEmpty(_withdraw.result.transferId) == true)
                        _withdraw.result.transferId = _private_api.privateClient.GenerateNonceString(13);

                    var _cancel_withdraw = await _private_api.CancelCoinWithdraw("XRP", _withdraw.result.transferId, GetJsonContent(_private_api.privateClient, "cancelCoinWithdraw", _args));
                    if ((_cancel_withdraw.supported == true || TestConfig.SupportedCheck == true) && _cancel_withdraw.message.IndexOf("not_found") < 0)
                    {
                        this.WriteJson(_private_api.privateClient, _cancel_withdraw);

                        Assert.NotNull(_cancel_withdraw);
                        Assert.True(_cancel_withdraw.success, $"cancel withdraw return error: {_cancel_withdraw.message}");
                        Assert.True(_cancel_withdraw.message == "success");
                        Assert.False(String.IsNullOrEmpty(_cancel_withdraw.result.transferId));
                    }
                }
#endif
                var _fetch_transfers = await _private_api.FetchTransfers("XRP", _timeframe, _since, _limit, GetJsonContent(_private_api.privateClient, "fetchTransfers", _args));
                if (_fetch_transfers.supported == true || TestConfig.SupportedCheck == true)
                {
                    this.WriteJson(_private_api.privateClient, _fetch_transfers);

                    Assert.NotNull(_fetch_transfers);
                    Assert.True(_fetch_transfers.success, $"fetch transfers return error: {_fetch_transfers.message}");
                    Assert.True(_fetch_transfers.message == "success");

                    foreach (var _d in _fetch_transfers.result)
                    {
                        Assert.False(String.IsNullOrEmpty(_d.transferId));
                        Assert.False(String.IsNullOrEmpty(_d.transactionId));
                        Assert.False(String.IsNullOrEmpty(_d.fromAddress) && String.IsNullOrEmpty(_d.toAddress));
                        Assert.True(_d.timestamp >= 1000000000000 && _d.timestamp <= 9999999999999);
                    }
                }

                var _transfer_id = "";
                if (_fetch_transfers.result.Count > 0)
                    _transfer_id = _fetch_transfers.result[0].transferId;
                else
                    _transfer_id = Guid.NewGuid().ToString();

                var _fetch_transfer = await _private_api.FetchTransfer("XRP", _transfer_id, GetJsonContent(_private_api.privateClient, "fetchTransfer", _args));
                if (_fetch_transfer.supported == true || TestConfig.SupportedCheck == true)
                {
                    this.WriteJson(_private_api.privateClient, _fetch_transfer);

                    Assert.NotNull(_fetch_transfer);
                    Assert.True(_fetch_transfer.success, $"fetch transfer return error: {_fetch_transfer.message}");
                    Assert.True(_fetch_transfer.message == "success");

                    Assert.False(String.IsNullOrEmpty(_fetch_transfer.result.transferId));
                    Assert.False(String.IsNullOrEmpty(_fetch_transfer.result.transactionId));
                    Assert.False(String.IsNullOrEmpty(_fetch_transfer.result.fromAddress) && String.IsNullOrEmpty(_fetch_transfer.result.toAddress));
                    Assert.True(_fetch_transfer.result.timestamp >= 1000000000000 && _fetch_transfer.result.timestamp <= 9999999999999);
                }

                var _fetch_all_transfers = await _private_api.FetchAllTransfers(_timeframe, _since, _limit, GetJsonContent(_private_api.privateClient, "fetchAllTransfers", _args));
                if (_fetch_all_transfers.supported == true || TestConfig.SupportedCheck == true)
                {
                    this.WriteJson(_private_api.privateClient, _fetch_all_transfers);

                    Assert.NotNull(_fetch_all_transfers);
                    Assert.True(_fetch_all_transfers.success, $"fetch all transfers return error: {_fetch_all_transfers.message}");
                    Assert.True(_fetch_all_transfers.message == "success");

                    foreach (var _d in _fetch_all_transfers.result)
                    {
                        Assert.False(String.IsNullOrEmpty(_d.transferId));
                        Assert.False(String.IsNullOrEmpty(_d.transactionId));
                        Assert.False(String.IsNullOrEmpty(_d.fromAddress) && String.IsNullOrEmpty(_d.toAddress));
                        Assert.True(_d.timestamp >= 1000000000000 && _d.timestamp <= 9999999999999);
                    }
                }

                var _balance = await _private_api.FetchBalance("XRP", "", GetJsonContent(_private_api.privateClient, "fetchBalance", _args));
                if (_balance.supported == true || TestConfig.SupportedCheck == true)
                {
                    this.WriteJson(_private_api.privateClient, _balance);

                    Assert.NotNull(_balance);
                    Assert.True(_balance.success, $"fetch balances return error: {_balance.message}");
                    Assert.True(_balance.message == "success");

                    Assert.True(_balance.result.currency == "XRP");
                    Assert.True(Numerical.CompareDecimal(_balance.result.total, _balance.result.free + _balance.result.used));
                }

                var _balances = await _private_api.FetchBalances(GetJsonContent(_private_api.privateClient, "fetchBalances", _args));
                if (_balances.supported == true || TestConfig.SupportedCheck == true)
                {
                    this.WriteJson(_private_api.privateClient, _balances);

                    Assert.NotNull(_balances);
                    Assert.True(_balances.success, $"fetch all balances return error: {_balances.message}");
                    Assert.True(_balances.message == "success");
                    Assert.True(_balances.result.Count >= 0);

                    foreach (var _b in _balances.result)
                    {
                        Assert.True(Numerical.CompareDecimal(_b.total, _b.free + _b.used));
                    }
                }

                var _wallet = await _private_api.FetchWallet("XRP", "USD", GetJsonContent(_private_api.privateClient, "fetchWallet", _args));
                if (_wallet.supported == true || TestConfig.SupportedCheck == true)
                {
                    this.WriteJson(_private_api.privateClient, _wallet);

                    Assert.NotNull(_wallet);
                    Assert.True(_wallet.success, $"fetch wallet return error: {_wallet.message}");
                    Assert.True(_wallet.message == "success");

                    Assert.False(String.IsNullOrEmpty(_wallet.result.balance.currency));
                    Assert.True(Numerical.CompareDecimal(_wallet.result.balance.total, _wallet.result.balance.free + _wallet.result.balance.used));
                }

                var _wallets = await _private_api.FetchWallets(_api_key.user_id, GetJsonContent(_private_api.privateClient, "fetchWallets", _args));
                if (_wallets.supported == true || TestConfig.SupportedCheck == true)
                {
                    this.WriteJson(_private_api.privateClient, _wallets);

                    Assert.NotNull(_wallets);
                    Assert.True(_wallets.success, $"fetch wallets return error: {_wallets.message}");
                    Assert.True(_wallets.message == "success");

                    foreach (var _b in _balances.result)
                    {
                        Assert.True(Numerical.CompareDecimal(_b.total, _b.free + _b.used));
                    }
                }
            }
        }
    }
}