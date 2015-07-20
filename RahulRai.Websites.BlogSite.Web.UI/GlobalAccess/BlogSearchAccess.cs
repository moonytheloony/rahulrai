// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 05-28-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="BlogSearchAccess.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.BlogSite.Web.UI.GlobalAccess
{
    #region

    using System.Configuration;
    using System.Web.Configuration;
    using Utilities.AzureStorage.Search;
    using Utilities.Common.RegularTypes;

    #endregion

    /// <summary>
    /// BlogSearchAccess class.
    /// </summary>
    public class BlogSearchAccess
    {
        /// <summary>
        ///     The synchronize root
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        ///     The instance
        /// </summary>
        private static volatile BlogSearchAccess instance;

        /// <summary>
        /// The key
        /// </summary>
        private readonly string key = WebConfigurationManager.AppSettings[ApplicationConstants.SearchServiceKey];

        /// <summary>
        /// The name
        /// </summary>
        private readonly string name = WebConfigurationManager.AppSettings[ApplicationConstants.SearchServiceName];

        /// <summary>
        /// Prevents a default instance of the <see cref="BlogSearchAccess"/> class from being created.
        /// </summary>
        private BlogSearchAccess()
        {
            this.AzureSearchService = new AzureSearchService(this.name, this.key, ApplicationConstants.SearchIndex);
        }

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static BlogSearchAccess Instance
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
                        instance = new BlogSearchAccess();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets the azure search service.
        /// </summary>
        /// <value>
        /// The azure search service.
        /// </value>
        public AzureSearchService AzureSearchService { get; private set; }
    }
}