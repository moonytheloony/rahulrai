namespace RahulRai.Websites.BlogSite.Web.UI.Controllers
{
    #region

    using System.Net;
    using System.Web.Mvc;
    using Utilities.Web;

    #endregion

    [Route("Error")]
    public class ErrorController : BaseController
    {
        // GET: Error
        public ActionResult Index()
        {
            //Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View("Error");
        }

        public ViewResult NotFound()
        {
            Response.StatusCode = (int) HttpStatusCode.NotFound;
            return View("Error");
        }
    }
}