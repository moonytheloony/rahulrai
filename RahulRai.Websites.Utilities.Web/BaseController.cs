namespace RahulRai.Websites.Utilities.Web
{
    #region

    using System.Web.Mvc;

    #endregion

    public abstract class BaseController : Controller
    {
        public void SetTitle(string title)
        {
            ViewBag.Title = title;
        }


    }
}