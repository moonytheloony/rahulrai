#region

namespace RahulRai.Websites.Utilities.Web
{
    #region

    using Common.RegularTypes;
    using Microsoft.WindowsAzure.Storage.Table;

    #region

    using System;
    using System.Configuration;
    using System.Linq;
    using AzureStorage.TableStorage;
    using Common.Entities;
    using Common.Exceptions;
    using CookComputing.XmlRpc;

    #endregion

    #endregion

    #endregion

    public class MetaWeblogHandler : XmlRpcService, IMetaWeblog
    {
        private readonly string blogTableName = ConfigurationManager.AppSettings["BlogTableName"];
        private readonly string connectionString = ConfigurationManager.AppSettings["StorageAccountConnectionString"];
        private readonly AzureTableStorageRepository<TableBlogEntity> metaweblogTable;

        public MetaWeblogHandler()
        {
            if (null != metaweblogTable)
            {
                return;
            }

            metaweblogTable = new AzureTableStorageRepository<TableBlogEntity>(
                connectionString,
                blogTableName,
                AzureTableStorageAssist.ConvertEntityToDynamicTableEntity,
                AzureTableStorageAssist.ConvertDynamicEntityToEntity<TableBlogEntity>);
            metaweblogTable.CreateStorageObjectAndSetExecutionContext();
        }

        string IMetaWeblog.AddPost(string blogid, string username, string password, dynamic post, bool publish)
        {
            ValidateUser(username, password);
            var postTitle = post["title"];
            var description = post["description"];
            var blogPost = new BlogPost
            {
                Body = description,
                IsDeleted = false,
                Title = postTitle,
                PostedDate = DateTime.UtcNow,
                IsDraft = !publish
            };
            var tablePost = new TableBlogEntity(blogPost);
            metaweblogTable.InsertOrReplace(tablePost);
            var result = metaweblogTable.SaveAll();
            if (result.All(element => element.IsSuccess))
            {
                return blogPost.BlogId;
            }

            throw new BlogSystemException("Can not save blog post");
        }

        bool IMetaWeblog.UpdatePost(string postid, string username, string password, dynamic post, bool publish)
        {
            ValidateUser(username, password);
            var postTitle = post["title"];
            var description = post["description"];
            var blogPost = new BlogPost
            {
                Body = description,
                IsDeleted = false,
                Title = postTitle,
                PostedDate = DateTime.UtcNow,
                IsDraft = !publish
            };
            var tablePost = new TableBlogEntity(blogPost) {BlogId = postid};
            metaweblogTable.InsertOrReplace(tablePost);
            var result = metaweblogTable.SaveAll();
            if (result.All(element => element.IsSuccess))
            {
                return true;
            }

            throw new BlogSystemException("Can not update blog post");
        }

        bool IMetaWeblog.DeletePost(string key, string postid, string username, string password, bool publish)
        {
            ValidateUser(username, password);
            var blogPost = metaweblogTable.GetById(ApplicationConstants.BlogKey, postid);
            blogPost.IsDeleted = true;
            metaweblogTable.Update(blogPost);
            return true;
        }

        object IMetaWeblog.GetPost(string postid, string username, string password)
        {
            ValidateUser(username, password);
            var blogPost = metaweblogTable.GetById(ApplicationConstants.BlogKey, postid);
            var blog = TableBlogEntity.GetBlogPost(blogPost);
            return new
            {
                description = blog.Body,
                title = blog.Title,
                dateCreated = blog.PostedDate,
                wp_slug = string.Empty,
                categories = new[] {string.Empty},
                postid = blog.BlogId
            };
        }

        object[] IMetaWeblog.GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            ValidateUser(username, password);
            var activeTable = metaweblogTable.CustomOperation();
            //// Create a projected query and order by date posted.
            var partitionKey = TableQuery.GenerateFilterCondition(KnownTypes.PartitionKey, QueryComparisons.Equal,
                ApplicationConstants.BlogKey);
            var rowKey = TableQuery.GenerateFilterCondition(KnownTypes.RowKey, QueryComparisons.Equal, blogid);
            var notDeleted = TableQuery.GenerateFilterCondition("IsDeleted", QueryComparisons.Equal, false.ToString());
            var querySegment = TableQuery.CombineFilters(partitionKey, TableOperators.And, rowKey);
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
            }), metaweblogTable.TableRequestOptions);
            var resultBlogs =
                result.Select(AzureTableStorageAssist.ConvertDynamicEntityToEntity<TableBlogEntity>)
                    .ToList()
                    .OrderByDescending(element => element.PostedDate)
                    .Take(numberOfPosts);
            var blogEntity = resultBlogs.Select(TableBlogEntity.GetBlogPost);
            // ReSharper disable CoVariantArrayConversion This format is required by Live Writer
            return blogEntity.Select(element => new
            {
                description = element.Body,
                title = element.Title,
                dateCreated = element.PostedDate,
                wp_slug = element.BlogKey,
                categories = new[] {string.Empty},
                postid = element.BlogId
            }).ToArray();
            // ReSharper restore CoVariantArrayConversion
        }

        object[] IMetaWeblog.GetCategories(string blogid, string username, string password)
        {
            ValidateUser(username, password);
            //// Not using categories.
            return new object[] {string.Empty};
        }

        object IMetaWeblog.NewMediaObject(string blogid, string username, string password, MediaObject media)
        {
            ValidateUser(username, password);

            return "newurl";
        }

        object[] IMetaWeblog.GetUsersBlogs(string key, string username, string password)
        {
            ValidateUser(username, password);
            //Get all blogs
            var activeTable = metaweblogTable.CustomOperation();
            //// Create a projected query and order by date posted.
            var filter = TableQuery.GenerateFilterCondition(KnownTypes.PartitionKey, QueryComparisons.Equal,
                ApplicationConstants.BlogKey);
            var result = activeTable.ExecuteQuery(new TableQuery().Where(filter).Select(new[]
            {
                KnownTypes.RowKey,
                "Title",
                "PostedDate"
            }), metaweblogTable.TableRequestOptions);
            var blogEntity =
                result.Select(AzureTableStorageAssist.ConvertDynamicEntityToEntity<TableBlogEntity>)
                    .ToList()
                    .OrderByDescending(element => element.PostedDate);
            // ReSharper disable CoVariantArrayConversion Format expected by Live Writer
            return blogEntity.Select(element => new
            {
                blogName = element.Title,
                url = Context.Request.Url.Scheme + "://" + Context.Request.Url.Authority + "/post/" + element.BlogKey,
                blogid = element.BlogId
            }).ToArray();
            // ReSharper restore CoVariantArrayConversion
        }

        private static void ValidateUser(string username, string password)
        {
            var adminName = ConfigurationManager.AppSettings["UserName"];
            var secret = ConfigurationManager.AppSettings["Secret"];
            if (!(string.Equals(username, adminName, StringComparison.OrdinalIgnoreCase) &&
                  string.Equals(password, secret, StringComparison.InvariantCulture)))
            {
                throw new UnauthorizedAccessException(string.Format("Not Allowed U:{0} P:{1}", username, password));
            }
        }
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct MediaObject
    {
        public byte[] bits;
        public string name;
        public string type;
    }
}