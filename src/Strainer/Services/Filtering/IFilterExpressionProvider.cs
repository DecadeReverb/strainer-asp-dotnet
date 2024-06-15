using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Models.Metadata;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering;

public interface IFilterExpressionProvider
{
    Expression GetExpression(
        IPropertyMetadata metadata,
        IFilterTerm filterTerm,
        ParameterExpression parameterExpression,
        Expression innerExpression);
}
