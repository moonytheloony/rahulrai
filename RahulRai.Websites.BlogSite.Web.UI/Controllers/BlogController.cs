// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 04-15-2015
//
// Last Modified By : rahulrai
// Last Modified On : 07-01-2015
// ***********************************************************************
// <copyright file="BlogController.cs" company="Rahul Rai">
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
    using System.Web.Mvc;
    using GlobalAccess;
    using Services;
    using Utilities.AzureStorage.TableStorage;
    using Utilities.Common.Entities;
    using Utilities.Common.Exceptions;
    using Utilities.Common.RegularTypes;
    using Utilities.Web;

    #endregion

    /// <summary>
    ///     Class BlogController.
    /// </summary>
    public class BlogController : BaseController
    {
        /// <summary>
        ///     The blog context
        /// </summary>
        private readonly AzureTableStorageService<TableBlogEntity> blogContext = BlogStoreAccess.Instance.BlogTable;

        /// <summary>
        ///     The page size
        /// </summary>
        private readonly int pageSize =
            int.Parse(ConfigurationManager.AppSettings[ApplicationConstants.BlogListPageSize]);

        /// <summary>
        /// The search records
        /// </summary>
        private readonly int searchRecordsSize =
            int.Parse(ConfigurationManager.AppSettings[ApplicationConstants.SearchRecordsSize]);

        /// <summary>
        ///     The blog service
        /// </summary>
        private BlogService blogService;

        /// <summary>
        ///     Gets the latest blogs.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult GetLatestBlogs()
        {
            UserContinuationStack.ContinuationStack = null;
            var blogPosts = this.blogService.GetLatestBlogs();
            this.SetPreviousNextPage();
            return this.View("BlogList", blogPosts);
        }

        /// <summary>
        ///     Gets the searched blogs.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult GetSearchedBlogs(string searchTerm)
        {
            var blogList = new List<BlogPost>();
            return this.View("BlogList", blogList);
        }

        /// <summary>
        /// Navigations the specified collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns>ActionResult.</returns>
        /// <exception cref="BlogSystemException">invalid form input</exception>
        [HttpPost]
        public ActionResult Navigation(FormCollection collection)
        {
            var postedValue = string.Empty;
            List<BlogPost> blogList;
            if (collection != null && collection["postField"] != null)
            {
                postedValue = collection["postField"];
            }

            switch (postedValue.ToLowerInvariant())
            {
                case "next":
                    blogList = this.blogService.GoToNextBlogList();
                    break;
                case "previous":
                    blogList = this.blogService.GoToPreviousBlogList();
                    break;
                default:
                    throw new BlogSystemException("invalid form input");
            }

            this.SetPreviousNextPage();
            return this.View("BlogList", blogList);
        }

        /// <summary>
        ///     Gets the blog post.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult GetBlogPost()
        {
            return this.View("BlogPost");
        }

        /// <summary>
        ///     Goes the archive.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Archive()
        {
            var blogList = new List<BlogPost>();
            return this.View("BlogList", blogList);
        }

        /// <summary>
        ///     Initializes the action.
        /// </summary>
        protected override void InitializeAction()
        {
            this.blogService = new BlogService(this.blogContext, this.pageSize, this.searchRecordsSize);
        }

        /// <summary>
        ///     Sets the previous next page.
        /// </summary>
        private void SetPreviousNextPage()
        {
            this.ViewBag.Previous = UserContinuationStack.ContinuationStack.CanMoveBack();
            this.ViewBag.Next = UserContinuationStack.ContinuationStack.CanMoveForward();
        }
    }
}