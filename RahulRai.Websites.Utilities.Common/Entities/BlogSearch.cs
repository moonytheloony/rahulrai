// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 05-31-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="BlogSearch.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using Microsoft.Azure.Search.Models;

    #endregion

    /// <summary>
    ///     Class BlogSearch.
    /// </summary>
    [SerializePropertyNamesAsCamelCase]
    public class BlogSearch
    {
        /// <summary>
        ///     Gets or sets the blog identifier.
        /// </summary>
        /// <value>The blog identifier.</value>
        public string BlogId { get; set; }

        /// <summary>
        ///     Gets or sets the search tags.
        /// </summary>
        /// <value>The search tags.</value>
        public string[] SearchTags { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
    }
}