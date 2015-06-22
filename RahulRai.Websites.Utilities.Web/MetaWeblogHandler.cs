namespace RahulRai.Websites.Utilities.Web
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using AzureStorage.BlobStorage;
    using AzureStorage.Search;
    using AzureStorage.TableStorage;
    using Common.Entities;
    using Common.Exceptions;
    using Common.Helpers;
    using Common.RegularTypes;
    using CookComputing.XmlRpc;
    using Microsoft.WindowsAzure.Storage.Table;

    #endregion

    public class MetaWeblogHandler : XmlRpcService, IMetaWeblog
    {
        private readonly string blogResourceContainerName =
            ConfigurationManager.AppSettings[ApplicationConstants.BlogResourceContainerName];

        private readonly string blogTableName = ConfigurationManager.AppSettings[ApplicationConstants.BlogTableName];
        private readonly string connectionString = ConfigurationManager.AppSettings[ApplicationConstants.StorageAccountConnectionString];
        private readonly BlobStorageService mediaStorageService;
        private readonly AzureTableStorageService<TableBlogEntity> metaweblogTable;
        private readonly AzureSearchService searchService;
        private readonly string searchServiceKey = ConfigurationManager.AppSettings[ApplicationConstants.SearchServiceKey];
        private readonly string searchServiceName = ConfigurationManager.AppSettings[ApplicationConstants.SearchServiceName];

        public MetaWeblogHandler()
        {
            if (null == metaweblogTable)
            {
                metaweblogTable = new AzureTableStorageService<TableBlogEntity>(
                    connectionString,
                    blogTableName,
                    AzureTableStorageAssist.ConvertEntityToDynamicTableEntity,
                    AzureTableStorageAssist.ConvertDynamicEntityToEntity<TableBlogEntity>);
                metaweblogTable.CreateStorageObjectAndSetExecutionContext();
            }
            if (null == mediaStorageService)
            {
                mediaStorageService = new BlobStorageService(connectionString);
                if (FileOperationStatus.FolderCreated !=
                    mediaStorageService.CreateContainer(blogResourceContainerName, VisibilityType.FilesVisibleToAll))
                {
                    throw new BlogSystemException("unable to create container");
                }
            }

            if (null == searchService)
            {
                searchService = new AzureSearchService(searchServiceName, searchServiceKey,
                    ApplicationConstants.SearchIndex);
            }
        }

        string IMetaWeblog.AddPost(string blogid, string username, string password, dynamic post, bool publish)
        {
            ValidateUser(username, password);
            var postTitle = post["title"];
            var description = post["description"];
            var categories = (null == post["categories"] || null == post["categories"] as string[])
                ? string.Empty
                : (post["categories"] as string[]).ToCsv();
            var blogPost = new BlogPost
            {
                Body = description,
                IsDeleted = false,
                Title = postTitle,
                CategoriesCsv = categories,
                PostedDate = DateTime.UtcNow,
                IsDraft = !publish
            };

            var tablePost = new TableBlogEntity(blogPost);
            metaweblogTable.InsertOrReplace(tablePost);
            var result = metaweblogTable.SaveAll();
            if (!result.All(element => element.IsSuccess))
            {
                throw new BlogSystemException("Can not save blog post");
            }

            //Create search document if search terms exist.
            if (!string.IsNullOrWhiteSpace(categories))
            {
                searchService.UpsertDataToIndex(new BlogSearch
                {
                    BlogId = blogPost.BlogId,
                    SearchTags = blogPost.CategoriesCsv.ToCollection().ToArray(),
                    Title = blogPost.Title
                });
            }

            return blogPost.BlogId;
        }

        bool IMetaWeblog.UpdatePost(string postid, string username, string password, dynamic post, bool publish)
        {
            ValidateUser(username, password);
            var postTitle = post["title"];
            var description = post["description"];
            var categories = (null == post["categories"] || null == post["categories"] as string[])
                ? string.Empty
                : (post["categories"] as string[]).ToCsv();
            var blogPost = new BlogPost
            {
                Body = description,
                IsDeleted = false,
                Title = postTitle,
                PostedDate = DateTime.UtcNow,
                CategoriesCsv = categories,
                IsDraft = !publish,
                BlogId = postid //Persist post id
            };
            var tablePost = new TableBlogEntity(blogPost);
            metaweblogTable.InsertOrReplace(tablePost);
            var result = metaweblogTable.SaveAll();
            if (!result.All(element => element.IsSuccess))
            {
                throw new BlogSystemException("Can not update blog post");
            }

            //Update search document.
            searchService.UpsertDataToIndex(new BlogSearch
            {
                BlogId = blogPost.BlogId,
                SearchTags =
                    string.IsNullOrWhiteSpace(blogPost.CategoriesCsv)
                        ? new[] {string.Empty}
                        : blogPost.CategoriesCsv.ToCollection().ToArray(),
                Title = blogPost.Title
            });

            return true;
        }

        bool IMetaWeblog.DeletePost(string key, string postid, string username, string password, bool publish)
        {
            ValidateUser(username, password);
            var blogPost = metaweblogTable.GetById(ApplicationConstants.BlogKey, postid);
            blogPost.IsDeleted = true;
            metaweblogTable.Update(blogPost);

            //Delete search document.
            searchService.DeleteData(postid);
            return true;
        }

        object IMetaWeblog.GetPost(string postid, string username, string password)
        {
            ValidateUser(username, password);
            var blogPost = metaweblogTable.GetById(ApplicationConstants.BlogKey, postid);
            var blog = TableBlogEntity.GetBlogPost(blogPost);
            if (null == blog)
            {
                return null;
            }

            return new
            {
                description = blog.Body,
                title = blog.Title,
                dateCreated = blog.PostedDate,
                wp_slug = string.Empty,
                categories =
                    string.IsNullOrWhiteSpace(blogPost.CategoriesCsv)
                        ? new[] {string.Empty}
                        : blogPost.CategoriesCsv.ToCollection().ToArray(),
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
                postid = element.BlogFormattedUri
            }).ToArray();
            // ReSharper restore CoVariantArrayConversion
        }

        object[] IMetaWeblog.GetCategories(string blogid, string username, string password)
        {
            ValidateUser(username, password);
            ////Get All Blog Search Keys
            var activeTable = metaweblogTable.CustomOperation();
            //// Create a projected query to get all categories.
            var partitionKeyQuery = TableQuery.GenerateFilterCondition(KnownTypes.PartitionKey, QueryComparisons.Equal,
                ApplicationConstants.BlogKey);
            var query = new TableQuery().Where(partitionKeyQuery);
            var result = activeTable.ExecuteQuery(query.Select(new[] {"CategoriesCsv"}),
                metaweblogTable.TableRequestOptions);
            var resultList = result as IList<DynamicTableEntity> ?? result.ToList();
            if (!resultList.Any())
            {
                return null;
            }

            ////Combine all categories.
            var dynamicTableEntities = result as IList<DynamicTableEntity> ?? resultList.ToList();
            var allCategories = string.Join(KnownTypes.CsvSeparator.ToInvariantCultureString(),
                dynamicTableEntities.Select(element => element["CategoriesCsv"].StringValue)).ToCollection();
            var categories = allCategories as IList<string> ?? allCategories.ToList();
            var posts = new XmlRpcStruct[categories.Count()];
            var counter = 0;
            foreach (var category in categories)
            {
                var rpcstruct = new XmlRpcStruct
                {
                    {"categoryid", counter.ToInvariantCultureString()},
                    {"title", category},
                    {"description", category}
                };

                posts[counter++] = rpcstruct;
            }

            return posts;
        }

        object IMetaWeblog.NewMediaObject(string blogid, string username, string password, dynamic media)
        {
            ValidateUser(username, password);
            var resourceStream = new MemoryStream(media["bits"]);
            return new
            {
                url = mediaStorageService.AddBlobToContainer(blogResourceContainerName, resourceStream, media["name"])
                    .ToString()
            };
        }

        object[] IMetaWeblog.GetUsersBlogs(string key, string username, string password)
        {
            ValidateUser(username, password);
            // There is only one publisher, so this should return a default value.
            return new object[]
            {
                new
                {
                    blogName = "rahulrai",
                    url = Context.Request.Url.Scheme + "://" + Context.Request.Url.Authority,
                    blogid = string.Empty
                }
            };
        }


        public int NewCategory(string blogid, string username, string password, dynamic category)
        {
            ValidateUser(username, password);
            //// Use categories to index this blog post in search service.
            return 1;
        }

        private static void ValidateUser(string username, string password)
        {
            var adminName = ConfigurationManager.AppSettings[ApplicationConstants.PublisherName];
            var secret = ConfigurationManager.AppSettings[ApplicationConstants.Secret];
            if (!(string.Equals(username, adminName, StringComparison.OrdinalIgnoreCase) &&
                  string.Equals(password, secret, StringComparison.InvariantCulture)))
            {
                throw new UnauthorizedAccessException(string.Format("Not Allowed U:{0} P:{1}", username, password));
            }
        }
    }
}