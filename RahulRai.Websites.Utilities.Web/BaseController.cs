namespace RahulRai.Websites.Utilities.Web
{
    #region

    using System.Configuration;
    using System.Web.Mvc;
    using Common.RegularTypes;

    #endregion

    /// <summary>
    ///     Base controller
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseController" /> class.
        /// </summary>
        protected BaseController()
        {
            ViewBag.ResourceBasePath = ConfigurationManager.AppSettings[ApplicationConstants.ApplicationResourceRoot];
            ViewBag.MyEmail = ConfigurationManager.AppSettings[ApplicationConstants.MyEmail];
        }

        /// <summary>
        ///     Called before the action method is invoked.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
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