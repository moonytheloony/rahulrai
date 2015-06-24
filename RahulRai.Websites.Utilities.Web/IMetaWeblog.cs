// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Web
// Author           : rahulrai
// Created          : 05-27-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="IMetaWeblog.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Web
{
    #region

    using CookComputing.XmlRpc;

    #endregion

    /// <summary>
    /// Metaweblog interface
    /// </summary>
    public interface IMetaWeblog
    {
        #region MetaWeblog API

        /// <summary>
        /// Adds the post.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="post">The post.</param>
        /// <param name="publish">if set to <c>true</c> [publish].</param>
        /// <returns>System.String.</returns>
        [XmlRpcMethod("metaWeblog.newPost")]
        string AddPost(string blogid, string username, string password, dynamic post, bool publish);

        /// <summary>
        /// Updates the post.
        /// </summary>
        /// <param name="postid">The postid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="post">The post.</param>
        /// <param name="publish">if set to <c>true</c> [publish].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [XmlRpcMethod("metaWeblog.editPost")]
        bool UpdatePost(string postid, string username, string password, dynamic post, bool publish);

        /// <summary>
        /// Gets the post.
        /// </summary>
        /// <param name="postid">The postid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>System.Object.</returns>
        [XmlRpcMethod("metaWeblog.getPost")]
        object GetPost(string postid, string username, string password);

        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>System.Object[].</returns>
        [XmlRpcMethod("metaWeblog.getCategories")]
        object[] GetCategories(string blogid, string username, string password);

        /// <summary>
        /// Gets the recent posts.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="numberOfPosts">The number of posts.</param>
        /// <returns>System.Object[].</returns>
        [XmlRpcMethod("metaWeblog.getRecentPosts")]
        object[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts);

        /// <summary>
        /// News the media object.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="mediaObject">The media object.</param>
        /// <returns>System.Object.</returns>
        [XmlRpcMethod("metaWeblog.newMediaObject")]
        object NewMediaObject(string blogid, string username, string password, dynamic mediaObject);

        /// <summary>
        /// News the category.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="category">The category.</param>
        /// <returns>System.Int32.</returns>
        [XmlRpcMethod("wp.newCategory",
            Description = "Adds a new category to the blog engine.")]
        int NewCategory(string blogid, string username, string password, dynamic category);

        #endregion

        #region Blogger API

        /// <summary>
        /// Deletes the post.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="postid">The postid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="publish">if set to <c>true</c> [publish].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [XmlRpcMethod("blogger.deletePost")]
        [return: XmlRpcReturnValue(Description = "Returns true.")]
        bool DeletePost(string key, string postid, string username, string password, bool publish);

        /// <summary>
        /// Gets the users blogs.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>System.Object[].</returns>
        [XmlRpcMethod("blogger.getUsersBlogs")]
        object[] GetUsersBlogs(string key, string username, string password);

        #endregion
    }
}