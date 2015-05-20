#region

using System;
using CookComputing.XmlRpc;

#endregion

public interface IMetaWeblog
{
    #region MetaWeblog API

    [XmlRpcMethod("metaWeblog.newPost")]
    string AddPost(string blogid, string username, string password, dynamic post, bool publish);

    [XmlRpcMethod("metaWeblog.editPost")]
    bool UpdatePost(string postid, string username, string password, dynamic post, bool publish);

    [XmlRpcMethod("metaWeblog.getPost")]
    object GetPost(string postid, string username, string password);

    [XmlRpcMethod("metaWeblog.getCategories")]
    object[] GetCategories(string blogid, string username, string password);

    [XmlRpcMethod("metaWeblog.getRecentPosts")]
    object[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts);

    [XmlRpcMethod("metaWeblog.newMediaObject")]
    object NewMediaObject(string blogid, string username, string password, MediaObject mediaObject);

    #endregion

    #region Blogger API

    [XmlRpcMethod("blogger.deletePost")]
    [return: XmlRpcReturnValue(Description = "Returns true.")]
    bool DeletePost(string key, string postid, string username, string password, bool publish);

    [XmlRpcMethod("blogger.getUsersBlogs")]
    object[] GetUsersBlogs(string key, string username, string password);

    #endregion
}

public class MetaWeblogHandler : XmlRpcService, IMetaWeblog
{
    string IMetaWeblog.AddPost(string blogid, string username, string password, dynamic post, bool publish)
    {
        ValidateUser(username, password);
        return "someval";
    }

    bool IMetaWeblog.UpdatePost(string postid, string username, string password, dynamic post, bool publish)
    {
        ValidateUser(username, password);

        return true;
    }

    bool IMetaWeblog.DeletePost(string key, string postid, string username, string password, bool publish)
    {
        ValidateUser(username, password);

        return false;
    }

    object IMetaWeblog.GetPost(string postid, string username, string password)
    {
        ValidateUser(username, password);

        return new
        {
            description = "test description",
            title = "title",
            dateCreated = DateTime.UtcNow,
            wp_slug = "adsadad",
            categories = new[] {"cat1", "cat2"},
            postid = "asdad"
        };
    }

    object[] IMetaWeblog.GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
    {
        ValidateUser(username, password);

        return new[]
        {
            new
            {
                description = "test description",
                title = "titleee",
                dateCreated = DateTime.UtcNow,
                wp_slug = "adsadad",
                categories = new[] {"cat1", "cat2"},
                postid = "asdad"
            }
        };
    }

    object[] IMetaWeblog.GetCategories(string blogid, string username, string password)
    {
        ValidateUser(username, password);

        return new[] {"cat1", "cat2"};
    }

    object IMetaWeblog.NewMediaObject(string blogid, string username, string password, MediaObject media)
    {
        ValidateUser(username, password);

        return "newurl";
    }

    object[] IMetaWeblog.GetUsersBlogs(string key, string username, string password)
    {
        ValidateUser(username, password);

        return new[]
        {
            new
            {
                blogid = "1",
                blogName = "myblog",
                url = Context.Request.Url.Scheme + "://" + Context.Request.Url.Authority
            }
        };
    }

    private void ValidateUser(string username, string password)
    {
        //// empty
    }
}

[XmlRpcMissingMapping(MappingAction.Ignore)]
public struct MediaObject
{
    public byte[] bits;
    public string name;
    public string type;
}