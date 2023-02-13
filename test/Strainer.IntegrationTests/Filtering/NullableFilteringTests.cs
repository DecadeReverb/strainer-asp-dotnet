using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.IntegrationTests.Filtering
{
    public class NullableFilteringTests : StrainerFixtureBase
    {
        public NullableFilteringTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void IsFilterableNullableInts()
        {
            // Arrange
            var queryable = new List<Post>
            {
                new Post
                {
                    CategoryId = 0,
                },
                new Post
                {
                    CategoryId = 1,
                },
            }.AsQueryable();
            var model = new StrainerModel()
            {
                Filters = "CategoryId==1"
            };
            var processor = Factory.CreateDefaultProcessor();

            // Act
            var result = processor.Apply(model, queryable);

            // Assert
            result.Should().OnlyContain(p => p.CategoryId == 1);
        }

        private class Post
        {
            [StrainerProperty]
            public int CategoryId { get; set; }
        }
    }
}
