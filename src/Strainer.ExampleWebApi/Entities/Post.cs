using System;
using System.ComponentModel.DataAnnotations.Schema;
using Fluorite.Strainer.Attributes;

namespace Fluorite.Strainer.ExampleWebApi.Entities
{
	public class Post
    {
        [StrainerProperty(IsFilterable = true, IsSortable = true)]
        public int Id { get; set; }

        [StrainerProperty(IsFilterable = true, IsSortable = true)]
        public string Title { get; set; }

        [StrainerProperty(IsFilterable = true, IsSortable = true)]
        public int LikeCount { get; set; }

        [StrainerProperty(IsFilterable = true, IsSortable = true)]
        public int CommentCount { get; set; }

        [StrainerProperty(IsFilterable = true, IsSortable = true)]
        [Column(TypeName = "datetime")]
        public DateTime DateCreated { get; set; }

        [StrainerProperty(IsFilterable = true, IsSortable = true)]
        [Column(TypeName = "datetime")]
        public DateTime DateLastViewed { get; set; }

        [StrainerProperty(IsFilterable = true, IsSortable = true)]
        public int? CategoryId { get; set; }
    }
}
