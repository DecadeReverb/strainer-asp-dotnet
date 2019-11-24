using FluentAssertions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models;
using System;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Services.Filtering.Operators
{
    public class EndsWithOperatorTests : StrainerFixtureBase
    {
        public EndsWithOperatorTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void EndsWith_Works_When_CaseSensivity_IsDisabled()
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
                Filters = "Text=_o",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(c => c.Text.EndsWith("o", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void EndsWith_Works_When_CaseSensivity_IsEnabled()
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
                Filters = "Text=_o",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(c => c.Text.EndsWith("o", StringComparison.Ordinal));
        }

        [Fact]
        public void EndsWith_Works_For_NonStringValues()
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
                    LikeCount = 55,
                },
            }.AsQueryable();
            var processor = Factory.CreateDefaultProcessor(options => options.ThrowExceptions = true);
            var model = new StrainerModel
            {
                Filters = "LikeCount=_5",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(c => c.LikeCount.ToString().EndsWith("5", StringComparison.Ordinal));
        }

        private class Comment
        {
            [StrainerProperty(IsFilterable = true)]
            public int LikeCount { get; set; }

            [StrainerProperty(IsFilterable = true)]
            public string Text { get; set; }
        }
    }
}
