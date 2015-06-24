// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Web
// Author           : rahulrai
// Created          : 06-24-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="ErrorHandlerAttribute.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Web
{
    #region

    using System.Web.Mvc;
    using Common.Helpers;

    #endregion

    /// <summary>
    /// Class ErrorHandlerAttribute. This class cannot be inherited.
    /// </summary>
    public sealed class ErrorHandlerAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// Called when an exception occurs.
        /// </summary>
        /// <param name="filterContext">The action-filter context.</param>
        public override void OnException(ExceptionContext filterContext)
        {
            var initializer = new SetupProperties();
            var viewBag = initializer.InitializeProperties();
            object controller, action;
            TraceUtility.LogError(filterContext.Exception, filterContext.Exception.Message);
            filterContext.ExceptionHandled = true;
            var controllerName = filterContext.RouteData.Values.TryGetValue("controller", out controller)
                ? (string)controller
                : "Controller";
            var actionName = filterContext.RouteData.Values.TryGetValue("action", out action)
                ? (string)action
                : "Action";
            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            var errorView = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model),
            };
            errorView.ViewBag.Title = viewBag.Title;
            errorView.ViewBag.ResourceBasePath = viewBag.ResourceBasePath;
            errorView.ViewBag.MyEmail = viewBag.MyEmail;
            filterContext.Result = errorView;
        }
    }
}