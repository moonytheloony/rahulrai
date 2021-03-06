﻿// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 04-14-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="Global.asax.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.BlogSite.Web.UI
{
    #region

    using System.Web;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Microsoft.ApplicationInsights.Extensibility;
    using Utilities.Common.RegularTypes;
    using Utilities.Web;

    #endregion

    /// <summary>
    ///     Class MvcApplication.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        ///     Application_s the start.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterSessionStateProvider.Set();
            SetupTelemetry();
        }

        /// <summary>
        /// Setups the telemetry.
        /// </summary>
        private static void SetupTelemetry()
        {
            TelemetryConfiguration.Active.InstrumentationKey = WebConfigurationManager.AppSettings[ApplicationConstants.AppInsightKey];
        }

        /// <summary>
        ///     Registers the global filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        private static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute(), 2);
            filters.Add(new ErrorHandlerAttribute(), 1);
        }
    }
}