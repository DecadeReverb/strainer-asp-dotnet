using System;
using Fluorite.Strainer.Attributes;

namespace Fluorite.Strainer.TestModels
{
	public class Comment
    {
        public int Id { get; set; }

        [Strainer(IsFilterable = true, IsSortable = true)]
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;

        [Strainer(IsFilterable = true)]
        public string Text { get; set; }
    }
}
