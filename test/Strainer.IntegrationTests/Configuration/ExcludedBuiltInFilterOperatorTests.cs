using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Modules;
using System.Linq.Expressions;

namespace Fluorite.Strainer.IntegrationTests.Configuration;

public class ExcludedBuiltInFilterOperatorTests : StrainerFixtureBase
{
    private const string SymbolToRemove = FilterOperatorSymbols.DoesNotEndWithCaseInsensitive;

    public ExcludedBuiltInFilterOperatorTests(StrainerFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Should_Return_ExcludedFilterOperatorSymbolInConfiguration()
    {
        // Arrange
        var configurationProvider = Factory.CreateDefaultConfigurationProvider<TestModuleWithExcludedOperator>();

        // Act
        var result = configurationProvider.GetStrainerConfiguration();

        // Assert
        result.ExcludedBuiltInFilterOperators.Should().BeEquivalentTo(SymbolToRemove);
    }

    [Fact]
    public void Should_NotParse_ExcludedFilterOperatorSymbol()
    {
        // Arrange
        var context = Factory.CreateDefaultContext<TestModuleWithExcludedOperator>();
        var filterOperatorParser = context.Filter.OperatorParser;

        // Act
        var result = filterOperatorParser.GetParsedOperator(SymbolToRemove);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Should_NotThrow_WhenCustomFilterOperatorIsUsingOneOfBuiltInSymbols()
    {
        // Act
        Action act = () => Factory.CreateDefaultConfigurationProvider<TestModuleWithCustomConflictingOperator>();

        // Assert
        act.Should().NotThrow<InvalidOperationException>();
    }

    [Fact]
    public void Should_Throw_WhenCustomFilterOperatorIsUsingOneOfBuiltInSymbols_WithoutPriorRemoval()
    {
        // Act
        Action act = () => Factory.CreateDefaultConfigurationProvider<TestModuleWithCustomConflictingOperator_WithoutPriorRemoval>();

        // Assert
        act.Should().ThrowExactly<InvalidOperationException>()
            .WithMessage("Unable to add configuration from Strainer modules.")
            .WithInnerExceptionExactly<InvalidOperationException>()
            .WithMessage(
                $"A custom filter operator is conflicting with built-in filter operator on symbol {SymbolToRemove}. " +
                $"Either mark the built-in filter operator to be excluded or remove custom filter operator.");
    }

    private class TestModuleWithExcludedOperator : StrainerModule
    {
        public override void Load(IStrainerModuleBuilder builder)
        {
            builder.RemoveBuiltInFilterOperator(SymbolToRemove);
        }
    }

    private class TestModuleWithCustomConflictingOperator : StrainerModule
    {
        public override void Load(IStrainerModuleBuilder builder)
        {
            builder.RemoveBuiltInFilterOperator(SymbolToRemove);

            builder.AddFilterOperator(SymbolToRemove)
                .HasName("custom operator")
                .HasExpression(_ => Expression.Constant(true));
        }
    }

    private class TestModuleWithCustomConflictingOperator_WithoutPriorRemoval : StrainerModule
    {
        public override void Load(IStrainerModuleBuilder builder)
        {
            builder.AddFilterOperator(SymbolToRemove)
                .HasName("custom operator")
                .HasExpression(_ => Expression.Constant(true));
        }
    }
}
