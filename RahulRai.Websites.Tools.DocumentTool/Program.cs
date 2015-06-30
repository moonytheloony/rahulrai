// ***********************************************************************
// Assembly         : RahulRai.Websites.Tools.DocumentTool
// Author           : rahulrai
// Created          : 05-28-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="Program.cs" company="Rahul Rai">
//     Copyright ©  2015 Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Tools.DocumentTool
{
    #region

    using System;
    using System.Configuration;
    using System.Linq;
    using Utilities.AzureStorage.Search;
    using Utilities.Common.Entities;
    using Utilities.Common.Helpers;
    using Utilities.Common.RegularTypes;

    #endregion

    /// <summary>
    ///     Class Program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        ///     The search service key
        /// </summary>
        private static readonly string SearchServiceKey =
            ConfigurationManager.AppSettings[ApplicationConstants.SearchServiceKey];

        /// <summary>
        ///     The search service name
        /// </summary>
        private static readonly string SearchServiceName =
            ConfigurationManager.AppSettings[ApplicationConstants.SearchServiceKey];

        /// <summary>
        ///     The search service
        /// </summary>
        private static AzureSearchService searchService;

        /// <summary>
        ///     Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            Console.WriteLine("Options: \n1. Clear Search (ALL) \n2. Clear Search (Document) \n3. Add Document");
            var value = Console.ReadLine();
            var input = 0;
            if (!int.TryParse(value, out input))
            {
                return;
            }

            searchService = new AzureSearchService(SearchServiceName, SearchServiceKey, ApplicationConstants.SearchIndex);
            switch (input)
            {
                case 1:
                    ClearSearchAll();
                    break;
                case 2:
                    Console.WriteLine("Enter Blog ID");
                    ClearSearchDocument(Console.ReadLine());
                    break;
                case 3:
                    AddSearchDocument();
                    break;
                default:
                    Console.WriteLine("Invalid Option");
                    break;
            }

            Console.WriteLine("Completed");
            Console.ReadKey();
        }

        /// <summary>
        /// Adds the search document.
        /// </summary>
        private static void AddSearchDocument()
        {
            Console.WriteLine("Enter Blog ID: ");
            var blogId = Console.ReadLine();
            Console.WriteLine("Enter search tags CSV: ");
            var categoriesCsv = Console.ReadLine();
            Console.WriteLine("Enter blog post title: ");
            var title = Console.ReadLine();
            searchService.UpsertDataToIndex(new BlogSearch
            {
                BlogId = blogId,
                SearchTags =
                    string.IsNullOrWhiteSpace(categoriesCsv)
                        ? new[] { string.Empty }
                        : categoriesCsv.ToCollection().ToArray(),
                Title = title
            });
        }

        /// <summary>
        ///     Clears the search document.
        /// </summary>
        /// <param name="blogId">The blog identifier.</param>
        private static void ClearSearchDocument(string blogId)
        {
            searchService.DeleteData(blogId);
        }

        /// <summary>
        ///     Clears the search all.
        /// </summary>
        private static void ClearSearchAll()
        {
            searchService.DeleteIndex(ApplicationConstants.SearchIndex);
        }
    }
}