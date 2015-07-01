// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 06-30-2015
//
// Last Modified By : rahulrai
// Last Modified On : 07-01-2015
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
    using System.Web;
    using Microsoft.WindowsAzure.Storage.Table;
    using Microsoft.WindowsAzure.Storage.Table.Queryable;
    using Utilities.AzureStorage.TableStorage;
    using Utilities.Common.Entities;
    using Utilities.Common.RegularTypes;

    #endregion

    /// <summary>
    /// Class BlogService.
    /// </summary>
    public class BlogService
    {
        /// <summary>
        /// The blog context
        /// </summary>
        private readonly AzureTableStorageService<TableBlogEntity> blogContext;

        /// <summary>
        /// The continuation stack
        /// </summary>
        private readonly ContinuationStack continuationStack;

        /// <summary>
        /// The page size
        /// </summary>
        private readonly int pageSize;

        /// <summary>
        /// The row key to use
        /// </summary>
        private readonly string rowKeyToUse = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);

        /// <summary>
        /// The session
        /// </summary>
        private readonly HttpSessionStateBase session;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogService"/> class.
        /// </summary>
        /// <param name="continuationStack">The continuation stack.</param>
        /// <param name="httpSessionStateBase">The HTTP session state base.</param>
        /// <param name="blogContext">The blog context.</param>
        /// <param name="pageSize">Size of the page.</param>
        public BlogService(ContinuationStack continuationStack, HttpSessionStateBase httpSessionStateBase, AzureTableStorageService<TableBlogEntity> blogContext, int pageSize)
        {
            this.continuationStack = continuationStack;
            this.session = httpSessionStateBase;
            this.blogContext = blogContext;
            this.pageSize = pageSize;
        }

        /// <summary>
        /// Gets the latest blogs.
        /// </summary>
        /// <returns>List&lt;BlogPost&gt;.</returns>
        public List<BlogPost> GetLatestBlogs()
        {
            this.session[ApplicationConstants.StackKey] = null;
            var result = this.GetPagedBlogPreviews(null);
            return result.Select(TableBlogEntity.GetBlogPost).ToList();
        }

        /// <summary>
        /// Goes to previous blog list.
        /// </summary>
        /// <returns>List&lt;BlogPost&gt;.</returns>
        public List<BlogPost> GoToPreviousBlogList()
        {
            var segment = this.continuationStack.GetBackToken();
            var resultBlogs = this.GetPagedBlogPreviews(segment);
            return resultBlogs.Select(TableBlogEntity.GetBlogPost).ToList();
        }

        /// <summary>
        /// Goes to next blog list.
        /// </summary>
        /// <returns>List&lt;BlogPost&gt;.</returns>
        public List<BlogPost> GoToNextBlogList()
        {
            var segment = this.continuationStack.GetForwardToken();
            var entityResult = this.GetPagedBlogPreviews(segment);
            return entityResult.Select(TableBlogEntity.GetBlogPost).ToList();
        }

        /// <summary>
        /// Gets the paged blog previews.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>IEnumerable&lt;TableBlogEntity&gt;.</returns>
        private IEnumerable<TableBlogEntity> GetPagedBlogPreviews(TableContinuationToken token)
        {
            var activeTable = this.blogContext.CustomOperation();
            var query = (from record in activeTable.CreateQuery<TableBlogEntity>()
                         where record.PartitionKey == ApplicationConstants.BlogKey
                               && record.IsDraft == false
                               && record.IsDeleted == false
                               && string.Compare(record.RowKey, this.rowKeyToUse, StringComparison.OrdinalIgnoreCase) > 0
                         select record).Take(this.pageSize);
            var result = query.AsTableQuery().ExecuteSegmented(token, this.blogContext.TableRequestOptions);
            this.continuationStack.AddToken(result.ContinuationToken);
            return result;
        }
    }
}