namespace RahulRai.Websites.BlogSite.Web.UI
{
    #region

    using System.Configuration;
    using System.Web.Configuration;
    using System.Web.SessionState;
    using Utilities.Common.RegularTypes;

    #endregion

    public static class RegisterSessionStateProvider
    {
        public static void Set()
        {
            var config = WebConfigurationManager.OpenWebConfiguration("~");
            var sessionState = config.GetSection("system.web/sessionState") as SessionStateSection;
            if (sessionState != null && sessionState.Providers.Count != 0)
            {
                return;
            }

            sessionState = new SessionStateSection
            {
                Mode = SessionStateMode.Custom,
                CustomProvider = "RedisSessionProvider"
            };

            sessionState.Providers.Add(RedisProviderSettings());
            config.Save();
        }

        private static ProviderSettings RedisProviderSettings()
        {
            var providerSetting = new ProviderSettings
            {
                Name = "RedisSessionProvider",
                Type = "Microsoft.Web.Redis.RedisSessionStateProvider"
            };
            providerSetting.Parameters.Add("port", "6380");
            providerSetting.Parameters.Add("host",
                ConfigurationManager.AppSettings[ApplicationConstants.SessionStateHost]);
            providerSetting.Parameters.Add("accessKey",
                ConfigurationManager.AppSettings[ApplicationConstants.SessionStateKey]);
            providerSetting.Parameters.Add("ssl", "true");
            return providerSetting;
        }
    }
}