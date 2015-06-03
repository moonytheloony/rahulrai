namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using System;
    using Helpers;
    using RegularTypes;

    #endregion

    public class BlogPost
    {
        private string blogId;
        public string Title { get; set; }
        public string Body { get; set; }

        public string BlogKey
        {
            get { return ApplicationConstants.BlogKey; }
        }

        public string BlogFormattedUri
        {
            get { return Routines.FormatTitle(Title.ToLowerInvariant()); }
        }

        public string BlogId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(blogId))
                {
                    blogId = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);
                }

                return blogId;
            }
            set { blogId = value; }
        }

        public string CategoriesCsv { get; set; }
        public DateTime PostedDate { get; set; }
        public string EntityTag { get; set; }

        public bool IsDraft { get; set; }

        public bool IsDeleted { get; set; }
    }
}