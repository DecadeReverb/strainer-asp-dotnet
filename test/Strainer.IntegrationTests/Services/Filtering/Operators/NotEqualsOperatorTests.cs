using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.TestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Services.Filtering.Operators
{
    public class NotEqualsOperatorTests : StrainerFixtureBase
    {
        public NotEqualsOperatorTests(StrainerFactory factory) : base(factory)
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
                new Post
                {
                    LikeCount = 20,
                },
                new Post
                {
                    LikeCount = 10,
                },
                new Post
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
            result.Should().OnlyContain(p => !p.LikeCount.Equals(20));
        }
    }
}
