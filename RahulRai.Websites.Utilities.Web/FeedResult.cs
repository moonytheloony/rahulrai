// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Web
// Author           : rahulrai
// Created          : 08-18-2015
//
// Last Modified By : rahulrai
// Last Modified On : 08-18-2015
// ***********************************************************************
// <copyright file="FeedResult.cs" company="Rahul Rai">
//     Copyright ©  2015 Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Web
{
    #region

    using System;
    using System.ServiceModel.Syndication;
    using System.Text;
    using System.Web.Mvc;
    using System.Xml;

    #endregion

    /// <summary>
    /// Class FeedResult.
    /// </summary>
    public class FeedResult : ActionResult
    {
        #region Fields

        /// <summary>
        /// The feed
        /// </summary>
        private readonly SyndicationFeedFormatter feed;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedResult"/> class.
        /// </summary>
        /// <param name="feed">The feed.</param>
        public FeedResult(SyndicationFeedFormatter feed)
        {
            this.feed = feed;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the content encoding.
        /// </summary>
        /// <value>The content encoding.</value>
        public Encoding ContentEncoding { get; set; }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets the feed.
        /// </summary>
        /// <value>The feed.</value>
        public SyndicationFeedFormatter Feed
        {
            get
            {
                return this.feed;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult" /> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(this.ContentType) ? this.ContentType : "application/rss+xml";

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            if (this.feed == null)
            {
                return;
            }

            using (var xmlWriter = new XmlTextWriter(response.Output))
            {
                xmlWriter.Formatting = Formatting.Indented;
                this.feed.WriteTo(xmlWriter);
            }
        }

        #endregion
    }
}