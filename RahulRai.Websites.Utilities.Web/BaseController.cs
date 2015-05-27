namespace RahulRai.Websites.Utilities.Web
{
    #region

    using System.Web.Mvc;

    #endregion

    public abstract class BaseController : Controller
    {
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