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
    ///     Class AzureTableStorageRepositoryTests.
    /// </summary>
    [TestClass]
    public class AzureTableStorageRepositoryTests
    {
        /// <summary>
        ///     The connection string
        /// </summary>
        private string connectionString = "";

        /// <summary>
        ///     The test object
        /// </summary>
        private AzureTableStorageService<TableBlogEntity> testObject;

        /// <summary>
        ///     Azures the table storage repository test.
        /// </summary>
        [TestInitialize]
        public void AzureTableStorageRepositoryTest()
        {
            testObject = new AzureTableStorageService<TableBlogEntity>(
                connectionString, "sampleTable", AzureTableStorageAssist.ConvertEntityToDynamicTableEntity,
                AzureTableStorageAssist.ConvertDynamicEntityToEntity<TableBlogEntity>);
            testObject.CreateStorageObjectAndSetExecutionContext();
        }

        /// <summary>
        ///     Inserts the or replace test.
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

            testObject.InsertOrReplace(new TableBlogEntity(blogpost));
            var result = testObject.SaveAll();
            Assert.IsTrue(result.All(element => element.IsSuccess));
        }

        /// <summary>
        ///     Queries the test.
        /// </summary>
        [TestMethod]
        public void QueryTest()
        {
            var partitionFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "MYBLOG");
            var result = testObject.Query("PartitionKey eq 'MYBLOG'", 2);
        }
    }
}