namespace RahulRai.Websites.Utilities.Common.RegularTypes
{
    /// <summary>
    ///     The retry policy.
    /// </summary>
    public static class CustomRetryPolicy
    {
        #region Constants

        /// <summary>
        ///     The max retries.
        /// </summary>
        public const int MaxRetries = 10;

        /// <summary>
        ///     The retry back off.
        /// </summary>
        public const int RetryBackOffSeconds = 2;

        #endregion
    }
}