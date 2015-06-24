// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 04-16-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="BlogPost.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using System;
    using Helpers;
    using RegularTypes;

    #endregion

    /// <summary>
    /// Class BlogPost.
    /// </summary>
    public class BlogPost
    {
        /// <summary>
        /// The blog identifier
        /// </summary>
        private string blogId;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        public string Body { get; set; }

        /// <summary>
        /// Gets the blog key.
        /// </summary>
        /// <value>The blog key.</value>
        public string BlogKey
        {
            get { return ApplicationConstants.BlogKey; }
        }

        /// <summary>
        /// Gets the blog formatted URI.
        /// </summary>
        /// <value>The blog formatted URI.</value>
        public string BlogFormattedUri
        {
            get { return Routines.FormatTitle(this.Title.ToLowerInvariant()); }
        }

        /// <summary>
        /// Gets or sets the blog identifier.
        /// </summary>
        /// <value>The blog identifier.</value>
        public string BlogId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.blogId))
                {
                    this.blogId = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);
                }

                return this.blogId;
            }

            set
            {
                this.blogId = value;
            }
        }

        /// <summary>
        /// Gets or sets the categories CSV.
        /// </summary>
        /// <value>The categories CSV.</value>
        public string CategoriesCsv { get; set; }

        /// <summary>
        /// Gets or sets the posted date.
        /// </summary>
        /// <value>The posted date.</value>
        public DateTime PostedDate { get; set; }

        /// <summary>
        /// Gets or sets the entity tag.
        /// </summary>
        /// <value>The entity tag.</value>
        public string EntityTag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is draft.
        /// </summary>
        /// <value><c>true</c> if this instance is draft; otherwise, <c>false</c>.</value>
        public bool IsDraft { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value><c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
        public bool IsDeleted { get; set; }
    }
}