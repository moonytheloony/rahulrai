﻿// ***********************************************************************
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
    using GlobalAccess;
    using Microsoft.WindowsAzure.Storage.Table;
    using Microsoft.WindowsAzure.Storage.Table.Queryable;
    using Utilities.AzureStorage.TableStorage;
    using Utilities.Common.Entities;
    using Utilities.Common.RegularTypes;

    #endregion

    /// <summary>
    ///     Class BlogService.
    /// </summary>
    public class BlogService
    {
        /// <summary>
        ///     The blog context
        /// </summary>
        private readonly AzureTableStorageService<TableBlogEntity> blogContext;

        /// <summary>
        ///     The page size
        /// </summary>
        private readonly int pageSize;

        /// <summary>
        ///     The row key to use
        /// </summary>
        private readonly string rowKeyToUse = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);

        /// <summary>
        ///     The search records size
        /// </summary>
        private readonly int searchRecordsSize;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BlogService" /> class.
        /// </summary>
        /// <param name="blogContext">The blog context.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="searchRecordsSize">Size of the search records.</param>
        public BlogService(AzureTableStorageService<TableBlogEntity> blogContext, int pageSize, int searchRecordsSize)
        {
            this.blogContext = blogContext;
            this.pageSize = pageSize;
            this.searchRecordsSize = searchRecordsSize;
        }

        /// <summary>
        ///     Gets the latest blogs.
        /// </summary>
        /// <returns>List&lt;BlogPost&gt;.</returns>
        public List<BlogPost> GetLatestBlogs()
        {
            var result = this.GetPagedBlogPreviews(null);
            return result.Select(TableBlogEntity.GetBlogPost).ToList();
        }

        /// <summary>
        ///     Goes to previous blog list.
        /// </summary>
        /// <returns>List&lt;BlogPost&gt;.</returns>
        public List<BlogPost> GoToPreviousBlogList()
        {
            var segment = UserContinuationStack.ContinuationStack.GetBackToken();
            var resultBlogs = this.GetPagedBlogPreviews(segment);
            return resultBlogs.Select(TableBlogEntity.GetBlogPost).ToList();
        }

        /// <summary>
        ///     Goes to next blog list.
        /// </summary>
        /// <returns>List&lt;BlogPost&gt;.</returns>
        public List<BlogPost> GoToNextBlogList()
        {
            var segment = UserContinuationStack.ContinuationStack.GetForwardToken();
            var entityResult = this.GetPagedBlogPreviews(segment);
            return entityResult.Select(TableBlogEntity.GetBlogPost).ToList();
        }

        /// <summary>
        ///     Gets the paged blog previews.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>IEnumerable&lt;TableBlogEntity&gt;.</returns>
        private IEnumerable<TableBlogEntity> GetPagedBlogPreviews(TableContinuationToken token)
        {
            var activeTable = this.blogContext.CustomOperation();
            var query = (from record in activeTable.CreateQuery<DynamicTableEntity>()
                         where record.PartitionKey == ApplicationConstants.BlogKey
                               && record.Properties["IsDraft"].BooleanValue == false
                               && record.Properties["IsDeleted"].BooleanValue == false
                               && string.Compare(record.RowKey, this.rowKeyToUse, StringComparison.OrdinalIgnoreCase) > 0
                         select record).Take(this.pageSize);
            var result = query.AsTableQuery().ExecuteSegmented(token, this.blogContext.TableRequestOptions);
            UserContinuationStack.ContinuationStack.AddToken(result.ContinuationToken);
            return result.Select(element => element.ConvertDynamicEntityToEntity<TableBlogEntity>());
        }

        /// <summary>
        ///     Gets the searched blog previews.
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
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, ApplicationConstants.BlogKey);
            var isDraftCondition = TableQuery.GenerateFilterConditionForBool("IsDraft", QueryComparisons.Equal, false);
            var isDeletedCondition = TableQuery.GenerateFilterConditionForBool("IsDeleted", QueryComparisons.Equal, false);
            foreach (var rowkey in rowKeyList)
            {
                var rowFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowkey);
                rowKeyFilterCondition = string.IsNullOrWhiteSpace(rowKeyFilterCondition) ? rowFilter : TableQuery.CombineFilters(rowKeyFilterCondition, TableOperators.Or, rowFilter);
            }

            var activeTable = this.blogContext.CustomOperation();
            var combinedQuery = activeTable.CreateQuery<DynamicTableEntity>().Where(
                TableQuery.CombineFilters(
                    TableQuery.CombineFilters(
                        TableQuery.CombineFilters(
                            partitionKeyFilter,
                            TableOperators.And,
                            rowKeyFilterCondition),
                         TableOperators.And,
                         isDraftCondition),
                    TableOperators.And,
                   isDeletedCondition)).Take(this.searchRecordsSize);
            var result = combinedQuery.AsTableQuery().ExecuteSegmented(null, this.blogContext.TableRequestOptions);
            return result.Select(element => element.ConvertDynamicEntityToEntity<TableBlogEntity>());
        }
    }
}