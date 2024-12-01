using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Services.Metadata.Attributes;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Services.Metadata.Attributes;

public class AttributeCriteriaCheckerTests
{
    private readonly AttributeCriteriaChecker _checker = new();

    [Fact]
    public void ObjectChecking_Should_Return_False_When_AttributeIsNull()
    {
        // Arrange
        StrainerObjectAttribute attribute = null;
        var propertyInfo = Substitute.For<PropertyInfo>();
        var isSortableRequired = false;
        var isFilterableRequired = false;

        // Act
        var result = _checker.CheckIfObjectAttributeIsMatching(
            attribute,
            propertyInfo,
            isSortableRequired,
            isFilterableRequired);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ObjectChecking_Should_Return_False_When_PropertyInfoIsNull()
    {
        // Arrange
        var attribute = new StrainerObjectAttribute("foo");
        PropertyInfo propertyInfo = null;
        var isSortableRequired = false;
        var isFilterableRequired = false;

        // Act
        var result = _checker.CheckIfObjectAttributeIsMatching(
            attribute,
            propertyInfo,
            isSortableRequired,
            isFilterableRequired);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(false, false, true)]
    [InlineData(false, true, true)]
    [InlineData(true, true, true)]
    [InlineData(true, false, false)]
    public void ObjectChecking_Should_Return_CorrectResult_ForSortableFlag(bool isSortableRequired, bool isAttributeSortable, bool expectedResult)
    {
        // Arrange
        var attribute = new StrainerObjectAttribute("foo")
        {
            IsSortable = isAttributeSortable,
        };
        var propertyInfo = Substitute.For<PropertyInfo>();
        var isFilterableRequired = false;

        // Act
        var result = _checker.CheckIfObjectAttributeIsMatching(
            attribute,
            propertyInfo,
            isSortableRequired,
            isFilterableRequired);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(false, false, true)]
    [InlineData(false, true, true)]
    [InlineData(true, true, true)]
    [InlineData(true, false, false)]
    public void ObjectChecking_Should_Return_CorrectResult_ForFilterableFlag(bool isFilterableRequired, bool isAttributeFilterable, bool expectedResult)
    {
        // Arrange
        var attribute = new StrainerObjectAttribute("foo")
        {
            IsFilterable = isAttributeFilterable,
        };
        var propertyInfo = Substitute.For<PropertyInfo>();
        var isSortableRequired = false;

        // Act
        var result = _checker.CheckIfObjectAttributeIsMatching(
            attribute,
            propertyInfo,
            isSortableRequired,
            isFilterableRequired);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void PropertyChecking_Should_Return_False_When_AttributeIsNull()
    {
        // Arrange
        StrainerPropertyAttribute attribute = null;
        var propertyInfo = Substitute.For<PropertyInfo>();
        var isSortableRequired = false;
        var isFilterableRequired = false;
        var name = "foo";

        // Act
        var result = _checker.CheckIfPropertyAttributeIsMatching(
            attribute,
            propertyInfo,
            isSortableRequired,
            isFilterableRequired,
            name);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void PropertyChecking_Should_Return_False_When_PropertyInfoIsNull()
    {
        // Arrange
        var attribute = new StrainerPropertyAttribute();
        PropertyInfo propertyInfo = null;
        var isSortableRequired = false;
        var isFilterableRequired = false;
        var name = "foo";

        // Act
        var result = _checker.CheckIfPropertyAttributeIsMatching(
            attribute,
            propertyInfo,
            isSortableRequired,
            isFilterableRequired,
            name);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(false, false, true)]
    [InlineData(false, true, true)]
    [InlineData(true, true, true)]
    [InlineData(true, false, false)]
    public void PropertyChecking_Should_Return_CorrectResult_ForSortableFlag(bool isSortableRequired, bool isAttributeSortable, bool expectedResult)
    {
        // Arrange
        var name = "foo";
        var attribute = new StrainerPropertyAttribute
        {
            IsSortable = isAttributeSortable,
            DisplayName = name,
        };
        var propertyInfo = Substitute.For<PropertyInfo>();
        var isFilterableRequired = false;

        // Act
        var result = _checker.CheckIfPropertyAttributeIsMatching(
            attribute,
            propertyInfo,
            isSortableRequired,
            isFilterableRequired,
            name);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(false, false, true)]
    [InlineData(false, true, true)]
    [InlineData(true, true, true)]
    [InlineData(true, false, false)]
    public void PropertyChecking_Should_Return_CorrectResult_ForFilterableFlag(bool isFilterableRequired, bool isAttributeFilterable, bool expectedResult)
    {
        // Arrange
        var name = "foo";
        var attribute = new StrainerPropertyAttribute
        {
            IsFilterable = isAttributeFilterable,
            DisplayName = name,
        };
        var propertyInfo = Substitute.For<PropertyInfo>();
        var isSortableRequired = false;

        // Act
        var result = _checker.CheckIfPropertyAttributeIsMatching(
            attribute,
            propertyInfo,
            isSortableRequired,
            isFilterableRequired,
            name);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("foo", null, null, false)]
    [InlineData("foo", "foo", null, true)]
    [InlineData("foo", "bar", null, false)]
    [InlineData("foo", null, "foo", true)]
    [InlineData("foo", null, "bar", false)]
    [InlineData("foo", "foo", "foo", true)]
    public void PropertyChecking_Should_Return_CorrectResult_WhenNameChecking(
        string criteriaName,
        string attributeDisplayName,
        string propertyInfoName,
        bool expectedResult)
    {
        // Arrange
        var propertyInfo = Substitute.For<PropertyInfo>();
        propertyInfo.Name.Returns(propertyInfoName);
        var attribute = new StrainerPropertyAttribute
        {
            DisplayName = attributeDisplayName,
            PropertyInfo = propertyInfo,
        };
        var isSortableRequired = false;
        var isFilterableRequired = false;

        // Act
        var result = _checker.CheckIfPropertyAttributeIsMatching(
            attribute,
            propertyInfo,
            isSortableRequired,
            isFilterableRequired,
            criteriaName);

        // Assert
        result.Should().Be(expectedResult);
    }
}
