namespace RahulRai.Websites.Utilities.Common.Helpers
{
    #region

    using System;
    using System.Diagnostics;
    using System.Globalization;

    #endregion

    /// <summary>
    ///     Class TraceUtility.
    /// </summary>
    public class TraceUtility
    {
        /// <summary>
        ///     The information format
        /// </summary>
        private const string InformationFormat = "Time: {0} Content: {1}";

        /// <summary>
        ///     The error format
        /// </summary>
        private const string ErrorFormat = "Time: {0} Message: {1} Exception: {2}";

        /// <summary>
        ///     Logs the information.
        /// </summary>
        /// <param name="information">The information.</param>
        /// <param name="value">The value.</param>
        public static void LogInformation(string information, params object[] value)
        {
            var formattedValue = string.Format(CultureInfo.InvariantCulture, information, value);
            Trace.TraceInformation(InformationFormat, DateTime.UtcNow, formattedValue);
        }

        /// <summary>
        ///     Logs the warning.
        /// </summary>
        /// <param name="warning">The warning.</param>
        /// <param name="value">The value.</param>
        public static void LogWarning(string warning, params object[] value)
        {
            var formattedValue = string.Format(CultureInfo.InvariantCulture, warning, value);
            Trace.TraceWarning(InformationFormat, DateTime.UtcNow, formattedValue);
        }

        /// <summary>
        ///     Logs the error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="customMessage">The custom message.</param>
        /// <param name="value">The value.</param>
        public static void LogError(Exception exception, string customMessage, params object[] value)
        {
            var formattedValue = string.Format(CultureInfo.InvariantCulture, customMessage, value);
            Trace.TraceError(ErrorFormat, DateTime.UtcNow, formattedValue, exception.ToString());
        }
    }
}