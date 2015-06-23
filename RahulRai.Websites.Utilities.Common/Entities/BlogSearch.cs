namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using Microsoft.Azure.Search.Models;

    #endregion

    /// <summary>
    ///     Class BlogSearch.
    /// </summary>
    [SerializePropertyNamesAsCamelCase]
    public class BlogSearch
    {
        /// <summary>
        ///     Gets or sets the blog identifier.
        /// </summary>
        /// <value>The blog identifier.</value>
        public string BlogId { get; set; }

        /// <summary>
        ///     Gets or sets the search tags.
        /// </summary>
        /// <value>The search tags.</value>
        public string[] SearchTags { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
    }
}