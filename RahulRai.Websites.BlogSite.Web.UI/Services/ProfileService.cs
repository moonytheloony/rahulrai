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
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using GlobalAccess;
    using Utilities.Common.Entities;

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
        /// <param name="itemCount">The item count.</param>
        /// <returns>
        /// Queryable document
        /// </returns>
        public IOrderedQueryable<T> QueryDocument<T>(int itemCount)
        {
            return this.documentDbAccess.DocumentDbService.GetQueryObject<T>(itemCount);
        }

        /// <summary>
        /// Cleanses the testimonial.
        /// </summary>
        /// <param name="postedTestimonial">The posted testimonial.</param>
        public void CleanseTestimonial(ref Testimonial postedTestimonial)
        {
            //// Format Name
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            postedTestimonial.AuthorFirstName = textInfo.ToTitleCase(postedTestimonial.AuthorFirstName.Trim());
            postedTestimonial.AuthorLastName = textInfo.ToTitleCase(postedTestimonial.AuthorLastName.Trim());

            //// Format Job
            postedTestimonial.AuthorDesignation = Regex.Replace(postedTestimonial.AuthorDesignation.Trim(), @"\s+", " ");
            postedTestimonial.AuthorOrganization = Regex.Replace(postedTestimonial.AuthorOrganization.Trim(), @"\s+", " ");
            postedTestimonial.AuthorDesignation = textInfo.ToTitleCase(postedTestimonial.AuthorDesignation.Trim());
            postedTestimonial.AuthorOrganization = textInfo.ToTitleCase(postedTestimonial.AuthorOrganization.Trim());

            postedTestimonial.AuthorEmail = postedTestimonial.AuthorEmail.ToLowerInvariant().Trim();
            postedTestimonial.AuthorToken = postedTestimonial.AuthorToken.ToLowerInvariant().Trim();

            //// Remove multiple line breaks from comments. Multiple spaces. Make first character capital. Fix punctuation.
            postedTestimonial.AuthorComments = Regex.Replace(postedTestimonial.AuthorComments, @"(?:\r\n|\r(?!\n)|(?<!\r)\n){2,}", "<br />");
            postedTestimonial.AuthorComments = Regex.Replace(postedTestimonial.AuthorComments.Trim(), @"\s+", " ");
            if (!string.IsNullOrWhiteSpace(postedTestimonial.AuthorComments))
            {
                postedTestimonial.AuthorComments = postedTestimonial.AuthorComments.First().ToString(CultureInfo.InvariantCulture).ToUpper() + postedTestimonial.AuthorComments.Substring(1);
                var comments = postedTestimonial.AuthorComments.ToCharArray();
                foreach (Match match in Regex.Matches(postedTestimonial.AuthorComments, @"[\.\?\!,]\s+([a-z])", RegexOptions.Singleline))
                {
                    comments[match.Groups[1].Index] = char.ToUpper(comments[match.Groups[1].Index]);
                }

                postedTestimonial.AuthorComments = new string(comments);
            }
        }
    }
}