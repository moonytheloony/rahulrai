#region

using System.Web.Mvc;

#endregion

namespace RahulRai.Websites.SportsAndHealth.Web.UI.Controllers
{
    public class Base : Controller
    {
        // GET: Base
        public ActionResult Index()
        {
            return View();
        }
    }
}