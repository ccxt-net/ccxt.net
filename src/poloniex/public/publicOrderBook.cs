using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace CCXT.NET.Poloniex.Public
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPublicOrderBook
    {
        /// <summary>
        /// 
        /// </summary>
        IList<IPublicOrder> BuyOrders
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        IList<IPublicOrder> SellOrders
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PublicOrderBook : IPublicOrderBook
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
        public IList<IPublicOrder> BuyOrders
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
        public IList<IPublicOrder> SellOrders
        {
            get; private set;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IList<IPublicOrder> ParseOrders(IList<string[]> orders)
        {
            var output = new List<IPublicOrder>(orders.Count);
            for (var i = 0; i < orders.Count; i++)
            {
                output.Add(
                    new PublicOrder(
                        decimal.Parse(orders[i][0], NumberStyles.Float),
                        decimal.Parse(orders[i][1], NumberStyles.Float)
                    )
                );
            }
            return output;
        }
    }
}