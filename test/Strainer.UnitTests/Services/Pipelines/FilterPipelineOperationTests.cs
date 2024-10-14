using Fluorite.Strainer.Exceptions;
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

    [Fact]
    public void Should_Throw_WhenMetadataIsNotFound()
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

        _strainerOptionsProvider
            .GetStrainerOptions()
            .Returns(new StrainerOptions { ThrowExceptions = true });
        _filterTermParser
            .GetParsedTerms(model.Filters)
            .Returns(terms);
        _metadataFacade
            .GetMetadata<string>(isSortableRequired: false, isFilterableRequired: true, filterTermName)
            .Returns((IPropertyMetadata)null);

        // Act
        Action act = () => _operation.Execute(model, source);

        // Assert
        act.Should().ThrowExactly<StrainerMethodNotFoundException>()
            .WithMessage($"Property or custom filter method '{filterTermName}' was not found.");

        _strainerOptionsProvider.Received(1).GetStrainerOptions();
        _filterTermParser.Received(1).GetParsedTerms(model.Filters);
        _metadataFacade.Received(1).GetMetadata<string>(false, true, filterTermName);
    }

    [Fact]
    public void Should_NotThrow_WhenMetadataIsNotFound_ButOptionsDisableExceptions()
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

        _strainerOptionsProvider
            .GetStrainerOptions()
            .Returns(new StrainerOptions { ThrowExceptions = false });
        _filterTermParser
            .GetParsedTerms(model.Filters)
            .Returns(terms);
        _metadataFacade
            .GetMetadata<string>(isSortableRequired: false, isFilterableRequired: true, filterTermName)
            .Returns((IPropertyMetadata)null);

        // Act
        var result = _operation.Execute(model, source);

        // Assert
        result.Should().BeSameAs(source);

        _strainerOptionsProvider.Received(1).GetStrainerOptions();
        _filterTermParser.Received(1).GetParsedTerms(model.Filters);
        _metadataFacade.Received(1).GetMetadata<string>(false, true, filterTermName);
    }

    [Fact]
    public void Should_NotThrow_WhenMetadataIsNotFound_ButCustomFilterMethodIsFound()
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
        Expression<Func<string, bool>> customExpression = _ => false;

        _strainerOptionsProvider
            .GetStrainerOptions()
            .Returns(new StrainerOptions());
        _filterTermParser
            .GetParsedTerms(model.Filters)
            .Returns(terms);
        _metadataFacade
            .GetMetadata<string>(isSortableRequired: false, isFilterableRequired: true, filterTermName)
            .Returns((IPropertyMetadata)null);
        _customFilteringExpressionProvider
            .TryGetCustomExpression(filterTerm, filterTermName, out Arg.Any<Expression<Func<string, bool>>>())
            .Returns(x => {
                x[2] = customExpression;
                return true;
            });

        // Act
        var result = _operation.Execute(model, source);

        // Assert
        result.Should().NotBeSameAs(sourceClone);
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _strainerOptionsProvider.Received(1).GetStrainerOptions();
        _filterTermParser.Received(1).GetParsedTerms(model.Filters);
        _metadataFacade.Received(1).GetMetadata<string>(false, true, filterTermName);
    }

    [Fact]
    public void Should_Return_CollectionWithMultipleFilteringsApplied_FromTheSameTerm()
    {
        // Arrange
        var source = new[] { "foo", "boat", "ID" }.AsQueryable();
        var sourceClone = ((string[])new[] { "foo" }.Clone()).AsQueryable();
        var model = new StrainerModel
        {
            Filters = "input",
        };
        var filterTermName1 = "name";
        var filterTermName2 = "second-name";
        var filterTerm = Substitute.For<IFilterTerm>();
        filterTerm.Names.Returns(new[] { filterTermName1, filterTermName2 });
        var terms = new List<IFilterTerm>
        {
            filterTerm,
        };
        var metadata1 = Substitute.For<IPropertyMetadata>();
        var metadata2 = Substitute.For<IPropertyMetadata>();

        _strainerOptionsProvider
            .GetStrainerOptions()
            .Returns(new StrainerOptions());
        _filterTermParser
            .GetParsedTerms(model.Filters)
            .Returns(terms);
        _metadataFacade
            .GetMetadata<string>(isSortableRequired: false, isFilterableRequired: true, filterTermName1)
            .Returns(metadata1);
        _metadataFacade
            .GetMetadata<string>(isSortableRequired: false, isFilterableRequired: true, filterTermName2)
            .Returns(metadata2);
        _filterExpressionProvider
            .GetExpression(metadata1, filterTerm, Arg.Any<ParameterExpression>(), Arg.Any<Expression>())
            .Returns(x => CreateStringGreaterThanLengthExpression(x[2] as ParameterExpression, length: 2));
        _filterExpressionProvider
            .GetExpression(metadata2, filterTerm, Arg.Any<ParameterExpression>(), Arg.Is<Expression>(x => x != null))
            .Returns(x => CreateStringGreaterThanLengthExpression(x[2] as ParameterExpression, length: 3));

        // Act
        var result = _operation.Execute(model, source);

        // Assert
        result.Should().NotBeSameAs(sourceClone);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo("boat");

        _strainerOptionsProvider.Received(1).GetStrainerOptions();
        _filterTermParser.Received(1).GetParsedTerms(model.Filters);
        _metadataFacade.Received(1).GetMetadata<string>(false, true, filterTermName1);
        _metadataFacade.Received(1).GetMetadata<string>(false, true, filterTermName2);
        _filterExpressionProvider.Received(1).GetExpression(metadata1, filterTerm, Arg.Any<ParameterExpression>(), Arg.Any<Expression>());
        _filterExpressionProvider.Received(1).GetExpression(metadata2, filterTerm, Arg.Any<ParameterExpression>(), Arg.Is<Expression>(x => x != null));
    }

    private Expression CreateStringGreaterThanLengthExpression(ParameterExpression parameterExpression, int length)
    {
        return Expression.GreaterThan(
            Expression.Property(
                parameterExpression,
                nameof(string.Length)),
            Expression.Constant(length));
    }
}
