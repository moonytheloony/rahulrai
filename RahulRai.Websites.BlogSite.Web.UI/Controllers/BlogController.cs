using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RahulRai.Websites.Utilities.Web;

namespace RahulRai.Websites.BlogSite.Web.UI.Controllers
{
    public class BlogController : BaseController
    {
        // GET: Blog
        public ActionResult Index()
        {
            return View();
        }
    }
}