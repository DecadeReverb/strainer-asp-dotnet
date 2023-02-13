using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.IntegrationTests.Sorting
{
    public class MultipleSortingTests : StrainerFixtureBase
    {
        public MultipleSortingTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void SortingByMultiplePropertiesWorks()
        {
            // Arrange
            var source = new[]
            {
                new Post
                {
                    IsDraft = true,
                    LikeCount = 20,
                },
                new Post
                {
                    IsDraft = false,
                    LikeCount = 20,
                },
                new Post
                {
                    IsDraft = false,
                    LikeCount = 10,
                },
            }.AsQueryable();
            var model = new StrainerModel()
            {
                Sorts = "-IsDraft,-LikeCount"
            };
            var processor = Factory.CreateDefaultProcessor(opt =>
            {
                opt.DefaultSortingWay = SortingWay.Ascending;
            });

            // Act
            var result = processor.ApplySorting(model, source);
            var expectedCollection = source.OrderByDescending(p => p.IsDraft).ThenByDescending(p => p.LikeCount);

            // Assert
            result.Should().BeInDescendingOrder(e => e.IsDraft);
            result.Should().BeEquivalentTo(expectedCollection);
        }

        private class Post
        {
            [StrainerProperty(IsDefaultSorting = true)]
            public bool IsDraft { get; set; }

            [StrainerProperty]
            public int LikeCount { get; set; }
        }
    }
}
