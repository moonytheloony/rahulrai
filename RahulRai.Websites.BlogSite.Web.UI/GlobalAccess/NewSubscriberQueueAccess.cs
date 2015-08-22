// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 08-20-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-20-2015
// ***********************************************************************
// <copyright file="NewSubscriberQueueAccess.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.BlogSite.Web.UI.GlobalAccess
{
    #region

    using System.Web.Configuration;

    using RahulRai.Websites.Utilities.AzureStorage.QueueStorage;
    using RahulRai.Websites.Utilities.Common.RegularTypes;

    #endregion

    /// <summary>
    /// Class NewSubscriberQueueAccess.
    /// </summary>
    public class NewSubscriberQueueAccess
    {
        #region Static Fields

        /// <summary>
        /// The synchronize root
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// The instance
        /// </summary>
        private static volatile NewSubscriberQueueAccess instance;

        #endregion

        #region Fields

        /// <summary>
        /// The connection string
        /// </summary>
        private readonly string connectionString =
            WebConfigurationManager.AppSettings[ApplicationConstants.StorageAccountConnectionString];

        /// <summary>
        /// The name
        /// </summary>
        private readonly string queueName =
            WebConfigurationManager.AppSettings[ApplicationConstants.NewSubscriberQueueName];

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="NewSubscriberQueueAccess"/> class from being created.
        /// </summary>
        private NewSubscriberQueueAccess()
        {
            this.AzureQueueService = new AzureQueueService(this.connectionString, this.queueName);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static NewSubscriberQueueAccess Instance
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
                        instance = new NewSubscriberQueueAccess();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets the azure queue service.
        /// </summary>
        /// <value>The azure queue service.</value>
        public AzureQueueService AzureQueueService { get; private set; }

        #endregion
    }
}