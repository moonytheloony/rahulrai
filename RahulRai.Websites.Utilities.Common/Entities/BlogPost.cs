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

        public string BlogId
        {
            get
            {
                return string.IsNullOrWhiteSpace(blogId) ? Routines.FormatTitle(Title.ToLowerInvariant()) : blogId;
                ;
            }
            set { blogId = string.IsNullOrWhiteSpace(value) ? Routines.FormatTitle(Title.ToLowerInvariant()) : value; }
        }

        public DateTime PostedDate { get; set; }
        public string EntityTag { get; set; }

        public bool IsDraft { get; set; }

        public bool IsDeleted { get; set; }
    }
}