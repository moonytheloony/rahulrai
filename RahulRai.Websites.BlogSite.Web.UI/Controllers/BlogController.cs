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

    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.Mvc;
    using GlobalAccess;
    using Services;
    using Utilities.AzureStorage.TableStorage;
    using Utilities.Common.Entities;
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
        ///     The blog service
        /// </summary>
        private BlogService blogService;

        /// <summary>
        ///     Gets the continuation stack.
        /// </summary>
        /// <value>The continuation stack.</value>
        public ContinuationStack ContinuationStack
        {
            get
            {
                if (this.Session[ApplicationConstants.StackKey] != null)
                {
                    return this.Session[ApplicationConstants.StackKey] as ContinuationStack;
                }

                var stack = new ContinuationStack();
                this.Session[ApplicationConstants.StackKey] = stack;
                return stack;
            }
        }

        /// <summary>
        ///     Gets the latest blogs.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult GetLatestBlogs()
        {
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
        ///     Goes the previous.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult GoPrevious()
        {
            var blogList = this.blogService.GoToPreviousBlogList();
            this.SetPreviousNextPage();
            return this.View("BlogList", blogList);
        }

        /// <summary>
        ///     Goes the next.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult GoNext()
        {
            var blogList = this.blogService.GoToNextBlogList();
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
        public ActionResult GoArchive()
        {
            var blogList = new List<BlogPost>();
            return this.View("BlogList", blogList);
        }

        /// <summary>
        ///     Initializes the action.
        /// </summary>
        protected override void InitializeAction()
        {
            this.blogService = new BlogService(this.ContinuationStack, this.Session, this.blogContext, this.pageSize);
        }

        /// <summary>
        ///     Sets the previous next page.
        /// </summary>
        private void SetPreviousNextPage()
        {
            this.ViewBag.Previous = this.ContinuationStack.CanMoveBack();
            this.ViewBag.Next = this.ContinuationStack.CanMoveForward();
        }
    }
}