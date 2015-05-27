namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Helpers;
    using RegularTypes;

    #endregion

    public class TableBlogEntity
    {
        public TableBlogEntity()
        {
        }

        public TableBlogEntity(BlogPost post)
        {
            Title = post.Title;
            Body = post.Body.SplitByLength(ApplicationConstants.ContentSplitLength).ToList();
            BlogKey = post.BlogKey;
            BlogId = post.BlogId;
            PostedDate = post.PostedDate == DateTime.MinValue ? DateTime.UtcNow : post.PostedDate;
            EntityTag = post.EntityTag;
            IsDraft = post.IsDraft;
            IsDeleted = post.IsDeleted;
        }

        public string Title { get; set; }
        public IList<string> Body { get; set; }

        public string BlogKey { get; set; }

        public string BlogId { get; set; }

        public DateTime PostedDate { get; set; }

        public string EntityTag { get; set; }

        public bool IsDraft { get; set; }
        public bool IsDeleted { get; set; }

        public static BlogPost GetBlogPost(TableBlogEntity entity)
        {
            if (null == entity)
            {
                return null;
            }

            return new BlogPost
            {
                Title = entity.Title,
                Body = entity.Body.Combine(),
                PostedDate = entity.PostedDate,
                EntityTag = entity.EntityTag,
                IsDraft = entity.IsDraft,
                IsDeleted = entity.IsDeleted
            };
        }

        public static BlogPostPreview GetBlogPostPreview(TableBlogEntity entity)
        {
            if (null == entity)
            {
                return null;
            }

            return new BlogPostPreview
            {
                Title = entity.Title,
                Body = entity.Body.FirstOrDefault(),
                PostedDate = entity.PostedDate,
                EntityTag = entity.EntityTag,
                IsDraft = entity.IsDraft,
                IsDeleted = entity.IsDeleted
            };
        }
    }
}