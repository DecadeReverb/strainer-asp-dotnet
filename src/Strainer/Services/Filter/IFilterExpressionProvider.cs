using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering.Terms;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filter
{
    public interface IFilterExpressionProvider
    {
        //Expression<Func<TEntity, bool>> GetExpression<TEntity>(IEnumerable<IFilterTerm> filterTerms);
        //Expression<Func<TEntity, bool>> GetExpression<TEntity>(IFilterTerm filterTerm);
        Expression GetExpression(IPropertyMetadata metadata, IFilterTerm filterTerm, ParameterExpression parameterExpression, Expression innerExpression);

    }
}
