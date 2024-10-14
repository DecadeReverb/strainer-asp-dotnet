using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.IntegrationTests.Sorting;

public class BoolSortingTests : StrainerFixtureBase
{
    public BoolSortingTests(StrainerFactory factory) : base(factory)
    {

    }

    [Fact]
    public void IsSortableBools()
    {
        // Arrange
        var source = new[]
        {
            new Post
            {
                IsDraft = true,
            },
            new Post
            {
                IsDraft = false,
            },
        }.AsQueryable();
        var model = new StrainerModel()
        {
            Sorts = "-IsDraft"
        };
        var processor = Factory.CreateDefaultProcessor(opt =>
        {
            opt.DefaultSortingWay = SortingWay.Ascending;
        });

        // Act
        var result = processor.ApplySorting(model, source);

        // Assert
        result.Should().BeInDescendingOrder(e => e.IsDraft);
    }

    private class Post
    {
        [StrainerProperty(IsDefaultSorting = true)]
        public bool IsDraft { get; set; }
    }
}
