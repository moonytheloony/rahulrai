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
    #region

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;

    using RahulRai.Websites.BlogSite.Web.UI.GlobalAccess;
    using RahulRai.Websites.Utilities.Common.Entities;

    #endregion

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
        public ProfileService(DocumentDbAccess documentDbAccess)
        {
            this.documentDbAccess = documentDbAccess;
        }

        /// <summary>
        /// Populates the publications.
        /// </summary>
        /// <returns>List of publications</returns>
        public static List<Publication> PopulatePublications()
        {
            //// Keeping this here now. Will move it to DocumentDb soon.
            var publications = new List<Publication>();

            //// Populate Books
            var book1 = new Publication
            {
                PublicationType = PublicationType.Books,
                PublicationUri = new Uri("http://www.amazon.com/gp/product/1621140369/"),
                PublicationName = "Cloud Design Patterns: Prescriptive Architecture Guidance for Cloud Applications",
                Line1 = "Publisher: Patterns and Practices",
                Line2 = "Contributor"
            };
            var book2 = new Publication
            {
                PublicationType = PublicationType.Books,
                PublicationUri = new Uri("http://www.amazon.com/Cloud-Architecture-Patterns-Using-Microsoft/dp/1449319777"),
                PublicationName = "Cloud Architecture Patterns: Using Microsoft Azure",
                Line1 = "Publisher: O’Reilly",
                Line2 = "Reviewer"
            };

            //// Populate Articles
            var article1 = new Publication
            {
                PublicationType = PublicationType.Articles,
                PublicationUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri),
                PublicationName = "Reliable Uploads to Blob Storage via a Silverlight Control (Obsolete)",
                Line1 = "Publisher: MSDN",
                Line2 = "Author"
            };
            var article2 = new Publication
            {
                PublicationType = PublicationType.Articles,
                PublicationUri = new Uri("https://msdn.microsoft.com/library/azure/hh824678.aspx"),
                PublicationName = "Reliable Uploads to Blob Storage via an HTML5 Control",
                Line1 = "Publisher: MSDN",
                Line2 = "Author"
            };

            //// Populate Talks
            var talk1 = new Publication
            {
                PublicationType = PublicationType.Talk,
                PublicationUri = new Uri("https://www.mytechready.com/"),
                PublicationName = "TechReady",
                Line1 = "TechReady",
                Line2 = "Speaker"
            };
            var talk2 = new Publication
            {
                PublicationType = PublicationType.Talk,
                PublicationUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri),
                PublicationName = "Hybrid Application Architecture. Live Telecast.",
                Line1 = "Virtual Tech Days India",
                Line2 = "Speaker"
            };

            //// Populate Community
            var community1 = new Publication
            {
                PublicationType = PublicationType.Community,
                PublicationUri = new Uri("https://msdn.microsoft.com/en-us/library/dn622074.aspx"),
                PublicationName = "Cloud Design Patterns",
                Line1 = "Publisher: Patterns and Practices",
                Line2 = "Contributor"
            };
            var community2 = new Publication
            {
                PublicationType = PublicationType.Community,
                PublicationUri = new Uri("https://msdn.microsoft.com/en-us/library/jj613124(v=pandp.50).aspx"),
                PublicationName = "Enterprise Library Integration Pack for Microsoft Azure",
                Line1 = "Publisher: Patterns and Practices",
                Line2 = "Contributor"
            };

            //// Populate Upcoming
            var upcoming1 = new Publication
            {
                PublicationType = PublicationType.Upcoming,
                PublicationUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri),
                PublicationName = "Big Data Analysis and Predictions Framework",
                Line1 = "Team: Join me",
                Line2 = "We are working on a framework to collect data from DF Pipelines, Logic Apps etc. and moderate data using workflows. The data would be moved to Big Data repository on top of which we will run custom big data analysis frameworks and use ML to predict outcomes."
            };

            publications.AddRange(new[] { book1, book2, article1, article2, talk1, talk2, community1, community2, upcoming1 });
            return publications;
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
            postedTestimonial.AuthorDesignation = string.IsNullOrWhiteSpace(postedTestimonial.AuthorDesignation) ? string.Empty : Regex.Replace(postedTestimonial.AuthorDesignation.Trim(), @"\s+", " ");
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