// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Web
// Author           : rahulrai
// Created          : 04-17-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="MetaWeblogHandler.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

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

    /// <summary>
    ///     Class MetaWeblogHandler. This class cannot be inherited.
    /// </summary>
    public sealed class MetaWeblogHandler : XmlRpcService, IMetaWeblog, IDisposable
    {
        /// <summary>
        ///     The blog resource container name
        /// </summary>
        private readonly string blogResourceContainerName =
            ConfigurationManager.AppSettings[ApplicationConstants.BlogResourceContainerName];

        /// <summary>
        ///     The blog table name
        /// </summary>
        private readonly string blogTableName = ConfigurationManager.AppSettings[ApplicationConstants.BlogTableName];

        /// <summary>
        ///     The connection string
        /// </summary>
        private readonly string connectionString =
            ConfigurationManager.AppSettings[ApplicationConstants.StorageAccountConnectionString];

        /// <summary>
        ///     The media storage service
        /// </summary>
        private readonly BlobStorageService mediaStorageService;

        /// <summary>
        ///     The metaweblog table
        /// </summary>
        private readonly AzureTableStorageService<TableBlogEntity> metaweblogTable;

        /// <summary>
        ///     The search service
        /// </summary>
        private readonly AzureSearchService searchService;

        /// <summary>
        ///     The search service key
        /// </summary>
        private readonly string searchServiceKey =
            ConfigurationManager.AppSettings[ApplicationConstants.SearchServiceKey];

        /// <summary>
        ///     The search service name
        /// </summary>
        private readonly string searchServiceName =
            ConfigurationManager.AppSettings[ApplicationConstants.SearchServiceName];

        /// <summary>
        ///     The disposed
        /// </summary>
        private bool disposed;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MetaWeblogHandler" /> class.
        /// </summary>
        /// <exception cref="RahulRai.Websites.Utilities.Common.Exceptions.BlogSystemException">unable to create container</exception>
        /// <exception cref="BlogSystemException">unable to create container</exception>
        public MetaWeblogHandler()
        {
            if (null == this.metaweblogTable)
            {
                this.metaweblogTable = new AzureTableStorageService<TableBlogEntity>(
                    this.connectionString,
                    this.blogTableName,
                    AzureTableStorageAssist.ConvertEntityToDynamicTableEntity,
                    AzureTableStorageAssist.ConvertDynamicEntityToEntity<TableBlogEntity>);
                this.metaweblogTable.CreateStorageObjectAndSetExecutionContext();
            }

            if (null == this.mediaStorageService)
            {
                this.mediaStorageService = new BlobStorageService(this.connectionString);
                if (FileOperationStatus.FolderCreated != this.mediaStorageService.CreateContainer(this.blogResourceContainerName, VisibilityType.FilesVisibleToAll))
                {
                    throw new BlogSystemException("unable to create container");
                }
            }

            if (null == this.searchService)
            {
                this.searchService = new AzureSearchService(
                    this.searchServiceName,
                    this.searchServiceKey,
                    ApplicationConstants.SearchIndex);
            }
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="MetaWeblogHandler" /> class.
        /// </summary>
        ~MetaWeblogHandler()
        {
            this.Dispose(false);
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Adds the post.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="post">The post.</param>
        /// <param name="publish">if set to <c>true</c> [publish].</param>
        /// <returns>System.String.</returns>
        /// <exception cref="RahulRai.Websites.Utilities.Common.Exceptions.BlogSystemException">Can not save blog post</exception>
        /// <exception cref="BlogSystemException">Can not save blog post</exception>
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
            this.metaweblogTable.InsertOrReplace(tablePost);
            var result = this.metaweblogTable.SaveAll();
            if (!result.All(element => element.IsSuccess))
            {
                throw new BlogSystemException("Can not save blog post");
            }

            ////Create search document if search terms exist.
            if (!string.IsNullOrWhiteSpace(categories))
            {
                this.searchService.UpsertDataToIndex(new BlogSearch
                {
                    BlogId = blogPost.BlogId,
                    SearchTags = blogPost.CategoriesCsv.ToCollection().ToArray(),
                    Title = blogPost.Title
                });
            }

            return blogPost.BlogId;
        }

        /// <summary>
        ///     Updates the post.
        /// </summary>
        /// <param name="postid">The postid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="post">The post.</param>
        /// <param name="publish">if set to <c>true</c> [publish].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="RahulRai.Websites.Utilities.Common.Exceptions.BlogSystemException">Can not update blog post</exception>
        /// <exception cref="BlogSystemException">Can not update blog post</exception>
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
                BlogId = postid ////Persist post id
            };
            var tablePost = new TableBlogEntity(blogPost);
            this.metaweblogTable.InsertOrReplace(tablePost);
            var result = this.metaweblogTable.SaveAll();
            if (!result.All(element => element.IsSuccess))
            {
                throw new BlogSystemException("Can not update blog post");
            }

            ////Update search document.
            this.searchService.UpsertDataToIndex(new BlogSearch
            {
                BlogId = blogPost.BlogId,
                SearchTags =
                    string.IsNullOrWhiteSpace(blogPost.CategoriesCsv)
                        ? new[] { string.Empty }
                        : blogPost.CategoriesCsv.ToCollection().ToArray(),
                Title = blogPost.Title
            });

            return true;
        }

        /// <summary>
        ///     Deletes the post.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="postid">The postid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="publish">if set to <c>true</c> [publish].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool IMetaWeblog.DeletePost(string key, string postid, string username, string password, bool publish)
        {
            ValidateUser(username, password);
            var blogPost = this.metaweblogTable.GetById(ApplicationConstants.BlogKey, postid);
            blogPost.IsDeleted = true;
            this.metaweblogTable.Update(blogPost);

            ////Delete search document.
            this.searchService.DeleteData(postid);
            return true;
        }

        /// <summary>
        ///     Gets the post.
        /// </summary>
        /// <param name="postid">The postid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>System.Object.</returns>
        object IMetaWeblog.GetPost(string postid, string username, string password)
        {
            ValidateUser(username, password);
            var blogPost = this.metaweblogTable.GetById(ApplicationConstants.BlogKey, postid);
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
                        ? new[] { string.Empty }
                        : blogPost.CategoriesCsv.ToCollection().ToArray(),
                postid = blog.BlogId
            };
        }

        /// <summary>
        ///     Gets the recent posts.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="numberOfPosts">The number of posts.</param>
        /// <returns>System.Object[].</returns>
        object[] IMetaWeblog.GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            ValidateUser(username, password);
            var activeTable = this.metaweblogTable.CustomOperation();
            //// Create a projected query and order by date posted.
            var partitionKey = TableQuery.GenerateFilterCondition(KnownTypes.PartitionKey, QueryComparisons.Equal, ApplicationConstants.BlogKey);
            var rowKey = TableQuery.GenerateFilterCondition(KnownTypes.RowKey, QueryComparisons.Equal, blogid);
            var notDeleted = TableQuery.GenerateFilterCondition("IsDeleted", QueryComparisons.Equal, false.ToString());
            var querySegment = TableQuery.CombineFilters(partitionKey, TableOperators.And, rowKey);
            var completeQuery = TableQuery.CombineFilters(querySegment, TableOperators.And, notDeleted);
            var query = new TableQuery().Where(completeQuery);
            var result = activeTable.ExecuteQuery(
                query.Select(
                new[]
                {
                    KnownTypes.RowKey,
                    KnownTypes.PartitionKey,
                    KnownTypes.Timestamp,
                    "AutoIndexedElement_0_Body",
                    "PostedDate",
                    "Title",
                    "IsDeleted",
                    "IsDraft"
                }),
            this.metaweblogTable.TableRequestOptions);
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
                categories = new[] { string.Empty },
                postid = element.BlogFormattedUri
            }).ToArray();
            // ReSharper restore CoVariantArrayConversion
        }

        /// <summary>
        ///     Gets the categories.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>System.Object[].</returns>
        object[] IMetaWeblog.GetCategories(string blogid, string username, string password)
        {
            ValidateUser(username, password);
            ////Get All Blog Search Keys
            var activeTable = this.metaweblogTable.CustomOperation();
            //// Create a projected query to get all categories.
            var partitionKeyQuery = TableQuery.GenerateFilterCondition(
                KnownTypes.PartitionKey,
                QueryComparisons.Equal,
                ApplicationConstants.BlogKey);
            var query = new TableQuery().Where(partitionKeyQuery);
            var result = activeTable.ExecuteQuery(query.Select(new[] { "CategoriesCsv" }), this.metaweblogTable.TableRequestOptions);
            var resultList = result as IList<DynamicTableEntity> ?? result.ToList();
            if (!resultList.Any())
            {
                return null;
            }

            ////Combine all categories.
            var dynamicTableEntities = result as IList<DynamicTableEntity> ?? resultList.ToList();
            var allCategories = string.Join(KnownTypes.CsvSeparator.ToInvariantCultureString(), dynamicTableEntities.Select(element => element["CategoriesCsv"].StringValue)).ToCollection();
            var categories = allCategories as IList<string> ?? allCategories.ToList();
            var posts = new XmlRpcStruct[categories.Count()];
            var counter = 0;
            foreach (var category in categories)
            {
                var rpcstruct = new XmlRpcStruct
                {
                    { "categoryid", counter.ToInvariantCultureString() },
                    { "title", category },
                    { "description", category }
                };

                posts[counter++] = rpcstruct;
            }

            return posts;
        }

        /// <summary>
        ///     News the media object.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="media">The media.</param>
        /// <returns>System.Object.</returns>
        object IMetaWeblog.NewMediaObject(string blogid, string username, string password, dynamic media)
        {
            ValidateUser(username, password);
            var resourceStream = new MemoryStream(media["bits"]);
            return new
            {
                url =
                    this.mediaStorageService.AddBlobToContainer(
                    this.blogResourceContainerName,
                    resourceStream,
                    media["name"]).ToString()
            };
        }

        /// <summary>
        ///     Gets the users blogs.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>System.Object[].</returns>
        object[] IMetaWeblog.GetUsersBlogs(string key, string username, string password)
        {
            ValidateUser(username, password);
            //// There is only one publisher, so this should return a default value.
            return new object[]
            {
                new
                {
                    blogName = "rahulrai",
                    url = this.Context.Request.Url.Scheme + "://" + this.Context.Request.Url.Authority,
                    blogid = string.Empty
                }
            };
        }

        /// <summary>
        ///     News the category.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="category">The category.</param>
        /// <returns>System.Int32.</returns>
        public int NewCategory(string blogid, string username, string password, dynamic category)
        {
            ValidateUser(username, password);
            //// Use categories to index this blog post in search service.
            return 1;
        }

        /// <summary>
        ///     Validates the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <exception cref="System.UnauthorizedAccessException">Not authorized</exception>
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

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.searchService.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}