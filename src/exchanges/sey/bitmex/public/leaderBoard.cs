using CCXT.NET.Coin;
using System.Collections.Generic;

namespace CCXT.NET.BitMEX.Public
{
    /*
    [
        {
            "profit": 571066944743,
            "isRealName": false,
            "name": "Heavy-Autumn-Wolf"
        },
        {
            "profit": 421616431343,
            "isRealName": false,
            "name": "Hot-Relic-Fancier"
        }
    ]
    */

    /// <summary>
    ///
    /// </summary>
    public interface ILeaderBoardItem
    {
        /// <summary>
        ///
        /// </summary>
        decimal profit
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        bool isRealName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string name
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
    public class LeaderBoardItem : ILeaderBoardItem
    {
        /// <summary>
        ///
        /// </summary>
        public decimal profit
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool isRealName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string name
        {
            get;
            set;
        }

#if DEBUG

        /// <summary>
        ///
        /// </summary>
        public string rawJson
        {
            get;
            set;
        }

#endif
    }

    /// <summary>
    ///
    /// </summary>
    public interface ILeaderBoards : IApiResult<List<ILeaderBoardItem>>
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
    public class LeaderBoards : ApiResult<List<ILeaderBoardItem>>, ILeaderBoards
    {
        /// <summary>
        ///
        /// </summary>
        public LeaderBoards()
        {
            this.result = new List<ILeaderBoardItem>();
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