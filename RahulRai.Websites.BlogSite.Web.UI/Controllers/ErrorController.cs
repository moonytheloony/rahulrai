// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 06-21-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="ErrorController.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.BlogSite.Web.UI.Controllers
{
    #region

    using System.Net;
    using System.Web.Mvc;
    using Utilities.Web;

    #endregion

    /// <summary>
    ///     Class ErrorController.
    /// </summary>
    [Route("Error")]
    public class ErrorController : BaseController
    {
        /// <summary>
        ///     Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            ////Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return this.View("Error");
        }

        /// <summary>
        ///     Nots the found.
        /// </summary>
        /// <returns>ViewResult.</returns>
        public ViewResult NotFound()
        {
            ////this.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return this.View();
        }
    }
}