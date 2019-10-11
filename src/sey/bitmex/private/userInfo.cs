using Newtonsoft.Json.Linq;
using OdinSdk.BaseLib.Coin;
using System;
using System.Collections.Generic;

namespace CCXT.NET.BitMEX.Private
{
    /// <summary>
    ///
    /// </summary>
    public class BUserPreferences
    {
        /// <summary>
        ///
        /// </summary>
        public bool alertOnLiquidations
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool animationsEnabled
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime announcementsLastSeen
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public int chatChannelID
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public string colorTheme
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public string currency
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool debug
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public List<string> disableEmails
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public List<string> disablePush
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public List<string> hideConfirmDialogs
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool hideConnectionModal
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool hideFromLeaderboard
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool hideNameFromLeaderboard
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public List<string> hideNotifications
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public string locale
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public List<string> msgsSeen
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public JToken orderBookBinning
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string orderBookType
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool orderClearImmediate
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool orderControlsPlusMinus
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool showLocaleNumbers
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public List<string> sounds
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool strictIPCheck
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool strictTimeout
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public string tickerGroup
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool tickerPinned
        {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        public string tradeLayout
        {
            get; set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IUserInfoItem
    {
        /// <summary>
        ///
        /// </summary>
        string id
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string ownerId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string firstName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string lastName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string userName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string email
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string phone
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        DateTime created
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        DateTime lastUpdated
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        BUserPreferences preferences
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        JToken restrictedEngineFields
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string TFAEnabled
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string affiliateId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string pgpPubKey
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string country
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string geoipCountry
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string geoipRegion
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string typ
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        bool isRestricted
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        JArray roles
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class BUserInfoItem : IUserInfoItem
    {
        /// <summary>
        ///
        /// </summary>
        public string id
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string ownerId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string firstName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string lastName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string userName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string email
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string phone
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime created
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime lastUpdated
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public BUserPreferences preferences
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public JToken restrictedEngineFields
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string TFAEnabled
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string affiliateId
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string pgpPubKey
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string country
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string geoipCountry
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string geoipRegion
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string typ
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public bool isRestricted
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public JArray roles
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IUserInfo : IApiResult<IUserInfoItem>
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
    public class BUserInfo : ApiResult<IUserInfoItem>, IUserInfo
    {
        /// <summary>
        ///
        /// </summary>
        public BUserInfo()
        {
            this.result = new BUserInfoItem();
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