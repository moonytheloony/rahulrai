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

    using System.Web.Mvc;
    using Utilities.Web;

    #endregion

    /// <summary>
    /// Class ProfileController.
    /// </summary>
    public class ProfileController : BaseController
    {
        /// <summary>
        /// My profile.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult MyProfile()
        {
            return this.View();
        }

        /// <summary>
        /// Writes a testimonial for me.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult WriteATestimonialForMe()
        {
            return this.View();
        }

        /// <summary>
        /// Profiles the content.
        /// </summary>
        /// <returns>ViewResult.</returns>
        public ViewResult ProfileContent()
        {
            return this.View("Profile");
        }

        /// <summary>
        /// Testimonialses this instance.
        /// </summary>
        /// <returns>ViewResult.</returns>
        public ViewResult Testimonials()
        {
            return this.View("Testimonials");
        }

        /// <summary>
        /// Resumes this instance.
        /// </summary>
        /// <returns>ViewResult.</returns>
        public ViewResult Resume()
        {
            return this.View("Resume");
        }

        /// <summary>
        /// Contacts this instance.
        /// </summary>
        /// <returns>ViewResult.</returns>
        public ViewResult Contact()
        {
            return this.View("Contact");
        }
    }
}