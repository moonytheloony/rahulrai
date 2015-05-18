using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RahulRai.Websites.Utilities.Common.Entities
{
    using Helpers;
    using RegularTypes;

    public class BlogPost
    {
        public BlogPost() { }
        public string Title { get; set; }
        public string Body { get; set; }

        public string BlogKey
        {
            get
            {
                return ApplicationConstants.BlogKey;
            }
        }

        public string BlogId
        {
            get
            {
                return Routines.FormatTitle(Title.ToLowerInvariant());
            }
        }

        public DateTime PostedDate { get; set; }
        public string EntityTag { get; set; }

        public bool IsDraft { get; set; }

        public bool IsDeleted { get; set; }
    }
}
