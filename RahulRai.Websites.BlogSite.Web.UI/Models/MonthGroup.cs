// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 07-07-2015
//
// Last Modified By : rahulrai
// Last Modified On : 07-07-2015
// ***********************************************************************
// <copyright file="MonthGroup.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.BlogSite.Web.UI.Models
{
    #region

    using System.Collections.Generic;
    using Utilities.Common.Entities;

    #endregion

    /// <summary>
    /// Month group class
    /// </summary>
    public class MonthGroup
    {
        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>
        /// The month.
        /// </value>
        public int Month { get; set; }

        /// <summary>
        /// Gets or sets the posts.
        /// </summary>
        /// <value>
        /// The posts.
        /// </value>
        public List<BlogPost> Posts { get; set; }
    }
}