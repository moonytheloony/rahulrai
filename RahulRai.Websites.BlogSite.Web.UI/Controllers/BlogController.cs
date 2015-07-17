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

    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using GlobalAccess;
    using Models;
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
        ///     The search records
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
            UserPageDictionary.PageDictionary = null;
            var blogPosts = this.blogService.GetLatestBlogs();
            this.SetPreviousNextPage(0);
            return this.View("BlogList", blogPosts);
        }

        /// <summary>
        ///     Gets the searched blogs.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public async Task<ActionResult> SearchResult(string searchTerm)
        {
            var errorList = this.blogService.SanitizeSearchTerm(ref searchTerm);
            if (errorList.Any())
            {
                this.ViewBag.Errors = errorList;
                return this.View("SearchResult", null);
            }

            var searchedBlogs = await Task.Run(() => this.blogService.SearchBlogs(searchTerm));
            return this.View("SearchResult", searchedBlogs);
        }

        /// <summary>
        ///     Pages the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Page(int id)
        {
            if (id > UserPageDictionary.PageDictionary.MaximumPageNumber || id < 0)
            {
                this.RedirectToAction("GetLatestBlogs");
            }

            var blogList = this.blogService.GetBlogsForPage(id);
            this.SetPreviousNextPage(id);
            return this.View("BlogList", blogList);
        }

        /// <summary>
        ///     Gets the blog post.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult GetBlogPost(string postId)
        {
            if (string.IsNullOrWhiteSpace(postId))
            {
                return new HttpNotFoundResult("no post specified");
            }

            var blogPost = this.blogService.GetBlogPost(postId);
            if (null == blogPost)
            {
                return new HttpNotFoundResult("article does not exist");
            }

            return this.View("BlogPost", blogPost);
        }

        /// <summary>
        ///     Goes the archive.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Archive()
        {
            var blogList = this.blogService.GetBlogArchive();
            //// Group results by month and year.
            var groupedBlogPosts = from post in blogList
                                   group post by post.PostedDate.Year
                                       into yearGroup
                                       let postYear = yearGroup.Key
                                       orderby postYear descending
                                       select new Archive
                                       {
                                           Year = postYear,
                                           MonthGroups =
                                               from yearPost in yearGroup
                                               group yearPost by yearPost.PostedDate.Month
                                                   into monthGroup
                                                   let postMonth = monthGroup.Key
                                                   orderby postMonth descending
                                                   select new MonthGroup { Month = postMonth, Posts = monthGroup.ToList() }
                                       };
            return this.View(groupedBlogPosts);
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
        /// <param name="currentPageNumber">The current page number.</param>
        private void SetPreviousNextPage(int currentPageNumber)
        {
            this.ViewBag.PreviousPageNumber = 0;
            this.ViewBag.NextPageNumber = 0;
            if (UserPageDictionary.PageDictionary.CanMoveBack())
            {
                this.ViewBag.Previous = true;
                this.ViewBag.PreviousPageNumber = currentPageNumber - 1;
            }
            else
            {
                this.ViewBag.Previous = false;
                this.ViewBag.PreviousPageNumber = 0;
            }

            if (UserPageDictionary.PageDictionary.CanMoveForward())
            {
                this.ViewBag.Next = true;
                this.ViewBag.NextPageNumber = currentPageNumber + 1;
            }
            else
            {
                this.ViewBag.Next = false;
                this.ViewBag.NextPageNumber = currentPageNumber;
            }
        }
    }
}