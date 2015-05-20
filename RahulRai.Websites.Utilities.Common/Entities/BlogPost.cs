namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using System;
    using Helpers;
    using RegularTypes;

    #endregion

    public class BlogPost
    {
        public string Title { get; set; }
        public string Body { get; set; }

        public string BlogKey
        {
            get { return ApplicationConstants.BlogKey; }
        }

        public string BlogId
        {
            get { return Routines.FormatTitle(Title.ToLowerInvariant()); }
        }

        public DateTime PostedDate { get; set; }
        public string EntityTag { get; set; }

        public bool IsDraft { get; set; }

        public bool IsDeleted { get; set; }
    }
}