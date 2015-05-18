namespace RahulRai.Websites.BlogSite.Web.UI.Controllers
{
    #region

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Utilities.Common.Entities;
    using Utilities.Web;

    #endregion

    public class BlogController : BaseController
    {
        // GET: Blog
        public ActionResult GetLatestBlogs()
        {
            var blogList = new List<BlogPost>();
            return View(blogList);
        }
    }
}