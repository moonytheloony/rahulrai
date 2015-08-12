// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 07-30-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-12-2015
// ***********************************************************************
// <copyright file="BlogController.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.BlogSite.Web.UI.Controllers
{
    #region

    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Configuration;
    using System.Web.Mvc;

    using RahulRai.Websites.BlogSite.Web.UI.GlobalAccess;
    using RahulRai.Websites.BlogSite.Web.UI.Models;
    using RahulRai.Websites.BlogSite.Web.UI.Services;
    using RahulRai.Websites.Utilities.AzureStorage.BlobStorage;
    using RahulRai.Websites.Utilities.AzureStorage.TableStorage;
    using RahulRai.Websites.Utilities.Common.Entities;
    using RahulRai.Websites.Utilities.Common.RegularTypes;
    using RahulRai.Websites.Utilities.Web;

    #endregion

    /// <summary>
    /// Class BlogController.
    /// </summary>
    public class BlogController : BaseController
    {
        #region Fields

        /// <summary>
        /// The BLOB storage context
        /// </summary>
        private readonly BlobStorageService blobStorageContext = SurveyStoreAccess.Instance.BlobStorageService;

        /// <summary>
        /// The blog context
        /// </summary>
        private readonly AzureTableStorageService<TableBlogEntity> blogContext = BlogStoreAccess.Instance.BlogTable;

        /// <summary>
        /// The page size
        /// </summary>
        private readonly int pageSize =
            int.Parse(WebConfigurationManager.AppSettings[ApplicationConstants.BlogListPageSize]);

        /// <summary>
        /// The search records
        /// </summary>
        private readonly int searchRecordsSize =
            int.Parse(WebConfigurationManager.AppSettings[ApplicationConstants.SearchRecordsSize]);

        /// <summary>
        /// The survey connection string
        /// </summary>
        private readonly string surveyConnectionString =
            WebConfigurationManager.AppSettings[ApplicationConstants.SurveyConnectionString];

        /// <summary>
        /// The blog service
        /// </summary>
        private BlogService blogService;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Goes the archive.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public async Task<ActionResult> Archive()
        {
            var blogList = await Task.Run(() => this.blogService.GetBlogArchive());
            //// Group results by month and year.
            var groupedBlogPosts = from post in blogList
                                   group post by post.PostedDate.Year
                                       into yearGroup
                                       let postYear = yearGroup.Key
                                       orderby postYear descending
                                       select new Archive
                                           {
                                               Year = postYear,
                                               MonthGroups = from yearPost in yearGroup
                                                             group yearPost by yearPost.PostedDate.Month
                                                                 into monthGroup
                                                                 let postMonth = monthGroup.Key
                                                                 orderby postMonth descending
                                                                 select
                                                                     new MonthGroup
                                                                         {
                                                                             Month = postMonth,
                                                                             Posts = monthGroup.ToList()
                                                                         }
                                           };
            return this.View(groupedBlogPosts);
        }

        /// <summary>
        /// Gets the blog post.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns>ActionResult.</returns>
        public async Task<ActionResult> GetBlogPost(string postId)
        {
            if (string.IsNullOrWhiteSpace(postId))
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
                this.Response.TrySkipIisCustomErrors = true;
                return this.View();
            }

            var blogPost = await Task.Run(() => this.blogService.GetBlogPost(postId));
            if (null != blogPost)
            {
                return this.View("BlogPost", blogPost);
            }

            this.Response.StatusCode = (int)HttpStatusCode.NotFound;
            this.Response.TrySkipIisCustomErrors = true;
            return this.View();
        }

        /// <summary>
        /// Gets the latest blogs.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public async Task<ActionResult> GetLatestBlogs()
        {
            UserPageDictionary.PageDictionary = null;
            var blogPosts = await this.blogService.GetLatestBlogs();
            this.SetPreviousNextPage(0);
            return this.View("BlogList", blogPosts);
        }

        /// <summary>
        /// Pages the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public async Task<ActionResult> Page(int id)
        {
            if (id > UserPageDictionary.PageDictionary.MaximumPageNumber || id < 0)
            {
                this.RedirectToAction("GetLatestBlogs");
            }

            var blogList = await this.blogService.GetBlogsForPage(id);
            this.SetPreviousNextPage(id);
            return this.View("BlogList", blogList);
        }

        /// <summary>
        /// Gets the searched blogs.
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
        /// Surveys the specified survey name.
        /// </summary>
        /// <param name="surveyName">Name of the survey.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        public async Task<ActionResult> Survey(string surveyName)
        {
            var surveyList = await Task.Run(() => this.blogService.GetAvailableSurveys(surveyName));
            return this.View(surveyList);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the action.
        /// </summary>
        protected override void InitializeAction()
        {
            this.blogService = new BlogService(
                this.blobStorageContext,
                this.blogContext,
                this.pageSize,
                this.searchRecordsSize);
        }

        /// <summary>
        /// Sets the previous next page.
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

        #endregion
    }
}