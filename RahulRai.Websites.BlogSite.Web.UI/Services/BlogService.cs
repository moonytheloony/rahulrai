// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 07-30-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-12-2015
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
    using RahulRai.Websites.Utilities.AzureStorage.TableStorage;
    using RahulRai.Websites.Utilities.Common.Entities;
    using RahulRai.Websites.Utilities.Common.RegularTypes;

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
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="searchRecordsSize">Size of the search records.</param>
        public BlogService(
            AzureTableStorageService<TableBlogEntity> blogContext,
            int pageSize,
            int searchRecordsSize)
        {
            this.blogContext = blogContext;
            this.pageSize = pageSize;
            this.searchRecordsSize = searchRecordsSize;
        }

        #endregion

        #region Public Methods and Operators

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
        /// Gets the latest blogs.
        /// </summary>
        /// <returns>List&lt;BlogPost&gt;.</returns>
        public async Task<List<BlogPost>> GetLatestBlogs()
        {
            var result = await this.GetPagedBlogPreviews(null, true);
            return result.Select(TableBlogEntity.GetBlogPost).ToList();
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
            if (!Regex.IsMatch(searchTerm, @"^([ a-zA-Z0-9-]+)$"))
            {
                errorList.Add("You can only use alphabets, numbers, spaces and dashes in your search query.");
            }

            return errorList;
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
                                 && string.Compare(record.RowKey, this.rowKeyToUse, StringComparison.OrdinalIgnoreCase)
                                     > 0
                         select record).Take(this.pageSize);
            var result =
                await Task.Run(() => query.AsTableQuery().ExecuteSegmented(token, this.blogContext.TableRequestOptions));
            if (shouldAddPage && null != result.ContinuationToken)
            {
                UserPageDictionary.PageDictionary.AddPage(result.ContinuationToken);
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
                    ? rowFilter
                    : TableQuery.CombineFilters(combinedFilter, TableOperators.Or, rowFilter);
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