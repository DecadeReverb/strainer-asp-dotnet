using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.TestModels;
using System;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Services.Filtering.Operators
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
            var processor = Factory.CreateDefaultProcessor(options => options.CaseSensitive = false);
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
        public void EqualsCaseInsensitive_Works_When_CaseSensivity_IsEnabled()
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
            var processor = Factory.CreateDefaultProcessor(options => options.CaseSensitive = true);
            var model = new StrainerModel
            {
                Filters = "Text==*foo",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(b => b.Text.Equals("foo", StringComparison.OrdinalIgnoreCase));
        }
    }
}
