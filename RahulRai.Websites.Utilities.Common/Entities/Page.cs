// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 07-03-2015
//
// Last Modified By : rahulrai
// Last Modified On : 07-03-2015
// ***********************************************************************
// <copyright file="Page.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using Microsoft.WindowsAzure.Storage.Table;

    #endregion

    /// <summary>
    /// Class Page.
    /// </summary>
    public class Page
    {
        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        /// <value>The page number.</value>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the continuation token.
        /// </summary>
        /// <value>The continuation token.</value>
        public TableContinuationToken ContinuationToken { get; set; }
    }
}