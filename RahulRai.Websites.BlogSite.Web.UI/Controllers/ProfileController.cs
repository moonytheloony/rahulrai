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
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web.Mvc;
    using GlobalAccess;
    using Models;
    using Services;
    using Utilities.Common.Entities;
    using Utilities.Common.RegularTypes;
    using Utilities.Web;

    #endregion

    /// <summary>
    ///     Class ProfileController.
    /// </summary>
    [RequireHttps]
    public class ProfileController : BaseController
    {
        /// <summary>
        ///     The document database access
        /// </summary>
        private readonly DocumentDbAccess documentDbAccess = DocumentDbAccess.Instance;

        /// <summary>
        ///     The profile service
        /// </summary>
        private ProfileService profileService;

        /// <summary>
        ///     Writes a testimonial for me.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult WriteTestimonial()
        {
            //// In case of redirect. Check whether a testimonial was submitted.
            this.ViewBag.TestimonialSubmitted = this.TempData["TestimonialSubmitted"];
            return this.View();
        }

        /// <summary>
        ///     Writes a testimonial for me.
        /// </summary>
        /// <param name="postedTestimonial">The posted testimonial.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult WriteTestimonial(Testimonial postedTestimonial)
        {
            this.profileService = new ProfileService(this.documentDbAccess);
            this.ViewBag.TestimonialSubmitted = false;
            this.ViewBag.KeyMatchFailed = false;
            if (!this.ModelState.IsValid)
            {
                return this.View(postedTestimonial);
            }

            if (
                !string.Equals(
                postedTestimonial.AuthorToken.Trim(),
                ConfigurationManager.AppSettings[ApplicationConstants.TestimonialToken],
                StringComparison.OrdinalIgnoreCase))
            {
                this.ViewBag.KeyMatchFailed = true;
                return this.View(postedTestimonial);
            }

            postedTestimonial.TestimonialId = DateTime.UtcNow.Ticks;
            postedTestimonial.IsApproved = false;
            postedTestimonial.IsFeatured = false;
            this.profileService.CleanseTestimonial(ref postedTestimonial);
            this.profileService.AddDocument(postedTestimonial);
            this.TempData["TestimonialSubmitted"] = true;
            return this.RedirectToRoute("Profile", new { controller = "Profile", action = "WriteTestimonial" });
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
            this.profileService = new ProfileService(this.documentDbAccess);
            var result = new List<Testimonial>();
            List<Testimonial> cacheResult;
            //// Check in cache first. Retry operation on first failure. Product Issue :(
            try
            {
                cacheResult = ApplicationCache.Get<List<Testimonial>>(ApplicationConstants.TestimonialCacheKey);
            }
            catch (Exception)
            {
                cacheResult = ApplicationCache.Get<List<Testimonial>>(ApplicationConstants.TestimonialCacheKey);
            }

            if (null != cacheResult)
            {
                return this.View("Testimonials", cacheResult);
            }

            var testimonialCount = Convert.ToInt32(ConfigurationManager.AppSettings[ApplicationConstants.TopTestimonialCount]);
            ////Get top N Testimonials.
            var documentsFeatured = this.profileService.QueryDocument<Testimonial>(testimonialCount);
            var featured = documentsFeatured.Where(document => document.IsFeatured && document.IsApproved).OrderByDescending(document => document.TestimonialId).ToList();
            result.AddRange(featured);
            if (testimonialCount - featured.Count() <= 0)
            {
                return this.View("Testimonials", result);
            }

            var documentsApproved = this.profileService.QueryDocument<Testimonial>(testimonialCount - featured.Count());
            var approved = documentsApproved.Where(document => document.IsApproved && !document.IsFeatured).OrderByDescending(document => document.TestimonialId).ToList();
            result.AddRange(approved);
            ApplicationCache.Set(ApplicationConstants.TestimonialCacheKey, result);
            return this.View("Testimonials", result);
        }

        /// <summary>
        ///     Resumes this instance.
        /// </summary>
        /// <returns>ViewResult.</returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public ViewResult Resume()
        {
            return this.View("Resume");
        }

        /// <summary>
        ///     Resumes the specified key data.
        /// </summary>
        /// <param name="keyData">The key data.</param>
        /// <returns>ViewResult.</returns>
        [HttpPost]
        public ViewResult Resume(PassKey keyData)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Resume", keyData);
            }

            if (keyData != null && keyData.Key != null)
            {
                this.ViewBag.IsValid = keyData.Key.Trim().Equals(ConfigurationManager.AppSettings[ApplicationConstants.ViewerToken], StringComparison.OrdinalIgnoreCase);
            }

            return this.View("Resume");
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