// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Web
// Author           : rahulrai
// Created          : 04-15-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="BaseController.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

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
            this.ViewBag.ResourceBasePath =
                ConfigurationManager.AppSettings[ApplicationConstants.ApplicationResourceRoot];
            this.ViewBag.MyEmail = ConfigurationManager.AppSettings[ApplicationConstants.MyEmail];
            this.ViewBag.Previous = false;
            this.ViewBag.Next = false;
            this.ViewBag.PreviousPageNumber = 0;
            this.ViewBag.NextPageNumber = 0;
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
                case "getlatestblogs":
                    this.ViewBag.Title = "Recent Posts";
                    break;
                case "notfound":
                    this.ViewBag.Title = "404: Not Found";
                    break;
                case "internalserver":
                    this.ViewBag.Title = "Server Error";
                    break;
                case "searchresult":
                    this.ViewBag.Title = "Search Results";
                    break;
                default:
                    this.ViewBag.Title = "Rahul on Technology and Things";
                    break;
            }

            this.InitializeAction();
        }

        /// <summary>
        /// Initializes the action.
        /// </summary>
        protected virtual void InitializeAction()
        {
        }
    }
}