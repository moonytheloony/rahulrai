﻿// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 07-30-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-19-2015
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
    using System.Linq;
    using System.ServiceModel.Syndication;
    using System.Threading.Tasks;
    using System.Web.Configuration;
    using System.Web.Mvc;

    using RahulRai.Websites.BlogSite.Web.UI.GlobalAccess;
    using RahulRai.Websites.BlogSite.Web.UI.Models;
    using RahulRai.Websites.BlogSite.Web.UI.Services;
    using RahulRai.Websites.Utilities.AzureStorage.QueueStorage;
    using RahulRai.Websites.Utilities.AzureStorage.TableStorage;
    using RahulRai.Websites.Utilities.Common.Entities;
    using RahulRai.Websites.Utilities.Common.Helpers;
    using RahulRai.Websites.Utilities.Common.RegularTypes;
    using RahulRai.Websites.Utilities.Common.SurveyMonkey;
    using RahulRai.Websites.Utilities.Web;

    using SurveyMonkey;

    #endregion

    /// <summary>
    /// Class BlogController.
    /// </summary>
    public class BlogController : BaseController
    {
        #region Fields

        /// <summary>
        /// The blog context
        /// </summary>
        private readonly AzureTableStorageService<TableBlogEntity> blogContext = BlogStoreAccess.Instance.BlogTable;

        /// <summary>
        /// The queue context
        /// </summary>
        private readonly AzureQueueService newSubscriberQueueContext = NewSubscriberQueueAccess.Instance.AzureQueueService;

        /// <summary>
        /// The newsletter context
        /// </summary>
        private readonly AzureTableStorageService<TableNewsletterEntity> newsletterContext =
            NewsletterSubscriberAccess.Instance.NewsletterSubscriberTable;

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
                return new HttpNotFoundResult();
            }

            var blogPost = await Task.Run(() => this.blogService.GetBlogPost(postId));
            if (null != blogPost)
            {
                return this.View("BlogPost", blogPost);
            }

            return new HttpNotFoundResult();
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
        /// Newsletters the sign up.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [RequireHttps]
        public ActionResult NewsletterSignUp()
        {
            return this.View();
        }

        /// <summary>
        /// Newsletters the sign up.
        /// </summary>
        /// <param name="signUpForm">The sign up form.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [RequireHttps]
        [HttpPost]
        public async Task<ActionResult> NewsletterSignUp(NewsletterSignUpForm signUpForm)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(signUpForm);
            }

            var firstName = signUpForm.FirstName.Trim().ToLowerInvariant();
            signUpForm.FirstName = firstName.First().ToString().ToUpper() + firstName.Substring(1);
            signUpForm.Email = signUpForm.Email.ToLowerInvariant();
            this.ViewBag.FormSubmitted =
                await Task.Run(() => this.blogService.AddUserToNewsletterSubscriberList(signUpForm));
            return this.View();
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
        /// RSSs the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        public async Task<ActionResult> Rss(string id)
        {
            var blogPosts = await this.blogService.GetBlogsForRssFeed();
            var postItems =
                blogPosts.Select(
                    blogPost => new SyndicationItem(
                        blogPost.Title,
                        Routines.GeneratePreview(blogPost.Body),
                        // ReSharper disable once AssignNullToNotNullAttribute
                        new Uri(
                            this.Url.RouteUrl(
                                "BlogPost",
                                new
                                {
                                    postId = blogPost.BlogFormattedUri
                                },
                        // ReSharper disable once PossibleNullReferenceException
                                this.Request.Url.Scheme)),
                        blogPost.BlogFormattedUri,
                        new DateTimeOffset(blogPost.PostedDate)));
            var feed = new SyndicationFeed(
                "rahulrai.in",
                "Rahul Rai on Cloud, Technology and Code",
                // ReSharper disable once PossibleNullReferenceException
                new Uri(
                    string.Format(
                        "{0}://{1}{2}",
                        this.Request.Url.Scheme,
                        this.Request.Url.Authority,
                        this.Url.Content("~"))),
                postItems)
                {
                    Copyright =
                        new TextSyndicationContent(
                            string.Format("Copyright (c) Rahul Rai {0}. All rights reserved.", DateTime.UtcNow.Year)),
                    Language = "en-US"
                };

            int defaultValue;
            if (!int.TryParse(id, out defaultValue) || defaultValue == 0)
            {
                return new FeedResult(new Rss20FeedFormatter(feed));
            }

            return new FeedResult(new Atom10FeedFormatter(feed));
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
        /// <returns>ActionResult.</returns>
        public ActionResult Survey(string surveyName)
        {
            return this.View();
        }

        /// <summary>
        /// Surveys the specified survey name.
        /// </summary>
        /// <param name="surveyName">Name of the survey.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public async Task<ActionResult> SurveyData(string surveyName)
        {
            try
            {
                var surveyMonkeyAppKey = WebConfigurationManager.AppSettings[ApplicationConstants.SurveyMonkeyApiKey];
                var surveyMonkeyToken = WebConfigurationManager.AppSettings[ApplicationConstants.SurveyMonkeyToken];
                var surveyList = await Task.Run(
                    () =>
                    {
                        var surveyMonkeyService = new SurveyMonkeyService(surveyMonkeyAppKey, surveyMonkeyToken);
                        return surveyMonkeyService.GetAvailableSurveys();
                    }) ?? new List<Survey>();

                return this.PartialView(surveyList.ToList());
            }
            catch (Exception exception)
            {
                TraceUtility.LogError(exception);
                return this.PartialView(new List<Survey>());
            }
        }

        /// <summary>
        /// Unsubscribe the specified user string.
        /// </summary>
        /// <param name="userString">The user string.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [RequireHttps]
        public async Task<ActionResult> Unsubscribe(string userString)
        {
            Guid testGuid;
            if (string.IsNullOrWhiteSpace(userString) || !Guid.TryParse(userString, out testGuid))
            {
                return this.View(NewsletterSignUpState.NoInput);
            }

            var response = await Task.Run(() => this.blogService.UnsubscribeUser(userString));
            return this.View(response);
        }

        /// <summary>
        /// Completes the unsubscribe process by deleting user data.
        /// </summary>
        /// <param name="userString">User unsubscribe string</param>
        /// <returns>Status of unsubscribe</returns>
        public async Task<ActionResult> CompleteUnsubscritionProcess(string userString)
        {
            Guid testGuid;
            if (string.IsNullOrWhiteSpace(userString) || !Guid.TryParse(userString, out testGuid))
            {
                return this.View("Unsubscribe", NewsletterSignUpState.NoInput);
            }

            var response = await Task.Run(() => this.blogService.CompleteUnsubscritionProcess(userString));
            return this.View("Unsubscribe", response);
        }

        /// <summary>
        /// Activates the subscription.
        /// </summary>
        /// <param name="userString">The user string.</param>
        /// <returns>Task&lt;ActionResult&gt;.</returns>
        [RequireHttps]
        public async Task<ActionResult> ActivateSubscription(string userString)
        {
            Guid testGuid;
            if (string.IsNullOrWhiteSpace(userString) || !Guid.TryParse(userString, out testGuid))
            {
                return this.View(NewsletterSignUpState.NoInput);
            }

            var response = await Task.Run(() => this.blogService.ActivateUserSubscription(userString));
            return this.View(response);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the action.
        /// </summary>
        protected override void InitializeAction()
        {
            this.blogService = new BlogService(
                this.blogContext,
                this.newsletterContext,
                this.newSubscriberQueueContext,
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