using System;
using System.ComponentModel.DataAnnotations.Schema;
using Fluorite.Strainer.Attributes;

namespace Fluorite.Strainer.ExampleWebApi.Entities
{
	public class Post
    {
        public int Id { get; set; }

        [Strainer(CanFilter = true, CanSort = true)]
        public string Title { get; set; } = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 8);

        [Strainer(CanFilter = true, CanSort = true)]
        public int LikeCount { get; set; } = new Random().Next(0, 1000);

        [Strainer(CanFilter = true, CanSort = true)]
        public int CommentCount { get; set; } = new Random().Next(0, 1000);

        [Strainer(CanFilter = true, CanSort = true)]
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;

        [Strainer(CanFilter = true, CanSort = true)]
        [Column(TypeName = "datetime")]
        public DateTime DateLastViewed { get; set; } = DateTime.UtcNow;

        [Strainer(CanFilter = true, CanSort = true)]
        public int? CategoryId { get; set; } = new Random().Next(0, 4);
    }
}
