// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 06-25-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-25-2015
// ***********************************************************************
// <copyright file="Testimonial.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.Entities
{
    /// <summary>
    ///     Class Testimonial.
    /// </summary>
    public class Testimonial
    {
        /// <summary>
        ///     Gets or sets the author.
        /// </summary>
        /// <value>The author.</value>
        public string Author { get; set; }

        /// <summary>
        ///     Gets or sets the author email.
        /// </summary>
        /// <value>The author email.</value>
        public string AuthorEmail { get; set; }

        /// <summary>
        ///     Gets or sets the author organization.
        /// </summary>
        /// <value>The author organization.</value>
        public string AuthorOrganization { get; set; }

        /// <summary>
        ///     Gets or sets the author comments.
        /// </summary>
        /// <value>The author comments.</value>
        public string AuthorComments { get; set; }

        /// <summary>
        ///     Gets or sets the author token.
        /// </summary>
        /// <value>The author token.</value>
        public string AuthorToken { get; set; }
    }
}