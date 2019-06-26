using Newtonsoft.Json;
using CCXT.NET.Coin.Trade;
using CCXT.NET.Coin.Types;
using System.Collections.Generic;

namespace CCXT.NET.CEXIO.Trade
{
    /// <summary>
    ///
    /// </summary>
    public class CMyPositions : CCXT.NET.Coin.Trade.MyPositions, IMyPositions
    {
        /// <summary>
        ///
        /// </summary>
        public CMyPositions()
        {
            this.result = new List<CMyPositionItem>();
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "e")]
        public string command
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "ok")]
        private string ok
        {
            set
            {
                message = value;
                success = message == "ok";
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public new List<CMyPositionItem> result
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class CMyPositionItem : CCXT.NET.Coin.Trade.MyPositionItem, IMyPositionItem
    {
        /// <summary>
        /// User ID
        /// </summary>
        [JsonProperty(PropertyName = "user")]
        public string userId
        {
            get;
            set;
        }

        /// <summary>
        /// position id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public override string positionId
        {
            get;
            set;
        }

        /// <summary>
        /// timestamp the position was opened at
        /// </summary>
        [JsonProperty(PropertyName = "otime")]
        public override long timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// total amount of "product" in the position
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public override decimal quantity
        {
            get;
            set;
        }

        /// <summary>
        /// leverage, with which the position was opened
        /// </summary>
        public int leverage
        {
            get;
            set;
        }

        /// <summary>
        /// position type. long - buying product, profitable if product price grows; short - selling product, profitable if product price falls;
        /// </summary>
        [JsonProperty(PropertyName = "ptype")]
        private string positionType
        {
            set
            {
                sideType = SideTypeConverter.FromString(value);
            }
        }

        /// <summary>
        /// currency, in which the position was opened (product)
        /// </summary>
        public string psymbol
        {
            get;
            set;
        }

        /// <summary>
        /// currency, in which user is going to gain profit, can be one of the currencies, presented in the pair
        /// </summary>
        public string msymbol
        {
            get;
            set;
        }

        /// <summary>
        /// currency of borrowed funds, can be one of the currencies, presented in the pair
        /// </summary>
        public string lsymbol
        {
            get;
            set;
        }

        /// <summary>
        /// trading pair as a string like "XXX:XXX"
        /// </summary>
        public string pair
        {
            get;
            set;
        }

        /// <summary>
        /// price the position was opened at, calculated as average of underlying executed orders
        /// </summary>
        [JsonProperty(PropertyName = "oprice")]
        public override decimal price
        {
            get;
            set;
        }

        /// <summary>
        /// price, near which your position will be closed automatically in case of unfavorable market conditions
        /// </summary>
        public decimal stopLossPrice
        {
            get;
            set;
        }

        /// <summary>
        /// fee (in %) from user's amount, that was charged for position opening
        /// </summary>
        public decimal ofee
        {
            get;
            set;
        }

        /// <summary>
        /// estimated fee (in %) from user's amount, that will be charged for position rollover for the next 4 hours
        /// </summary>
        public decimal pfee
        {
            get;
            set;
        }

        /// <summary>
        /// fee (in %) from user's amount, that will be charged for position closing
        /// </summary>
        public decimal cfee
        {
            get;
            set;
        }

        /// <summary>
        /// total fees paid by user, it is equal to opening fee amount, when position has been just opened
        /// </summary>
        public decimal tfeeAmount
        {
            get;
            set;
        }

        /// <summary>
        /// total position amount, presented in "psymbol"
        /// </summary>
        public decimal pamount
        {
            get;
            set;
        }

        /// <summary>
        /// ("open money amount') user's amount used in the position, presented in 'msymbol"
        /// </summary>
        public decimal oamount
        {
            get;
            set;
        }

        /// <summary>
        /// amount of borrowed funds in the position, presented in "lsymbol"
        /// </summary>
        public decimal lamount
        {
            get;
            set;
        }

        /// <summary>
        /// underlying order id for position opening
        /// </summary>
        public long oorder
        {
            get;
            set;
        }

        /// <summary>
        /// rollover interval in miliseconds (14400000ms = 4Hours)
        /// </summary>
        public long rinterval
        {
            get;
            set;
        }

        /// <summary>
        /// (TECH) desired fast liquidation price
        /// </summary>
        public decimal dfl
        {
            get;
            set;
        }

        /// <summary>
        /// (stop-loss amount) amount that will be returned, including user`s and borrowed funds
        /// </summary>
        public decimal slamount
        {
            get;
            set;
        }

        /// <summary>
        /// (TECH) remains of slamount to return
        /// </summary>
        public decimal slremains
        {
            get;
            set;
        }

        /// <summary>
        /// (TECH) estimated price of total loss
        /// </summary>
        public decimal flPrice
        {
            get;
            set;
        }

        /// <summary>
        /// position's current status (e.g. a for active)
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        private string status
        {
            set
            {
                orderStatus = value == "a" ? OrderStatus.Open : OrderStatus.Closed;
            }
        }
    }
}