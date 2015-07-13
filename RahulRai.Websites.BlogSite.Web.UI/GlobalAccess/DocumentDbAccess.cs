// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 07-13-2015
//
// Last Modified By : rahulrai
// Last Modified On : 07-13-2015
// ***********************************************************************
// <copyright file="DocumentDbAccess.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.BlogSite.Web.UI.GlobalAccess
{
    #region

    using System.Configuration;
    using System.Text.RegularExpressions;
    using Utilities.AzureStorage.DocumentDB;
    using Utilities.Common.Exceptions;
    using Utilities.Common.RegularTypes;

    #endregion

    /// <summary>
    ///     Class DocumentDbAccess. This class cannot be inherited.
    /// </summary>
    public sealed class DocumentDbAccess
    {
        /// <summary>
        ///     The synchronize root
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        ///     The instance
        /// </summary>
        private static volatile DocumentDbAccess instance;

        /// <summary>
        ///     The connection string
        /// </summary>
        private readonly string connectionString =
            ConfigurationManager.AppSettings[ApplicationConstants.DocumentDbConnectionString];

        /// <summary>
        ///     The testimonial collection
        /// </summary>
        private readonly string testimonialCollection =
            ConfigurationManager.AppSettings[ApplicationConstants.TestimonialCollection];

        /// <summary>
        ///     The testimonial database
        /// </summary>
        private readonly string testimonialDatabase =
            ConfigurationManager.AppSettings[ApplicationConstants.TestimonialDatabase];

        /// <summary>
        ///     Prevents a default instance of the <see cref="DocumentDbAccess" /> class from being created.
        /// </summary>
        private DocumentDbAccess()
        {
            var matchAccount = Regex.Match(this.connectionString, "AccountEndpoint=([^;]*)");
            var matchKey = Regex.Match(this.connectionString, "AccountKey=([^;]*)");
            if (!matchAccount.Success || !matchKey.Success)
            {
                throw new BlogSystemException("Invalid DocumentDb Connection");
            }

            this.DocumentDbService = new DocumentDbService(matchAccount.Groups[1].Value, matchKey.Groups[1].Value);
            this.DocumentDbService.CreateDatabase(this.testimonialDatabase);
            this.DocumentDbService.CreateCollection(this.testimonialCollection);
        }

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static DocumentDbAccess Instance
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
                        instance = new DocumentDbAccess();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        ///     Gets the document database service.
        /// </summary>
        /// <value>
        ///     The document database service.
        /// </value>
        public DocumentDbService DocumentDbService { get; private set; }
    }
}