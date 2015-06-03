namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using Microsoft.Azure.Search.Models;

    #endregion

    [SerializePropertyNamesAsCamelCase]
    public class BlogSearch
    {
        public string BlogId { get; set; }
        public string[] SearchTags { get; set; }
        public string Title { get; set; }
    }
}