// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 06-25-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-25-2015
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
    using System.Web.Mvc;
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
        /// Gets the content of the profile.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>PartialViewResult.</returns>
        /// <exception cref="System.IndexOutOfRangeException">id</exception>
        [HttpGet]
        public PartialViewResult GetProfileContent(int id)
        {
            switch (id)
            {
                case 1:
                    return this.PartialView("Profile");
                case 2:
                    return this.PartialView("Testimonials");
                case 3:
                    return this.PartialView("Resume");
                case 4:
                    return this.PartialView("Contact");
                default:
                    throw new IndexOutOfRangeException("id");
            }
        }
    }
}