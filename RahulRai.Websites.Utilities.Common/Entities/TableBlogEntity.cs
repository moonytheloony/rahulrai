using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RahulRai.Websites.Utilities.Common.Entities
{
    using Helpers;
    using RegularTypes;

    public class TableBlogEntity
    {
        public string Title { get; set; }
        public IList<string> Body { get; set; }

        public string BlogKey { get; set; }

        public string BlogId { get; set; }

        public DateTime PostedDate { get; set; }

        public string EntityTag { get; set; }

        public bool IsDraft { get; set; }

        public TableBlogEntity(BlogPost post)
        {
            this.Title = post.Title;
            this.Body = post.Body.SplitByLength(ApplicationConstants.ContentSplitLength).ToList();
            this.BlogKey = post.BlogKey;
            this.BlogId = post.BlogId;
            this.PostedDate = post.PostedDate == DateTime.MinValue ? DateTime.UtcNow : post.PostedDate;
            this.EntityTag = post.EntityTag;
            this.IsDraft = post.IsDraft;
        }

        public static BlogPost GetBlogPost(TableBlogEntity entity)
        {
            return new BlogPost
            {
                Title = entity.Title,
                Body = entity.Body.Combine(),
                PostedDate = entity.PostedDate,
                EntityTag = entity.EntityTag,
                IsDraft = entity.IsDraft
            };
        }
    }
}
