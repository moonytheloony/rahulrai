// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Web
// Author           : rahulrai
// Created          : 04-14-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="ControllerFactory.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Web
{
    #region

    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.SessionState;

    #endregion

    /// <summary>
    ///     Controller factory
    /// </summary>
    public class ControllerFactory : IControllerFactory
    {
        /// <summary>
        ///     Creates the specified controller by using the specified request context.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <returns>The controller.</returns>
        /// <exception cref="System.NotImplementedException">Not Implemented</exception>
        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Gets the controller's session behavior.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="controllerName">The name of the controller whose session behavior you want to get.</param>
        /// <returns>The controller's session behavior.</returns>
        public SessionStateBehavior GetControllerSessionBehavior(
            RequestContext requestContext, string controllerName)
        {
            return SessionStateBehavior.Default;
        }

        /// <summary>
        ///     Releases the specified controller.
        /// </summary>
        /// <param name="controller">The controller.</param>
        public void ReleaseController(IController controller)
        {
            var disposable = controller as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}