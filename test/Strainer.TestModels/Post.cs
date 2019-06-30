using System;
using Fluorite.Strainer.Attributes;

namespace Fluorite.Strainer.TestModels
{
	public class Post
    {
        public int Id { get; set; }

        [Strainer(IsFilterable = true, IsSortable = true)]
        public string Title { get; set; } = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 8);

        [Strainer(IsFilterable = true, IsSortable = true)]
        public int LikeCount { get; set; } = new Random().Next(0, 1000);

        [Strainer(IsFilterable = true, IsSortable = true)]
        public int CommentCount { get; set; } = new Random().Next(0, 1000);

        [Strainer(IsFilterable = true, IsSortable = true)]
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;

        [Strainer(IsFilterable = true, IsSortable = true)]
        public int? CategoryId { get; set; } = new Random().Next(0, 4);

        [Strainer(IsFilterable = true, IsSortable = true)]
        public bool IsDraft { get; set; }

        public string ThisHasNoAttribute { get; set; }

        public string ThisHasNoAttributeButIsAccessible { get; set; }

        public int OnlySortableViaFluentApi { get; set; }

        public Comment TopComment { get; set; }
        public Comment FeaturedComment { get; set; }
    }
}
