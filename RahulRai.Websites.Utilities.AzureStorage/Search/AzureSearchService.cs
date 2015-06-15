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

    public class AzureSearchService : IDisposable
    {
        private readonly SearchIndexClient indexClient;
        private readonly SearchServiceClient serviceClient;
        private bool disposed;

        public AzureSearchService(string searchServiceName, string searchServiceKey, string searchIndex)
        {
            serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(searchServiceKey));
            CreateIndexIfNotExists(searchIndex, serviceClient);
            indexClient = serviceClient.Indexes.GetClient(searchIndex);
        }

        private AzureSearchService()
        {
            //// Default constructor not allowed.
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AzureSearchService()
        {
            Dispose(false);
        }

        private static void CreateIndexIfNotExists(string searchIndex, ISearchServiceClient serviceClient)
        {
            if (!serviceClient.Indexes.Exists(searchIndex))
            {
                CreateSearchIndex(searchIndex, serviceClient);
            }
        }

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
                    {IsSearchable = true, IsFilterable = true, IsFacetable = true}
                }
            };

            serviceClient.Indexes.Create(definition);
        }

        public void UpsertDataToIndex(BlogSearch blogSearchEntity)
        {
            var documents =
                new[] {blogSearchEntity};

            try
            {
                indexClient.Documents.Index(IndexBatch.Create(documents.Select(IndexAction.Create)));
            }
            catch (IndexBatchException e)
            {
                // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                // the batch. Depending on your application, you can take compensating actions like delaying and
                // retrying. For this simple demo, we just log the failed document keys and continue.
                Trace.WriteLine(
                    "Failed to index some of the documents: {0}",
                    String.Join(", ", e.IndexResponse.Results.Where(r => !r.Succeeded).Select(r => r.Key)));
                throw new BlogSystemException("failed on index");
            }
        }

        public IEnumerable<BlogSearch> SearchDocuments(string searchText, string filter = null)
        {
            // Execute search based on search text and optional filter
            var searchParameter = new SearchParameters();

            if (!String.IsNullOrEmpty(filter))
            {
                searchParameter.Filter = filter;
            }

            var response = indexClient.Documents.Search<BlogSearch>(searchText, searchParameter);
            return response.Select(result => result.Document);
        }

        public void DeleteData(string idToDelete)
        {
            try
            {
                var batch = IndexBatch.Create(
                    new IndexAction(IndexActionType.Delete, new Document {{"blogId", idToDelete}}));
                indexClient.Documents.Index(batch);
            }
            catch (IndexBatchException e)
            {
                // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                // the batch. Depending on your application, you can take compensating actions like delaying and
                // retrying. For this simple demo, we just log the failed document keys and continue.
                Trace.WriteLine(
                    "Failed to index some of the documents: {0}",
                    String.Join(", ", e.IndexResponse.Results.Where(r => !r.Succeeded).Select(r => r.Key)));
                throw new BlogSystemException("failed on index");
            }
        }

        public void DeleteIndex(string indexName)
        {
            if (serviceClient.Indexes.Exists(indexName))
            {
                serviceClient.Indexes.Delete(indexName);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    indexClient.Dispose();
                }
            }
            disposed = true;
        }
    }
}