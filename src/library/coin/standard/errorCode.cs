using System.ComponentModel;

namespace CCXT.NET.Coin
{
    /// <summary>
    /// Type of nonce styles
    /// </summary>
    public enum ErrorCode : int
    {
        /// <summary>
        /// general failure
        /// </summary>
        [Description("general failure")]
        Failure = -1,

        /// <summary>
        /// general success
        /// </summary>
        [Description("general success")]
        Success = 0,

        /// <summary>
        ///
        /// </summary>
        [Description("rest response error")]
        ResponseRestError = 100,

        /// <summary>
        ///
        /// </summary>
        [Description("response data error")]
        ResponseDataError = 101,

        /// <summary>
        ///
        /// </summary>
        [Description("response data empty")]
        ResponseDataEmpty = 102,

        /// <summary>
        ///
        /// </summary>
        [Description("not found data")]
        NotFoundData = 103,

        /// <summary>
        ///
        /// </summary>
        [Description("not supported error")]
        NotSupported,

        /// <summary>
        /// your time is ahead of server
        /// </summary>
        [Description("your time is ahead of server")]
        InvalidNonce,

        /// <summary>
        /// Insufficient balance available for withdrawal
        /// </summary>
        [Description("insufficient balance available for withdrawal")]
        InsufficientFunds,

        /// <summary>
        ///
        /// </summary>
        [Description("cancel pending")]
        CancelPending,

        /// <summary>
        /// Too many address
        /// </summary>
        [Description("too many address")]
        TooManyAddress,

        /// <summary>
        /// Invalid amount
        /// </summary>
        [Description("invalid amount")]
        InvalidAmount,

        /// <summary>
        /// Does not support market orders
        /// </summary>
        [Description("does not support market orders")]
        InvalidOrder,

        /// <summary>
        /// no found order
        /// </summary>
        [Description("no found order")]
        UnknownOrder,

        /// <summary>
        ///
        /// </summary>
        [Description("unknown withdraw")]
        UnknownWithdraw,

        /// <summary>
        /// operation failed! Orders have been completed or revoked, Unknown reference id
        /// </summary>
        [Description("operation failed! orders have been completed or revoked, unknown reference id")]
        OrderNotFound,

        /// <summary>
        /// The signature did not match the expected signature
        /// </summary>
        [Description("the signature did not match the expected signature")]
        InvalidSignature,

        /// <summary>
        /// Google authenticator is wrong, signature failed, API Authentication failed
        /// </summary>
        [Description("google authenticator is wrong, signature failed, api authentication failed")]
        AuthenticationError,

        /// <summary>
        /// wrong apikey permissions
        /// </summary>
        [Description("wrong apikey permissions")]
        PermissionDenied,

        /// <summary>
        /// API function do not exist, Invalid parameter
        /// </summary>
        [Description("api function do not exist, invalid parameter")]
        ExchangeError,

        /// <summary>
        /// current network is unstable, Maintenance work in progress
        /// API interface is locked or not enabled
        /// </summary>
        [Description("current network is unstable, maintenance work in progress")]
        ExchangeNotAvailable,

        /// <summary>
        /// Requests were made too frequently
        /// </summary>
        [Description("requests were made too frequently")]
        RateLimit,

        /// <summary>
        /// server busy please try again later, request too often
        /// </summary>
        [Description("server busy please try again later, request too often")]
        DDoSProtection
    }
}