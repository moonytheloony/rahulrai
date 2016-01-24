// ***********************************************************************
// Assembly         : RahulRai.Websites.BlogSite.Web.UI
// Author           : rahulrai
// Created          : 01-24-2016
//
// Last Modified By : rahulrai
// Last Modified On : 01-24-2016
// ***********************************************************************
// <copyright file="SitemapController.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.BlogSite.Web.UI.Controllers
{
    #region

    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using System.Xml.Linq;

    using GlobalAccess;
    using Services;
    using Utilities.AzureStorage.QueueStorage;
    using Utilities.AzureStorage.TableStorage;
    using Utilities.Common.Entities;
    using Utilities.Common.RegularTypes;
    using Utilities.Web;

    #endregion

    /// <summary>
    /// Class SitemapController.
    /// </summary>
    public class SitemapController : BaseController
    {
        #region Fields

        /// <summary>
        /// The blog context
        /// </summary>
        private readonly AzureTableStorageService<TableBlogEntity> blogContext = BlogStoreAccess.Instance.BlogTable;

        /// <summary>
        /// The newsletter context
        /// </summary>
        private readonly AzureTableStorageService<TableNewsletterEntity> newsletterContext =
            NewsletterSubscriberAccess.Instance.NewsletterSubscriberTable;

        /// <summary>
        /// The queue context
        /// </summary>
        private readonly AzureQueueService newSubscriberQueueContext =
            NewSubscriberQueueAccess.Instance.AzureQueueService;

        /// <summary>
        /// The page size
        /// </summary>
        private readonly int pageSize =
            int.Parse(WebConfigurationManager.AppSettings[ApplicationConstants.BlogListPageSize]);

        /// <summary>
        /// The search records
        /// </summary>
        private readonly int searchRecordsSize =
            int.Parse(WebConfigurationManager.AppSettings[ApplicationConstants.SearchRecordsSize]);

        /// <summary>
        /// The blog service
        /// </summary>
        private BlogService blogService;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>System.Threading.Tasks.Task&lt;System.Web.Mvc.ActionResult&gt;.</returns>
        public async Task<ActionResult> Index()
        {
            var sitemap = new XDocument();
            var ns = XNamespace.Get("http://www.sitemaps.org/schemas/sitemap/0.9");
            var root = new XElement(ns + "urlset");
            //// Custom sitemap generated from archive.
            var blogList = await Task.Run(() => this.blogService.GetBlogArchive());
            foreach (var blog in blogList)
            {
                var node = new XElement(
                    ns + "url",
                    new XElement(
                        ns + "loc",
                        this.Request.Url?.GetLeftPart(UriPartial.Authority) + "/" + "post" + "/" + blog.BlogFormattedUri),
                    new XElement(ns + "lastmod", blog.PostedDate.ToString("yyyy-MM-dd")),
                    new XElement(ns + "changefreq", "always"),
                    new XElement(ns + "priority", "1"));
                root.Add(node);
            }

            sitemap.Add(root);
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            sitemap.Save(writer);
            stream.Seek(0, SeekOrigin.Begin);
            return this.File(stream, "text/xml");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the action.
        /// </summary>
        protected override void InitializeAction()
        {
            this.blogService = new BlogService(
                this.blogContext,
                this.newsletterContext,
                this.newSubscriberQueueContext,
                this.pageSize,
                this.searchRecordsSize);
        }

        #endregion
    }
}