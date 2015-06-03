namespace RahulRai.Websites.BlogSite.Web.UI.GlobalAccess
{
    #region

    using System;
    using System.Configuration;
    using Utilities.AzureStorage.TableStorage;
    using Utilities.Common.Entities;

    #endregion

    public sealed class BlogStoreAccess
    {
        private static volatile BlogStoreAccess instance;
        private static readonly object SyncRoot = new Object();
        private readonly string blogTableName = ConfigurationManager.AppSettings["BlogTableName"];
        private readonly string connectionString = ConfigurationManager.AppSettings["StorageAccountConnectionString"];

        private BlogStoreAccess()
        {
            BlogTable = new AzureTableStorageService<TableBlogEntity>(
                connectionString,
                blogTableName,
                AzureTableStorageAssist.ConvertEntityToDynamicTableEntity,
                AzureTableStorageAssist.ConvertDynamicEntityToEntity<TableBlogEntity>);
            BlogTable.CreateStorageObjectAndSetExecutionContext();
        }

        public AzureTableStorageService<TableBlogEntity> BlogTable { get; private set; }

        public static BlogStoreAccess Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                lock (SyncRoot)
                {
                    if (instance == null)
                    {
                        instance = new BlogStoreAccess();
                    }
                }

                return instance;
            }
        }
    }
}