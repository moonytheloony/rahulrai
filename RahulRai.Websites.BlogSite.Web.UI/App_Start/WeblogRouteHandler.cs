namespace RahulRai.Websites.BlogSite.Web.UI
{
    using System.Web;
    using System.Web.Routing;

    public class WeblogRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new MetaWeblogHandler();
        }
    }
}