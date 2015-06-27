// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 04-14-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="RouteConfig.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.BlogSite.Web.UI
{
    #region

    using System.Web.Mvc;
    using System.Web.Routing;

    #endregion

    /// <summary>
    ///     Class RouteConfig.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        ///     Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.Add(new Route("metaweblog", new WeblogRouteHandler()));
            routes.MapRoute("BlogPost", "post/{postId}", new { controller = "Blog", action = "GetBlogPost" });
            routes.MapRoute("Error", "Error/{action}", new { controller = "Error" });
            routes.MapRoute("Profile", "Profile/{action}/{id}", new { controller = "Profile", id = UrlParameter.Optional });
            routes.MapRoute("Default", "{controller}/{action}", new { controller = "Blog", action = "GetLatestBlogs" });
        }
    }
}