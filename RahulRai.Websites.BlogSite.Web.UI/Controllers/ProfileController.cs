// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 06-25-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-28-2015
// ***********************************************************************
// <copyright file="ProfileController.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.BlogSite.Web.UI.Controllers
{
    #region

    using System;
    using System.Configuration;
    using System.Web.Mvc;
    using Models;
    using Utilities.Common.RegularTypes;
    using Utilities.Web;

    #endregion

    /// <summary>
    ///     Class ProfileController.
    /// </summary>
    public class ProfileController : BaseController
    {
        /// <summary>
        ///     My profile.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult MyProfile()
        {
            return this.View();
        }

        /// <summary>
        ///     Writes a testimonial for me.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult WriteATestimonialForMe()
        {
            return this.View();
        }

        /// <summary>
        ///     Profiles the content.
        /// </summary>
        /// <returns>ViewResult.</returns>
        public ViewResult ProfileContent()
        {
            return this.View("Profile");
        }

        /// <summary>
        ///     Testimonials this instance.
        /// </summary>
        /// <returns>ViewResult.</returns>
        public ViewResult Testimonials()
        {
            return this.View("Testimonials");
        }

        /// <summary>
        ///     Resumes this instance.
        /// </summary>
        /// <returns>ViewResult.</returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public ViewResult Resume()
        {
            return this.View("Resume", new PassKey());
        }

        /// <summary>
        /// Resumes the specified key data.
        /// </summary>
        /// <param name="keyData">The key data.</param>
        /// <returns>ViewResult.</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ViewResult Resume(PassKey keyData)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Resume", new PassKey());
            }

            if (keyData != null && keyData.Key != null)
            {
                this.ViewBag.IsValid = keyData.Key.Trim().Equals(ConfigurationManager.AppSettings[ApplicationConstants.ViewerToken], StringComparison.OrdinalIgnoreCase);
            }

            return this.View("Resume", new PassKey());
        }

        /// <summary>
        ///     Contacts this instance.
        /// </summary>
        /// <returns>ViewResult.</returns>
        public ViewResult Contact()
        {
            return this.View("Contact");
        }
    }
}