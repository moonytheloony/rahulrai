namespace RahulRai.Websites.BlogSite.Web.UI
{
    #region

    using System.Web;
    using System.Web.Routing;
    using Utilities.Web;

    #endregion

    public class WeblogRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new MetaWeblogHandler();
        }
    }
}