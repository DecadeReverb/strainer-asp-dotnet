using System;
using Sieve.Attributes;

namespace Sieve.UnitTests.Entities
{
	public class Comment
    {
        public int Id { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;

        [Sieve(CanFilter = true)]
        public string Text { get; set; }
    }
}
