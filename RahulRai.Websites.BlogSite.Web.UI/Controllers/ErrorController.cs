// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 06-24-2015
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

    using System.Web.Mvc;
    using Utilities.Web;

    #endregion

    /// <summary>
    /// Class ErrorController.
    /// </summary>
    public class ErrorController : BaseController
    {
        /// <summary>
        /// Nots the found.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult NotFound()
        {
            this.Response.StatusCode = 200;
            return this.View("NotFound");
        }

        /// <summary>
        /// Internals the server.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult InternalServer()
        {
            this.Response.StatusCode = 200;
            return this.View("Error");
        }
    }
}