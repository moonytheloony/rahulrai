// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 07-07-2015
//
// Last Modified By : rahulrai
// Last Modified On : 07-07-2015
// ***********************************************************************
// <copyright file="ProfileService.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.BlogSite.Web.UI.Services
{
    using System.Linq;
    using GlobalAccess;

    /// <summary>
    /// Profile Service class.
    /// </summary>
    public class ProfileService
    {
        /// <summary>
        /// The document database access
        /// </summary>
        private readonly DocumentDbAccess documentDbAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService"/> class.
        /// </summary>
        /// <param name="documentDbAccess">The document database access.</param>
        public ProfileService(GlobalAccess.DocumentDbAccess documentDbAccess)
        {
            this.documentDbAccess = documentDbAccess;
        }

        /// <summary>
        /// Adds the document.
        /// </summary>
        /// <typeparam name="T">Type to query</typeparam>
        /// <param name="document">The document.</param>
        public void AddDocument<T>(T document)
        {
            this.documentDbAccess.DocumentDbService.AddDocumentToCollection(document);
        }

        /// <summary>
        /// Queries the document.
        /// </summary>
        /// <typeparam name="T">Type to query</typeparam>
        /// <returns>Queryable document</returns>
        public IOrderedQueryable<T> QueryDocument<T>()
        {
            return this.documentDbAccess.DocumentDbService.GetQueryObject<T>();
        }
    }
}