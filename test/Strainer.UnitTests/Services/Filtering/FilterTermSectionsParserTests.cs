using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Services.Filtering;

namespace Fluorite.Strainer.UnitTests.Services.Filtering;

public class FilterTermSectionsParserTests
{
    private readonly IConfigurationFilterOperatorsProvider _filterOperatorsConfigurationProvider = Substitute.For<IConfigurationFilterOperatorsProvider>();

    private readonly FilterTermSectionsParser _parser;

    public FilterTermSectionsParserTests()
    {
        _parser = new FilterTermSectionsParser(_filterOperatorsConfigurationProvider);
    }

    [Fact]
    public void Should_Throw_WhenNoOperatorsAreSupplied()
    {
        // Arrange
        var input = string.Empty;
        var operators = new Dictionary<string, IFilterOperator>();

        _filterOperatorsConfigurationProvider
            .GetFilterOperators()
            .Returns(operators);

        // Act
        Action act = () => _parser.Parse(input);

        // Assert
        act.Should().ThrowExactly<InvalidOperationException>()
            .WithMessage("No filter operators found.");
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("foo", "foo")]
    public void Should_Parse_ToEmptyTermSections(string input, string names)
    {
        // Arrange
        var sign = "==";
        var filterOperator = Substitute.For<IFilterOperator>();
        filterOperator.Symbol.Returns(sign);
        var operators = new Dictionary<string, IFilterOperator>
        {
            [sign] = filterOperator,
        };

        _filterOperatorsConfigurationProvider
            .GetFilterOperators()
            .Returns(operators);

        // Act
        var result = _parser.Parse(input);

        // Assert
        result.Should().NotBeNull();
        result.Names.Should().Be(names);
        result.OperatorSymbol.Should().BeEmpty();
        result.Values.Should().BeEmpty();
    }

    [Theory]
    [InlineData("==", "", "==", "")]
    [InlineData("Name==", "Name", "==", "")]
    [InlineData("==John", "", "==", "John")]
    [InlineData("Name==John", "Name", "==", "John")]
    public void Should_Parse_ToOneTermSection(string input, string names, string @operator, string values)
    {
        // Arrange
        var sign = "==";
        var filterOperator = Substitute.For<IFilterOperator>();
        filterOperator.Symbol.Returns(sign);
        var operators = new Dictionary<string, IFilterOperator>
        {
            [sign] = filterOperator,
        };

        _filterOperatorsConfigurationProvider
            .GetFilterOperators()
            .Returns(operators);

        // Act
        var result = _parser.Parse(input);

        // Assert
        result.Should().NotBeNull();
        result.Names.Should().Be(names);
        result.OperatorSymbol.Should().Be(@operator);
        result.Values.Should().Be(values);
    }
}
