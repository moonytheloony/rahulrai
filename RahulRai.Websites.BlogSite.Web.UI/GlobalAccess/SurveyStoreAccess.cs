// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 08-12-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-12-2015
// ***********************************************************************
// <copyright file="SurveyStoreAccess.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.BlogSite.Web.UI.GlobalAccess
{
    #region

    using System.Web.Configuration;

    using RahulRai.Websites.Utilities.AzureStorage.BlobStorage;
    using RahulRai.Websites.Utilities.Common.RegularTypes;

    #endregion

    /// <summary>
    /// Class SurveyStoreAccess. This class cannot be inherited.
    /// </summary>
    public sealed class SurveyStoreAccess
    {
        #region Static Fields

        /// <summary>
        /// The synchronize root
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// The instance
        /// </summary>
        private static volatile SurveyStoreAccess instance;

        #endregion

        #region Fields

        /// <summary>
        /// The connection string
        /// </summary>
        private readonly string connectionString =
            WebConfigurationManager.AppSettings[ApplicationConstants.SurveyConnectionString];

        /// <summary>
        /// The survey container name
        /// </summary>
        private readonly string surveyContainerName =
            WebConfigurationManager.AppSettings[ApplicationConstants.SurveyContainerName];

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="SurveyStoreAccess" /> class from being created.
        /// </summary>
        private SurveyStoreAccess()
        {
            this.BlobStorageService = new BlobStorageService(this.connectionString);
            this.BlobStorageService.CreateContainer(this.surveyContainerName, VisibilityType.VisibleToOwnerOnly);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the instance.
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
        /// Gets the BLOB storage service.
        /// </summary>
        /// <value>The BLOB storage service.</value>
        public BlobStorageService BlobStorageService { get; private set; }

        #endregion
    }
}