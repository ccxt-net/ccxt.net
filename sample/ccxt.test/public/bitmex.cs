using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace XUnit
{
    public partial class PublicApi
    {
        [Fact]
        public async void BitMEX()
        {
            var _public_api = new CCXT.NET.BitMEX.Public.PublicApi();

            var _args = new Dictionary<string, object>();
            var _timeframe = "1d";
            var _since = 0; //1514764800000;
            var _limit = 100;

            var _markets = await _public_api.LoadMarketsAsync(false, GetJsonContent(_public_api.publicClient, "fetchMarkets", _args));
            if (_markets.supported == true || TestConfig.SupportedCheck == true)
            {
                this.WriteJson(_public_api.publicClient, _markets);

                Assert.NotNull(_markets);
                Assert.True(_markets.success, $"fetch markets return error: {_markets.message}");
                Assert.True(_markets.message == "success");
                Assert.True(_markets.result.Count > 0);

                foreach (var _market in _markets.result)
                {
                    Assert.True(_market.Key == _market.Value.marketId);
                    Assert.True(_markets.GetCurrencyId(_market.Value.baseName) == _market.Value.baseId);
                    Assert.True(_markets.GetCurrencyName(_market.Value.baseId) == _market.Value.baseName);
                    Assert.True(_markets.GetCurrencyId(_market.Value.quoteName) == _market.Value.quoteId);
                    Assert.True(_markets.GetCurrencyName(_market.Value.quoteId) == _market.Value.quoteName);
                }
            }

            var _ticker = await _public_api.FetchTickerAsync("XRP", "U18", GetJsonContent(_public_api.publicClient, "fetchTicker", _args));
            if (_ticker.supported == true || TestConfig.SupportedCheck == true)
            {
                this.WriteJson(_public_api.publicClient, _ticker);

                Assert.NotNull(_ticker);
                Assert.True(_ticker.success, $"fetch a ticker return error: {_ticker.message}");
                Assert.True(_ticker.message == "success");

                Assert.NotNull(_ticker.result);
                Assert.True(_ticker.result.timestamp >= 1000000000000 && _ticker.result.timestamp <= 9999999999999);

                Assert.False(String.IsNullOrEmpty(_ticker.marketId));
                var _market = _markets.GetMarketByMarketId(_ticker.marketId);
                Assert.NotNull(_market);

                Assert.False(String.IsNullOrEmpty(_ticker.result.symbol));
                var _symbol = _markets.GetMarketBySymbol(_ticker.result.symbol);
                Assert.NotNull(_symbol);
            }

            var _tickers = await _public_api.FetchTickersAsync(GetJsonContent(_public_api.publicClient, "fetchTickers", _args));
            if (_tickers.supported == true || TestConfig.SupportedCheck == true)
            {
                this.WriteJson(_public_api.publicClient, _tickers);

                Assert.NotNull(_tickers);
                Assert.True(_tickers.success, $"fetch tickers return error: {_tickers.message}");
                Assert.True(_tickers.message == "success");

                Assert.NotNull(_tickers.result);

                Assert.True(_tickers.result.Count > 0);
                foreach (var _t in _tickers.result)
                {
                    Assert.True(_t.timestamp >= 1000000000000 && _t.timestamp <= 9999999999999);

                    var _symbol_id = _t.symbol;
                    Assert.False(String.IsNullOrEmpty(_symbol_id));
                    var _symbol = _markets.GetMarketBySymbol(_symbol_id);
                    Assert.NotNull(_symbol);
                }
            }

            var _orderbook = await _public_api.FetchOrderBooksAsync("XRP", "U18", _limit, GetJsonContent(_public_api.publicClient, "fetchOrderBooks", _args));
            if (_orderbook.supported == true || TestConfig.SupportedCheck == true)
            {
                this.WriteJson(_public_api.publicClient, _orderbook);

                Assert.NotNull(_orderbook);
                Assert.True(_orderbook.success, $"fetch orderbooks return error: {_orderbook.message}");
                Assert.True(_orderbook.message == "success");

                Assert.NotNull(_orderbook.result);

                Assert.True(_orderbook.result.asks.Count <= _limit && _orderbook.result.asks.Count > 0);
                Assert.True(_orderbook.result.bids.Count <= _limit && _orderbook.result.bids.Count > 0);
                Assert.True(_orderbook.result.timestamp >= 1000000000000 && _orderbook.result.timestamp <= 9999999999999);

                Assert.False(String.IsNullOrEmpty(_orderbook.marketId));
                var _market = _markets.GetMarketByMarketId(_orderbook.marketId);
                Assert.NotNull(_market);

                Assert.False(String.IsNullOrEmpty(_orderbook.result.symbol));
                var _symbol = _markets.GetMarketBySymbol(_orderbook.result.symbol);
                Assert.NotNull(_symbol);

                foreach (var _ask in _orderbook.result.asks)
                {
                    Assert.True(_ask.count > 0);
                    Assert.True(_ask.quantity > 0.0m);
                    Assert.True(_ask.price > 0.0m);
                    Assert.True(_ask.amount == _ask.quantity * _ask.price);
                }

                foreach (var _bid in _orderbook.result.bids)
                {
                    Assert.True(_bid.count > 0);
                    Assert.True(_bid.quantity > 0.0m);
                    Assert.True(_bid.price > 0.0m);
                    Assert.True(_bid.amount == _bid.quantity * _bid.price);
                }
            }

            var _ohlcvs = await _public_api.FetchOHLCVsAsync("XRP", "U18", _timeframe, _since, _limit, GetJsonContent(_public_api.publicClient, "fetchOHLCVs", _args));
            if (_ohlcvs.supported == true || TestConfig.SupportedCheck == true)
            {
                this.WriteJson(_public_api.publicClient, _ohlcvs);

                Assert.NotNull(_ohlcvs);
                Assert.True(_ohlcvs.success, $"fetch ohlcvs return error: {_ohlcvs.message}");
                Assert.True(_ohlcvs.message == "success");

                Assert.NotNull(_ohlcvs.result);

                Assert.True(_ohlcvs.result.Count <= _limit && _ohlcvs.result.Count > 0);
                var _timestamp = _ohlcvs.result.First().timestamp;
                Assert.True(_timestamp >= 1000000000000 && _timestamp <= 9999999999999);

                Assert.False(String.IsNullOrEmpty(_ohlcvs.marketId));
                var _market = _markets.GetMarketByMarketId(_ohlcvs.marketId);
                Assert.NotNull(_market);
            }

            var _trades = await _public_api.FetchCompleteOrdersAsync("XRP", "U18", _timeframe, _since, _limit, GetJsonContent(_public_api.publicClient, "fetchCompleteOrders", _args));
            if (_trades.supported == true || TestConfig.SupportedCheck == true)
            {
                this.WriteJson(_public_api.publicClient, _trades);

                Assert.NotNull(_trades);
                Assert.True(_trades.success, $"fetch trades return error: {_trades.message}");
                Assert.True(_trades.message == "success");

                Assert.NotNull(_trades.result);

                Assert.True(_trades.result.Count <= _limit && _trades.result.Count > 0);

                Assert.False(String.IsNullOrEmpty(_trades.marketId));
                var _market = _markets.GetMarketByMarketId(_trades.marketId);
                Assert.NotNull(_market);

                foreach (var _trade in _trades.result)
                {
                    Assert.False(String.IsNullOrEmpty(_trade.transactionId));
                    Assert.True(_trade.timestamp >= 1000000000000 && _trade.timestamp <= 9999999999999);

                    Assert.True(_trade.quantity > 0.0m);
                    Assert.True(_trade.price > 0.0m);
                    Assert.True(_trade.amount == _trade.quantity * _trade.price);
                }
            }
        }
    }
}