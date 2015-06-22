namespace RahulRai.Websites.BlogSite.Web.UI.Controllers
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web.Mvc;
    using GlobalAccess;
    using Microsoft.WindowsAzure.Storage.Table;
    using Microsoft.WindowsAzure.Storage.Table.Queryable;
    using Utilities.AzureStorage.TableStorage;
    using Utilities.Common.Entities;
    using Utilities.Common.RegularTypes;
    using Utilities.Web;

    #endregion

    public class BlogController : BaseController
    {
        private readonly AzureTableStorageService<TableBlogEntity> blogContext = BlogStoreAccess.Instance.BlogTable;
        private readonly int pageSize = int.Parse(ConfigurationManager.AppSettings["BlogListPageSize"]);
        private readonly string rowKeyToUse = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);

        public ContinuationStack ContinuationStack
        {
            get
            {
                var stack = (ContinuationStack) Session[ApplicationConstants.StackKey];
                if (stack != null)
                {
                    return stack;
                }

                stack = new ContinuationStack();
                Session[ApplicationConstants.StackKey] = stack;
                return stack;
            }
        }

        public ActionResult GetLatestBlogs()
        {
            //Session[ApplicationConstants.StackKey] = null;
            //var result = GetPagedBlogPreviews(null);
            //var resultBlogs = result.Select(AzureTableStorageAssist.ConvertDynamicEntityToEntity<TableBlogEntity>);
            //var blogList = resultBlogs.Select(TableBlogEntity.GetBlogPost);
            //SetPreviousNextPage();
            return View("BlogList", new List<BlogPost>());
        }

        private TableQuerySegment<DynamicTableEntity> GetPagedBlogPreviews(TableContinuationToken token)
        {
            var activeTable = blogContext.CustomOperation();
            var query = (from record in activeTable.CreateQuery<DynamicTableEntity>()
                where record.PartitionKey == ApplicationConstants.BlogKey
                      && record["IsDraft"].BooleanValue == false
                      && record["IsDeleted"].BooleanValue == false
                      && String.Compare(record.RowKey, rowKeyToUse, StringComparison.OrdinalIgnoreCase) > 0
                select record).Take(pageSize);
            var result = query.AsTableQuery().ExecuteSegmented(token, blogContext.TableRequestOptions);
            ContinuationStack.AddToken(result.ContinuationToken);
            return result;
        }

        private void SetPreviousNextPage()
        {
            throw new NotImplementedException();
        }

        public ActionResult GetSearchedBlogs(string searchTerm)
        {
            var blogList = new List<BlogPost>();
            return View("BlogList", blogList);
        }


        public ActionResult GoPrevious()
        {
            var segment = ContinuationStack.GetBackToken();
            var entityResult = GetPagedBlogPreviews(segment);
            var resultBlogs = entityResult.Select(AzureTableStorageAssist.ConvertDynamicEntityToEntity<TableBlogEntity>);
            var blogList = resultBlogs.Select(TableBlogEntity.GetBlogPost);
            SetPreviousNextPage();
            return View("BlogList", blogList);
        }

        public ActionResult GoNext()
        {
            var segment = ContinuationStack.GetForwardToken();
            var entityResult = GetPagedBlogPreviews(segment);
            var resultBlogs = entityResult.Select(AzureTableStorageAssist.ConvertDynamicEntityToEntity<TableBlogEntity>);
            var blogList = resultBlogs.Select(TableBlogEntity.GetBlogPost);
            SetPreviousNextPage();
            return View("BlogList", blogList);
        }

        public ActionResult GetBlogPost()
        {
            return View("BlogPost");
        }

        public ActionResult GoArchieve()
        {
            var blogList = new List<BlogPost>();
            return View("BlogList", blogList);
        }
    }
}