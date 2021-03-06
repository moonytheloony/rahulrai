﻿// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.AzureStorage
// Author           : rahulrai
// Created          : 04-15-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="AzureTableStorageService.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.AzureStorage.TableStorage
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Entities;
    using Common.Exceptions;
    using Common.RegularTypes;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Microsoft.WindowsAzure.Storage.Table;

    #endregion

    #region

    #endregion

    /// <summary>
    ///     The azure table storage repository.
    /// </summary>
    /// <typeparam name="TElement">Element for entity</typeparam>
    public class AzureTableStorageService<TElement>
        where TElement : class
    {
        #region Fields

        /// <summary>
        ///     The converter from table entity to entity.
        /// </summary>
        private readonly Func<DynamicTableEntity, TElement> convertToEntity;

        /// <summary>
        ///     The converter from entity to table entity.
        /// </summary>
        private readonly Func<TElement, DynamicTableEntity> convertToTableEntity;

        /// <summary>
        ///     The table name
        /// </summary>
        private readonly string tableName = string.Empty;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AzureTableStorageService{TElement}" /> class.
        /// </summary>
        /// <param name="storageAccountConnectionString">The storage account connection string.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="convertToTableEntity">The converter from entity to table entity.</param>
        /// <param name="convertToEntity">The converter from table entity to entity.</param>
        public AzureTableStorageService(
            string storageAccountConnectionString,
            string tableName,
            Func<TElement, DynamicTableEntity> convertToTableEntity,
            Func<DynamicTableEntity, TElement> convertToEntity)
        {
            this.convertToTableEntity = convertToTableEntity;
            this.convertToEntity = convertToEntity;
            this.tableName = tableName;
            var storageAccount = CloudStorageAccount.Parse(storageAccountConnectionString);
            this.CloudTableClient = storageAccount.CreateCloudTableClient();
            this.CloudTableClient.DefaultRequestOptions.RetryPolicy =
                new ExponentialRetry(
                    TimeSpan.FromSeconds(CustomRetryPolicy.RetryBackOffSeconds),
                    CustomRetryPolicy.MaxRetries);
            this.TableRequestOptions = new TableRequestOptions
            {
                RetryPolicy =
                    new ExponentialRetry(
                        TimeSpan.FromSeconds(CustomRetryPolicy.RetryBackOffSeconds),
                        CustomRetryPolicy.MaxRetries)
            };
            this.TableOperations = new TableBatchOperation();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the table request options.
        /// </summary>
        /// <value>The table request options.</value>
        public TableRequestOptions TableRequestOptions { get; set; }

        /// <summary>
        ///     Gets or sets the active table.
        /// </summary>
        /// <value>The active table.</value>
        private CloudTable ActiveTable { get; set; }

        /// <summary>
        ///     Gets or sets the cloud table client.
        /// </summary>
        /// <value>The cloud table client.</value>
        private CloudTableClient CloudTableClient { get; set; }

        /// <summary>
        ///     Gets or sets the table operation.
        /// </summary>
        /// <value>The table operations.</value>
        private TableBatchOperation TableOperations { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The create storage object and set execution context.
        /// </summary>
        public virtual void CreateStorageObjectAndSetExecutionContext()
        {
            this.ActiveTable = this.CloudTableClient.GetTableReference(this.tableName);
            this.ActiveTable.CreateIfNotExists(this.TableRequestOptions);
        }

        /// <summary>
        ///     The delete.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Delete(TElement entity)
        {
            var dynamicEntity = this.convertToTableEntity(entity);
            this.TableOperations.Add(TableOperation.Delete(dynamicEntity));
        }

        /// <summary>
        ///     The delete storage object.
        /// </summary>
        public void DeleteStorageObject()
        {
            this.ActiveTable.DeleteIfExists(this.TableRequestOptions);
        }

        /// <summary>
        ///     The get all.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The <see cref="List{T}" />.</returns>
        public virtual IList<TElement> GetAll(string key)
        {
            var query =
                new TableQuery().Where(
                    TableQuery.GenerateFilterCondition(KnownTypes.PartitionKey, QueryComparisons.Equal, key));
            var result = this.ActiveTable.ExecuteQuery(query, this.TableRequestOptions);
            return result.Select(this.convertToEntity).ToList();
        }

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <returns>IList&lt;TElement&gt;.</returns>
        public virtual IList<TElement> GetAll()
        {
            var result = this.ActiveTable.ExecuteQuery(
                new TableQuery(),
                this.TableRequestOptions);
            return result.Select(this.convertToEntity).ToList();
        }

        /// <summary>
        ///     Customs the operation.
        /// </summary>
        /// <returns>CloudTable.</returns>
        public virtual CloudTable CustomOperation()
        {
            return this.ActiveTable;
        }

        /// <summary>
        ///     The get by id.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="id">The id.</param>
        /// <returns>
        ///     The <see cref="TElement" />.
        ///     Element for entity
        /// </returns>
        public virtual TElement GetById(string key, string id)
        {
            var operation = TableOperation.Retrieve(key, id);
            var result = this.ActiveTable.Execute(operation, this.TableRequestOptions).Result as DynamicTableEntity;
            return result == null ? null : this.convertToEntity(result);
        }

        /// <summary>
        ///     The insert.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Insert(TElement entity)
        {
            var dynamicEntity = this.convertToTableEntity(entity);
            this.TableOperations.Add(TableOperation.Insert(dynamicEntity));
        }

        /// <summary>
        ///     Inserts the or replace.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void InsertOrReplace(TElement entity)
        {
            var dynamicEntity = this.convertToTableEntity(entity);
            this.TableOperations.Add(TableOperation.InsertOrReplace(dynamicEntity));
        }

        /// <summary>
        ///     The merge.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Merge(TElement entity)
        {
            var dynamicEntity = this.convertToTableEntity(entity);
            this.TableOperations.Add(TableOperation.InsertOrMerge(dynamicEntity));
        }

        /// <summary>
        ///     Queries the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="takeCount">The take count.</param>
        /// <returns>IList&lt;TElement&gt;.</returns>
        public virtual IList<TElement> Query(string filter, int? takeCount)
        {
            var tableQuery = new TableQuery
            {
                FilterString = filter,
                TakeCount = takeCount
            };
            return this.ActiveTable.ExecuteQuery(tableQuery).Select(this.convertToEntity).ToList();
        }

        /// <summary>
        ///     The save.
        /// </summary>
        /// <returns>The <see cref="bool" />.</returns>
        public virtual bool Save()
        {
            var result = this.ActiveTable.Execute(this.TableOperations[0], this.TableRequestOptions);
            this.TableOperations = new TableBatchOperation();
            return IsSuccessStatusCode(result.HttpStatusCode);
        }

        /// <summary>
        ///     Saves all.
        /// </summary>
        /// <returns>IList&lt;OperationResult&gt;.</returns>
        /// <exception cref="BlogSystemException">Error executing batch table operation.</exception>
        /// <exception cref="RahulRai.Websites.Utilities.Common.Exceptions.BlogSystemException">
        ///     Error executing batch table
        ///     operation.
        /// </exception>
        public virtual IList<OperationResult> SaveAll()
        {
            var result = this.ActiveTable.ExecuteBatch(
                this.TableOperations,
                this.TableRequestOptions);
            this.TableOperations = new TableBatchOperation();
            return
                result.Select(x => new OperationResult(x.HttpStatusCode, IsSuccessStatusCode(x.HttpStatusCode)))
                    .ToList();
        }

        /// <summary>
        ///     The set execution context.
        /// </summary>
        public virtual void SetExecutionContext()
        {
            this.ActiveTable = this.CloudTableClient.GetTableReference(this.tableName);
        }

        /// <summary>
        ///     The update.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Update(TElement entity)
        {
            var dynamicEntity = this.convertToTableEntity(entity);
            this.TableOperations.Add(TableOperation.InsertOrReplace(dynamicEntity));
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Determines whether the HTTP status code represents a success.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>If the status code represents a success.</returns>
        private static bool IsSuccessStatusCode(int statusCode)
        {
            return statusCode >= 200 && statusCode < 300;
        }

        #endregion
    }
}