// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 06-22-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="RegisterSessionStateProvider.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.BlogSite.Web.UI
{
    #region

    using System.Configuration;
    using System.Web.Configuration;
    using System.Web.SessionState;
    using Utilities.Common.RegularTypes;

    #endregion

    /// <summary>
    ///     Class RegisterSessionStateProvider.
    /// </summary>
    public static class RegisterSessionStateProvider
    {
        /// <summary>
        ///     Sets this instance.
        /// </summary>
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

        /// <summary>
        ///     Redises the provider settings.
        /// </summary>
        /// <returns>ProviderSettings.</returns>
        private static ProviderSettings RedisProviderSettings()
        {
            var providerSetting = new ProviderSettings
            {
                Name = "RedisSessionProvider",
                Type = "Microsoft.Web.Redis.RedisSessionStateProvider"
            };
            providerSetting.Parameters.Add("port", "6380");
            providerSetting.Parameters.Add(
                "host",
                WebConfigurationManager.AppSettings[ApplicationConstants.SessionStateHost]);
            providerSetting.Parameters.Add(
                "accessKey",
                WebConfigurationManager.AppSettings[ApplicationConstants.SessionStateKey]);
            providerSetting.Parameters.Add("ssl", "true");
            return providerSetting;
        }
    }
}