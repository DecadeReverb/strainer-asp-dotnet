using FluentAssertions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models;
using System;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Services.Filtering.Operators
{
    public class ContainsOperatorTests : StrainerFixtureBase
    {
        public ContainsOperatorTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void Contains_Works_When_CaseSensivity_IsDisabled()
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
                Filters = "Text@=foo",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(b => b.Text.Contains("foo", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void Contains_Works_When_CaseSensivity_IsEnabled()
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
                Filters = "Text@=oo",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(b => b.Text.Contains("oo", StringComparison.Ordinal));
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
