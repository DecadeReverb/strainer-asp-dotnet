using System;
using Fluorite.Strainer.Attributes;

namespace Fluorite.Strainer.UnitTests.Entities
{
	public class Comment
    {
        public int Id { get; set; }

        [Strainer(CanFilter = true, CanSort = true)]
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;

        [Strainer(CanFilter = true)]
        public string Text { get; set; }
    }
}
