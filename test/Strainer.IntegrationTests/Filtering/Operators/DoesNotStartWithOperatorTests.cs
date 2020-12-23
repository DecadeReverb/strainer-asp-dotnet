using FluentAssertions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;
using System;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Filtering.Operators
{
    public class DoesNotStartWithOperatorTests : StrainerFixtureBase
    {
        public DoesNotStartWithOperatorTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void DoesNotStartWith_Works_When_CaseSensivity_IsDisabled()
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
            var processor = Factory.CreateDefaultProcessor(options =>
            {
                options.IsCaseInsensitiveForValues = true;
                options.ThrowExceptions = true;
            });
            var model = new StrainerModel
            {
                Filters = "Text!_=f",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(c => !c.Text.StartsWith("f", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void DoesNotStartWith_Works_When_CaseSensivity_IsEnabled()
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
            var processor = Factory.CreateDefaultProcessor(options => options.ThrowExceptions = true);
            var model = new StrainerModel
            {
                Filters = "Text!_=f",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(c => !c.Text.StartsWith("f", StringComparison.Ordinal));
        }

        [Fact]
        public void DoesNotStartWith_Works_For_NonStringValues()
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
            var processor = Factory.CreateDefaultProcessor(options => options.ThrowExceptions = true);
            var model = new StrainerModel
            {
                Filters = "LikeCount!_=2",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(c => !c.LikeCount.ToString().StartsWith("2", StringComparison.Ordinal));
        }

        private class Comment
        {
            [StrainerProperty]
            public int LikeCount { get; set; }

            [StrainerProperty]
            public string Text { get; set; }
        }
    }
}
