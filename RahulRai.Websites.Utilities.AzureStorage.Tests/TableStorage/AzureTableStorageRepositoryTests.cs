// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.AzureStorage.Tests
// Author           : rahulrai
// Created          : 04-16-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="AzureTableStorageRepositoryTests.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.AzureStorage.Tests.TableStorage
{
    #region

    using System;
    using System.Linq;
    using AzureStorage.TableStorage;
    using Common.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Storage.Table;

    #endregion

    /// <summary>
    /// Class AzureTableStorageRepositoryTests.
    /// </summary>
    [TestClass]
    public class AzureTableStorageRepositoryTests
    {
        /// <summary>
        /// The connection string
        /// </summary>
        private string connectionString = string.Empty;

        /// <summary>
        /// The test object
        /// </summary>
        private AzureTableStorageService<TableBlogEntity> testObject;

        /// <summary>
        /// Azures the table storage repository test.
        /// </summary>
        [TestInitialize]
        public void AzureTableStorageRepositoryTest()
        {
            this.testObject = new AzureTableStorageService<TableBlogEntity>(
                this.connectionString,
                "sampleTable",
                AzureTableStorageAssist.ConvertEntityToDynamicTableEntity,
                AzureTableStorageAssist.ConvertDynamicEntityToEntity<TableBlogEntity>);
            this.testObject.CreateStorageObjectAndSetExecutionContext();
        }

        /// <summary>
        /// Inserts the or replace test.
        /// </summary>
        [TestMethod]
        public void InsertOrReplaceTest()
        {
            var blogpost = new BlogPost
            {
                Title = "sampleTitle a multispace title",
                Body = "samplebody",
                PostedDate = DateTime.UtcNow
            };

            this.testObject.InsertOrReplace(new TableBlogEntity(blogpost));
            var result = this.testObject.SaveAll();
            Assert.IsTrue(result.All(element => element.IsSuccess));
        }

        /// <summary>
        /// Queries the test.
        /// </summary>
        [TestMethod]
        public void QueryTest()
        {
            var partitionFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "MYBLOG");
            var result = this.testObject.Query("PartitionKey eq 'MYBLOG'", 2);
        }
    }
}