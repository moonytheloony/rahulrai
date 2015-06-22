namespace RahulRai.Websites.Tools.DocumentTool
{
    #region

    using System;
    using System.Configuration;
    using Utilities.AzureStorage.Search;
    using Utilities.Common.RegularTypes;

    #endregion

    internal class Program
    {
        private static readonly string SearchServiceKey = ConfigurationManager.AppSettings[ApplicationConstants.SearchServiceKey];
        private static readonly string SearchServiceName = ConfigurationManager.AppSettings[ApplicationConstants.SearchServiceKey];
        private static AzureSearchService searchService;

        private static void Main(string[] args)
        {
            Console.WriteLine("Options: \n1. Clear Search (ALL) \n2. Clear Search (Document)");
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
                default:
                    Console.WriteLine("Invalid Option");
                    break;
            }

            Console.WriteLine("Completed");
            Console.ReadKey();
        }

        private static void ClearSearchDocument(string blogId)
        {
            searchService.DeleteData(blogId);
        }

        private static void ClearSearchAll()
        {
            searchService.DeleteIndex(ApplicationConstants.SearchIndex);
        }
    }
}