#region



#endregion

namespace RahulRai.Websites.BlogSite.Web.UI
{
    #region

    using System.Web.Mvc;
    using System.Web.Routing;

    #endregion

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.Add(new Route("metaweblog", new WeblogRouteHandler()));
            routes.MapRoute("Default", string.Empty,
                new
                {
                    controller = "Blog",
                    action = "GetLatestBlogs"
                });
            routes.MapRoute("BlogPost", "post/{postId}",
                new
                {
                    controller = "Blog",
                    action = "GetBlogPost"
                }, new
                {
                    postId = @"^[a-zA-Z0-9\-]$"
                });
        }
    }
}