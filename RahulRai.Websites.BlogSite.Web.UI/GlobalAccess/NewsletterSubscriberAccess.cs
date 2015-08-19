// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 08-19-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-19-2015
// ***********************************************************************
// <copyright file="NewsletterSubscriberAccess.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.BlogSite.Web.UI.GlobalAccess
{
    #region

    using System.Web.Configuration;

    using RahulRai.Websites.Utilities.AzureStorage.TableStorage;
    using RahulRai.Websites.Utilities.Common.Entities;
    using RahulRai.Websites.Utilities.Common.RegularTypes;

    #endregion

    /// <summary>
    /// Class NewsletterSubscriberAccess.
    /// </summary>
    public class NewsletterSubscriberAccess
    {
        #region Static Fields

        /// <summary>
        /// The synchronize root
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// The instance
        /// </summary>
        private static volatile NewsletterSubscriberAccess instance;

        #endregion

        #region Fields

        /// <summary>
        /// The blog table name
        /// </summary>
        private readonly string blogTableName =
            WebConfigurationManager.AppSettings[ApplicationConstants.NewsletterSubscriberTableName];

        /// <summary>
        /// The connection string
        /// </summary>
        private readonly string connectionString =
            WebConfigurationManager.AppSettings[ApplicationConstants.StorageAccountConnectionString];

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="NewsletterSubscriberAccess"/> class from being created.
        /// </summary>
        private NewsletterSubscriberAccess()
        {
            this.NewsletterSubscriberTable = new AzureTableStorageService<TableNewsletterEntity>(
                this.connectionString,
                this.blogTableName,
                AzureTableStorageAssist.ConvertEntityToDynamicTableEntity,
                AzureTableStorageAssist.ConvertDynamicEntityToEntity<TableNewsletterEntity>);
            this.NewsletterSubscriberTable.CreateStorageObjectAndSetExecutionContext();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static NewsletterSubscriberAccess Instance
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
                        instance = new NewsletterSubscriberAccess();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets the newsletter subscriber table.
        /// </summary>
        /// <value>The newsletter subscriber table.</value>
        public AzureTableStorageService<TableNewsletterEntity> NewsletterSubscriberTable { get; private set; }

        #endregion
    }
}