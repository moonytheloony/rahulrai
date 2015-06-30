// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 04-15-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
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
    using System.Linq;
    using System.Web.Mvc;
    using GlobalAccess;
    using Microsoft.WindowsAzure.Storage.Table;
    using Microsoft.WindowsAzure.Storage.Table.Queryable;
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
        ///     The row key to use
        /// </summary>
        private readonly string rowKeyToUse = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);

        /// <summary>
        ///     Gets the continuation stack.
        /// </summary>
        /// <value>The continuation stack.</value>
        public ContinuationStack ContinuationStack
        {
            get
            {
                var stack = (ContinuationStack)this.Session[ApplicationConstants.StackKey];
                if (stack != null)
                {
                    return stack;
                }

                stack = new ContinuationStack();
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
            ////Session[ApplicationConstants.StackKey] = null;
            ////var result = GetPagedBlogPreviews(null);
            ////var resultBlogs = result.Select(AzureTableStorageAssist.ConvertDynamicEntityToEntity<TableBlogEntity>);
            ////var blogList = resultBlogs.Select(TableBlogEntity.GetBlogPost);
            ////SetPreviousNextPage();
            return this.View("BlogList", new List<BlogPost>());
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
            var segment = this.ContinuationStack.GetBackToken();
            var entityResult = this.GetPagedBlogPreviews(segment);
            var resultBlogs = entityResult.Select(AzureTableStorageAssist.ConvertDynamicEntityToEntity<TableBlogEntity>);
            var blogList = resultBlogs.Select(TableBlogEntity.GetBlogPost);
            this.SetPreviousNextPage();
            return this.View("BlogList", blogList);
        }

        /// <summary>
        ///     Goes the next.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult GoNext()
        {
            var segment = this.ContinuationStack.GetForwardToken();
            var entityResult = this.GetPagedBlogPreviews(segment);
            var resultBlogs = entityResult.Select(AzureTableStorageAssist.ConvertDynamicEntityToEntity<TableBlogEntity>);
            var blogList = resultBlogs.Select(TableBlogEntity.GetBlogPost);
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
        ///     Gets the paged blog previews.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>TableQuerySegment&lt;DynamicTableEntity&gt;.</returns>
        private TableQuerySegment<DynamicTableEntity> GetPagedBlogPreviews(TableContinuationToken token)
        {
            var activeTable = this.blogContext.CustomOperation();
            var query = (from record in activeTable.CreateQuery<DynamicTableEntity>()
                         where record.PartitionKey == ApplicationConstants.BlogKey
                               && record["IsDraft"].BooleanValue == false
                               && record["IsDeleted"].BooleanValue == false
                               && string.Compare(record.RowKey, this.rowKeyToUse, StringComparison.OrdinalIgnoreCase) > 0
                         select record).Take(this.pageSize);
            var result = query.AsTableQuery().ExecuteSegmented(token, this.blogContext.TableRequestOptions);
            this.ContinuationStack.AddToken(result.ContinuationToken);
            return result;
        }

        /// <summary>
        ///     Sets the previous next page.
        /// </summary>
        private void SetPreviousNextPage()
        {
        }
    }
}