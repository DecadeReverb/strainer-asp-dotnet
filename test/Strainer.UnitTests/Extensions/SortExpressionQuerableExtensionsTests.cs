using Fluorite.Extensions;
using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.UnitTests.Extensions;

public class SortExpressionQuerableExtensionsTests
{
    [Fact]
    public void Should_Throw_ForNullSource()
    {
        // Arrange
        var source = Enumerable.Empty<string>().AsQueryable();

        // Act
        Action action = () => source.OrderWithSortExpression(sortExpression: null);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Should_Throw_ForSortExpression()
    {
        // Arrange
        IQueryable<object> source = null;
        var sortExpression = new SortExpression<object>(x => x);

        // Act
        Action action = () => source.OrderWithSortExpression(sortExpression);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Should_Order_FirstTime(bool descending)
    {
        // Arrange
        var source = new[] { 4, 1, 3 }.AsQueryable();
        var sortExpression = new SortExpression<int>(x => x)
        {
            IsDescending = descending,
        };

        // Act
        var result = source.OrderWithSortExpression(sortExpression);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().BeEquivalentTo(source);

        if (descending)
        {
            result.Should().BeInDescendingOrder();
        }
        else
        {
            result.Should().BeInAscendingOrder();
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Should_Order_Subsequently(bool descending)
    {
        // Arrange
        var source = new[] { "foo", "ba", "bb" }.AsQueryable().OrderByDescending(x => x.Length);
        var sortExpression = new SortExpression<string>(x => x.Count(y => y == 'b'))
        {
            IsDescending = descending,
            IsSubsequent = true,
        };

        // Act
        var result = source.OrderWithSortExpression(sortExpression);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().BeEquivalentTo(source);

        if (descending)
        {
            result.Should().BeEquivalentTo(source.ThenByDescending(sortExpression.Expression));
        }
        else
        {
            result.Should().BeEquivalentTo(source.ThenBy(sortExpression.Expression));
        }
    }
}
