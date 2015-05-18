namespace RahulRai.Websites.Utilities.AzureStorage.Tests.TableStorage
{
    using System;
    using System.Linq;
    using AzureStorage.TableStorage;
    using Common.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Storage.Table;

    [TestClass()]
    public class AzureTableStorageRepositoryTests
    {
        private string connectionString = "";

        private AzureTableStorageRepository<TableBlogEntity> testObject;

        [TestInitialize()]
        public void AzureTableStorageRepositoryTest()
        {
            testObject = new AzureTableStorageRepository<TableBlogEntity>(
                connectionString, "sampleTable", AzureTableStorageAssist.ConvertEntityToDynamicTableEntity,
                AzureTableStorageAssist.ConvertDynamicEntityToEntity<TableBlogEntity>);
            testObject.CreateStorageObjectAndSetExecutionContext();
        }

        [TestMethod()]
        public void InsertOrReplaceTest()
        {
            var blogpost = new BlogPost()
            {
                Title = "sampleTitle a multispace title",
                Body = "samplebody",
                PostedDate = DateTime.UtcNow
            };

            testObject.InsertOrReplace(new TableBlogEntity(blogpost));
            var result = testObject.SaveAll();
            Assert.IsTrue(result.All(element => element.IsSuccess));
        }

        [TestMethod()]
        public void QueryTest()
        {
            string partitionFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "MYBLOG");
            var result = testObject.Query("PartitionKey eq 'MYBLOG'", 2);
        }
    }
}

