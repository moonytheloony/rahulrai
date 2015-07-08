// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 07-07-2015
//
// Last Modified By : rahulrai
// Last Modified On : 07-07-2015
// ***********************************************************************
// <copyright file="Archive.cs" company="Rahul Rai">
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
    /// Archive class.
    /// </summary>
    public class Archive
    {
        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the month groups.
        /// </summary>
        /// <value>
        /// The month groups.
        /// </value>
        public IEnumerable<MonthGroup> MonthGroups { get; set; }
    }
}