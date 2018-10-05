using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace CCXT.NET.Poloniex.Public
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPOrderBooks
    {
        /// <summary>
        /// 
        /// </summary>
        IList<IPOrderItem> BuyOrders
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        IList<IPOrderItem> SellOrders
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class POrderBooks : IPOrderBooks
    {
        [JsonProperty("bids")]
        private IList<string[]> BuyOrdersInternal
        {
            set
            {
                BuyOrders = ParseOrders(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<IPOrderItem> BuyOrders
        {
            get; private set;
        }

        [JsonProperty("asks")]
        private IList<string[]> SellOrdersInternal
        {
            set
            {
                SellOrders = ParseOrders(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<IPOrderItem> SellOrders
        {
            get; private set;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IList<IPOrderItem> ParseOrders(IList<string[]> orders)
        {
            var output = new List<IPOrderItem>(orders.Count);
            for (var i = 0; i < orders.Count; i++)
            {
                output.Add(
                    new POrderItem(
                        decimal.Parse(orders[i][0], NumberStyles.Float),
                        decimal.Parse(orders[i][1], NumberStyles.Float)
                    )
                );
            }
            return output;
        }
    }
}