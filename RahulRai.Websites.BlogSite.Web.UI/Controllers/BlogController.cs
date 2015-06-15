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
                if (stack == null)
                {
                    stack = new ContinuationStack();
                    Session[ApplicationConstants.StackKey] = stack;
                }

                return stack;
            }
        }

        public ActionResult GetLatestBlogs()
        {
            //var result = GetPagedBlogPreviews(null);
            //var resultBlogs = result.Select(AzureTableStorageAssist.ConvertDynamicEntityToEntity<TableBlogEntity>);
            //var blogList = resultBlogs.Select(TableBlogEntity.GetBlogPostPreview);
            //var blogCollection = new BlogPostPreviewCollection()
            //{
            //    BlogPostPreviews = blogList.ToList(),
            //    ContinuationStack = new ContinuationStack()
            //};
            //SetPreviousNextPage(result.ContinuationToken);
            var blogCollection = new List<BlogPost>();

            var continuationStack = new ContinuationStack();
            continuationStack.AddToken(new TableContinuationToken {NextPartitionKey = "one"});
            continuationStack.AddToken(new TableContinuationToken {NextPartitionKey = "two"});
            blogCollection.Add(new BlogPost
            {
                BlogId = "1",
                Body = "some text",
                IsDraft = false,
                IsDeleted = false,
                PostedDate = DateTime.UtcNow,
                Title = "title1"
            });
            blogCollection.Add(new BlogPost
            {
                BlogId = "2",
                Body = "some text2",
                IsDraft = false,
                IsDeleted = false,
                PostedDate = DateTime.UtcNow,
                Title = "title2"
            });
            blogCollection.Add(new BlogPost
            {
                BlogId = "3",
                Body = "some text3",
                IsDraft = false,
                IsDeleted = false,
                PostedDate = DateTime.UtcNow,
                Title = "title13"
            });
            return View("BlogList", blogCollection);
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
            return result;
        }

        private void SetPreviousNextPage(object continuationToken)
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

            var blogList = new List<BlogPost>();
            return View("BlogList", blogList);
        }

        public ActionResult GoNext()
        {
            var segment = ContinuationStack.GetForwardToken();
            var blogList = new List<BlogPost>();
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