using FluentAssertions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models;
using System;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Models.Filtering.Operators
{
    public class EqualsCaseInsensitiveTests : StrainerFixtureBase
    {
        public EqualsCaseInsensitiveTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void EqualsCaseInsensitive_Works_When_CaseSensivity_IsDisabled()
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
                Filters = "Text==*foo",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(b => b.Text.Equals("foo", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void EqualsCaseInsensitive_Works_Even_When_CaseSensivity_IsEnabled()
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
                Filters = "Text==*foo",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(b => b.Text.Equals("foo", StringComparison.OrdinalIgnoreCase));
        }

        private class Comment
        {
            [StrainerProperty(IsFilterable = true)]
            public string Text { get; set; }
        }
    }
}
