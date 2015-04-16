namespace RahulRai.Websites.BlogSite.Web.UI.Controllers
{
    #region

    using System.Web.Mvc;
    using Utilities.Web;

    #endregion

    public class BlogController : BaseController
    {
        // GET: Blog
        public ActionResult Index()
        {
            return View();
        }
    }
}