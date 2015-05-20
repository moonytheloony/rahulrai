﻿namespace RahulRai.Websites.BlogSite.Web.UI
{
    #region

    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    #endregion

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}