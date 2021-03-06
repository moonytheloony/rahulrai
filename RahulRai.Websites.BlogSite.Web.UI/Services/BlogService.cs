﻿// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 07-30-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-20-2015
// ***********************************************************************
// <copyright file="BlogService.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.BlogSite.Web.UI.Services
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Microsoft.WindowsAzure.Storage.Table;
    using Microsoft.WindowsAzure.Storage.Table.Queryable;

    using RahulRai.Websites.BlogSite.Web.UI.GlobalAccess;
    using RahulRai.Websites.Utilities.AzureStorage.QueueStorage;
    using RahulRai.Websites.Utilities.AzureStorage.TableStorage;
    using RahulRai.Websites.Utilities.Common.Entities;
    using RahulRai.Websites.Utilities.Common.Helpers;
    using RahulRai.Websites.Utilities.Common.RegularTypes;
    using RahulRai.Websites.Utilities.Web;

    #endregion

    /// <summary>
    /// Class BlogService.
    /// </summary>
    public class BlogService
    {
        #region Fields

        /// <summary>
        /// The blog context
        /// </summary>
        private readonly AzureTableStorageService<TableBlogEntity> blogContext;

        /// <summary>
        /// The newsletter context
        /// </summary>
        private readonly AzureTableStorageService<TableNewsletterEntity> newsletterContext;

        /// <summary>
        /// The new subscriber queue context
        /// </summary>
        private readonly AzureQueueService newSubscriberQueueContext;

        /// <summary>
        /// The page size
        /// </summary>
        private readonly int pageSize;

        /// <summary>
        /// The row key to use
        /// </summary>
        private readonly string rowKeyToUse = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);

        /// <summary>
        /// The search records size
        /// </summary>
        private readonly int searchRecordsSize;

        /// <summary>
        /// The blog search access
        /// </summary>
        private BlogSearchAccess blogSearchAccess;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogService" /> class.
        /// </summary>
        /// <param name="blogContext">The blog context.</param>
        /// <param name="newsletterContext">The newsletter context.</param>
        /// <param name="newSubscriberQueueContext">The new subscriber queue context.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="searchRecordsSize">Size of the search records.</param>
        public BlogService(
            AzureTableStorageService<TableBlogEntity> blogContext,
            AzureTableStorageService<TableNewsletterEntity> newsletterContext,
            AzureQueueService newSubscriberQueueContext,
            int pageSize,
            int searchRecordsSize)
        {
            this.blogContext = blogContext;
            this.newsletterContext = newsletterContext;
            this.newSubscriberQueueContext = newSubscriberQueueContext;
            this.pageSize = pageSize;
            this.searchRecordsSize = searchRecordsSize;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Activates the user subscription.
        /// </summary>
        /// <param name="userString">The user string.</param>
        /// <returns>NewsletterSignUpState.</returns>
        public NewsletterSignUpState ActivateUserSubscription(string userString)
        {
            var newsletterTable = this.newsletterContext.CustomOperation();
            var foundUser = (from record in newsletterTable.CreateQuery<TableNewsletterEntity>()
                             where
                                 record.PartitionKey == ApplicationConstants.SubscriberListKey
                                     && record.VerificationString == userString
                             select record).FirstOrDefault();
            if (null == foundUser)
            {
                return NewsletterSignUpState.UserDoesNotExist;
            }

            foundUser.IsVerified = true;
            this.newsletterContext.InsertOrReplace(foundUser);
            this.newsletterContext.Save();
            return NewsletterSignUpState.Success;
        }

        /// <summary>
        /// Adds the user to newsletter subscriber list.
        /// </summary>
        /// <param name="signUpForm">The sign up form.</param>
        /// <returns>NewsletterSignUpState.</returns>
        public NewsletterSignUpState AddUserToNewsletterSubscriberList(NewsletterSignUpForm signUpForm)
        {
            var newsletterTable = this.newsletterContext.CustomOperation();
            var foundUser = (from record in newsletterTable.CreateQuery<TableNewsletterEntity>()
                             where
                                 record.PartitionKey == ApplicationConstants.SubscriberListKey
                                     && record.RowKey == signUpForm.Email
                             select record).FirstOrDefault();

            //// If user does not exist. Create a user.
            if (null == foundUser)
            {
                signUpForm.CreatedDate = DateTime.UtcNow;
                signUpForm.LastEmailIdentifier = string.Empty;
                signUpForm.VerificationString = Guid.NewGuid().ToString();
                signUpForm.UnsubscribeString = Guid.NewGuid().ToString();
                signUpForm.IsVerified = false;
                var tableEntity = new TableNewsletterEntity(signUpForm);
                this.newsletterContext.InsertOrReplace(tableEntity);
                this.newsletterContext.Save();
                //// Add message to queue.
                this.AddMessageToNewUserQueue(signUpForm.Email);
                return NewsletterSignUpState.Success;
            }

            //// If user exists and is not verified, reactivate by changing verification string and date.
            if (!foundUser.IsVerified)
            {
                signUpForm.CreatedDate = DateTime.UtcNow;
                signUpForm.LastEmailIdentifier = string.Empty;
                signUpForm.VerificationString = Guid.NewGuid().ToString();
                signUpForm.UnsubscribeString = Guid.NewGuid().ToString();
                signUpForm.IsVerified = false;
                var tableEntity = new TableNewsletterEntity(signUpForm);
                this.newsletterContext.InsertOrReplace(tableEntity);
                this.newsletterContext.Save();
                //// Add message to queue.
                this.AddMessageToNewUserQueue(signUpForm.Email);
                return NewsletterSignUpState.Success;
            }

            //// If user exists. Do nothing.
            return NewsletterSignUpState.UserExists;
        }

        /// <summary>
        /// Gets the blog archive.
        /// </summary>
        /// <returns>List of blogs</returns>
        public IList<BlogPost> GetBlogArchive()
        {
            return this.BlogArchive(null, true);
        }

        /// <summary>
        /// Gets the blog post.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns>BlogPost.</returns>
        public BlogPost GetBlogPost(string postId)
        {
            var activeTable = this.blogContext.CustomOperation();
            var query = (from record in activeTable.CreateQuery<DynamicTableEntity>()
                         where
                             record.PartitionKey == ApplicationConstants.BlogKey
                                 && record.Properties["IsDeleted"].BooleanValue == false
                                 && record.Properties["FormattedUri"].StringValue.Equals(
                                     postId,
                                     StringComparison.OrdinalIgnoreCase)
                         select record).Take(this.pageSize);
            var result = query.AsTableQuery().ExecuteSegmented(null, this.blogContext.TableRequestOptions);
            if (!result.Any())
            {
                return null;
            }

            return
                TableBlogEntity.GetBlogPost(
                    result.Select(element => element.ConvertDynamicEntityToEntity<TableBlogEntity>()).FirstOrDefault());
        }

        /// <summary>
        /// Gets the blogs for page.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>List&lt;BlogPost&gt;.</returns>
        public async Task<List<BlogPost>> GetBlogsForPage(int pageNumber)
        {
            var segment = UserPageDictionary.PageDictionary.GetPageContinuationToken(pageNumber);
            var resultBlogs =
                await this.GetPagedBlogPreviews(segment, !UserPageDictionary.PageDictionary.CanMoveForward());
            return resultBlogs.Select(TableBlogEntity.GetBlogPost).ToList();
        }

        /// <summary>
        /// Gets the blogs for RSS feed.
        /// </summary>
        /// <returns>Task&lt;List&lt;BlogPost&gt;&gt;.</returns>
        public async Task<List<BlogPost>> GetBlogsForRssFeed()
        {
            var result = await this.GetPagedBlogPreviews(null, false);
            return result.Select(TableBlogEntity.GetBlogPost).ToList();
        }

        /// <summary>
        /// Gets the latest blogs.
        /// </summary>
        /// <returns>List&lt;BlogPost&gt;.</returns>
        public async Task<List<BlogPost>> GetLatestBlogs()
        {
            IEnumerable<BlogPost> cacheResult;
            TableContinuationToken token;
            //// Check in cache first. Retry operation on first failure. Product Issue :(
            try
            {
                cacheResult = ApplicationCache.Get<IEnumerable<BlogPost>>(ApplicationConstants.BlogsCacheKey);
                token = ApplicationCache.Get<TableContinuationToken>(ApplicationConstants.BlogsFirstTokenCacheKey);
            }
            catch (Exception)
            {
                cacheResult = ApplicationCache.Get<IEnumerable<BlogPost>>(ApplicationConstants.BlogsCacheKey);
                token = ApplicationCache.Get<TableContinuationToken>(ApplicationConstants.BlogsFirstTokenCacheKey);
            }

            if (null != cacheResult)
            {
                //// We need to add token to user page dictionary as well.
                if (null != token)
                {
                    UserPageDictionary.PageDictionary.AddPage(token);
                }

                return cacheResult.ToList();
            }

            var result = await this.GetPagedBlogPreviews(null, true);
            var firstPageBlogs = result.Select(TableBlogEntity.GetBlogPost).ToList();
            ApplicationCache.Set(ApplicationConstants.BlogsCacheKey, firstPageBlogs);
            return firstPageBlogs;
        }

        /// <summary>
        /// Searches the blogs.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>blogs searched</returns>
        public IEnumerable<BlogPost> SearchBlogs(string searchTerm)
        {
            this.blogSearchAccess = BlogSearchAccess.Instance;
            var searchService = this.blogSearchAccess.AzureSearchService;
            var searchedBlogs = searchService.SearchDocuments(searchTerm, null, this.pageSize);
            var foundBlogs = searchedBlogs as IList<BlogSearch> ?? searchedBlogs.ToList();
            return !foundBlogs.Any()
                ? null
                : this.GetSearchedBlogPreviews(foundBlogs.Select(attribute => attribute.BlogId))
                    .Select(TableBlogEntity.GetBlogPost);
        }

        /// <summary>
        /// Unsubscribes the user.
        /// </summary>
        /// <param name="userString">The user string.</param>
        /// <returns>NewsletterSignUpState.</returns>
        public NewsletterSignUpState UnsubscribeUser(string userString)
        {
            var newsletterTable = this.newsletterContext.CustomOperation();
            var foundUser = (from record in newsletterTable.CreateQuery<TableNewsletterEntity>()
                             where record.PartitionKey == ApplicationConstants.SubscriberListKey && record.VerificationString == userString
                             select record).FirstOrDefault();
            if (null == foundUser)
            {
                return NewsletterSignUpState.UserDoesNotExist;
            }

            //// Send email to user.
            try
            {
                this.AddMessageToUnsubsribeUserQueue(foundUser.Email);
            }
            catch (Exception exception)
            {
                TraceUtility.LogError(exception);
                return NewsletterSignUpState.UnsubscribeMailFailed;
            }

            return NewsletterSignUpState.UnsubsribeMailSent;
        }

        /// <summary>
        /// Complete unsubscription process by deleting user data.
        /// </summary>
        /// <param name="userString">User unsubscribe string.</param>
        /// <returns>Status of deletion.</returns>
        public NewsletterSignUpState CompleteUnsubscritionProcess(string userString)
        {
            var newsletterTable = this.newsletterContext.CustomOperation();
            var foundUser = (from record in newsletterTable.CreateQuery<TableNewsletterEntity>()
                             where
                                 record.PartitionKey == ApplicationConstants.SubscriberListKey
                                     && record.UnsubscribeString == userString
                             select record).FirstOrDefault();
            if (null == foundUser)
            {
                return NewsletterSignUpState.UserDoesNotExist;
            }

            //// Delete user record.
            this.newsletterContext.Delete(foundUser);
            this.newsletterContext.Save();
            return NewsletterSignUpState.Unsubscribed;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sanitizes the search term.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>Error messages</returns>
        internal IEnumerable<string> SanitizeSearchTerm(ref string searchTerm)
        {
            var errorList = new List<string>();

            //// Not empty or whitespace.
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                errorList.Add("Search term can not be empty.");
                return errorList;
            }

            searchTerm = searchTerm.ToLowerInvariant().Trim();

            //// Remove multispaces
            searchTerm = Regex.Replace(searchTerm.Trim(), @"\s+", " ");

            //// Appropriate length.
            if (searchTerm.Length < 2 || searchTerm.Length > 20)
            {
                errorList.Add("Search term should have at least 2 and at most 20 characters.");
            }

            //// No special characters.
            if (!Regex.IsMatch(searchTerm, @"^([ a-zA-Z0-9-\*]+)$"))
            {
                errorList.Add("You can use only alphabets, numbers, spaces, dashes and astericks in your search query. Use * for wildcard matching.");
            }

            return errorList;
        }

        /// <summary>
        /// Adds the message to new user queue.
        /// </summary>
        /// <param name="email">The email.</param>
        private void AddMessageToNewUserQueue(string email)
        {
            this.newSubscriberQueueContext.AddMessageToQueue(email);
        }

        /// <summary>
        /// Adds the message to 
        /// </summary>
        /// <param name="email">The email.</param>
        private void AddMessageToUnsubsribeUserQueue(string email)
        {
            this.newSubscriberQueueContext.AddMessageToQueue(string.Format("Unsubscribe:{0}", email));
        }

        /// <summary>
        /// Blogs the archive.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="shouldAddPage">if set to <c>true</c> [should add page].</param>
        /// <returns>List of blogs</returns>
        private IList<BlogPost> BlogArchive(TableContinuationToken token, bool shouldAddPage)
        {
            var activeTable = this.blogContext.CustomOperation();
            var query =
                new TableQuery<DynamicTableEntity>().Where(
                    TableQuery.CombineFilters(
                        TableQuery.GenerateFilterConditionForBool("IsDraft", QueryComparisons.Equal, false),
                        TableOperators.And,
                        TableQuery.GenerateFilterConditionForBool("IsDeleted", QueryComparisons.Equal, false)))
                    .Select(new[] { "Title", "PostedDate" });
            EntityResolver<BlogPost> resolver =
                (pk, rk, ts, props, etag) =>
                    new BlogPost
                        {
                            Title = props["Title"].StringValue,
                            PostedDate = props["PostedDate"].DateTime ?? DateTime.MinValue,
                            BlogId = rk
                        };
            var result = activeTable.ExecuteQuerySegmented(query, resolver, token, this.blogContext.TableRequestOptions);
            if (shouldAddPage && null != result.ContinuationToken)
            {
                UserPageDictionary.PageDictionary.AddPage(result.ContinuationToken);
            }

            return result.Results;
        }

        /// <summary>
        /// Gets the paged blog previews.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="shouldAddPage">if set to <c>true</c> [should add page].</param>
        /// <returns>IEnumerable&lt;TableBlogEntity&gt;.</returns>
        private async Task<IEnumerable<TableBlogEntity>> GetPagedBlogPreviews(
            TableContinuationToken token,
            bool shouldAddPage)
        {
            var activeTable = this.blogContext.CustomOperation();
            var query = (from record in activeTable.CreateQuery<DynamicTableEntity>()
                         where
                             record.PartitionKey == ApplicationConstants.BlogKey
                                 && record.Properties["IsDraft"].BooleanValue == false
                                 && record.Properties["IsDeleted"].BooleanValue == false
                                 && string.Compare(record.RowKey, this.rowKeyToUse, StringComparison.OrdinalIgnoreCase) > 0
                         select record).Take(this.pageSize);
            var result = await Task.Run(() => query.AsTableQuery().ExecuteSegmented(token, this.blogContext.TableRequestOptions));
            if (!shouldAddPage || null == result.ContinuationToken)
            {
                return result.Select(element => element.ConvertDynamicEntityToEntity<TableBlogEntity>());
            }

            UserPageDictionary.PageDictionary.AddPage(result.ContinuationToken);
            //// If this is the first page response. Add it to cache as well.
            if (null == token)
            {
                ApplicationCache.Set(ApplicationConstants.BlogsFirstTokenCacheKey, result.ContinuationToken);
            }

            return result.Select(element => element.ConvertDynamicEntityToEntity<TableBlogEntity>());
        }

        /// <summary>
        /// Gets the searched blog previews.
        /// </summary>
        /// <param name="rowkeys">The rowkeys.</param>
        /// <returns>IEnumerable&lt;TableBlogEntity&gt;.</returns>
        private IEnumerable<TableBlogEntity> GetSearchedBlogPreviews(IEnumerable<string> rowkeys)
        {
            var rowKeyList = rowkeys as IList<string> ?? rowkeys.ToList();
            if (!rowKeyList.Any())
            {
                return null;
            }

            var rowKeyFilterCondition = string.Empty;
            var isDraftCondition = TableQuery.GenerateFilterConditionForBool("IsDraft", QueryComparisons.Equal, false);
            var isDeletedCondition = TableQuery.GenerateFilterConditionForBool(
                "IsDeleted",
                QueryComparisons.Equal,
                false);
            foreach (var rowkey in rowKeyList)
            {
                var rowFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowkey);
                var partitionFilter = TableQuery.GenerateFilterCondition(
                    "PartitionKey",
                    QueryComparisons.Equal,
                    ApplicationConstants.BlogKey);
                var combinedFilter = TableQuery.CombineFilters(rowFilter, TableOperators.And, partitionFilter);
                rowKeyFilterCondition = string.IsNullOrWhiteSpace(rowKeyFilterCondition)
                    ? combinedFilter
                    : TableQuery.CombineFilters(rowKeyFilterCondition, TableOperators.Or, combinedFilter);
            }

            var activeTable = this.blogContext.CustomOperation();
            var combinedQuery =
                new TableQuery<DynamicTableEntity>().Where(
                    TableQuery.CombineFilters(
                        TableQuery.CombineFilters(rowKeyFilterCondition, TableOperators.And, isDraftCondition),
                        TableOperators.And,
                        isDeletedCondition)).Take(this.searchRecordsSize);
            var result = activeTable.ExecuteQuerySegmented(combinedQuery, null, this.blogContext.TableRequestOptions);
            return result.Select(element => element.ConvertDynamicEntityToEntity<TableBlogEntity>());
        }

        #endregion
    }
}