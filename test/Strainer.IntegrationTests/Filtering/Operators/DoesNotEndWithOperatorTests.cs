using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.IntegrationTests.Filtering.Operators
{
    public class DoesNotEndWithOperatorTests : StrainerFixtureBase
    {
        public DoesNotEndWithOperatorTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void DoesNotEndWith_Works_When_CaseSensivity_IsDisabled()
        {
            // Arrange
            var source = new[]
            {
                new Comment
                {
                    Text = "ARR",
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
                Filters = "Text!=_r",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(c => !c.Text.EndsWith("r", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void DoesNotEndWith_Works_When_CaseSensivity_IsEnabled()
        {
            // Arrange
            var source = new[]
            {
                new Comment
                {
                    Text = "ARR",
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
                Filters = "Text!=_r",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(c => !c.Text.EndsWith("r", StringComparison.Ordinal));
        }

        [Fact]
        public void DoesNotEndWith_Works_For_NonStringValues()
        {
            // Arrange
            var source = new[]
            {
                new Comment
                {
                    LikeCount = 22,
                },
                new Comment
                {
                    LikeCount = 12,
                },
                new Comment
                {
                    LikeCount = 50,
                },
            }.AsQueryable();
            var processor = Factory.CreateDefaultProcessor(options => options.ThrowExceptions = true);
            var model = new StrainerModel
            {
                Filters = "LikeCount!=_2",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(c => !c.LikeCount.ToString().EndsWith("2", StringComparison.Ordinal));
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
