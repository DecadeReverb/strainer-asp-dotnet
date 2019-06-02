using System;
using System.ComponentModel.DataAnnotations.Schema;
using Fluorite.Strainer.Attributes;

namespace Fluorite.Strainer.ExampleWebApi.Entities
{
	public class Post
    {
        public int Id { get; set; }

        [Strainer(CanFilter = true, CanSort = true)]
        public string Title { get; set; }

        [Strainer(CanFilter = true, CanSort = true)]
        public int LikeCount { get; set; }

        [Strainer(CanFilter = true, CanSort = true)]
        public int CommentCount { get; set; }

        [Strainer(CanFilter = true, CanSort = true)]
        [Column(TypeName = "datetime")]
        public DateTime DateCreated { get; set; }

        [Strainer(CanFilter = true, CanSort = true)]
        [Column(TypeName = "datetime")]
        public DateTime DateLastViewed { get; set; }

        [Strainer(CanFilter = true, CanSort = true)]
        public int? CategoryId { get; set; }
    }
}
