using Newtonsoft.Json;
using OdinSdk.BaseLib.Coin.Trade;
using OdinSdk.BaseLib.Configuration;
using System;

namespace CCXT.NET.BitMEX.Trade
{
    /// <summary>
    /// 
    /// </summary>
    public class BMyPositionItem : OdinSdk.BaseLib.Coin.Trade.MyPositionItem, IMyPositionItem
    {
        /// <summary>
        /// Your unique account ID.
        /// </summary>
        [JsonProperty(PropertyName = "account")]
        public override string positionId
        {
            get;
            set;
        }

        /// <summary>
        /// The contract for this position.
        /// </summary>
        [JsonProperty(PropertyName = "symbol")]
        public override string symbol
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "originTimestamp")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "Timestamp")]
        private DateTime timeValue
        {
            set
            {
                timestamp = CUnixTime.ConvertToUnixTimeMilli(value);
            }
        }

        /// <summary>
        /// 1 / initMarginReq.
        /// </summary>
        [JsonProperty(PropertyName = "Leverage")]
        public decimal? leverage
        {
            get;
            set;
        }

        /// <summary>
        /// The current position amount in contracts.
        /// </summary>
        [JsonProperty(PropertyName = "CurrentQty")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "isOpen")]
        public bool isOpen
        {
            get;
            set;
        }

        /// <summary>
        /// The mark price of the symbol in quoteCurrency.
        /// </summary>
        [JsonProperty(PropertyName = "MarkPrice")]
        public decimal markPrice
        {
            get;
            set;
        }

        /// <summary>
        /// The currentQty at the mark price in the settlement currency of the symbol (currency).
        /// </summary>
        [JsonProperty(PropertyName = "MarkValue")]
        public decimal markValue
        {
            get;
            set;
        }

        /// <summary>
        /// unrealisedGrossPnl.
        /// </summary>
        [JsonProperty(PropertyName = "UnrealisedPnl")]
        public decimal unrealisedPnl
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "UnrealisedPnlPcnt")]
        public decimal unrealisedPnlPcnt
        {
            get;
            set;
        }

        /// <summary>
        /// AvgEntryPrice
        /// </summary>
        [JsonProperty(PropertyName = "AvgEntryPrice")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "BreakEvenPrice")]
        public decimal breakEvenPrice
        {
            get;
            set;
        }

        /// <summary>
        /// Once markPrice reaches this price, this position will be liquidated.
        /// </summary>
        [JsonProperty(PropertyName = "LiquidationPrice")]
        public override decimal liqPrice
        {
            get;
            set;
        }

        /// <summary>
        /// The maximum of the maker, taker, and settlement fee.
        /// </summary>
        [JsonProperty(PropertyName = "commission")]
        public override decimal fee
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "prevUnrealisedPnl")]
        public decimal prevUnrealisedPnl
        {
            get;
            set;
        }

        /// <summary>
        /// The margin currency for this position.
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        public string currency
        {
            get;
            set;
        }

        /// <summary>
        /// Meta data of the symbol.
        /// </summary>
        [JsonProperty(PropertyName = "underlying")]
        public string underlying
        {
            get;
            set;
        }

        /// <summary>
        /// Meta data of the symbol, All prices are in the quoteCurrency
        /// </summary>
        [JsonProperty(PropertyName = "quoteCurrency")]
        public string quoteCurrency
        {
            get;
            set;
        }

        /// <summary>
        /// The initial margin requirement. This will be at least the symbol's default initial maintenance margin, but can be higher if you choose lower leverage.
        /// </summary>
        [JsonProperty(PropertyName = "initMarginReq")]
        public decimal initMarginReq
        {
            get;
            set;
        }

        /// <summary>
        /// The maintenance margin requirement. This will be at least the symbol's default maintenance maintenance margin, but can be higher if you choose a higher risk limit.
        /// </summary>
        [JsonProperty(PropertyName = "maintMarginReq")]
        public decimal maintMarginReq
        {
            get;
            set;
        }

        /// <summary>
        /// This is a function of your maintMarginReq.
        /// </summary>
        [JsonProperty(PropertyName = "riskLimit")]
        public decimal riskLimit
        {
            get;
            set;
        }

        /// <summary>
        /// True/false depending on whether you set cross margin on this position.
        /// </summary>
        [JsonProperty(PropertyName = "crossMargin")]
        public bool crossMargin
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates where your position is in the ADL queue.
        /// </summary>
        [JsonProperty(PropertyName = "deleveragePercentile")]
        public decimal deleveragePercentile
        {
            get;
            set;
        }

        /// <summary>
        /// The value of realised PNL that has transferred to your wallet for this position.
        /// </summary>
        [JsonProperty(PropertyName = "rebalancedPnl")]
        public decimal rebalancedPnl
        {
            get;
            set;
        }

        // prevRebalancedPnl: The value of realised PNL that has transferred to your wallet for this position since the position was closed.
        // sample에 항목이 없음 설명에만 있음

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "prevRealisedPnl")]
        public decimal prevRealisedPnl
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "prevClosePrice")]
        public decimal prevClosePrice
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "openingTimestamp")]
        public DateTime openingTimestamp
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "openingQty")]
        public decimal openingQty
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "openingComm")]
        public decimal openingComm
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "openOrderBuyCost")]
        public decimal openOrderBuyCost
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "openOrderBuyPremium")]
        public decimal openOrderBuyPremium
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "openOrderSellQty")]
        public decimal openOrderSellQty
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "openOrderSellCost")]
        public decimal openOrderSellCost
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "openOrderSellPremium")]
        public decimal openOrderSellPremium
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "execBuyQty")]
        public decimal execBuyQty
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "execBuyCost")]
        public decimal execBuyCost
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "execSellQty")]
        public decimal execSellQty
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "execSellCost")]
        public decimal execSellCost
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "execCost")]
        public decimal execCost
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "execComm")]
        public decimal execComm
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "currentTimestamp")]
        public DateTime currentTimestamp
        {
            get;
            set;
        }

        /// <summary>
        /// The current cost of the position in the settlement currency of the symbol (currency).
        /// </summary>
        [JsonProperty(PropertyName = "currentCost")]
        public decimal currentCost
        {
            get;
            set;
        }

        /// <summary>
        /// The current commission of the position in the settlement currency of the symbol (currency).
        /// </summary>
        [JsonProperty(PropertyName = "currentComm")]
        public decimal currentComm
        {
            get;
            set;
        }

        /// <summary>
        /// The realised cost of this position calculated with regard to average cost accounting.
        /// </summary>
        [JsonProperty(PropertyName = "realisedCost")]
        public decimal realisedCost
        {
            get;
            set;
        }

        /// <summary>
        /// currentCost - realisedCost.
        /// </summary>
        [JsonProperty(PropertyName = "unrealisedCost")]
        public decimal unrealisedCost
        {
            get;
            set;
        }

        /// <summary>
        /// The absolute value of your open orders for this symbol.
        /// </summary>
        [JsonProperty(PropertyName = "grossOpenCost")]
        public decimal grossOpenCost
        {
            get;
            set;
        }

        /// <summary>
        /// The amount your bidding above the mark price in the settlement currency of the symbol (currency).
        /// </summary>
        [JsonProperty(PropertyName = "grossOpenPremium")]
        public decimal grossOpenPremium
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "grossExecCost")]
        public decimal grossExecCost
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "riskValue")]
        public decimal riskValue
        {
            get;
            set;
        }

        /// <summary>
        /// Value of position in units of underlying.(homeNotional)
        /// </summary>
        [JsonProperty(PropertyName = "homeNotional")]
        public decimal homeNotional
        {
            get;
            set;
        }

        /// <summary>
        /// Value of position in units of quoteCurrency.
        /// </summary>
        [JsonProperty(PropertyName = "foreignNotional")]
        public decimal foreignNotional
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "posState")]
        public string posState
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "posCost")]
        public decimal posCost
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "posCost2")]
        public decimal posCost2
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "posCross")]
        public decimal posCross
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "posInit")]
        public decimal posInit
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "posComm")]
        public decimal posComm
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "posLoss")]
        public decimal posLoss
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "posMargin")]
        public decimal posMargin
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "posMaint")]
        public decimal posMaint
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "posAllowance")]
        public decimal posAllowance
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "taxableMargin")]
        public decimal taxableMargin
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "initMargin")]
        public decimal initMargin
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "maintMargin")]
        public decimal maintMargin
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "sessionMargin")]
        public decimal sessionMargin
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "targetExcessMargin")]
        public decimal targetExcessMargin
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "varMargin")]
        public decimal varMargin
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "realisedGrossPnl")]
        public decimal realisedGrossPnl
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "realisedTax")]
        public decimal realisedTax
        {
            get;
            set;
        }

        /// <summary>
        /// The negative of realisedCost.
        /// </summary>
        [JsonProperty(PropertyName = "realisedPnl")]
        public decimal realisedPnl
        {
            get;
            set;
        }

        /// <summary>
        /// markValue - unrealisedCost.
        /// </summary>
        [JsonProperty(PropertyName = "unrealisedGrossPnl")]
        public decimal unrealisedGrossPnl
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "longBankrupt")]
        public decimal longBankrupt
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "shortBankrupt")]
        public decimal shortBankrupt
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "taxBase")]
        public decimal taxBase
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "indicativeTaxRate")]
        public decimal indicativeTaxRate
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "indicativeTax")]
        public decimal indicativeTax
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "unrealisedTax")]
        public decimal unrealisedTax
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "unrealisedRoePcnt")]
        public decimal unrealisedRoePcnt
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "simpleQty")]
        public decimal simpleQty
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "simpleCost")]
        public decimal simpleCost
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "simpleValue")]
        public decimal simpleValue
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "simplePnl")]
        public decimal simplePnl
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "simplePnlPcnt")]
        public decimal simplePnlPcnt
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "avgCostPrice")]
        public decimal avgCostPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "marginCallPrice")]
        public decimal marginCallPrice
        {
            get;
            set;
        }

        /// <summary>
        /// Once markPrice reaches this price, this position will have no equity.
        /// </summary>
        [JsonProperty(PropertyName = "bankruptPrice")]
        public decimal bankruptPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "lastPrice")]
        public decimal lastPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "lastValue")]
        public decimal lastValue
        {
            get;
            set;
        }
    }
}