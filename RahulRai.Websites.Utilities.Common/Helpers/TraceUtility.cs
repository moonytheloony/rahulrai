namespace RahulRai.Websites.Utilities.Common.Helpers
{
    #region

    using System;
    using System.Diagnostics;
    using System.Globalization;

    #endregion

    public class TraceUtility
    {
        private const string InformationFormat = "Time: {0} Content: {1}";
        private const string ErrorFormat = "Time: {0} Message: {1} Exception: {2}";

        public static void LogInformation(string information, params object[] value)
        {
            var formattedValue = string.Format(CultureInfo.InvariantCulture, information, value);
            Trace.TraceInformation(InformationFormat, DateTime.UtcNow, formattedValue);
        }

        public static void LogWarning(string warning, params object[] value)
        {
            var formattedValue = string.Format(CultureInfo.InvariantCulture, warning, value);
            Trace.TraceWarning(InformationFormat, DateTime.UtcNow, formattedValue);
        }

        public static void LogError(Exception exception, string customMessage, params object[] value)
        {
            var formattedValue = string.Format(CultureInfo.InvariantCulture, customMessage, value);
            Trace.TraceError(ErrorFormat, DateTime.UtcNow, formattedValue, exception.ToString());
        }
    }
}