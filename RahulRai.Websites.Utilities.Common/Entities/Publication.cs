// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 07-03-2015
//
// Last Modified By : rahulrai
// Last Modified On : 07-03-2015
// ***********************************************************************
// <copyright file="Publication.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using System;

    #endregion

    /// <summary>
    /// Publication class.
    /// </summary>
    public class Publication
    {
        /// <summary>
        /// Gets or sets the type of the publication.
        /// </summary>
        /// <value>
        /// The type of the publication.
        /// </value>
        public PublicationType PublicationType { get; set; }

        /// <summary>
        /// Gets or sets the name of the publication.
        /// </summary>
        /// <value>
        /// The name of the publication.
        /// </value>
        public string PublicationName { get; set; }

        /// <summary>
        /// Gets or sets the publication URI.
        /// </summary>
        /// <value>
        /// The publication URI.
        /// </value>
        public Uri PublicationUri { get; set; }

        /// <summary>
        /// Gets or sets the line1.
        /// </summary>
        /// <value>
        /// The line1.
        /// </value>
        public string Line1 { get; set; }

        /// <summary>
        /// Gets or sets the line2.
        /// </summary>
        /// <value>
        /// The line2.
        /// </value>
        public string Line2 { get; set; }

        /// <summary>
        /// Gets or sets the line3.
        /// </summary>
        /// <value>
        /// The line3.
        /// </value>
        public string Line3 { get; set; }
    }
}