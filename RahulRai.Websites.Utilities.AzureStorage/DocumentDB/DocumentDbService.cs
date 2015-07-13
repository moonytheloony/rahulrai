// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 07-07-2015
//
// Last Modified By : rahulrai
// Last Modified On : 07-07-2015
// ***********************************************************************
// <copyright file="DocumentDbService.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.AzureStorage.DocumentDB
{
    #region

    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using Common.Exceptions;
    using Common.Helpers;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;

    #endregion

    /// <summary>
    /// Document Database Service class.
    /// </summary>
    public class DocumentDbService
    {
        /// <summary>
        /// The client
        /// </summary>
        private readonly DocumentClient client;

        /// <summary>
        /// The database
        /// </summary>
        private Database database;

        /// <summary>
        /// The document collection
        /// </summary>
        private DocumentCollection documentCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDbService"/> class.
        /// </summary>
        /// <param name="endpointUri">The endpoint URI.</param>
        /// <param name="authKey">The authentication key.</param>
        public DocumentDbService(string endpointUri, string authKey)
        {
            this.client = new DocumentClient(new Uri(endpointUri), authKey);
        }

        /// <summary>
        /// Creates the database.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        public void CreateDatabase(string databaseName)
        {
            var databases = this.client.CreateDatabaseQuery().Where(db => db.Id == databaseName).ToArray();
            if (databases.Any())
            {
                this.database = databases.First();
                return;
            }

            this.database = new Database { Id = databaseName };
            this.database = this.client.CreateDatabaseAsync(this.database).Result.Resource;
        }

        /// <summary>
        /// Creates the collection.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        public void CreateCollection(string collectionName)
        {
            var collections = this.client.CreateDocumentCollectionQuery(this.database.CollectionsLink).Where(col => col.Id == collectionName).ToArray();
            if (collections.Any())
            {
                this.documentCollection = collections.First();
                return;
            }

            this.documentCollection = this.client.CreateDocumentCollectionAsync(
                this.database.CollectionsLink,
                new DocumentCollection
                {
                    Id = collectionName
                }).Result.Resource;
        }

        /// <summary>
        /// Adds the document to collection.
        /// </summary>
        /// <typeparam name="T">Type to add</typeparam>
        /// <param name="documentToAdd">The document to add.</param>
        public void AddDocumentToCollection<T>(T documentToAdd)
        {
            var operationStatusCode = (int)this.client.CreateDocumentAsync(this.documentCollection.DocumentsLink, documentToAdd).Result.StatusCode;
            if (operationStatusCode <= 299 && operationStatusCode >= 200)
            {
                return;
            }

            var exception = new BlogSystemException("could not save document");
            TraceUtility.LogError(exception, "Unable to save document in documentdb");
            throw exception;
        }

        /// <summary>
        /// Gets the query object.
        /// </summary>
        /// <typeparam name="T">Type to query</typeparam>
        /// <returns>Queryable object</returns>
        public IOrderedQueryable<T> GetQueryObject<T>()
        {
            return this.client.CreateDocumentQuery<T>(this.documentCollection.DocumentsLink);
        }

        /// <summary>
        /// Deletes the database.
        /// </summary>
        public void DeleteDatabase()
        {
            var operation = this.client.DeleteDatabaseAsync(this.database.SelfLink).Result.Resource;
        }
    }
}