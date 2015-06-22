namespace RahulRai.Websites.Utilities.Web
{
    #region

    using System.Configuration;
    using System.Web.Mvc;
    using Common.RegularTypes;

    #endregion

    public abstract class BaseController : Controller
    {
        public BaseController()
        {
            ViewBag.ResourceBasePath = ConfigurationManager.AppSettings[ApplicationConstants.ApplicationResourceRoot];
            ViewBag.MyEmail = ConfigurationManager.AppSettings[ApplicationConstants.MyEmail];
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionToInvoke = filterContext.ActionDescriptor.ActionName.ToLowerInvariant();
            switch (actionToInvoke)
            {
                case "get":
                    break;
                default:
                    ViewBag.Title = "Rahul on Technology and Things";
                    break;
            }
        }
    }
}