using Fluorite.Strainer.Attributes;
using System;
using System.Collections.Generic;

namespace Fluorite.Strainer.ExampleWebApi.Entities;

[StrainerObject(nameof(Id))]
	public class Post
{
    public Post()
    {
        Comments = new List<Comment>();
    }

    public int? CategoryId { get; set; }

    public IList<Comment> Comments { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTimeOffset DateLastViewed { get; set; }

    public int Id { get; set; }

    public int LikeCount { get; set; }

    public string Symbol { get; set; }

    public string Title { get; set; }
}
