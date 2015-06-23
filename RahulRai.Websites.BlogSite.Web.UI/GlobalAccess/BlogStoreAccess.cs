// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 05-28-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="BlogStoreAccess.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.BlogSite.Web.UI.GlobalAccess
{
    #region

    using System;
    using System.Configuration;
    using Utilities.AzureStorage.TableStorage;
    using Utilities.Common.Entities;
    using Utilities.Common.RegularTypes;

    #endregion

    /// <summary>
    ///     Class BlogStoreAccess. This class cannot be inherited.
    /// </summary>
    public sealed class BlogStoreAccess
    {
        /// <summary>
        ///     The synchronize root
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        ///     The instance
        /// </summary>
        private static volatile BlogStoreAccess instance;

        /// <summary>
        ///     The blog table name
        /// </summary>
        private readonly string blogTableName = ConfigurationManager.AppSettings[ApplicationConstants.BlogTableName];

        /// <summary>
        ///     The connection string
        /// </summary>
        private readonly string connectionString =
            ConfigurationManager.AppSettings[ApplicationConstants.StorageAccountConnectionString];

        /// <summary>
        ///     Prevents a default instance of the <see cref="BlogStoreAccess" /> class from being created.
        /// </summary>
        private BlogStoreAccess()
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

        /// <summary>
        ///     Gets the blog table.
        /// </summary>
        /// <value>The blog table.</value>
        public AzureTableStorageService<TableBlogEntity> BlogTable { get; private set; }
    }
}