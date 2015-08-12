using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RahulRai.Websites.BlogSite.Web.UI.GlobalAccess
{
    using System.Web.Configuration;

    using RahulRai.Websites.Utilities.AzureStorage.TableStorage;
    using RahulRai.Websites.Utilities.Common.Entities;
    using RahulRai.Websites.Utilities.Common.RegularTypes;

    public sealed class SurveyStoreAccess
    {
        /// <summary>
        ///     The synchronize root
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        ///     The instance
        /// </summary>
        private static volatile SurveyStoreAccess instance;

        /// <summary>
        ///     The blog table name
        /// </summary>
        private readonly string blogTableName = WebConfigurationManager.AppSettings[ApplicationConstants.SurveyContainerName];

        /// <summary>
        ///     The connection string
        /// </summary>
        private readonly string connectionString =
            WebConfigurationManager.AppSettings[ApplicationConstants.StorageAccountConnectionString];

        /// <summary>
        ///     Prevents a default instance of the <see cref="BlogStoreAccess" /> class from being created.
        /// </summary>
        private SurveyStoreAccess()
        {
            this.BlogTable = new AzureTableStorageService<TableBlogEntity>(
                this.connectionString,
                this.blogTableName,
                AzureTableStorageAssist.ConvertEntityToDynamicTableEntity,
                AzureTableStorageAssist.ConvertDynamicEntityToEntity<TableBlogEntity>);
            this.BlogTable.CreateStorageObjectAndSetExecutionContext();
        }

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static SurveyStoreAccess Instance
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
                        instance = new SurveyStoreAccess();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        ///     Gets the blog table.
        /// </summary>
        /// <value>The blog table.</value>
        public AzureTableStorageService<TableBlogEntity> BlogTable { get; private set; }
    }
}