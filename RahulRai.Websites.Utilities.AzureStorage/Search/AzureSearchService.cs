// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.AzureStorage
// Author           : rahulrai
// Created          : 05-30-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="AzureSearchService.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.AzureStorage.Search
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Common.Entities;
    using Common.Exceptions;
    using Microsoft.Azure.Search;
    using Microsoft.Azure.Search.Models;

    #endregion

    /// <summary>
    ///     Class AzureSearchService.
    /// </summary>
    public class AzureSearchService : IDisposable
    {
        /// <summary>
        ///     The index client
        /// </summary>
        private readonly SearchIndexClient indexClient;

        /// <summary>
        ///     The service client
        /// </summary>
        private readonly SearchServiceClient serviceClient;

        /// <summary>
        ///     The disposed
        /// </summary>
        private bool disposed;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AzureSearchService" /> class.
        /// </summary>
        /// <param name="searchServiceName">Name of the search service.</param>
        /// <param name="searchServiceKey">The search service key.</param>
        /// <param name="searchIndex">Index of the search.</param>
        public AzureSearchService(string searchServiceName, string searchServiceKey, string searchIndex)
        {
            this.serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(searchServiceKey));
            CreateIndexIfNotExists(searchIndex, this.serviceClient);
            this.indexClient = this.serviceClient.Indexes.GetClient(searchIndex);
        }

        /// <summary>
        ///     Prevents a default instance of the <see cref="AzureSearchService" /> class from being created.
        /// </summary>
        private AzureSearchService()
        {
            //// Default constructor not allowed.
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="AzureSearchService" /> class.
        /// </summary>
        ~AzureSearchService()
        {
            this.Dispose(false);
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Upserts the index of the data to.
        /// </summary>
        /// <param name="blogSearchEntity">The blog search entity.</param>
        /// <exception cref="BlogSystemException">failed on index</exception>
        /// <exception cref="RahulRai.Websites.Utilities.Common.Exceptions.BlogSystemException">failed on index</exception>
        public void UpsertDataToIndex(BlogSearch blogSearchEntity)
        {
            var documents = new[] { blogSearchEntity };
            this.indexClient.Documents.Index(IndexBatch.Create(documents.Select(IndexAction.Create)));
        }

        /// <summary>
        ///     Searches the documents.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>IEnumerable&lt;BlogSearch&gt;.</returns>
        public IEnumerable<BlogSearch> SearchDocuments(string searchText, string filter = null)
        {
            // Execute search based on search text and optional filter
            var searchParameter = new SearchParameters();

            if (!string.IsNullOrEmpty(filter))
            {
                searchParameter.Filter = filter;
            }

            var response = this.indexClient.Documents.Search<BlogSearch>(searchText, searchParameter);
            return response.Select(result => result.Document);
        }

        /// <summary>
        ///     Deletes the data.
        /// </summary>
        /// <param name="idToDelete">The identifier to delete.</param>
        /// <exception cref="BlogSystemException">failed on index</exception>
        /// <exception cref="RahulRai.Websites.Utilities.Common.Exceptions.BlogSystemException">failed on index</exception>
        public void DeleteData(string idToDelete)
        {
            var batch = IndexBatch.Create(
                      new IndexAction(IndexActionType.Delete, new Document { { "blogId", idToDelete } }));
            this.indexClient.Documents.Index(batch);
        }

        /// <summary>
        ///     Deletes the index.
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        public void DeleteIndex(string indexName)
        {
            if (this.serviceClient.Indexes.Exists(indexName))
            {
                this.serviceClient.Indexes.Delete(indexName);
            }
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.indexClient.Dispose();
                    this.serviceClient.Dispose();
                }
            }

            this.disposed = true;
        }

        /// <summary>
        ///     Creates the index of the search.
        /// </summary>
        /// <param name="searchIndex">Index of the search.</param>
        /// <param name="serviceClient">The service client.</param>
        private static void CreateSearchIndex(string searchIndex, ISearchServiceClient serviceClient)
        {
            var definition = new Index
            {
                Name = searchIndex,
                Fields = new[]
                {
                    new Field("blogId", DataType.String)
                    {
                        IsKey = true
                    },
                    new Field("title", DataType.String)
                    {
                        IsSearchable = true,
                        IsFilterable = true
                    },
                    new Field("searchTags", DataType.Collection(DataType.String))
                    {
                        IsSearchable = true,
                        IsFilterable = true,
                        IsFacetable = true
                    }
                }
            };

            serviceClient.Indexes.Create(definition);
        }

        /// <summary>
        ///     Creates the index if not exists.
        /// </summary>
        /// <param name="searchIndex">Index of the search.</param>
        /// <param name="serviceClient">The service client.</param>
        private static void CreateIndexIfNotExists(string searchIndex, ISearchServiceClient serviceClient)
        {
            if (!serviceClient.Indexes.Exists(searchIndex))
            {
                CreateSearchIndex(searchIndex, serviceClient);
            }
        }
    }
}