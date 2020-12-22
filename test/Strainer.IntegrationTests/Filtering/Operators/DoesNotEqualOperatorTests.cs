using FluentAssertions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;
using System;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Filtering.Operators
{
    public class DoesNotEqualOperatorTests : StrainerFixtureBase
    {
        public DoesNotEqualOperatorTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void NotEquals_Works_When_CaseSensivity_IsDisabled()
        {
            // Arrange
            var source = new[]
            {
                new Comment
                {
                    Text = "foo",
                },
                new Comment
                {
                    Text = "bar",
                },
                new Comment
                {
                    Text = "FOO",
                },
            }.AsQueryable();
            var processor = Factory.CreateDefaultProcessor(options => options.IsCaseInsensitiveForValues = true);
            var model = new StrainerModel
            {
                Filters = "Text!=foo",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(b => !b.Text.Equals("foo", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void NotEquals_Works_When_CaseSensivity_IsEnabled()
        {
            // Arrange
            var source = new[]
            {
                new Comment
                {
                    Text = "foo",
                },
                new Comment
                {
                    Text = "bar",
                },
                new Comment
                {
                    Text = "FOO",
                },
            }.AsQueryable();
            var processor = Factory.CreateDefaultProcessor();
            var model = new StrainerModel
            {
                Filters = "Text!=foo",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(b => !b.Text.Equals("foo", StringComparison.Ordinal));
        }

        [Fact]
        public void NotEquals_Works_For_NonStringValues()
        {
            // Arrange
            var source = new[]
            {
                new Comment
                {
                    LikeCount = 20,
                },
                new Comment
                {
                    LikeCount = 10,
                },
                new Comment
                {
                    LikeCount = 50,
                },
            }.AsQueryable();
            var processor = Factory.CreateDefaultProcessor();
            var model = new StrainerModel
            {
                Filters = "LikeCount!=20",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(c => !c.LikeCount.Equals(20));
        }

        [Fact]
        public void Equals_Works_For_ComplexValues()
        {
            // Arrange
            var source = new[]
            {
                new Comment
                {
                    TimeSpan = TimeSpan.FromMinutes(1),
                },
                new Comment
                {
                    TimeSpan = TimeSpan.FromMinutes(2),
                },
            }.AsQueryable();
            var processor = Factory.CreateDefaultProcessor();
            var model = new StrainerModel
            {
                Filters = "TimeSpan!=00:01:00",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(c => c.TimeSpan != TimeSpan.FromMinutes(1));
        }

        private class Comment
        {
            [StrainerProperty(IsFilterable = true)]
            public TimeSpan TimeSpan { get; set; }

            [StrainerProperty(IsFilterable = true)]
            public int LikeCount { get; set; }

            [StrainerProperty(IsFilterable = true)]
            public string Text { get; set; }
        }
    }
}
