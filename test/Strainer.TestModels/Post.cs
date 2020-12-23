using Fluorite.Strainer.Attributes;
using System;

namespace Fluorite.Strainer.TestModels
{
    public class Post
    {
        [StrainerProperty]
        public int? CategoryId { get; set; }

        [StrainerProperty]
        public int CommentCount { get; set; }

        [StrainerProperty]
        public DateTimeOffset DateCreated { get; set; }

        public Comment FeaturedComment { get; set; }

        public int Id { get; set; }

        [StrainerProperty]
        public bool IsDraft { get; set; }

        [StrainerProperty]
        public int LikeCount { get; set; }

        public int OnlySortableViaFluentApi { get; set; }

        public string ThisHasNoAttributeButIsAccessible { get; set; }

        [StrainerProperty]
        public string Title { get; set; }

        public Comment TopComment { get; set; }
    }
}
