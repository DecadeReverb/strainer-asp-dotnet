using Fluorite.Strainer.Attributes;
using System;

namespace Fluorite.Strainer.TestModels
{
    public class Post
    {
        [Strainer(IsFilterable = true, IsSortable = true)]
        public int? CategoryId { get; set; }

        [Strainer(IsFilterable = true, IsSortable = true)]
        public int CommentCount { get; set; }

        [Strainer(IsFilterable = true, IsSortable = true)]
        public DateTimeOffset DateCreated { get; set; }

        public Comment FeaturedComment { get; set; }

        public int Id { get; set; }

        [Strainer(IsFilterable = true, IsSortable = true)]
        public bool IsDraft { get; set; }

        [Strainer(IsFilterable = true, IsSortable = true)]
        public int LikeCount { get; set; }

        public int OnlySortableViaFluentApi { get; set; }

        public string ThisHasNoAttribute { get; set; }

        public string ThisHasNoAttributeButIsAccessible { get; set; }

        [Strainer(IsFilterable = true, IsSortable = true)]
        public string Title { get; set; }

        public Comment TopComment { get; set; }

    }
}
