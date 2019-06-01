using System;
using Strainer.Attributes;

namespace Strainer.UnitTests.Entities
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
