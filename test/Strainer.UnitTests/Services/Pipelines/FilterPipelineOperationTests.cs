using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Pipelines;
using System.Linq.Expressions;

namespace Fluorite.Strainer.UnitTests.Services.Pipelines;

public class FilterPipelineOperationTests
{
    private readonly ICustomFilteringExpressionProvider _customFilteringExpressionProvider = Substitute.For<ICustomFilteringExpressionProvider>();
    private readonly IFilterExpressionProvider _filterExpressionProvider = Substitute.For<IFilterExpressionProvider>();
    private readonly IFilterTermParser _filterTermParser = Substitute.For<IFilterTermParser>();
    private readonly IMetadataFacade _metadataFacade = Substitute.For<IMetadataFacade>();
    private readonly IStrainerOptionsProvider _strainerOptionsProvider = Substitute.For<IStrainerOptionsProvider>();

    private readonly FilterPipelineOperation _operation;

    public FilterPipelineOperationTests()
    {
        _operation = new FilterPipelineOperation(
            _customFilteringExpressionProvider,
            _filterExpressionProvider,
            _filterTermParser,
            _metadataFacade,
            _strainerOptionsProvider);
    }

    [Fact]
    public void Should_Return_CollectionWithoutChanges_WhenNoFilterTermsIsFound()
    {
        // Arrange
        var source = new[] { "foo" }.AsQueryable();
        var model = new StrainerModel
        {
            Filters = "input",
        };

        _strainerOptionsProvider
            .GetStrainerOptions()
            .Returns(new StrainerOptions());
        _filterTermParser
            .GetParsedTerms(model.Filters)
            .Returns(new List<IFilterTerm>());

        // Act
        var result = _operation.Execute(model, source);

        // Assert
        result.Should().BeSameAs(source);

        _strainerOptionsProvider.Received(1).GetStrainerOptions();
        _filterTermParser.Received(1).GetParsedTerms(model.Filters);
    }

    [Fact]
    public void Should_Return_CollectionWithoutChanges_WhenTermExpressionIsNull()
    {
        // Arrange
        var source = new[] { "foo" }.AsQueryable();
        var model = new StrainerModel
        {
            Filters = "input",
        };
        var filterTermName = "name";
        var filterTerm = Substitute.For<IFilterTerm>();
        filterTerm.Names.Returns(new[] { filterTermName });
        var terms = new List<IFilterTerm>
        {
            filterTerm,
        };
        var metadata = Substitute.For<IPropertyMetadata>();

        _strainerOptionsProvider
            .GetStrainerOptions()
            .Returns(new StrainerOptions());
        _filterTermParser
            .GetParsedTerms(model.Filters)
            .Returns(terms);
        _metadataFacade
            .GetMetadata<string>(isSortableRequired: false, isFilterableRequired: true, filterTermName)
            .Returns(metadata);

        // Act
        var result = _operation.Execute(model, source);

        // Assert
        result.Should().BeSameAs(source);

        _strainerOptionsProvider.Received(1).GetStrainerOptions();
        _filterTermParser.Received(1).GetParsedTerms(model.Filters);
        _metadataFacade.Received(1).GetMetadata<string>(false, true, filterTermName);
        _filterExpressionProvider.Received(1).GetExpression(metadata, filterTerm, Arg.Any<ParameterExpression>(), Arg.Any<Expression>());
    }

    [Fact]
    public void Should_Return_CollectionWithFilteringApplied()
    {
        // Arrange
        var source = new[] { "foo" }.AsQueryable();
        var sourceClone = ((string[])new[] { "foo" }.Clone()).AsQueryable();
        var model = new StrainerModel
        {
            Filters = "input",
        };
        var filterTermName = "name";
        var filterTerm = Substitute.For<IFilterTerm>();
        filterTerm.Names.Returns(new[] { filterTermName });
        var terms = new List<IFilterTerm>
        {
            filterTerm,
        };
        var metadata = Substitute.For<IPropertyMetadata>();
        var validExpression = Expression.Constant(false);

        _strainerOptionsProvider
            .GetStrainerOptions()
            .Returns(new StrainerOptions());
        _filterTermParser
            .GetParsedTerms(model.Filters)
            .Returns(terms);
        _metadataFacade
            .GetMetadata<string>(isSortableRequired: false, isFilterableRequired: true, filterTermName)
            .Returns(metadata);
        _filterExpressionProvider
            .GetExpression(metadata, filterTerm, Arg.Any<ParameterExpression>(), Arg.Any<Expression>())
            .Returns(validExpression);

        // Act
        var result = _operation.Execute(model, source);

        // Assert
        result.Should().NotBeSameAs(sourceClone);
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _strainerOptionsProvider.Received(1).GetStrainerOptions();
        _filterTermParser.Received(1).GetParsedTerms(model.Filters);
        _metadataFacade.Received(1).GetMetadata<string>(false, true, filterTermName);
        _filterExpressionProvider.Received(1).GetExpression(metadata, filterTerm, Arg.Any<ParameterExpression>(), Arg.Any<Expression>());
    }
}
