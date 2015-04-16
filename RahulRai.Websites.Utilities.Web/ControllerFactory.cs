#region



#endregion

namespace RahulRai.Websites.Utilities.Web
{
    #region

    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.SessionState;

    #endregion

    public class ControllerFactory : IControllerFactory
    {
        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            //ILogger logger = new DefaultLogger();
            //var controller = new HomeController(logger);
            //return controller;
            throw new NotImplementedException();
        }

        public SessionStateBehavior GetControllerSessionBehavior(
            RequestContext requestContext, string controllerName)
        {
            return SessionStateBehavior.Default;
        }

        public void ReleaseController(IController controller)
        {
            var disposable = controller as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }
    }
}