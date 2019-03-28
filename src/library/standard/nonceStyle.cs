namespace OdinSdk.BaseLib.Coin
{
    /// <summary>
    /// 
    /// </summary>
    public enum XUnitMode : int
    {
        /// <summary>
        /// 
        /// </summary>
        ModeUnknown = 0,

        /// <summary>
        /// 
        /// </summary>
        UseEmptyData = 1,

        /// <summary>
        /// 
        /// </summary>
        UseExchangeServer = 2,

        /// <summary>
        /// 
        /// </summary>
        UseJsonFile = 3
    }

    /// <summary>
    /// Type of nonce styles
    /// </summary>
    public enum NonceStyle
    {
        /// <summary>
        /// Ticks (int64)
        /// </summary>
        Ticks,

        /// <summary>
        /// Ticks (string)
        /// </summary>
        TicksString,

        /// <summary>
        /// Milliseconds (int64)
        /// </summary>
        UnixMilliseconds,

        /// <summary>
        /// Milliseconds (string)
        /// </summary>
        UnixMillisecondsString,

        /// <summary>
        /// Seconds (double)
        /// </summary>
        UnixSeconds,

        /// <summary>
        /// Seconds (string)
        /// </summary>
        UnixSecondsString
    }
}
