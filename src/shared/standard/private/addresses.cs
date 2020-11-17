using Newtonsoft.Json;
using System.Collections.Generic;

namespace CCXT.NET.Shared.Coin.Private
{
    /// <summary>
    ///
    /// </summary>
    public interface IAddressItem
    {
        /// <summary>
        ///
        /// </summary>
        string currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string address
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        string tag
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        bool success
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class AddressItem : IAddressItem
    {
        /// <summary>
        ///
        /// </summary>
        public AddressItem()
        {
            this.success = true;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual string currency
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual string address
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual string tag
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool success
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IAddress : IApiResult<IAddressItem>
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
    public class Address : ApiResult<IAddressItem>, IAddress
    {
        /// <summary>
        ///
        /// </summary>
        public Address()
        {
            this.result = new AddressItem();
        }
#if DEBUG
        /// <summary>
        ///
        /// </summary>
        [JsonIgnore]
        public virtual string rawJson
        {
            get;
            set;
        }
#endif
    }

    /// <summary>
    ///
    /// </summary>
    public interface IAddresses : IApiResult<List<IAddressItem>>
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
    public class Addresses : ApiResult<List<IAddressItem>>, IAddresses
    {
        /// <summary>
        ///
        /// </summary>
        public Addresses()
        {
            this.result = new List<IAddressItem>();
        }

#if DEBUG

        /// <summary>
        ///
        /// </summary>
        [JsonIgnore]
        public virtual string rawJson
        {
            get;
            set;
        }

#endif
    }
}