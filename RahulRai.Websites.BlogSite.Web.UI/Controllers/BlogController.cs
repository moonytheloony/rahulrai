namespace RahulRai.Websites.BlogSite.Web.UI.Controllers
{
    #region

    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web.Mvc;
    using GlobalAccess;
    using Microsoft.WindowsAzure.Storage.Table;
    using Utilities.AzureStorage.TableStorage;
    using Utilities.Common.Entities;
    using Utilities.Common.RegularTypes;
    using Utilities.Web;

    #endregion

    public class BlogController : BaseController
    {
        private readonly AzureTableStorageService<TableBlogEntity> blogContext = BlogStoreAccess.Instance.BlogTable;
        private readonly int pageSize = int.Parse(ConfigurationManager.AppSettings["BlogListPageSize"]);

        public ActionResult GetLatestBlogs()
        {
            var activeTable = blogContext.CustomOperation();
            //// Create a projected query and order by date posted.
            var partitionKey = TableQuery.GenerateFilterCondition(KnownTypes.PartitionKey, QueryComparisons.Equal,
                ApplicationConstants.BlogKey);
            var notDraft = TableQuery.GenerateFilterCondition("IsDraft", QueryComparisons.Equal, false.ToString());
            var notDeleted = TableQuery.GenerateFilterCondition("IsDeleted", QueryComparisons.Equal, false.ToString());
            var querySegment = TableQuery.CombineFilters(partitionKey, TableOperators.And, notDraft);
            var completeQuery = TableQuery.CombineFilters(querySegment, TableOperators.And, notDeleted);
            var query = new TableQuery().Where(completeQuery);
            var result = activeTable.ExecuteQuery(query.Select(new[]
            {
                KnownTypes.RowKey,
                KnownTypes.PartitionKey,
                KnownTypes.Timestamp,
                "AutoIndexedElement_0_Body",
                "PostedDate",
                "Title",
                "IsDeleted",
                "IsDraft"
            }), blogContext.TableRequestOptions);
            var resultBlogs =
                result.Select(AzureTableStorageAssist.ConvertDynamicEntityToEntity<TableBlogEntity>)
                    .ToList()
                    .OrderByDescending(element => element.PostedDate)
                    .Take(pageSize);
            var blogEntity = resultBlogs.Select(TableBlogEntity.GetBlogPost);
            var blogList = new List<BlogPostPreview>();
            return View("BlogList", blogList);

            //string rowKeyToUse = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);

            //var results = (from g in tableServiceContext.CreateQuery<LogEvent>("logevent")
            //               where g.PartitionKey == User.Identity.Name
            //               && g.RowKey.CompareTo(rowKeyToUse) > 0
            //               select g).Take(100);
        }

        public ActionResult GetSearchedBlogs(string searchTerm)
        {
            var blogList = new List<BlogPostPreview>();
            return View("BlogList", blogList);
        }

        public ActionResult GetBlogPost()
        {
            return View("BlogPost");
        }
    }
}